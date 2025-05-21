# ChangeLog

## [1.2.4] - 2025-05-20

### Changed

- 调整代码结构

## [1.2.3] - 2025-04-25

### Added

- 增加ConsumerProxy,JavaObjectConverter,用于安卓和C#对象之间的转化

## [1.2.2] - 2025-03-07

### Added

- 让 Json-Parser 和 YVR UniRx 作为弱依赖

## [1.2.1] - 2025-03-07

### Added

- AJCInvoker 增加直接使用 AndroidJavaObject 的构造函数

### Fixed

- 修复 CallJNIStatic 对返回值为 AndroidJavaObject 的函数调用失败的问题

## [1.2.0] - 2025-03-05

### Changed

- 将对 YVR Tools 的依赖改为弱依赖

## [1.1.3] - 2024-10-18

### Added

- 增加包含构造参数的 java object 构造方式，增加 CallJNI 接口，优化部分命名
- 增加 androidJavaObject 的 Ptr 对象

## [1.1.2] - 2024-07-24

### Added

- 支持非 Singleton 的 AJCMgr

## [1.1.1] - 2024-07-24

### Added

- 增加 API 接口，访问 AJCInvoker 中的 Android Java Object 数据

## [1.1.0] - 2024-04-08

### Added

- 完善 Mocking Data，将 Mocker 分为 DataMocker / ActionMocker / EventMocker 三种方式

### Changed

- 依赖 Json-Parser 0.0.9
- 依赖 UniRx 0.2.1

## [1.0.0] - 2024-03-18

### Changed

- 适配 Utilities 0.15.0

## [0.6.2] - 2023-11-21

### Added

- 增加 AJPCommonMockingIntSO

## [0.6.1] - 2023-11-14

### Fixed

- 修复无法编译至 Android 环境的问题

## [0.6.0] - 2023-11-14

### Changed

- 修改 AJPCommon Mocker 中部分函数和类的命名

## [0.5.2] - 2023-11-10

### Added

- 增加 AJPCommon Mocker

### Changed

- 将 AJPCommonTriggerSO 重命名为 AJPCommonMockingTriggerSO

## [0.5.1] - 2023-11-06

### Fixed

- 修复 AJPCommonSOMocker 无法使用的问题

### Changed

- 将 AJPCommon 注册到 AJPCommonSOMocker 中的操作单独拆分成一个方法，避免在构造函数中直接进行

## [0.5.0] - 2023-11-06

### Added

- 增加 AJPCommon Mocker 机制

### Changed

- 将 AJPCommon 替换为泛型

## [0.4.0] - 2023-11-01

### Added

- 增加 Editor 下自动生成 AJCMgr 相关代码的功能
- 新增 AndroidUtils，包含有 Android JNI 相关的调用封装
- 将 CallXXX 风格函数标记为 Obsolete

### Fixed

- 修复在最新 Utilities 情况下，Mocker 会引起无限死循环的问题

### Changed

- AJCMgr 中的 Elements 对象改为静态成员变量
- 删除 AndroidAction / AndroidFunc，改为使用 System.Action 和 System.Func
- 删除 AJCBase 中无用的构造函数

## [0.3.5] - 2023-09-22

### Added

- AJPCommon 添加如果回调时一次性的话执行 javaInterface.Dispose();

## [0.3.4] - 2023-09-11

### Fixed

- 修复 AndroidActivity 在 Editor 环境下无法运行的问题

## [0.3.3] - 2023-07-12

### Fixed

- 修复 AJCMgrSingleton 线程不安全的问题

## [0.3.2] - 2023-05-26

### Fixed

- 修复 CallJINStatic方法返回值为空导致应用崩溃 的问题

## [0.3.1] - 2023-05-11

### Fixed

- 修复 ID 2 Method 未保存至字典中的问题

## [0.3.0] - 2023-03-30

### Added

- 在 AJCBase 中添加 CallJNIOverload 相关的 api，服务于调用的安卓函数存在重载

## [0.2.0] - 2023-03-17

- 建立 AJCFactory 和 Mocker 等机制，用以创建对于 Android Java Object 的封装
- 增加 CallJNI 相关接口，使用更底层的 JNI 接口直接调用函数，节省 Method2ID 开销。
