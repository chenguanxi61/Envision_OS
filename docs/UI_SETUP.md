# Phase 0 — 手工 UI 搭建

在新工程中退出 Play Mode 后手工创建 `Launcher` 场景。当前仓库没有旧 `DashboardController` 脚本；后续脚本仅做交互和 Inspector 引用绑定。

## 字体
新工程中的中文字体尚未准备。选用具有合法许可的中文字体，保留许可证和源字体文件，在 Editor 中创建 TMP Font Asset，并检查 Dynamic、Multi Atlas Textures 和源字体引用。每个中文 TextMeshProUGUI 指定该资产，先测试“高德地图 / 音乐 / 设置”；不要依赖 LiberationSans 显示中文。

## 层级（TMP 指 TextMeshProUGUI）
```text
Launcher
├─ Main Camera
├─ EventSystem
├─ Canvas (Canvas / CanvasScaler / GraphicRaycaster)
│  ├─ Background (Image)
│  └─ SafeArea (RectTransform)
│     ├─ Header
│     │  ├─ LogoText / ClockText (TMP)
│     │  └─ SettingsButton (Image + Button，子对象 Label TMP)
│     ├─ Content (HorizontalLayoutGroup)
│     │  ├─ NavigationCard (Image + LayoutElement)
│     │  │  ├─ SectionText / TitleText / DescriptionText (TMP)
│     │  │  └─ StartButton (Image + Button，子对象 Label TMP)
│     │  └─ MusicCard (Image + LayoutElement)
│     │     ├─ SourceText / SongTitleText / ArtistText / StateText (TMP)
│     │     └─ Controls (HorizontalLayoutGroup)
│     │        └─ PreviousButton / PlayPauseButton / NextButton (Image + Button + 子 TMP)
│     ├─ StatusText (TMP)
│     ├─ BottomBar (HorizontalLayoutGroup)
│     │  └─ NavigationButton / MusicButton / SettingsButton (Image + Button + 子 TMP)
│     └─ SettingsOverlay (全屏半透明 Image，默认关闭)
│        └─ SettingsPanel (Image)
│           ├─ TitleText / VersionText / TestStatusText (TMP)
│           └─ BluetoothButton / NotificationButton / CloseButton (Image + Button + 子 TMP)
└─ LauncherController (空物体，供后续交互脚本挂载；当前脚本尚未创建)
```

## 建议参数
- Canvas：Screen Space - Overlay；Scaler：Scale With Screen Size，1920×1080，Match 0.5。
- Background/SafeArea 全屏 Stretch；SafeArea 后续适配 Screen.safeArea。
- Header 顶部 Stretch，高 120，左右留 64。
- Content Stretch，左右 64、Top 180、Bottom 240；Spacing 40，Control Child Size / Force Expand 宽高开启。
- 卡片 LayoutElement Flexible Width：导航 1.6、音乐 1。
- StatusText 底部 Stretch，高 60、Pos Y 160；BottomBar 高 100、Pos Y 40、左右 64、三个按钮均分。
- 卡片内用锚点布局，不叠加 ContentSizeFitter。文本 Raycast Target 关闭；按钮背景和遮罩保留，避免点击穿透。
- SettingsOverlay 最后一个子对象；SettingsPanel 居中约 1200×650。
- 背景 #10141B，卡片 #202833，主字 #F1F5FA，次字 #A7B0BE，强调 #9BB9FF。圆角使用自制带 Border Sprite，Image Type=Sliced。
- Game 视图选 16:9，再检查 16:10。保存 Launcher 场景和 NavigationCard/MusicCard Prefab 及 meta。

Phase 0 音乐标注 Mock，按钮只显示演示提示。中文、布局、按钮、Console、APK 构建与安装均需实际验证，当前未验收完成。
