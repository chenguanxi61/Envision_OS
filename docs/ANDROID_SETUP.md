# Android 环境与 Phase 0 构建

2026-10-13 家里：Unity 2022.3.62f3c1，Android Build Support；SDK platforms 33/34/35、NDK 23.1.7779620、内置 OpenJDK 11.0.14.1。ADB 无设备。全局 JDK 25，构建优先使用 Unity 内置工具。

## 手工场景完成后
1. 打开 UnityProject/Envision OS，保存 Assets/Scenes/Launcher.unity，加入 Build Settings。
2. Switch Platform 到 Android；Product Envision OS，Company EnvisionOS，包名 com.envisionos.launcher，版本 0.1.0，Landscape Left。
3. 使用 Unity 内置 SDK/NDK/JDK；原型工具设置 Minimum API 23、Target Automatic、IL2CPP、ARMv7 + ARM64。目标机兼容性待验证。
4. Development Build，输出 APK（非 AAB），Debug 签名，不使用自定义密钥，输出到工程 Builds/Android/。
5. 保存真实 BuildReport、APK 路径和错误；连接设备后记录安装/启动及设备 Android 版本。

Phase0Tools.BuildAndroid 预期输出 Builds/Android/EnvisionOS-Phase0-debug.apk 和 build-result.txt，要求 Launcher 场景存在。尚未运行构建，路径不是成功证据。场景生成工具保留，但按用户决定后续手工搭建 UI。

Phase 0 没有 Android 原生插件、Manifest 改写或权限请求。高德与 MediaSession 桥接后续需用户确认再开始。
