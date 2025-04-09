# SMOrganization `Public class`

## Description
SMCode organization class.

## Diagram
```mermaid
  flowchart LR
  classDef interfaceStyle stroke-dasharray: 5 5;
  classDef abstractStyle stroke-width:4px
  subgraph SMCodeSystem
  SMCodeSystem.SMOrganization[[SMOrganization]]
  end
```

## Members
### Properties
#### Public  properties
| Type | Name | Methods |
| --- | --- | --- |
| `bool` | [`ByDefault`](#bydefault)<br>Get or set by-default organization flag. | `get, set` |
| `bool` | [`Empty`](#empty)<br>Return true if organization is empty. | `get` |
| `string` | [`Icon`](#icon)<br>Get or set organization icon path. | `get, set` |
| `int` | [`IdOrganization`](#idorganization)<br>Get or set organization id. | `get, set` |
| `string` | [`Image`](#image)<br>Get or set organization image path. | `get, set` |
| [`SMDictionary`](./smcodesystem-SMDictionary) | [`Parameters`](#parameters)<br>Get or set organization parameters. | `get, private set` |
| `object` | [`Tag`](#tag)<br>Get or set object tag. | `get, set` |
| `string` | [`Text`](#text)<br>Get or set organization description. | `get, set` |
| `string` | [`UidOrganization`](#uidorganization)<br>Get or set organization UID. | `get, set` |

### Methods
#### Public  methods
| Returns | Name |
| --- | --- |
| `void` | [`Assign`](#assign)([`SMOrganization`](smcodesystem-SMOrganization) _OtherInstance)<br>Assign instance properties from another. |
| `void` | [`Clear`](#clear)()<br>Clear item. |
| `bool` | [`FromJSON`](#fromjson)(`string` _JSON)<br>Assign property from JSON serialization. |
| `bool` | [`FromJSON64`](#fromjson64)(`string` _JSON64)<br>Assign property from JSON64 serialization. |
| `int` | [`Load`](#load-12)(`...`)<br>Load organization information by id.<br>            Return 1 if success, 0 if fail or -1 if error. |
| `int` | [`LoadSQL`](#loadsql)(`string` _Sql)<br>Load organization information by sql query <br>            Return 1 if success, 0 if fail or -1 if error. |
| `int` | [`Read`](#read)([`SMDataset`](./smcodesystem-SMDataset) _Dataset)<br>Read item from current record of dataset. Return 1 if success, 0 if fail or -1 if error. |
| `string` | [`ToJSON`](#tojson)()<br>Return JSON serialization of instance. |
| `string` | [`ToJSON64`](#tojson64)()<br>Return JSON64 serialization of instance. |

## Details
### Summary
SMCode organization class.

### Constructors
#### SMOrganization [1/2]
```csharp
public SMOrganization(SMCode _SM)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| [`SMCode`](./smcodesystem-SMCode) | _SM |   |

##### Summary
Class constructor.

#### SMOrganization [2/2]
```csharp
public SMOrganization(SMOrganization _OtherInstance, SMCode _SM)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| [`SMOrganization`](smcodesystem-SMOrganization) | _OtherInstance |   |
| [`SMCode`](./smcodesystem-SMCode) | _SM |   |

##### Summary
Class constructor.

### Methods
#### Assign
```csharp
public void Assign(SMOrganization _OtherInstance)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| [`SMOrganization`](smcodesystem-SMOrganization) | _OtherInstance |   |

##### Summary
Assign instance properties from another.

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

#### Load [1/2]
```csharp
public int Load(int _IdOrganization)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| `int` | _IdOrganization |   |

##### Summary
Load organization information by id.
            Return 1 if success, 0 if fail or -1 if error.

#### Load [2/2]
```csharp
public int Load(string _UidOrganization)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| `string` | _UidOrganization |   |

##### Summary
Load organization information by id.
            Return 1 if success, 0 if fail or -1 if error.

#### LoadSQL
```csharp
public int LoadSQL(string _Sql)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| `string` | _Sql |   |

##### Summary
Load organization information by sql query 
            Return 1 if success, 0 if fail or -1 if error.

#### Read
```csharp
public int Read(SMDataset _Dataset)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| [`SMDataset`](./smcodesystem-SMDataset) | _Dataset |   |

##### Summary
Read item from current record of dataset. Return 1 if success, 0 if fail or -1 if error.

#### ToJSON
```csharp
public string ToJSON()
```
##### Summary
Return JSON serialization of instance.

#### ToJSON64
```csharp
public string ToJSON64()
```
##### Summary
Return JSON64 serialization of instance.

### Properties
#### IdOrganization
```csharp
public int IdOrganization { get; set; }
```
##### Summary
Get or set organization id.

#### UidOrganization
```csharp
public string UidOrganization { get; set; }
```
##### Summary
Get or set organization UID.

#### Text
```csharp
public string Text { get; set; }
```
##### Summary
Get or set organization description.

#### Icon
```csharp
public string Icon { get; set; }
```
##### Summary
Get or set organization icon path.

#### Image
```csharp
public string Image { get; set; }
```
##### Summary
Get or set organization image path.

#### ByDefault
```csharp
public bool ByDefault { get; set; }
```
##### Summary
Get or set by-default organization flag.

#### Parameters
```csharp
public SMDictionary Parameters { get; private set; }
```
##### Summary
Get or set organization parameters.

#### Empty
```csharp
public bool Empty { get; }
```
##### Summary
Return true if organization is empty.

#### Tag
```csharp
public object Tag { get; set; }
```
##### Summary
Get or set object tag.

*Generated with* [*ModularDoc*](https://github.com/hailstorm75/ModularDoc)
