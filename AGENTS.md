# Envision OS — Codex 项目规则

## 项目定义
- **从零开发**的 Unity + C# 安卓车机桌面；V0.1 只实现高德 App 启动和现有音乐 App 的媒体会话控制。
- Android Java/Kotlin 仅承担打开 App、音乐系统接口、跳转授权设置等桥接工作；不能将主应用重写成 Compose。
- V0.1 不实现倒车影像、车辆控制、OBD-II、复杂 3D HMI，也不默认替换系统 HOME。
- 需求详见 `EnvisionOS_Unity_V0.1_Codex完整开发任务书.md`。

## 开始工作前
1. 阅读任务书、`docs/DEVELOPMENT.md`、`docs/HANDOFF.md`。
2. 执行 `git status -sb`、`git branch -vv`、`git remote -v`、`git log -5 --oneline`。
3. 不假设公司与家里的 Codex 本地聊天记录同步。以实际代码和提交为事实依据，交接文档为开发上下文。
4. 如果工作树不干净、分支分叉或存在冲突，停止任何自动覆盖，先向用户解释。

## 开发规则
- 小步实施：先 Unity 工程和 Mock UI，再高德启动，再 Android MediaSession 插件，再蓝牙音频实测。
- 所有安卓系统调用隔离到 Android Bridge；Unity Editor 必须有 Mock 实现。
- 只使用 Android 公开 API，通知访问权限须用户手动开启，不请求特权媒体控制权限或 root。
- 不安装未知插件、不提交密钥和签名文件，不引入不必要的 SDK。
- 不更改车辆原有倒车影像和音响线路；不遮挡车辆原车显示和驾驶视野。
- 只将**实际执行**的构建或测试标记为通过；无法访问 Editor、模拟器或实车时直说“未验证”。
- 未经用户当次授权，不自动 commit、push、创建 PR、重置、清理或强制覆盖 Git 历史。

## 结束工作前
1. 更新 `docs/HANDOFF.md`：日期、开发地点（家里/公司）、分支和 commit、本次成果、改动文件、真实测试结果、阻塞、未提交状态、下一步。
2. 更新 `docs/DEVELOPMENT.md`：阶段状态、技术决策和风险。
3. 更新 `docs/TESTING.md`：只记录真实测试；未做的不许写通过。
4. 报告文件变更列表、构建证据与后续用户操作，询问是否授权 Git 提交和推送。
