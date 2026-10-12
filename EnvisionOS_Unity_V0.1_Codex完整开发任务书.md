# Envision OS V0.1 — Unity 车机 Launcher 完整开发任务书

> **适用对象：桌面 Codex / AI 编程助手**  
> **版本：V0.1（高德导航 + 系统音乐控制）**  
> **开发方式：从零开发，不基于 Open Launcher**  
> **主技术栈：Unity + C# + UGUI + TextMeshPro；Android Java/Kotlin 仅做必要系统桥接**  
> **目标设备：安卓平板 / 可安装 APK 的后装安卓车机；优先在模拟器或普通安卓设备验证，再上车实测。**

---

## 0. 给 Codex 的最高优先级指令

你现在是 **Envision OS 项目的开发者**。请以本文档为需求基线，先审计真实工作目录和开发环境，再逐阶段创建可运行的工程。你可以选择合理的具体实现细节，但必须满足以下约束：

1. **项目从零开发**，不依赖、不克隆、不复制 `dw2lam/openlauncher`，不沿用任何 Checkout 失败的旧仓库工作树。
2. **Unity 是主应用**：首页、卡片、交互、动画由 Unity C# 实现；系统级能力使用最小化的 Android 插件。**不要把主界面改写成 Jetpack Compose。**
3. **本轮只实现导航与音乐**：高德 App 启动、返回 Unity；读取和控制 Android 现有媒体会话；确保 UI 可操作并构建 APK。倒车影像、OBD、3D 汽车、地图内嵌、车载 CAN/MCU 等不属于 V0.1。
4. **先跑通再美化**：先建立 Unity 工程 → 高德启动 → MediaSession → 集成调试 → 最后细化界面。每阶段都必须有明确验收，不要一次性修改全部模块。
5. **不能假装环境或测试存在**：如果无法操作 Unity Editor、Android SDK、真机或打包工具，创建必要代码和配置，并明确标记“未执行/待实机验证”；不能声称已成功构建或验证。
6. **保护用户工作**：启动前执行 `git status -sb`，不覆盖现有文件、不执行 `git reset --hard`、`git clean -fd`、不改写历史、不未经授权推送或创建 PR。
7. **跨设备交接必须维护**：同步代码依靠 GitHub，开发上下文依靠根目录 `AGENTS.md`、`docs/DEVELOPMENT.md`、`docs/HANDOFF.md`；本地 Codex 会话本身不视为跨设备共享。
8. **安全第一**：本项目只做车辆娱乐/导航的辅助显示；保留原改装车机已有的倒车影像和原车功能，不控制制动、转向等车辆关键系统，测试时车辆安全停放。
9. **优先轻量**：考虑老旧安卓车机 CPU/GPU 和 RAM 可能较弱；V0.1 尽量避免复杂 3D、持续重绘动画、无必要轮询和额外后台服务。
10. 任何涉及 SDK、API、包名、插件构建方式的细节，如果当前设备或工具版本不一致，**应以实际 Unity/Android API 和可复现构建结果为准**，记录差异，不盲目照抄示意代码。

### 0.1 第一次执行的任务

请仅从 **Phase 0** 开始：核对仓库是否为真正的新项目，检查 Unity 安装与 Android Build Support，创建最小 Unity 横屏工程、静态导航卡片和音乐卡片，并构建第一个可启动 APK（如环境允许）；完成后汇报检查结果、改动文件、构建结果以及 Phase 1 的下一步，等待我确认后继续。

---

## 1. 项目背景与目标

### 1.1 使用背景

- 车辆：**2016 款别克昂科威**。
- 车辆中控：**已改装过的车机**，当前能显示倒车摄像头、辅助线及车辆示意图。
- 音响：用户确认车机的蓝牙已经可以播放手机音乐。计划让安卓平板或未来的新系统设备连接该车机作为**蓝牙媒体音频输出设备**。
- 尚未确定最终让 Envision OS 运行在原后装车机还是新增平板上。因此需保持兼容、模块化，并注意低性能设备。
- 用户熟悉 Unity/C#，暂不熟悉 Android/Kotlin 开发，要求 C# 承担绝大多数业务逻辑。
- 地图**不用嵌在 Unity UI 里**。直接启动已安装的高德地图 App 即可。
- 原改装车机倒车影像**在 V0.1 完全不改动**。如将来要统一显示，单独调研视频输入和倒挡触发，不要在本版本中尝试改线。

### 1.2 V0.1 的核心体验

