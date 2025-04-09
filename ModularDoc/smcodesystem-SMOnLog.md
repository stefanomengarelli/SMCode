# SMOnLog `Public class`

## Description
SMCode delegate method for log event.

## Diagram
```mermaid
  flowchart LR
  classDef interfaceStyle stroke-dasharray: 5 5;
  classDef abstractStyle stroke-width:4px
  subgraph SMCodeSystem
  SMCodeSystem.SMOnLog[[SMOnLog]]
  end
  subgraph System
System.MulticastDelegate[[MulticastDelegate]]
  end
System.MulticastDelegate --> SMCodeSystem.SMOnLog
```

## Members
### Methods
#### Public  methods
| Returns | Name |
| --- | --- |
| `IAsyncResult` | [`BeginInvoke`](#begininvoke)([`SMLogItem`](./smcodesystem-SMLogItem) _LogItem, `AsyncCallback` callback, `object` object) |
| `bool` | [`EndInvoke`](#endinvoke)(`IAsyncResult` result) |
| `bool` | [`Invoke`](#invoke)([`SMLogItem`](./smcodesystem-SMLogItem) _LogItem) |

## Details
### Summary
SMCode delegate method for log event.

### Inheritance
 - `MulticastDelegate`

### Constructors
#### SMOnLog
```csharp
public SMOnLog(object object, IntPtr method)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| `object` | object |   |
| `IntPtr` | method |   |

### Methods
#### Invoke
```csharp
public virtual bool Invoke(SMLogItem _LogItem)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| [`SMLogItem`](./smcodesystem-SMLogItem) | _LogItem |   |

#### BeginInvoke
```csharp
public virtual IAsyncResult BeginInvoke(SMLogItem _LogItem, AsyncCallback callback, object object)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| [`SMLogItem`](./smcodesystem-SMLogItem) | _LogItem |   |
| `AsyncCallback` | callback |   |
| `object` | object |   |

#### EndInvoke
```csharp
public virtual bool EndInvoke(IAsyncResult result)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| `IAsyncResult` | result |   |

*Generated with* [*ModularDoc*](https://github.com/hailstorm75/ModularDoc)
