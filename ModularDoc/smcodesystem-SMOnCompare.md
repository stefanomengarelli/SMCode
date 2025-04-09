# SMOnCompare `Public class`

## Description
Delegate method for compare event.

## Diagram
```mermaid
  flowchart LR
  classDef interfaceStyle stroke-dasharray: 5 5;
  classDef abstractStyle stroke-width:4px
  subgraph SMCodeSystem
  SMCodeSystem.SMOnCompare[[SMOnCompare]]
  end
  subgraph System
System.MulticastDelegate[[MulticastDelegate]]
  end
System.MulticastDelegate --> SMCodeSystem.SMOnCompare
```

## Members
### Methods
#### Public  methods
| Returns | Name |
| --- | --- |
| `IAsyncResult` | [`BeginInvoke`](#begininvoke)(`object` _A, `object` _B, `AsyncCallback` callback, `object` object) |
| `int` | [`EndInvoke`](#endinvoke)(`IAsyncResult` result) |
| `int` | [`Invoke`](#invoke)(`object` _A, `object` _B) |

## Details
### Summary
Delegate method for compare event.

### Inheritance
 - `MulticastDelegate`

### Constructors
#### SMOnCompare
```csharp
public SMOnCompare(object object, IntPtr method)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| `object` | object |   |
| `IntPtr` | method |   |

### Methods
#### Invoke
```csharp
public virtual int Invoke(object _A, object _B)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| `object` | _A |   |
| `object` | _B |   |

#### BeginInvoke
```csharp
public virtual IAsyncResult BeginInvoke(object _A, object _B, AsyncCallback callback, object object)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| `object` | _A |   |
| `object` | _B |   |
| `AsyncCallback` | callback |   |
| `object` | object |   |

#### EndInvoke
```csharp
public virtual int EndInvoke(IAsyncResult result)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| `IAsyncResult` | result |   |

*Generated with* [*ModularDoc*](https://github.com/hailstorm75/ModularDoc)