1. 启动 Envision OS，看到简洁的**横屏深色车机桌面**。
2. 点击“导航”卡片或按钮，打开设备上已安装的**高德地图 Android App**；用户能正常使用高德导航。高德退出或用户通过系统返回/最近任务回到 Envision OS 时，桌面保持可用。
3. 在音乐 App（如网易云音乐、QQ 音乐）播放歌曲后，回到 Envision OS，音乐卡片能显示当前歌曲名称、歌手和播放状态。
4. 点击音乐卡片的上一首、播放/暂停、下一首按钮，通过 Android 系统标准媒体会话接口控制当前播放器。支持程度取决于第三方音乐 App 的实现，必须反馈真实状态。
5. 在系统设置中把设备蓝牙与当前改装车机配对并启用**媒体音频**，让音乐与高德导航语音从车载扬声器播放。此步骤主要是系统连接操作，不要求 Unity 自行实现蓝牙协议或强制切换音频路由。

### 1.3 明确的非目标

本版本**不开发**：

- Unity 内嵌高德地图、地图渲染引擎、路线规划 API、高德地图 SDK Key 管理。
- 新音乐播放器、音乐搜索、会员/登录、音乐下载、第三方 App 非公开接口调用。
- 倒车影像读取/分路、摄像头采集、自动倒挡弹窗。
- 汽车空调、车窗、车门控制、CAN Bus、OBD-II、车辆控制系统。
- 复杂 3D HMI、车辆模型、AR HUD、语音助手。
- Android 系统 ROM、系统签名、root、无障碍权限自动点击、开机自启动破解。
- 默认 HOME Launcher 设置（留给 V0.2，V0.1 **先以普通安卓 App** 运行）。

---

## 2. 技术选型与设计原则

| 层次 | 推荐技术 | 用途 |
|---|---|---|
| 主应用 | Unity 稳定 LTS（优先已安装且支持目标 Android 的版本） | 渲染 UI、处理输入和生命周期 |
| 逻辑 | C# | 卡片数据、业务状态、导航与音乐控制抽象 |
| UI | UGUI + TextMeshPro | 横屏卡片、按钮、文本、反馈 |
| Android 桥接 | Java Android Library/AAR；必要时用 Kotlin | App 启动、媒体会话、系统设置跳转 |
| 导航 | 已安装的高德地图 App | 路线规划、导航、定位均由高德负责 |
| 音乐控制 | `MediaSessionManager` + `MediaController` | 控制正在播放的音乐 App |
| 授权 | `NotificationListenerService` | 取得读取活跃媒体会话所需的用户授权路径 |
| 蓝牙音频 | Android 系统蓝牙设置 | 与原车机蓝牙配对、媒体音频输出 |
| 协作 | Git + GitHub + AGENTS/HANDOFF | 两台电脑切换开发 |

### 2.1 推荐实现细节

- 默认 **30 FPS 上限**；采用 Canvas 静态布局，避免每帧刷新全部文本/布局；进入后台正常暂停，重新获得焦点后刷新音乐状态。
- 架构以**简单可维护**为第一优先级，使用明确的 `Service/Controller/Bridge` 分层，不必引入依赖注入框架、复杂事件总线和第三方 UI 框架。
- 不要为了未来可能增加的功能提前制造抽象层；但必须把 Android Java 调用隔离在单独类里，避免 UI 脚本直接包含大量 JNI 细节。
- Android Bridge 如果用独立 AAR，则必须提供源代码和**可复现构建方法**，不要只交付二进制 AAR。
- 根据实际 Unity 版本选择它支持的 Android Activity 类型（例如 UnityPlayerActivity 或 UnityPlayerGameActivity）和插件方案，避免擅自覆盖 Unity 内置主 Activity。
- 不依赖超大图片、外部付费 UI 素材、未经许可的商标素材；第一版使用自制简洁图标或合法许可资源。

---

## 3. 项目目录设计

Codex 应尽量建立以下结构。具体的 Unity 自动生成文件以 Editor 为准；如果采用不同结构，必须在文档解释。

```text
EnvisionOS/
├── .gitignore
├── README.md
├── AGENTS.md
├── EnvisionOS_Unity_V0.1_Codex完整开发任务书.md
├── docs/
│   ├── DEVELOPMENT.md               # 长期阶段进度/技术决策
│   ├── HANDOFF.md                   # 本次开发交接
│   ├── ANDROID_SETUP.md             # Unity Android 构建与授权步骤
│   └── TESTING.md                   # 实机用例与测试结果
│
├── UnityProject/
│   ├── Assets/
│   │   ├── Scenes/
│   │   │   └── Launcher.unity
│   │   ├── Scripts/
│   │   │   ├── Core/
│   │   │   │   ├── AppBootstrap.cs
│   │   │   │   └── AppLifecycle.cs
│   │   │   ├── UI/
│   │   │   │   ├── DashboardController.cs
│   │   │   │   ├── NavigationCardController.cs
│   │   │   │   ├── MusicCardController.cs
│   │   │   │   └── StatusMessageView.cs
│   │   │   ├── Navigation/
│   │   │   │   ├── INavigationService.cs
│   │   │   │   └── AmapNavigationService.cs
│   │   │   ├── Music/
│   │   │   │   ├── IMusicService.cs
│   │   │   │   ├── MusicState.cs
│   │   │   │   └── AndroidMusicService.cs
│   │   │   └── Android/
│   │   │       ├── AndroidBridge.cs
│   │   │       └── AndroidEventReceiver.cs
│   │   ├── Prefabs/
│   │   │   ├── NavigationCard.prefab
│   │   │   └── MusicCard.prefab
│   │   ├── Plugins/Android/             # AAR/Manifest/Gradle 插件
│   │   └── Art/
│   ├── Packages/
│   └── ProjectSettings/
│
└── AndroidPlugin/                   # 如使用独立 Android Library
    ├── README.md
    └── ... Android Gradle Library
```

