# Envision OS — 最近一次开发交接

- 日期：2026-10-13（Asia/Hong_Kong）
- 开发地点：家里（用户确认）。
- 工程：UnityProject/Envision OS；Unity 2022.3.62f3c1。
- Git：开始时根目录不是 Git 仓库；用户本次已授权提交和上传。
- 分支：本次拟初始化 main。当前交接提交 SHA 请用 git log -1 查询，文档不自引用提交 SHA。
- 远端：用户指定 https://github.com/chenguanxi61/Envision_OS.git 。上传结果以实际 push 输出为准。

## 当前进度
Phase 0 进行中，尚未验收。Phase 1–4 均未开始，不开发倒车影像、3D 车辆、默认 HOME 或 Compose 主界面。

已有 Unity 空模板工程，保留 SampleScene。DashboardController.cs 原型在运行时创建 UGUI/TMP 首页、静态高德卡片、标注 Mock 的音乐、设置面板和演示提示；没有 Android JNI 或真实系统功能。

Phase0Tools.cs 提供 Editor 场景生成和 Development APK 构建方法。首次生成因 TMP shader 缺失出现 ArgumentNullException；已修正导入流程并实际导入 TMP 基础资源。修正后未重试生成 Launcher，未执行 APK 构建。

已加入 NotoSansCJKsc-Regular.otf 和 OFL 许可证。用户已创建 NotoSansCJKsc-Regular SDF.asset，磁盘检查为 Dynamic。SampleScene 已挂原型脚本，保留用户保存的场景原样。

## 用户最新偏好（下一台电脑必须遵守）
用户有开发经验，明确要求不要用脚本生成 UI，由用户在 Unity Editor 手工创建层级和控件。后续脚本只负责交互、状态更新和 Inspector 引用绑定。

原型脚本和场景生成工具保留作为已有工作，不继续运行生成工具或扩展生成 UI。手工创建前退出 Play Mode，禁用旧 DashboardController，防止重复 Canvas。不要覆盖用户场景或字体。用户通过 Escape 停止过 computer-use；不要默认接管 Editor。

## 实际环境与验证
- Unity：2022.3.62f3c1；已打开工程，原型脚本导入后出现菜单，没有观察到 C# 编译错误。这不是完整构建通过证据。
- AndroidPlayer 模块存在；SDK platforms 33/34/35 存在。
- NDK source.properties：23.1.7779620。
- 实际执行内置 java -version：OpenJDK 11.0.14.1。
- 实际执行 adb devices：列表为空，没有连接设备。
- 用户 Play Mode 截图显示原型首页，但中文方框，字体指定 LiberationSans SDF - Fallback；中文修复尚未重新验证，按钮未验收。
- 磁盘只有 SampleScene，尚无 Launcher.unity 或 APK。
- 高德启动、MediaSession、蓝牙、模拟器、实车均未实现或未测试。

## 文件交接
- Assets/Scripts/UI/DashboardController.cs 及 meta：运行时 UI 原型，后续改为手工 UI 引用绑定。
- Assets/Editor/Phase0Tools.cs 及 meta：原型生成和构建工具。
- Assets/Art/Fonts/：Noto OTF、OFL、用户生成的 SDF 及 meta。
- Assets/TextMesh Pro/：Unity 导入的基础资源。
- Assets/Scenes/SampleScene.unity：用户当前场景，保留原样。
- README.md、.gitignore、docs/UI_SETUP.md、DEVELOPMENT.md、TESTING.md、ANDROID_SETUP.md、HANDOFF.md。

## 下一步（仅 Phase 0）
1. 按 UI_SETUP.md 手工创建 Launcher，并保存卡片 Prefab。
2. TMP 指定 Noto 中文 SDF；验证中文、16:9 与 16:10 布局。
3. 根据真实层级改写交互脚本，通过序列化引用绑定，测试 Mock 按钮和设置面板。
4. 将手工 Launcher 加入 Build Settings，尝试 Development APK，保存真实 BuildReport、路径、安装启动结果。
5. Phase 0 验收后汇报，等待用户确认再进入 Phase 1。

## Git 与未保存状态
本文件随交接提交保存。推送前检查远端，禁止强推或覆盖非空远端；上传结果以实际命令为准。Unity 未保存的改动不会进入 Git，用户需要自行保存。另一台电脑先读项目规则和本文件，执行四项 Git 检查再开发。
