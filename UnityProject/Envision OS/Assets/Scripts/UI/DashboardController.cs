using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace EnvisionOS
{
    // Phase 0: every action is a local demonstration, including on Android.
    public sealed class DashboardController : MonoBehaviour
    {
        public TMP_FontAsset font;
        private RectTransform safeArea;
        private Rect lastSafeArea;
        private Vector2Int lastSize;
        private TextMeshProUGUI status;
        private TextMeshProUGUI clock;
        private GameObject settings;
        private Sprite rounded;
        private Texture2D roundedTexture;
        private int lastMinute = -1;
        private static readonly Color Background = Hex("10141B");
        private static readonly Color Card = Hex("202833");
        private static readonly Color Accent = Hex("9BB9FF");

        private void Awake()
        {
            Application.targetFrameRate = 30;
            QualitySettings.vSyncCount = 0;
            Screen.orientation = ScreenOrientation.LandscapeLeft;
            CreateRoundedSprite();
            var canvas = new GameObject("Dashboard Canvas", typeof(RectTransform), typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            canvas.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
            var scaler = canvas.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920, 1080);
            scaler.matchWidthOrHeight = .5f;
            Panel(canvas.transform, "Background", Vector2.zero, Vector2.one, Background, false);
            safeArea = Box(canvas.transform, "Safe Area", Vector2.zero, Vector2.one);
            Label(safeArea, "Envision OS", new Vector2(.045f,.85f), new Vector2(.43f,.96f), 50);
            clock = Label(safeArea, "", new Vector2(.70f,.86f), new Vector2(.82f,.95f), 38);
            Button(safeArea, "设置", new Vector2(.84f,.86f), new Vector2(.95f,.95f), OpenSettings);
            var navigation = Panel(safeArea, "Navigation Card", new Vector2(.045f,.24f), new Vector2(.59f,.81f), Card);
            Label(navigation, "NAVIGATION / 导航", new Vector2(.06f,.78f), new Vector2(.94f,.92f), 30, Accent);
            Label(navigation, "高德地图", new Vector2(.06f,.47f), new Vector2(.94f,.70f), 70);
            Label(navigation, "Phase 0 · 静态导航卡片\n尚未连接高德 App", new Vector2(.06f,.27f), new Vector2(.94f,.46f), 30, Hex("A7B0BE"));
            Button(navigation, "开始导航", new Vector2(.06f,.07f), new Vector2(.58f,.24f), () => Message("演示：高德启动将在 Phase 1 实现。"));
            var music = Panel(safeArea, "Music Card", new Vector2(.615f,.24f), new Vector2(.955f,.81f), Card);
            Label(music, "MUSIC / 音乐", new Vector2(.08f,.78f), new Vector2(.92f,.92f), 30, Accent);
            Label(music, "Mock 演示歌曲", new Vector2(.08f,.54f), new Vector2(.92f,.74f), 46);
            Label(music, "演示歌手 · 无真实媒体会话\nPhase 0 / 静态预览", new Vector2(.08f,.32f), new Vector2(.92f,.53f), 28, Hex("A7B0BE"));
            Button(music, "上一首", new Vector2(.07f,.09f), new Vector2(.33f,.26f), () => Message("Mock：上一首按钮已响应，未控制播放器。"), 26);
            Button(music, "播放", new Vector2(.37f,.09f), new Vector2(.63f,.26f), () => Message("Mock：播放按钮已响应，未播放音频。"), 26);
            Button(music, "下一首", new Vector2(.67f,.09f), new Vector2(.93f,.26f), () => Message("Mock：下一首按钮已响应，未控制播放器。"), 26);
            status = Label(safeArea, "Phase 0 · 所有按钮仅作本地演示", new Vector2(.05f,.155f), new Vector2(.95f,.22f), 28, Hex("A7B0BE"));
            Button(safeArea, "导航", new Vector2(.05f,.035f), new Vector2(.32f,.13f), () => Message("导航卡片：等待 Phase 1。"));
            Button(safeArea, "音乐", new Vector2(.365f,.035f), new Vector2(.635f,.13f), () => Message("音乐卡片：Mock 数据，等待 Phase 2。"));
            Button(safeArea, "设置", new Vector2(.68f,.035f), new Vector2(.95f,.13f), OpenSettings);
            settings = Panel(safeArea, "Settings Overlay", new Vector2(.12f,.19f), new Vector2(.88f,.82f), Hex("2B3340")).gameObject;
            Label(settings.transform, "设置 / Phase 0", new Vector2(.06f,.79f), new Vector2(.94f,.94f), 48);
            Label(settings.transform, "Envision OS 0.1.0 · 静态 UI 基线\n蓝牙及通知访问入口尚未连接系统。\n当前没有读取通知、媒体会话或车辆数据。", new Vector2(.06f,.43f), new Vector2(.94f,.78f), 32);
            Button(settings.transform, "蓝牙设置（演示）", new Vector2(.06f,.26f), new Vector2(.48f,.41f), () => Message("演示：尚未打开 Android 蓝牙设置。"), 28);
            Button(settings.transform, "通知访问（演示）", new Vector2(.52f,.26f), new Vector2(.94f,.41f), () => Message("演示：尚未请求通知访问授权。"), 28);
            Button(settings.transform, "返回首页", new Vector2(.30f,.06f), new Vector2(.70f,.21f), () => settings.SetActive(false));
            settings.SetActive(false);
            RefreshSafeArea();
        }

        private void Update()
        {
            if (Screen.safeArea != lastSafeArea || lastSize.x != Screen.width || lastSize.y != Screen.height) RefreshSafeArea();
            var now = DateTime.Now;
            if (lastMinute != now.Minute) { clock.text = now.ToString("HH:mm"); lastMinute = now.Minute; }
            if (Input.GetKeyDown(KeyCode.Escape) && settings.activeSelf) settings.SetActive(false);
        }

        private void RefreshSafeArea()
        {
            lastSafeArea = Screen.safeArea;
            lastSize = new Vector2Int(Screen.width, Screen.height);
            if (Screen.width <= 0 || Screen.height <= 0) return;
            safeArea.anchorMin = lastSafeArea.position / new Vector2(Screen.width, Screen.height);
            safeArea.anchorMax = (lastSafeArea.position + lastSafeArea.size) / new Vector2(Screen.width, Screen.height);
        }
        private void OpenSettings() { settings.SetActive(true); Message("设置入口：Phase 0 本地演示。"); }
        private void Message(string text) { status.text = text; Debug.Log("[Phase0 UI] " + text); }
        private static Color Hex(string value) { ColorUtility.TryParseHtmlString("#" + value, out var color); return color; }
        private static RectTransform Box(Transform parent, string name, Vector2 min, Vector2 max)
        {
            var rect = new GameObject(name, typeof(RectTransform)).GetComponent<RectTransform>();
            rect.SetParent(parent, false); rect.anchorMin = min; rect.anchorMax = max; rect.offsetMin = rect.offsetMax = Vector2.zero;
            return rect;
        }
        private RectTransform Panel(Transform parent, string name, Vector2 min, Vector2 max, Color color, bool round = true)
        {
            var rect = Box(parent, name, min, max);
            var image = rect.gameObject.AddComponent<Image>(); image.color = color;
            if (round) { image.sprite = rounded; image.type = Image.Type.Sliced; }
            return rect;
        }
        private TextMeshProUGUI Label(Transform parent, string text, Vector2 min, Vector2 max, int size, Color? color = null)
        {
            var label = Box(parent, "Label", min, max).gameObject.AddComponent<TextMeshProUGUI>();
            label.font = font; label.text = text; label.fontSize = size; label.color = color ?? Hex("F1F5FA");
            label.alignment = TextAlignmentOptions.MidlineLeft; label.enableWordWrapping = true; label.raycastTarget = false;
            return label;
        }
        private void Button(Transform parent, string text, Vector2 min, Vector2 max, UnityAction action, int size = 32)
        {
            var rect = Panel(parent, text, min, max, Hex("34435B"));
            var button = rect.gameObject.AddComponent<Button>(); button.targetGraphic = rect.GetComponent<Image>(); button.onClick.AddListener(action);
            Label(rect, text, new Vector2(.04f,.04f), new Vector2(.96f,.96f), size).alignment = TextAlignmentOptions.Center;
        }
        private void CreateRoundedSprite()
        {
            const int size = 64; const float radius = 16;
            roundedTexture = new Texture2D(size, size, TextureFormat.RGBA32, false);
            var pixels = new Color[size * size];
            for (int y = 0; y < size; y++) for (int x = 0; x < size; x++)
            {
                float dx = Mathf.Max(radius - x - .5f, x + .5f - (size - radius), 0);
                float dy = Mathf.Max(radius - y - .5f, y + .5f - (size - radius), 0);
                pixels[y * size + x] = new Color(1, 1, 1, Mathf.Clamp01(radius - Mathf.Sqrt(dx * dx + dy * dy)));
            }
            roundedTexture.SetPixels(pixels); roundedTexture.Apply();
            rounded = Sprite.Create(roundedTexture, new Rect(0,0,size,size), new Vector2(.5f,.5f), 100, 0, SpriteMeshType.FullRect, new Vector4(16,16,16,16));
        }
        private void OnDestroy() { if (rounded != null) Destroy(rounded); if (roundedTexture != null) Destroy(roundedTexture); }
    }
}
