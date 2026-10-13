# Android 环境与 Phase 0 构建

更新：2026-10-13。仓库当前没有 Unity 工程或 APK；下面是新工程的待执行步骤，**不是构建通过记录**。

## 新工程准备

1. 在家里使用 Unity 2022.3.62f3 国际版创建工程，确认 Editor 导入和空场景编译正常。
2. 通过 Unity Hub 检查该 Editor 的 Android Build Support、Android SDK & NDK Tools、OpenJDK；在 `Edit → Preferences → External Tools` 确认使用对应 Editor 的工具。实际版本和路径以家里电脑检查结果记录，不沿用旧工程的数据。
3. 手工创建并保存 `Assets/Scenes/Launcher.unity`，加入 Build Settings。配置 Product `Envision OS`、包名 `com.envisionos.launcher`、版本 `0.1.0`、横屏。最低 API、目标 API、ABI 以目标设备和实际构建条件确认；不要只照旧原型工具设置。
4. 切换 Android 平台，选择 Development Build 和 APK 输出，使用调试签名，不提交密钥。保存真实 BuildReport、APK 路径、错误和 Unity 版本。
5. 有设备后记录 Android 版本、ABI、安装、启动与屏幕适配；没有设备则标为未测试。

旧 `Phase0Tools.cs` 已随旧工程移除，不再是构建入口。Phase 0 不增加 Android 原生插件、Manifest 改写或通知权限；高德和 MediaSession 桥接须在后续阶段实施。
