# Envision OS — 开发进度与技术决策

更新：2026-10-13，家里。

| Phase | 内容 | 状态 | 依据 |
|---|---|---|---|
| 0 | Unity 工程、静态 UI、基线构建 | 进行中，未验收 | 原型截图中文缺字；改为手工 UI；APK 未构建 |
| 1 | 高德 App 启动 | 未开始 | 待用户确认后开发 |
| 2 | MediaSession 桥接 | 未开始 | 待真实播放器和手动授权验证 |
| 3 | 蓝牙联合测试 | 未开始 | 待设备与实车 |
| 4 | 性能调优与交付 | 未开始 | 待真实运行结果 |

## 技术决策
- Unity + C# 主程序；UGUI + TextMeshPro；Android 只做公开系统 API 桥接。
- 使用已有空模板 Unity 2022.3.62f3c1，工程位于 UnityProject/Envision OS。
- 用户手工创建 UI；后续代码只绑定引用和处理状态。保留原型，不继续运行脚本生成 UI。
- 中文采用附 OFL 的 Noto 字体；用户已创建 Dynamic SDF，显示效果待验证。
- 高德使用第三方 App，不嵌地图；音乐使用 MediaSession，不做播放器。
- 跨设备依据 Git 和 HANDOFF，不假设 Codex 聊天同步。

## 风险与阻塞
- 中文缺字未重新验证；首次生成缺 TMP shader，修正导入后尚未重试。
- Android Build Support、SDK 33/34/35、NDK 23.1.7779620、内置 JDK 11.0.14.1 已检测；公司环境未知。
- 无连接 Android 设备，APK 未执行构建。目标 Android 版本、ABI、GPU、性能未知。
- 音乐 App 的媒体会话支持、ROM 权限限制和蓝牙音频需要后续实测。
- 不修改现有倒车影像、车辆控制或音响线路。

## 下一步
仅完成 Phase 0 手工 Launcher、中文绑定、Mock 交互与真实构建测试。详见 HANDOFF.md 和 UI_SETUP.md。
