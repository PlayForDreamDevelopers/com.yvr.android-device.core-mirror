# AJPCommon

[AJPCommon](xref:YVR.AndroidDevice.Core.AJPCommon`1) 是对 [Unity AndroidJavaProxy](https://docs.unity3d.com/ScriptReference/AndroidJavaProxy.html) 的继承实现，以约束可接收的回调数据类型。

下部分将首先阐述，[Unity AndroidJavaProxy](#unity-android-java-proxy) 是如何实现回调效果的，再说明 [AJPCommon](#ajpcommon-implement) 与普通的实现区别是什么。

## Unity Android Java Proxy

[Unity AndroidJavaProxy](https://docs.unity3d.com/ScriptReference/AndroidJavaProxy.html) 的作用是提供对 Java 接口的实现。如下函数，实现了 Java 中的 `android.app.DatePickerDialog$OnDateSetListener` 接口，其中的 `OnDataSet` 则是接口中定义的函数。

```csharp
class DataCallback : AndroidJavaProxy
{
    public DateCallback() : base("android.app.DatePickerDialog$OnDateSetListener") {}
    void onDateSet(int year, int monthOfYear, int dayOfMonth)
    {
        selectedDate = new DateTime(year, monthOfYear + 1, dayOfMonth);
    }
}
```

当调用一个 Java 函数时，可以将 [Unity AndroidJavaProxy](https://docs.unity3d.com/ScriptReference/AndroidJavaProxy.html) 对象传递给 Java 函数，并约定 Java 函数在特定时刻调用该对象，即可实现回调的效果。

如下例子中，调用了 Java 函数 `GetData`，并传递 `DataCallback` 作为回调对象。

```csharp
public void GetDate()
{
    AndroidJavaClass dataClass = new AndroidJavaClass("com.yvr.androiddevice.DateManager");
    dataClass.CallStatic("GetData", new DateCallback());
}
```

在 Java 中函数的实现伪代码如下：

```java
public void GetData(OnDataSetListener callback)
{
    // do something slow

    callback.onDateSet(2019, 1, 1);
}
```

Java 侧的 `OnDataSetListener` 对象即为 C# 侧的 `DateCallback` 对象。

## AJPCommon Implement

AJPCommon 在实现 [Unity Android Java Proxy](https://docs.unity3d.com/ScriptReference/AndroidJavaProxy.html) 时，约束所实现的 Java 接口必须定义回调函数 `onResult`，该函数返回的类型由泛型决定：

```csharp
[SuppressMessage("ReSharper", "UnusedMember.Global")]
public void onResult(T data);
```

> [!Tip]
>
> 如果有需要传递多个形参，通常约定将其使用 Json 序列化为一个字符串，然后传递给 `onResult`，在 C# 端再进行反序列化。

`AJPCommon` 的构造函数如下所示，其中参数中的 `Action<T>` 即为收到 `onResult` 后的回调；`className` 为实现的 Java Interface 名称，该名称将传递给 [Unity Android Java Proxy](https://docs.unity3d.com/ScriptReference/AndroidJavaProxy.html)，`isOneTimeCallback` 为回调*设计的* 被执行次数，如果为 `true`，则在回调后触发后，将释放基类中的 [Unity AndroidJavaProxy.javaInterface](https://docs.unity3d.com/2020.3/Documentation/ScriptReference/AndroidJavaProxy-javaInterface.html)

```csharp
public AJPCommon(Action<T> callback, string className, bool isOneTimeCallback = false)
```

> [!Note]
>
> [AJPCommon](xref:YVR.AndroidDevice.Core.AJPCommon`1) 的设计目的是 **统一和约束**，它是作为 YVR 应用对于 [Unity AndroidJavaProxy.javaInterface](https://docs.unity3d.com/2020.3/Documentation/ScriptReference/AndroidJavaProxy-javaInterface.html) 的统一实现。设计目的是让所有的 YVR 应用都能相同的方式处理 Java 回调数据。

一个典型的，使用 [AJPCommon](xref:YVR.AndroidDevice.Core.AJPCommon`1) 的例子如下：

```csharp
ajcBase.CallJNIStatic(elements.initAndroidBroadcast, new AJPCommon<string>(OnBroadcastMessage, k_JavaProxyName));

// ...
protected void OnBroadcastMessage(string jsonMgs)
{
    this.Info($"[AD][AndroidBroadcast] Broadcast whole json message is {jsonMgs}");
}
```
