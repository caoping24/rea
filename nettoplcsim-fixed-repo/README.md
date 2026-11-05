# NetToPLCsim S7o (v1.2.5.0) — 屏保/关屏卡死修复版

本仓库是 **NetToPLCsim S7o v1.2.5.0** 的社区修复分支（fork），原作者 **Thomas Wiens**（原项目以 LGPL v3 发布）。

- 原项目：`NetToPLCsim`，用于把 S7-PLCSIM 通过网络（ISO-on-TCP / S7-300/400 或 S7CommPlus）提供给外部以太网客户端连接。
- 本分支目标：修复一个已知 bug —— **Windows 下长时间运行，或运行中触发屏幕保护程序 / 关闭显示器后，程序 UI 卡死、按钮无响应，只能重启**。

> 原版代码版权归 Thomas Wiens，遵循 **GNU LGPL v3**（见 `LICENSE`）。本分支的修改同样以 LGPL v3 发布。

---

## 修复了什么

**根因**：S7 消息泵线程（独立后台线程的 WinForms 消息循环）在收到 S7ONLINE 驱动的 `WM_SINEC` 通知后调用 `SCP_receive(handle, 0, ...)`，其中 **超时参数 = 0 = 无限期等待**。当驱动在空闲/关屏后停止回数据，泵线程永久卡死，其窗口消息循环停止泵消息；而对泵窗口的所有通讯都使用**同步 `SendMessage`**（停止服务器/关闭程序时 UI 线程发送 `WM_M_EXIT`），因此一旦泵卡死，UI 线程也随之挂死。

**改动（7 处，仅托管层，不改协议/握手逻辑）**：

| 文件 | 改动 |
|---|---|
| `IsoOnTcpServer/IsoToS7online.cs` | 新增 `user32.dll` `PostMessage` P/Invoke；`ExitPlcsimMessagePump()` 的 `WM_M_EXIT` 由 `SendMessage` 改为 `PostMessage`（异步、立即返回）；`InitPlcsim()` 两个 `WaitOne()` 加 3 秒超时 |
| `IsoOnTcpServer/S7online/PlcS7onlineMsgPump.cs` | `ReceiveFromPlcsimDirect()`：`SCP_receive` 超时 `0 → 5000` |
| `IsoOnTcpServer/S7online/PlcS7onlineMsgPumpS7.cs` | `ReceiveFromPlcsimAndSendBack()`：`SCP_receive` 超时 `0 → 5000` |
| `IsoOnTcpServer/S7online/PlcS7onlineMsgPumpTia.cs` | `ReceiveFromPlcsimAndSendBack()`：`SCP_receive` 超时 `0 → 5000` |

- 泵线程的 `SCP_receive` 从"无限期阻塞"变为"最多等 5 秒"，驱动空闲挂住时会超时返回，消息泵不再永久卡死。
- `WM_M_EXIT` 改为 `PostMessage` 后，停止/关闭不再阻塞 UI，UI 永不因这条链路卡死。
- 正常工作时 S7ONLINE 用异步 `WM_SINEC` 通知、数据已就绪，`SCP_receive` 几乎立即返回，**5 秒超时不会误触发**，不影响正常通讯与握手。

---

## 构建

- 目标：.NET Framework、x86（PE32 / IL-only）。
- **注意**：本分支已把工程从 `.NET Framework 4.0` 重定向到 **`.NET Framework 4.8`**（本机未装 4.0 目标包；4.8 向后兼容，运行需本机已装 .NET Framework 4.8）。修复逻辑全在托管层，与原生 S7ONLINE 的 P/Invoke 交互不受影响。
- 使用 VS 2026 / MSBuild（Release|x86）：

```bat
MSBuild.exe Plcsim-S7online.sln /t:Rebuild /p:Configuration=Release /p:Platform=x86
```

产物：
- `NetToPLCSim\bin\Release\NetToPLCsim.exe`（70656 B）
- `IsoOnTcpServer\bin\Release\IsoToS7online.dll`（45568 B）

> 若源码是从网络下载后解压，需先移除 `.resx` 等文件的"Mark of the Web"（否则 MSBuild 报 MSB3821）。可对整个源码目录执行 `Unblock-File -Recurse` 或删除 `Zone.Identifier` 流。

---

## 使用

仓库已附带编译好的二进制（`dist/`）：

- `dist/NetToPLCsim.exe`
- `dist/IsoToS7online.dll`

将这两个文件覆盖到原 NetToPLCsim 安装目录（**建议先备份原文件**）即可，或直接运行 `NetToPLCsim.exe`。

**运行时依赖（不随仓库分发）**：程序通过 P/Invoke 调用本机的 **`S7onlinx.dll`（西门子 S7ONLINE 组件）**，并需要 S7-PLCSIM 环境。该 DLL 为西门子专有组件，不属于本仓库、不在 LGPL 范围内，**请勿将其提交到本仓库**；使用前请在本机安装对应的西门子软件。

---

## 许可证

- 本分支遵循 **GNU LGPL v3**，见 `LICENSE`。
- 原始版权与许可见源码各文件头部 `Copyright (C) 2011-2016 Thomas Wiens` 与 LGPL 声明。
- `S7onlinx.dll` 等第三方/专有组件保留各自权利。

---

## 验证情况

- 用静态分析（REA，`inspect-managed-*`）复核新产物：x86 / PE32 / CLI runtime 2.5 / il-only，`AssemblyVersion = 1.2.5.0`；新 dll 的 P/Invoke 清单中 `PostMessage`（user32.dll）已确认编入，`S7onlinx.dll` 的 `SetSinecHWnd / SetSinecHWndMsg / SCP_open / SCP_close / SCP_send / SCP_receive / SCP_get_errno` 全部保留。
- 未在真实 S7-PLCSIM 环境做屏保复现测试；请在您的环境里长时间运行 + 触发屏保/关屏验证。
