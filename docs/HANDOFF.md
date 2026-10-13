# Envision OS — 最近一次开发交接

- 日期：2026-10-13（Asia/Shanghai）
- 开发地点：公司
- 分支与基点：`main`，`4715af6`（修复一下bug）；开始时 `main` 与 `origin/main` 一致，工作区干净。
- 仓库：`https://github.com/chenguanxi61/Envision_OS.git`
- 本次范围：更新交接与开发文档；**未创建、构建或测试 Unity 工程**。

## 当前事实

最近两次已推送提交 `5dd3a35`、`4715af6` 先替换、后移除了 Unity 工程。当前仓库没有 `UnityProject/`。旧的 `UnityProject/Envision OS`、原型脚本、字体和 `SampleScene` 不再位于工作树；它们仍可从早期 Git 历史查阅，但不得误认为当前工程文件。

公司电脑使用 Unity 2022.3.62f3 新建工程时，`Packages/manifest.json`、`packages-lock.json` 与全部检查到的 `ProjectSettings` 文件以相同异常字节开头，Unity 报 manifest 无效；此前旧工程的 Bee 构建缓存也无法解析。Unity 安装目录中的模板 manifest 可正常读取。绿盾透明加密或授权策略可能影响工程文件读取，尚未经 IT 确认。重新创建和清理 `Library` 未得到可用工程；公司电脑当前不承担后续开发。

## 已确认的开发选择

- 回家后使用 **Unity 2022.3.62f3 国际版**重新创建工程，不复制公司电脑生成的异常文件。
- 保持 Unity + C# 主应用、uGUI + TextMeshPro 和 Android 最小公开 API 桥接。
- 用户在 Editor 手工搭 UI；后续脚本仅绑定控件并处理交互。Phase 0 完成前不进入高德或 MediaSession 实现。

## 回家后的下一步（仅 Phase 0）

1. 执行 `git status -sb`、`git branch -vv`、`git remote -v`、`git log -5 --oneline`；工作区干净时再拉取最新 `main`。
2. 在仓库 `UnityProject/` 创建轻量 Unity 工程；确认 `Packages/manifest.json` 是有效 JSON、Editor 导入和脚本编译正常，`GameObject → UI → Canvas` 可用。
3. 按 `docs/UI_SETUP.md` 手工建立 `Launcher`，先验证中文字体、16:9 与 16:10 布局，再编写引用绑定与 Mock 交互脚本。
4. 检查家里的 Android Build Support、SDK/NDK/JDK 与 ADB；具备条件后尝试 Development APK，记录真实结果。没有设备时安装与实机验证继续标为未测试。
5. 更新 `README.md`、`docs/DEVELOPMENT.md`、`docs/HANDOFF.md` 和 `docs/TESTING.md`；Phase 0 验收后汇报，再决定 Phase 1。

## 文件变更与验证

本次只改 `README.md`、`docs/DEVELOPMENT.md`、`docs/HANDOFF.md`、`docs/TESTING.md`、`docs/ANDROID_SETUP.md`、`docs/UI_SETUP.md`。未运行 Unity Editor、APK 构建、模拟器或设备测试。提交及推送由用户自行执行；最终未提交状态以提交前的 `git status -sb` 为准。