**补充要求：** `Assets/*.meta` 必须纳入 Git。Unity 的 `Library/`、`Temp/`、`Obj/`、`Logs/`、`UserSettings/`、构建目录、签名文件、机器独有路径、调试 APK 等应按情况排除。不要全局忽略 `*.aar` 或 `*.jar`，否则会遗漏必要插件。

---

## 4. UI 设计规范

### 4.1 整体风格

- 风格：参考现代智能车机的**深色、大圆角、清晰可点击**设计；不必复刻某品牌商业界面。
- 方向：**Landscape 横屏**，优先为 16:9 布局设计，需适配 16:10 与不同 DPI/安全区。
- Unity Canvas：`Canvas Scaler → Scale With Screen Size`，参考分辨率暂定 `1920×1080`；根据真机实际比例调整匹配方式。
- 推荐配色：背景 `#10141B`；主卡片 `#202833`；次级卡片 `#2B3340`；主文本 `#F1F5FA`；次文本 `#A7B0BE`；强调色 `#9BB9FF`。
- 圆角：24–32（以参考分辨率的像素为设计基准），视觉清晰优先。
- 不依赖每帧的复杂布局计算，避免在低性能车机产生卡顿。

### 4.2 首页布局示意（V0.1）

```text
┌─────────────────────────────────────────────────────────┐
│ Envision OS                               14:30   设置 │
├─────────────────────────────────────────────────────────┤
│                                                         │
│   ┌───────────────────────────┐ ┌───────────────────┐ │
│   │                           │ │   正在播放        │ │
│   │         高德地图          │ │   歌曲名称        │ │
│   │                           │ │   歌手名称        │ │
│   │      [ 开始导航 ]         │ │                   │ │
│   │                           │ │   |◀   ▶/Ⅱ   ▶|  │ │
│   └───────────────────────────┘ └───────────────────┘ │
│                                                         │
├─────────────────────────────────────────────────────────┤
│       导航                 音乐                  设置   │
└─────────────────────────────────────────────────────────┘
```

- **导航卡片**占更大面积，但 V0.1 不显示实时地图，只提供高德启动按钮与状态提示；可以做简洁的静态装饰，不得伪装为实时地图。
- **音乐卡片**显示：来源 App（若可获取）、歌曲名、歌手、播放状态、上一首、播放/暂停、下一首。
- **设置入口**至少包括：打开 Android 蓝牙设置、打开通知访问设置、显示当前版本、测试状态说明。
- 导航与音乐按钮要求方便手指操作，误触风险低；避免小字号和紧贴屏幕边缘的操作按钮。
- 编辑器中允许显示**明显标注的 Mock 音乐信息**；Android 真机无播放会话时必须显示真实空状态。

### 4.3 UI 状态约定

**导航卡片：** `Ready`、`AmapNotInstalled`、`LaunchFailed`。  
**音乐卡片：** `PermissionRequired`、`NoActiveSession`、`Playing`、`Paused`、`UnsupportedOperation`、`BridgeError`。

明确展示错误提示，不能在没有授权或没有音乐 App 时永远显示一首假的歌曲。

---

## 5. Phase 0 — 环境检查 + Unity 工程基线

### 5.1 先检查

1. 查看当前目录和已有文件，确保不是旧的失败 Checkout 仓库；检查 `git status -sb`、`git remote -v`、`git log -5 --oneline`。
2. 检查 Unity Hub/Unity Editor 及版本、Android Build Support、SDK/NDK/OpenJDK 模块、可用 Android 设备或模拟器。
3. 如果 Codex 环境无法自动打开 Unity Editor，不要手工伪造 `.unity` 场景文件或声称项目已经生成成功。可以先编写 C# 脚本和 Editor 自动构建/场景生成工具，说明用户在 Unity Editor 中需执行的步骤。
4. 确定新项目根目录、包名 `com.envisionos.launcher`（可调）及用户 Fork 或新建的**空白 Git 仓库**；不要自动覆盖之前的 `Envision-OS` 旧仓库。

