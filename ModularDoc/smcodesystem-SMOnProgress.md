# SMOnProgress `Public class`

## Description
Delegate method for progress event.

## Diagram
```mermaid
  flowchart LR
  classDef interfaceStyle stroke-dasharray: 5 5;
  classDef abstractStyle stroke-width:4px
  subgraph SMCodeSystem
  SMCodeSystem.SMOnProgress[[SMOnProgress]]
  end
  subgraph System
System.MulticastDelegate[[MulticastDelegate]]
  end
System.MulticastDelegate --> SMCodeSystem.SMOnProgress
```

## Members
### Methods
#### Public  methods
| Returns | Name |
| --- | --- |
| `IAsyncResult` | [`BeginInvoke`](#begininvoke)(`double` _Percent, ref `bool` _Stop, `AsyncCallback` callback, `object` object) |
| `void` | [`EndInvoke`](#endinvoke)(ref `bool` _Stop, `IAsyncResult` result) |
| `void` | [`Invoke`](#invoke)(`double` _Percent, ref `bool` _Stop) |

## Details
### Summary
Delegate method for progress event.

### Inheritance
 - `MulticastDelegate`

### Constructors
#### SMOnProgress
```csharp
public SMOnProgress(object object, IntPtr method)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| `object` | object |   |
| `IntPtr` | method |   |

### Methods
#### Invoke
```csharp
public virtual void Invoke(double _Percent, ref bool _Stop)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| `double` | _Percent |   |
| `ref` `bool` | _Stop |   |

#### BeginInvoke
```csharp
public virtual IAsyncResult BeginInvoke(double _Percent, ref bool _Stop, AsyncCallback callback, object object)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| `double` | _Percent |   |
| `ref` `bool` | _Stop |   |
| `AsyncCallback` | callback |   |
| `object` | object |   |

#### EndInvoke
```csharp
public virtual void EndInvoke(ref bool _Stop, IAsyncResult result)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| `ref` `bool` | _Stop |   |
| `IAsyncResult` | result |   |

*Generated with* [*ModularDoc*](https://github.com/hailstorm75/ModularDoc)
