# Mocking

针对 Android 数据，存在以下几种 Mocking 方式的支持：

-   [Data Mocker](Mocking/DataMocker.md)：用于模拟 Android 接口返回的数据，该 Mocker 依赖于 Unity 的 `ScriptObject`，Mock 数据定义在 SO 的实例中（[MockingDataSO](xref:YVR.AndroidDevice.Core.MockingDataSO)）。所有的 Mocking Data 受到 [MockingDataCollectionSO](xref:YVR.AndroidDevice.Core.MockingDataCollectionSO) 的管理，通过该类可以获取 Mocking Data 的值。
-   [Action Mocker](Mocking/ActionMocker.md): 用于模拟 Android 接口的行为，该 Mocker 依赖于 `Action`，使用 Action 标识 Mocking 的行为。[BroadcastMocker](xref:YVR.AndroidDevice.Broadcast.BroadcastMocker) 使用该 Mocker 来模拟广播的发送事件。
-   [Event Mocker](Mocking/EventMocker.md): 用于模拟 Android 设备上的事件（如 Wifi 的连接 / 断开），该 Mocker 依赖于 Unity 的 ``ScriptObject`，可以在 Editor 中通过 `ScriptObject` 实例模拟特定事件的发生。