### 5.2 创建工程

- 使用 Unity 3D Core 或 URP 以外的轻量模板均可，但 **V0.1 不需要 3D 场景和 URP 特效**。以 UI 场景为主。
- `Product Name`：`Envision OS`；`Company Name`：自定义不含敏感信息；Android 应用包名：`com.envisionos.launcher`。
- 屏幕：只允许横屏，按需启用沉浸模式，但必须保持用户可通过系统导航返回；**不要锁死 Android 系统导航或绕过锁屏**。
- 建立 `Launcher` Scene，Camera、Canvas、EventSystem、导航卡片、音乐卡片、设置入口。
- 在 Unity Editor 下使用 Mock 服务，使 UI 能在没有 Android 设备时正常预览。
- 编写 `README.md` 描述 Unity 版本、首次打开方法、构建方法、Android 插件来源。

### 5.3 基线验收

- [ ] Editor 中能看到横屏首页（真实场景或明确交代尚未创建）。
- [ ] 点击按钮在 Editor 不抛 `AndroidJavaException`。
- [ ] C# 代码无编译错误。
- [ ] 如具备构建环境，生成可安装 Debug APK，并记录路径与实际结果。
- [ ] 形成 `docs/DEVELOPMENT.md`、`docs/HANDOFF.md`、`docs/ANDROID_SETUP.md` 初稿。

**完成后先向用户报告，再继续 Phase 1。**

---

## 6. Phase 1 — 高德地图启动（先完成第一个真实功能）

### 6.1 需求与行为

按下“开始导航”后：

1. 判断当前平台：Unity Editor → 展示 Mock 提示，不执行 JNI；Android 设备 → 调用 Android Bridge。
2. 查询设备上是否存在**可启动的高德地图**。通常包名 `com.autonavi.minimap`，但如果设备渠道包不同，应以实际安装结果为准。
3. 存在可启动 Activity 时：通过 `PackageManager.getLaunchIntentForPackage(packageName)` 获取启动 Intent，调用 `startActivity(intent)`。**不需要高德地图 SDK Key，也不需要申请定位权限来启动另一个 App。**
4. 未安装或无法启动时：向 Unity 返回失败类型和可读原因，在 UI 显示“未找到高德地图，请先安装”；可以引导用户访问应用市场，但不能假设设备有 Play 商店。
5. 用户从高德返回 Unity（例如 Android 返回键、系统最近任务或 HOME），桌面正常刷新，不能黑屏、卡死、意外播放音频。
6. 禁止通过模拟点击、无障碍服务或私有 Intent 强行操纵高德地图。

### 6.2 Manifest 配置

针对 Android 11+ 应用可见性，按照真实插件合并结果增加精确的 package queries：

```xml
<manifest xmlns:android="http://schemas.android.com/apk/res/android">
    <queries>
        <package android:name="com.autonavi.minimap" />
    </queries>
</manifest>
```

这是一段**合并清单示意**，不是可直接覆盖 Unity 主 `AndroidManifest.xml` 的完整文件。不得申请不必要的 `QUERY_ALL_PACKAGES`。

### 6.3 接口设计

**C# 业务接口**（签名为示意，实际可按异步回调实现）：

```csharp
public interface INavigationService
{
    bool IsAmapAvailable();
    void OpenAmap();
}
```

**Android Bridge 建议方法：**

```text
boolean isPackageLaunchable(String packageName)
void launchPackage(String packageName)
```

必须捕获 `ActivityNotFoundException`、`SecurityException` 和其他预期 Android 异常，向 UI 显示合理状态；不要仅通过 `Debug.Log()` 吞掉失败。

### 6.4 验收用例

- [ ] 安卓设备安装高德时可正常打开。
- [ ] 未安装高德时有提示，不能闪退。
- [ ] 返回 Unity 时导航与音乐卡片仍可操作。
- [ ] Android 11+ 测试检查最终合并 Manifest 的 `<queries>` 是否生效。
- [ ] Editor 可以使用 Mock 导航服务而不访问安卓 Java 类。
- [ ] AMap SDK Key 不存在也能完成本阶段。

---

## 7. Phase 2 — Android 媒体会话插件（核心技术难点）

### 7.1 设计目标

音乐卡片**不自己播放音乐**，而是控制用户已打开的网易云音乐、QQ 音乐等 App 的 Android 媒体会话。采用公开 Android API：

- `MediaSessionManager.getActiveSessions(...)`
- `MediaController.getMetadata()`、`getPlaybackState()`
- `MediaController.TransportControls.play/pause/skipToNext/skipToPrevious`
- `MediaController.Callback` 监听元数据/状态变化
- `MediaSessionManager.OnActiveSessionsChangedListener` 监听会话列表变化
- `NotificationListenerService` 提供获得这些会话的授权路径

