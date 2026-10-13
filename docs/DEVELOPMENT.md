# Envision OS — 开发进度与技术决策

更新：2026-10-13，公司。

| Phase | 内容 | 状态 | 依据 |
|---|---|---|---|
| 0 | Unity 工程、手工 UI、Mock 交互、基线构建 | 待重新开始，未验收 | 仓库当前没有 Unity 工程；新工程须在家里创建 |
| 1 | 高德 App 启动 | 未开始 | Phase 0 验收后再继续 |
| 2 | MediaSession 桥接 | 未开始 | 待真实播放器和手动授权验证 |
| 3 | 蓝牙联合测试 | 未开始 | 待目标设备与实车 |
| 4 | 性能调优与交付 | 未开始 | 待真实运行结果 |

## 技术决策

- Unity + C# 为主程序；uGUI + TextMeshPro 制作横屏首页；Android 只用公开系统 API 完成必要桥接。
- 下一工程统一使用 Unity **2022.3.62f3 国际版**，优先从轻量 3D Core 模板创建；版本是目标选择，新工程与构建尚未验证。
- 用户在 Unity Editor 手工搭建 UI。代码负责交互、状态更新与 Inspector 引用绑定，不使用脚本生成 UI。
- 高德作为独立 App 启动；音乐使用 Android MediaSession 控制现有播放器，不开发内嵌地图或音乐播放器。
- 跨设备依据 Git 与 `docs/HANDOFF.md`；不假设本地 Codex 聊天记录同步。

## 风险与阻塞

- 公司电脑上新建工程的 `Packages` 与 `ProjectSettings` 文件呈相同的非文本内容，Unity 报 manifest 无效；清理构建缓存和切换至国际版没有解决。绿盾策略是较强嫌疑，尚未得到 IT 诊断结论。公司电脑不继续作为当前开发环境。
- 最近提交 `4715af6` 后仓库没有 Unity 工程，旧 UI 原型、字体和场景只在 Git 历史中；新工程不应依赖它们已存在。
- 新环境的 Android Build Support、SDK、NDK、JDK、ADB 和 APK 构建均需重新检查；目标设备 Android 版本、ABI、GPU 和性能未知。
- 音乐 App 的会话支持、ROM 权限和蓝牙音频仍须实测。保持原车倒车影像、车辆控制和音响线路不变。

## 下一步

在家里拉取 `main`，确认工作区干净；创建 Unity 2022.3.62f3 工程，验证空工程导入、脚本编译和 uGUI 菜单，再手工搭建 `Launcher`。只记录实际执行的测试，Phase 0 完成后汇报并等待进入 Phase 1 的确认。
