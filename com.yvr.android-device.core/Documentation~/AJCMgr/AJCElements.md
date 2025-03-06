# Elements

`AJCElements` 用以定义一个 [AJCBase](./AJCBase.md) 对象所有需要用到的元素，包括类名、方法名、字段名等。

对于每一个[模块](./AJCMgr.md) 都应该定义各自的 `AJCElements` 类，表示其管理的 [AJCBase](./AJCBase.md) 对象所需要的所有与 AJC 交互的函数/字段/类名称。每一个 `AJCElements` 都必须继承自 [IAJCElements](xref:YVR.AndroidDevice.Core.IAJCElements) 接口 ，其中必须定义 `className` 用以表示 AJC 对象所代表的类。

一个典型的 `AJCElements` 类如下：

```csharp
public class UnityPlayerElements : IAJCElements
{
    public string className => "com.unity3d.player.UnityPlayer";
    public readonly string sendMessage = "UnitySendMessage";
}
```

> [!Note]
>
> Elements 中的函数名，字段名，都应该使用 readonly 进行约束

其类关系如下：

```plantuml
scale max 1024 width
interface IAJCElements {
}

class UnityPlayerElements {
    + className : string <<get>>
    + sendMessage : string = "UnitySendMessage"
}

IAJCElements <|-- UnityPlayerElements
```