**不能默认所有播放器都会暴露会话、元数据和操作能力。** 某些音乐 App、后台限制或特定 Android ROM 可能导致无法控制，应明确显示原因并留有日志。

### 7.2 权限与 Manifest（必须严谨）

推荐在 Android Library 中实现一个 `EnvisionNotificationListenerService`（可命名不同）：

```xml
<service
    android:name="com.envisionos.bridge.EnvisionNotificationListenerService"
    android:permission="android.permission.BIND_NOTIFICATION_LISTENER_SERVICE"
    android:exported="true">
    <intent-filter>
        <action android:name="android.service.notification.NotificationListenerService" />
    </intent-filter>
</service>
```

注意：

1. `android.permission.BIND_NOTIFICATION_LISTENER_SERVICE` 应配置在 **`<service android:permission>`** 上；**不要误当成一个可以普通 `requestPermissions` 请求的运行时权限**。
2. 用户需在 Android 设置中手动允许 Envision OS 的**通知访问权限**，可以使用 `Settings.ACTION_NOTIFICATION_LISTENER_SETTINGS` 引导跳转，但不允许自动授权。
3. 检测权限状态时优先使用目标 Android 版本支持的官方 API；确保检测的是本应用的实际监听服务，不能仅靠一个永久缓存的 Boolean。
4. 获取其他 App 的活动会话时，需要把已授权的监听服务 `ComponentName` 传给 `getActiveSessions`；处理 `SecurityException`、服务尚未绑定及权限撤销。
5. 不要求设备 root、系统签名，不申请 `MEDIA_CONTENT_CONTROL` 等普通第三方应用无法正常获取的特权权限。
6. **只使用媒体会话数据**，不要收集、保存、上传无关通知内容；默认不要求联网后端。

### 7.3 音乐状态模型

Android 插件到 Unity 建议传递一次完整的 JSON 状态（仅示意，字段/命名可调整）：

```json
{
  "status": "Playing",
  "sourcePackage": "com.example.music",
  "title": "Example Song",
  "artist": "Example Artist",
  "isPlaying": true,
  "canPlayPause": true,
  "canSkipPrevious": true,
  "canSkipNext": true,
  "errorMessage": ""
}
```

C# 对应使用可被 Unity `JsonUtility` 序列化/反序列化的简单 DTO（或项目已使用、可合法集成的 JSON 库）。**别把示例包名、歌名当成真实业务数据**。

状态枚举至少包含：

```text
PermissionRequired
NoActiveSession
Playing
Paused
UnsupportedOperation
BridgeError
```

对空 `Metadata`、空 `PlaybackState`、未知歌名、会话刚被销毁、同时存在多个媒体会话等情况必须提供兜底。

### 7.4 会话选择策略

1. 在有权限前提下读取活动 `MediaController` 列表。
2. 优先选择 `PlaybackState.STATE_PLAYING` 的媒体会话。
3. 若没有正在播放会话，选择可用的最近媒体会话（依据系统返回顺序或状态活动时间）；不得无依据地随机控制多个 App。
4. 重新选择媒体会话时解除旧 `MediaController.Callback` 并注册新回调。
5. 响应媒体会话列表变更、播放状态更新、歌曲切换、播放器关闭和权限撤销。
6. Android 版本/ROM 对多媒体会话返回结果可能不同，提供可读日志，未来再考虑“切换音乐来源”功能。

### 7.5 控制操作

- **播放/暂停**：以**当前** `PlaybackState` 为准决定执行 `play()` 或 `pause()`；如状态不明确，合理提示用户。
- **上一首/下一首**：分别调用 `skipToPrevious()` 和 `skipToNext()`。
- **能力检查**：尽可能基于 `PlaybackState.getActions()` 判断当前按钮是否支持相应操作，状态不可靠时仍需捕获异常并反映实际结果。
- **异步与主线程**：Android 回调有不同线程来源，插件与 Unity UI 通信须遵守线程约束；不能让后台回调直接随意操作 Unity `GameObject` 或 TMP。
- **回调生命周期**：前后台切换和监听服务断开时注销监听器，重新进入首页时主动刷新状态，防止重复注册和内存泄露。

### 7.6 Unity ↔ Android 桥接接口建议

建议插件对 Unity 暴露以下最小接口，**需结合实际编译结果确定静态方法、Activity 参数与初始化方式**：

```text
initialize(Activity activity, String gameObjectName)
isNotificationAccessEnabled() -> boolean
openNotificationAccessSettings(Activity activity)
refreshMusicState() -> JSON 或异步回调
play()
pause()
next()
previous()
shutdown()
```

