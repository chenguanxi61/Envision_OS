# Envision OS

计划使用 Unity + C# 开发安卓车机横屏桌面。V0.1 只包含高德 App 启动和现有音乐 App 的媒体会话控制。目前 **Phase 0 未验收**，仓库尚无可运行的 Unity 工程或 APK。

## 当前状态

2026-10-13，公司电脑上的 Unity 工程已从仓库移除。新工程的 `Packages` 和 `ProjectSettings` 文件无法作为正常文本读取，Unity 无法导入。绿盾策略可能影响文件读写，但未经 IT 确认。不要从公司电脑复制该工程作为新项目基础。

下一步在家里使用 **Unity 2022.3.62f3 国际版**重新创建轻量工程，采用 **uGUI + TextMeshPro**，由用户在 Editor 中手工搭建 UI。先确认空工程能导入和编译，再建立 `Launcher` 场景。项目规则见 [AGENTS.md](AGENTS.md)，具体进度见 [docs/HANDOFF.md](docs/HANDOFF.md)。

## 计划目录

新工程建议放在仓库的 `UnityProject/`，以实际创建后的路径为准。首次创建前确认 Git 工作区干净；Unity 自动生成的 `Library/`、`Temp/`、构建产物和签名文件不提交，`Assets/*.meta`、`Packages/` 和 `ProjectSettings/` 需要保留。创建成功后更新本说明与构建步骤。

Phase 0 手工 UI 指引见 [docs/UI_SETUP.md](docs/UI_SETUP.md)，Android 准备步骤见 [docs/ANDROID_SETUP.md](docs/ANDROID_SETUP.md)。
