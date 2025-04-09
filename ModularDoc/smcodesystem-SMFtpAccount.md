# SMFtpAccount `Public class`

## Description
FTP account class.

## Diagram
```mermaid
  flowchart LR
  classDef interfaceStyle stroke-dasharray: 5 5;
  classDef abstractStyle stroke-width:4px
  subgraph SMCodeSystem
  SMCodeSystem.SMFtpAccount[[SMFtpAccount]]
  end
```

## Members
### Properties
#### Public  properties
| Type | Name | Methods |
| --- | --- | --- |
| `bool` | [`Binary`](#binary)<br>Get or set FTP binary mode flag. | `get, set` |
| `bool` | [`Empty`](#empty)<br>Indicate if FTP account is not defined. | `get` |
| `string` | [`Host`](#host)<br>Get or set FTP host name. | `get, set` |
| `bool` | [`KeepAlive`](#keepalive)<br>Get or set FTP keep alive mode flag. | `get, set` |
| `bool` | [`Passive`](#passive)<br>Get or set FTP passive mode flag. | `get, set` |
| `string` | [`Password`](#password)<br>Get or set FTP password. | `get, set` |
| `int` | [`Port`](#port)<br>Get or set FTP port number. | `get, set` |
| `bool` | [`SSL`](#ssl)<br>Get or set FTP SSL activation flag. | `get, set` |
| `string` | [`User`](#user)<br>Get or set FTP user name. | `get, set` |

### Methods
#### Public  methods
| Returns | Name |
| --- | --- |
| `void` | [`Assign`](#assign)([`SMFtpAccount`](smcodesystem-SMFtpAccount) _FtpAccount)<br>Assign item properties from another. |
| `void` | [`Clear`](#clear)()<br>Clear item. |
| `bool` | [`FromJSON`](#fromjson)(`string` _JSON)<br>Assign property from JSON serialization. |
| `bool` | [`FromJSON64`](#fromjson64)(`string` _JSON64)<br>Assign property from JSON64 serialization. |
| `bool` | [`Load`](#load)(`string` _IniFileName)<br>Load item from ini file. Return true if succeed. |
| `bool` | [`Save`](#save)(`string` _IniFileName)<br>Save item to ini file. Return true if succeed. |
| `string` | [`ToJSON`](#tojson)()<br>Return JSON serialization of instance. |
| `string` | [`ToJSON64`](#tojson64)([`SMCode`](./smcodesystem-SMCode) _SM)<br>Return JSON64 serialization of instance. |
| `string` | [`ToString`](#tostring)()<br>Return string containing instance value. |

## Details
### Summary
FTP account class.

### Constructors
#### SMFtpAccount [1/3]
```csharp
public SMFtpAccount(SMCode _SM)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| [`SMCode`](./smcodesystem-SMCode) | _SM |   |

##### Summary
Class constructor.

#### SMFtpAccount [2/3]
```csharp
public SMFtpAccount(SMFtpAccount _OtherInstance, SMCode _SM)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| [`SMFtpAccount`](smcodesystem-SMFtpAccount) | _OtherInstance |   |
| [`SMCode`](./smcodesystem-SMCode) | _SM |   |

##### Summary
Class constructor.

#### SMFtpAccount [3/3]
```csharp
public SMFtpAccount(string _IniFileName, SMCode _SM)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| `string` | _IniFileName |   |
| [`SMCode`](./smcodesystem-SMCode) | _SM |   |

##### Summary
Class constructor.

### Methods
#### Assign
```csharp
public void Assign(SMFtpAccount _FtpAccount)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| [`SMFtpAccount`](smcodesystem-SMFtpAccount) | _FtpAccount |   |

##### Summary
Assign item properties from another.

#### Clear
```csharp
public void Clear()
```
##### Summary
Clear item.

#### FromJSON
```csharp
public bool FromJSON(string _JSON)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| `string` | _JSON |   |

##### Summary
Assign property from JSON serialization.

#### FromJSON64
```csharp
public bool FromJSON64(string _JSON64)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| `string` | _JSON64 |   |

##### Summary
Assign property from JSON64 serialization.

#### Load
```csharp
public bool Load(string _IniFileName)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| `string` | _IniFileName |   |

##### Summary
Load item from ini file. Return true if succeed.

#### Save
```csharp
public bool Save(string _IniFileName)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| `string` | _IniFileName |   |

##### Summary
Save item to ini file. Return true if succeed.

#### ToJSON
```csharp
public string ToJSON()
```
##### Summary
Return JSON serialization of instance.

#### ToJSON64
```csharp
public string ToJSON64(SMCode _SM)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| [`SMCode`](./smcodesystem-SMCode) | _SM |   |

##### Summary
Return JSON64 serialization of instance.

#### ToString
```csharp
public override string ToString()
```
##### Summary
Return string containing instance value.

### Properties
#### Empty
```csharp
public bool Empty { get; }
```
##### Summary
Indicate if FTP account is not defined.

#### Host
```csharp
public string Host { get; set; }
```
##### Summary
Get or set FTP host name.

#### User
```csharp
public string User { get; set; }
```
##### Summary
Get or set FTP user name.

#### Password
```csharp
public string Password { get; set; }
```
##### Summary
Get or set FTP password.

#### Port
```csharp
public int Port { get; set; }
```
##### Summary
Get or set FTP port number.

#### SSL
```csharp
public bool SSL { get; set; }
```
##### Summary
Get or set FTP SSL activation flag.

#### Passive
```csharp
public bool Passive { get; set; }
```
##### Summary
Get or set FTP passive mode flag.

#### Binary
```csharp
public bool Binary { get; set; }
```
##### Summary
Get or set FTP binary mode flag.

#### KeepAlive
```csharp
public bool KeepAlive { get; set; }
```
##### Summary
Get or set FTP keep alive mode flag.

*Generated with* [*ModularDoc*](https://github.com/hailstorm75/ModularDoc)