- Unity 使用 `AndroidJavaClass` / `AndroidJavaObject` 封装桥接逻辑，所有 JNI 代码集中在 `AndroidBridge.cs`。
- Android 回 Unity 可使用 `UnityPlayer.UnitySendMessage(gameObjectName, method, json)`，也可采用易测试的事件轮询桥接；选择后必须解释线程与回调顺序，并确保 GameObject/方法存在。
- 避免一次传入巨大 Bitmap；V0.1 不做专辑封面图像传输。
- Unity 编辑器下实现 `MockMusicService`，能演示权限缺失、播放、暂停、下一首和无会话几种状态。
- Android 插件必须附源码与 AAR 构建步骤；使用 Gradle 构建时，JDK/Android Gradle Plugin 版本要与 Unity 主工程兼容。

### 7.7 音乐阶段验收用例

- [ ] 首次打开未授权，音乐卡片显示“需要开启音乐控制权限”。
- [ ] 点击按钮能进入对应 Android 系统设置，由用户手动授权。
- [ ] 授权成功，返回 Unity 后刷新状态。
- [ ] 没有音乐 App 在播放时，显示“打开音乐 App 后即可控制”。
- [ ] 音乐 App 开始播放后，显示真实歌名/歌手/播放状态。
- [ ] 播放/暂停正常；上一首、下一首按播放器支持情况执行。
- [ ] 音乐 App 退出、媒体会话失效或关闭权限后不会闪退。
- [ ] 从高德返回 Unity 后，音乐卡片能恢复最新状态。
- [ ] 在至少**一款真实音乐 App** 上完成实测，并在 `docs/TESTING.md` 写明设备、Android 版本、App 版本和结果。

---

## 8. Phase 3 — 蓝牙车载音响与高德/音乐联动

### 8.1 蓝牙音频边界

用户现有后装车机已经能通过蓝牙播放手机音乐，因此目标是**将承载 Envision OS 的安卓设备与车机进行正常蓝牙配对，并验证媒体音频输出**。

- Envision OS 设置卡片可通过公开 Android Intent 打开系统蓝牙设置；不要求应用自动扫描、自动配对或偷偷切换音频设备。
- 对于 Android 的 A2DP 媒体音频输出，**由 Android 系统路由和车机接收端决定**；Unity 自身不需要实现蓝牙音频编解码。
- 如果当前改装车机不支持与平板建立媒体输出连接，需要先确认 A2DP Sink 兼容性和配对限制，不把软件按钮“显示已连接”当成音频实测成功。
- 导航语音与背景音乐是否混音/压低音量取决于高德、音乐 App、Android Audio Focus 和车机行为；只需测试并记录，不要通过危险手段绕开 Android 音频焦点机制。

### 8.2 实机步骤

1. 车辆停稳且安全驻车。
2. 安卓设备与改装车机完成配对，检查系统显示媒体音频已连接。
3. 打开音乐 App，实际用车内扬声器试听。
4. 返回 Envision OS，确认音乐卡片可读取与控制歌曲。
5. 通过导航卡片打开高德，测试路线规划及导航提示音能否通过车载扬声器输出。
6. 观察导航语音出现时，音乐播放是否暂停、降低音量或保持音量；记录真实情况。
7. 测试断开蓝牙、重连、播放器中断、长时间后台后的行为。

### 8.3 验收结果需要区分

- “已实现打开系统蓝牙设置”不是“车载蓝牙已成功连接”。
- “音乐卡片能控制手机/平板上的音乐”不是“声音已经从原车音响输出”。
- 蓝牙音频与高德语音的联合体验必须**在目标设备 + 真实车机上单独测试**。

---

## 9. Phase 4 — 界面完善、性能与 V0.1 交付

- 深色主题统一；导航大卡片、音乐信息展示、状态提示和设置入口清晰。
- 按实际目标设备分辨率测试卡片间距、按钮触控区、文本溢出、中文字体、横屏与安全区。
- 在低配设备上设置合理 FPS 上限，检查 Unity 前后台切换、高德启动返回和音乐元数据更新时的内存增长。
- 仅在状态改变时更新 UI，避免不必要每帧字符串拼接和 JNI 调用。
- 若目标机无法运行 Unity 6 或插件，记录硬件、Android 版本、架构与日志，再评估降低 Unity 版本、减轻 UI 或转为原生实现；**不要为了兼容单一设备擅自推翻已确认的项目主架构**。
- 若设备可用，生成 **Debug APK**；更新安装指南和真实测试记录。
- 保留原改装车机的倒车影像功能，不对倒车链路进行任何改动。若未来外置平板安装，不得遮挡原屏幕或驾驶视野。

---

## 10. 推荐执行节奏与验收总表

