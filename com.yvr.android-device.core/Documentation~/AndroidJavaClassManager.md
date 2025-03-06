# Android Java Class Manager

> [!Note]
>
> Android Java Class 下将简称为 `AJC`
>
> > [!Caution]
> > 这里的 _Android Java Class_ 是一个笼统的概念，并非指 Unity 所封装的 [Android Java Class](https://docs.unity3d.com/ScriptReference/AndroidJavaClass.html) 数据对象
> > 下描述的 `AJC 对象`，实际指的是 `AJCBase` 实例化对象

Android Java Class Manager 一共包含有以下元素：

-   [AJCMgr](./AJCMgr/AJCMgr.md)：用以管理 AJC 对象的各模块管理器，外部调用者应当与此管理器交互，而非直接与 AJC 交互。
-   [AJCBase](./AJCMgr/AJCBase.md)：作为 AJC 的抽象类封装，[AJCMocker](./AJCMgr/AJCMocker.md) 和 [AJCInvoker](./AJCMgr/AJCInvoker.md) 都继承自该类。
    -   [AJCMocker](./AJCMgr/AJCMocker.md)：模拟 AJC 访问的虚拟数据
    -   [AJCInvoker](./AJCMgr/AJCInvoker.md)：用以调用真实的 AJC 对象
-   [AJCElements](./AJCMgr/AJCElements.md)：用以存储 AJC 对象中需要访问的 AJC 中的函数和类名
-   [AJCWrapper](./AJCMgr/AJCWrapper.md)：为 [AJCBase](./AJCMgr/AJCBase.md) 对象提供功能扩展的装饰器，如打印调用开销等
-   [AJCFactory](./AJCMgr/AJCFactory.md)：用以生成 AJC 对象的工厂，根据当前的 Mocking 和 Wrap 状态决定具体生成的 AJC 对象

概括而言，对于 Android Device 的使用者而言，他/她应当直接访问各模块对应的 [AJCMgr](./AJCMgr/AJCMgr.md) 对象，各 [AJCMgr](./AJCMgr/AJCMgr.md) 会负责生成依赖的 AJC 对象，以及对 AJC 对象的管理。AJC 对象的生成依赖于该模块对应的 [AJCMocker](./AJCMgr/AJCMocker.md) 和 [AJCElements](./AJCMgr/AJCElements.md) 类型。

在 `AJCMgr` 生成 AJC 对象时会调用 [AJCFactory](./AJCMgr/AJCFactory.md) 中的 [GetClass](<xref:YVR.AndroidDevice.Core.AJCFactory.GetClass(System.String)>) 方法，该方法会根据当前的 Mocking 和 Wrap 状态决定具体生成的 AJC 对象。

> [!Note]
>
> 对于 Android Device Core Package 的使用者而言，其只需要关心实现继承了 [AJCMgrSingleton](xref:YVR.AndroidDevice.Core.AJCMgrSingleton`3) 的 [AJCMgr](./AJCMgr/AJCMgr.md)，以及使用 [AJCMgr](./AJCMgr/AJCMgr.md)。

> [!Note]
>
> 之后文档中提及的 _模块_，都指的是继承了 [AJCMgrSingleton](xref:YVR.AndroidDevice.Core.AJCMgrSingleton`3) 的 [AJCMgr](./AJCMgr/AJCMgr.md)，如 [DeviceDataMgr](xref:YVR.AndroidDevice.DeviceData.DeviceDataMgr)。
