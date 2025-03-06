# AJCBase

[AJCBase](xref:YVR.AndroidDevice.Core.AJCBase) 作为 AJC 的抽象类封装。实例化该对象时，必须要提供需要封装的 AJC 的类名。其包含有两个实现：

-   [AJCMocker](xref:YVR.AndroidDevice.Core.AJCMocker)：为特定的 AJC 函数注册模拟数据行为/数据，让调用者期望触发 AJC 函数时，实际触发之前已经注册的模拟行为/数据。具体可见 [Mock](./AJCMocker.md)。
-   [AJCInvoker](xref:YVR.AndroidDevice.Core.AJCInvoker) ：对于 AJC 的真实封装，当调用者触发 AJC 函数时，实际调用的是正式的 Android Java Object 中的函数。

当通过 [AJCFactory.GetClass](<xref:YVR.AndroidDevice.Core.AJCFactory.GetClass(System.String)>) 时获取当前 AJC 对象时。[AJCFactory](./AJCFactory.md) 会根据当前是否处于 Mock 模式，决定返回 [AJCMocker](xref:YVR.AndroidDevice.Core.AJCMocker) 还是 [AJCInvoker](xref:YVR.AndroidDevice.Core.AJCInvoker)。具体见 [AJCFactory](./AJCFactory.md)

```plantuml
@startuml
scale max 1024 width
abstract class AJCBase {
    + {abstract} CallStatic(methodName:string, args:object[]) : T
    + {abstract} CallStatic(methodName:string, args:object[]) : void
    + {abstract} GetStatic(fieldName:string) : T
    + {abstract} SetStatic(fieldName:string, field:T) : void
}

class AJCMocker {
    + MockValue(name:string, value:object) : void
    + MockAction(name:string, action:AndroidAction) : void
    + MockFunc(name:string, func:AndroidFunc) : void
    + AJCMocker(className:string)
    + <<override>> CallStatic(methodName:string, args:object[]) : T
    + <<override>> CallStatic(methodName:string, args:object[]) : void
    + <<override>> GetStatic(fieldName:string) : T
    + <<override>> SetStatic(fieldName:string, field:T) : void
}

class AJCInvoker {
    + AJCInvoker(className:string)
    + <<override>> CallStatic(methodName:string, args:object[]) : T
    + <<override>> CallStatic(methodName:string, args:object[]) : void
    + <<override>> GetStatic(fieldName:string) : TFieldType
    + <<override>> SetStatic(fieldName:string, field:TFieldType) : void
}

AJCBase <|-- AJCInvoker
AJCBase <|-- AJCMocker
AJCBase --> "core" AJCBase

note bottom of AJCInvoker: Instantiate while not in mocking mode.
note bottom of AJCMocker: Instantiate while in mocking mode.
@enduml
```

在实际创建的 [AJCMocker](xref:YVR.AndroidDevice.Core.AJCMocker) 和 [AJCInvoker](xref:YVR.AndroidDevice.Core.AJCInvoker) 外，还可能会包装一层 [AJCWrapper](./AJCWrapper.md)。[AJCWrapperBase](xref:YVR.AndroidDevice.Core.AJCWrapperBase) 与它的一系列派生类使用装饰模式对 [AJCBase](xref:YVR.AndroidDevice.Core.AJCBase) 进行封装，以实现对 AJC 的功能扩展，具体可见 [Wrapper](./AJCWrapper.md)。

```plantuml
scale max 1024 width
@startuml
abstract class AJCWrapperBase {
    # AJCWrapperBase(wrappedClass:AJCBase)
    # {abstract} BeforeCallAJCMethod(methodName:string, args:object[]) : void
    # {abstract} AfterCallAJCMethod(methodName:string, args:object[]) : void
    + <<override>> CallStatic(methodName:string, args:object[]) : T
    + <<override>> CallStatic(methodName:string, args:object[]) : void
    + <<override>> GetStatic(fieldName:string) : T
    + <<override>> SetStatic(fieldName:string, field:T) : void
}

abstract class AJCBase {
    + {abstract} CallStatic(methodName:string, args:object[]) : T
    + {abstract} CallStatic(methodName:string, args:object[]) : void
    + {abstract} GetStatic(fieldName:string) : T
    + {abstract} SetStatic(fieldName:string, field:T) : void
}

AJCBase <|-- AJCWrapperBase
AJCWrapperBase --> "wrappedClass" AJCBase

note left of AJCWrapperBase::wrappedClass
    被包裹的 AJCBase 对象
end note
@enduml
```