| 阶段 | 交付物 | 通过标准 | 用户需要做什么 |
|---|---|---|---|
| Phase 0 | Unity 工程、UI 场景、Mock 状态 | Editor 正常显示 / 可行时 APK 启动 | 确认 Unity 版本和目标设备 |
| Phase 1 | 高德启动 Android Bridge | 安装高德后点击能打开，返回正常 | 提供装有高德的设备 |
| Phase 2 | MediaSession 插件和音乐卡片 | 授权后能读歌名和控制至少一款播放器 | 手动开启通知访问、打开音乐 App |
| Phase 3 | 蓝牙与组合测试文档 | 音乐和导航声音实测从车载音响发出 | 在车内安全停放测试并反馈 |
| Phase 4 | 调优、APK 和 README | 无重大崩溃，记录性能和已知限制 | 决定是否用于日常辅助使用 |

**每个阶段都需要给出：**

```text
阶段名称：
完成内容：
新增/修改文件：
运行命令/实际测试：
结果：通过 / 失败 / 未执行（附原因）
当前问题与风险：
下一步建议：
是否需要用户操作：
Git 工作区状态：
```

---

## 11. 测试矩阵（持续维护 docs/TESTING.md）

| 编号 | 场景 | 预期 |
|---|---|---|
| T-01 | Editor 打开首页 | 横屏 UI 显示；Mock 数据不报错 |
| T-02 | Android 未安装高德 | 明确提示，不闪退 |
| T-03 | Android 已安装高德 | 点击启动，高德切换到前台 |
| T-04 | 从高德回 Envision OS | 卡片和按钮正常使用 |
| T-05 | 未开启通知访问 | 音乐卡片引导授权 |
| T-06 | 授权后无播放器 | 显示无活动会话 |
| T-07 | 音乐 App 正在播放 | 歌名、歌手、播放状态真实更新 |
| T-08 | 播放/暂停/切歌 | 按实际支持执行，异常有反馈 |
| T-09 | 播放器退出或会话消失 | 自动变为空状态，不崩溃 |
| T-10 | 关闭通知访问后返回 | 状态刷新，按未授权处理 |
| T-11 | 多个媒体会话 | 优先播放中的会话，不误控其他应用 |
| T-12 | Android 进入后台/恢复 | 不黑屏、不泄露回调，音乐状态恢复 |
| T-13 | 蓝牙断开/重新配对 | 系统音频变化正确，不假报成功 |
| T-14 | 高德导航音频 + 音乐 | 记录车载音响中实际表现 |
| T-15 | 低配设备长时间使用 | 记录 RAM、FPS、温度、异常情况 |

测试结果必须记录“日期、设备型号、Android 版本、Unity 版本、测试步骤、真实结果、日志/截图路径”；没有做过的用例标记为 **未测试**。

---

## 12. Git、跨设备开发与 Codex 交接制度

本项目会分别在**公司电脑与家用电脑**上使用 Codex。**不要假定 Codex 桌面端本地聊天会话会同步**。我们只同步**代码和结构化交接文档**。

### 12.1 Git 约定

- 使用用户自己创建的全新 GitHub 仓库，建议仓库名 `EnvisionOS-Unity`；不直接复用含旧 Open Launcher 历史的项目目录。
- `origin` 必须是用户本人有权限的仓库；`main` 为稳定主线，`develop` 为开发分支（是否建分支以真实仓库和用户授权为准）。
- 在两台电脑分别 Clone 同一个仓库；不要同时在同一分支积累互相未同步的修改。
- 在任何拉取前检查是否存在未提交文件，先保存工作；不使用 `reset --hard` 或 `git clean` 代替解决冲突。
- **Codex 可以建议 commit 与 push，但未经本次用户授权不得直接执行。**

### 12.2 每次开始新会话

1. 阅读本任务书、根目录 `AGENTS.md`、`docs/DEVELOPMENT.md`、`docs/HANDOFF.md`。
2. 执行并解释：

   ```bash
   git status -sb
   git branch -vv
   git remote -v
   git log -5 --oneline
   ```

3. 核对 `docs/HANDOFF.md` 上次的分支、commit、未提交/未推送变更与实际 Git 状态。
4. 如用户同意同步且工作树干净，再执行 `git fetch origin`、必要时 `git pull --ff-only origin develop`（依据实际分支替换）。如果分叉或存在冲突，停止自动操作并征求用户意见。
5. 只从**下一步任务**恢复开发，不复述、重做上一台电脑已经完成的功能。

### 12.3 每次结束

Codex 必须更新：

- `docs/HANDOFF.md`：本次设备（仅填家里/公司）、日期、分支及 commit、改动列表、构建/测试真实结果、未解决问题、未提交或未推送状态、下一步。
- `docs/DEVELOPMENT.md`：每阶段进度（未开始 / 进行中 / 已完成 / 阻塞）、关键技术选择及依据、当前已知风险。
- `docs/TESTING.md`：真实测试结果，不要填充虚构的通过记录。

**切换电脑前，人工检查、授权后提交并推送代码和这些文档；到另一台电脑拉取再开始新会话。**

