# Envision OS

Unity + C# 安卓车机首页。当前 Phase 0 未验收；高德启动、音乐 MediaSession 和车载蓝牙尚未实现或实测。

## 打开与交接
Unity Hub 添加 UnityProject/Envision OS，使用 Unity 2022.3.62f3c1。先读 AGENTS.md、任务书、docs/DEVELOPMENT.md 和 docs/HANDOFF.md。

用户决定手工搭建 UI，参考 docs/UI_SETUP.md。禁用旧 DashboardController，防止运行时重复生成 Canvas。现有 SampleScene、字体和原型均保留。没有 Launcher.unity 或 APK。

## 构建
手工创建并保存 Assets/Scenes/Launcher.unity，加入 Build Settings，配置 Android Development Build。工具安装信息和流程见 docs/ANDROID_SETUP.md。原型 Phase0Tools 构建方法未执行，场景生成方法不再作为后续 UI 工作流程。

没有 Android 原生插件。Noto 中文字体附 OFL；TMP 基础资源来自 Unity 包。Assets/meta、Packages、ProjectSettings 和交接文档应提交；缓存、构建、签名和机器设置忽略。
