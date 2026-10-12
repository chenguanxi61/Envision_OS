# Envision OS — 实际测试记录

日期：2026-10-13，家里；Unity 2022.3.62f3c1。Android 型号/版本、音乐 App/版本：未提供；ADB 没有连接设备。

| 编号 | 验证内容 | 状态与结果 |
|---|---|---|
| T-01 | Editor UI 和 Mock 音乐 | 部分观察，未通过验收：用户截图显示首页，但中文方框；按钮未测试；手工 UI 未验证 |
| T-02 | 未安装高德提示 | 未测试 |
| T-03 | 已安装高德启动 | 未测试 |
| T-04 | 高德返回桌面 | 未测试 |
| T-05 | 未授权音乐通知访问 | 未测试 |
| T-06 | 无活动播放会话 | 未测试 |
| T-07 | 真实歌曲状态 | 未测试 |
| T-08 | 播放/暂停/上下首 | 未测试 |
| T-09 | 会话销毁 | 未测试 |
| T-10 | 撤销权限后恢复 | 未测试 |
| T-11 | 多个会话选择 | 未测试 |
| T-12 | 前后台切换 | 未测试 |
| T-13 | 蓝牙车载音响 | 未测试 |
| T-14 | 导航语音与音乐混音 | 未测试 |
| T-15 | 低性能设备稳定性 | 未测试 |

## 实际执行的检查
- Git 四项检查返回 not a git repository（初始化前）。
- 读取版本文件/EditorInstance，确认 Unity 2022.3.62f3c1；AndroidPlayer 安装目录存在。
- SDK platforms：33、34、35；NDK source.properties：23.1.7779620。
- 内置 java -version 输出 OpenJDK 11.0.14.1；adb devices 成功执行，列表为空。
- 原型脚本导入后出现 Editor 菜单，未观察到 C# 编译错误；没有完整构建证据。
- 首次生成 Launcher 失败：ArgumentNullException，shader 为空。已修正 TMP essentials 导入流程并实际导入资源；修正后未重试生成。
- 用户截图中 Font 为 LiberationSans SDF - Fallback，中文为方框。磁盘存在用户创建的 Noto Dynamic SDF，显示尚未重新验证。
- 没有执行 APK 构建、安装、模拟器或实车测试；没有 APK 或 BuildReport。

## 证据与限制
UI 截图由用户在本聊天提供，未复制到仓库。首次生成错误曾见本机 Editor.log，机器日志不提交。不能将环境存在、代码配置或用户截图当作功能验收通过。