### 12.4 安全与保密

- 忽略 Unity 缓存、构建输出、`.env`、签名私钥、令牌、机器私有路径和公司/客户数据。
- 不要把车机设备的敏感唯一标识、个人账号和私人数据写进测试样例。
- 相关功能只用公开系统 API；不需要 root、系统签名或绕过权限。
- Android 通知访问授权具有较大读取范围，产品 UI 应明确解释：本应用只读取必要的音乐会话状态，不上传无关通知；提供前往系统设置撤销权限的入口。

---

## 13. Codex 需要额外交付的文档

1. `README.md`：项目介绍、Unity 版本、目录、如何打开、当前功能、已知限制。
2. `AGENTS.md`：长期项目开发规则和跨设备交接协议。
3. `docs/DEVELOPMENT.md`：阶段进度和技术决策。
4. `docs/HANDOFF.md`：最近一次真实会话交接。
5. `docs/ANDROID_SETUP.md`：Unity Android Build Support、插件编译、Manifest 合并、APK 构建和权限设置。
6. `docs/TESTING.md`：测试矩阵、设备信息、已测试/未测试及结果。
7. 如果开发了 Android 原生插件，必须包含**插件源码**、构建脚本、版本兼容性说明。
8. 若可构建，应提供 APK 文件的实际路径、构建日期/环境与包名；**不能凭空编造 APK**。

---

## 14. 官方 API 参考（遇到兼容问题优先查阅）

- Unity Android 插件：<https://docs.unity3d.com/Manual/PluginsForAndroid.html>
- Unity `AndroidJavaObject`：<https://docs.unity3d.com/ScriptReference/AndroidJavaObject.html>
- Android Package Visibility / `<queries>`：<https://developer.android.com/guide/topics/manifest/queries-element>
- Android `PackageManager`：<https://developer.android.com/reference/android/content/pm/PackageManager>
- Android `MediaSessionManager`：<https://developer.android.com/reference/android/media/session/MediaSessionManager>
- Android `MediaController`：<https://developer.android.com/reference/android/media/session/MediaController>
- Android `NotificationListenerService`：<https://developer.android.com/reference/android/service/notification/NotificationListenerService>
- Android Notification Listener Settings：<https://developer.android.com/reference/android/provider/Settings#ACTION_NOTIFICATION_LISTENER_SETTINGS>

> 高德在 V0.1 中是**已安装的第三方 App**，不是嵌入式地图 SDK。首次启动功能不要申请高德开放平台 Key，也无需自行绘制地图。

---

## 15. 可直接复制给 Codex 的首次启动指令

```text
请阅读项目根目录的《EnvisionOS_Unity_V0.1_Codex完整开发任务书.md》。

这是从零开发的 Envision OS，不要再使用 Open Launcher。主程序必须是 Unity + C#，Android Java/Kotlin 只用于必要的系统插件。

V0.1 仅实现：
1. 横屏深色车机桌面（导航卡片、音乐卡片、设置）。
2. 点击导航卡片启动已安装的高德地图 App，处理未安装和返回 Unity。
3. 通过 Android MediaSession 读取当前音乐及控制播放、暂停、上一首、下一首；按规范处理通知访问授权和无活动播放器。
4. 可以在 Android 设备上运行 APK；如果当前环境无法构建，真实说明阻塞。
5. 维护 AGENTS.md、docs/DEVELOPMENT.md、docs/HANDOFF.md、docs/TESTING.md，实现公司与家里通过 GitHub 的跨设备交接。

请先执行 Phase 0：检查 Git、Unity、Android SDK/NDK/JDK、真实设备情况；然后建立最小 Unity 工程和静态 UI，验证 Editor 与 APK 构建条件。

不要现在就一次性实现所有阶段。每个阶段完成后向我汇报真实产物、变更、测试与下一步。未经我的当次确认，不要 commit/push 或执行破坏性 Git 命令。
```

## 16. 后续版本预留（不要在 V0.1 提前实现）

- **V0.2**：默认 HOME Launcher、应用抽屉、系统导航按键与恢复流程（按 Android 版本及设备限制评估）。
- **V0.3**：根据改装车机和摄像头链路实际资料，独立研究倒车影像；保留原有影像作可靠后备，不在未验证情况下覆盖原车功能。
- **V0.4**：3D 汽车可视化、仪表卡片、OBD-II 数据读取等增强功能。
- **V0.5**：针对实际车机芯片优化启动速度、内存占用、长时间发热、熄屏与断电恢复。

---

**任务边界总结：Unity 制作桌面，Android 插件负责“打开高德 + 控制现有音乐 App”，蓝牙音频由安卓系统连接现有车机；先把两个真实功能跑通，再考虑倒车影像与完整 Launcher。**
