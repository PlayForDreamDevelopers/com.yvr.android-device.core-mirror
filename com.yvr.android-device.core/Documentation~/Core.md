# Android Device Core

Android Device Core 作为 Android-Device Packages 的基石。其余如 [Android Device Common](../../com.yvr.android-device.common/Documentation~/Common.md) 等 Package 都是基于 Core 构建的。

> [!Note]
> 下简称 `Android Device Core` 为 `Core`

具体介绍将包括：

-   [Android Java Class Manager](./AndroidJavaClassManager.md)：Core 的最核心功能，提供对于 Android Java Class 的封装
-   [Android Java Proxy Common](./AndroidJavaProxyCommon.md): 对 Android 侧回调的封装，以提供统一的回调处理方式
-   [Mocking](./Mocking.md): 关于对于 Android 接口的 Mocking 方式
