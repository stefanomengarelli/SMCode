# SMOrganizations `Public class`

## Description
SMCode organizations collection class.

## Diagram
```mermaid
  flowchart LR
  classDef interfaceStyle stroke-dasharray: 5 5;
  classDef abstractStyle stroke-width:4px
  subgraph SMCodeSystem
  SMCodeSystem.SMOrganizations[[SMOrganizations]]
  end
```

## Members
### Properties
#### Public  properties
| Type | Name | Methods |
| --- | --- | --- |
| `int` | [`Count`](#count)<br>Get users count. | `get` |
| [`SMOrganization`](./smcodesystem-SMOrganization) | [`Item`](#item) | `get` |

### Methods
#### Public  methods
| Returns | Name |
| --- | --- |
| `int` | [`Add`](#add)([`SMOrganization`](./smcodesystem-SMOrganization) _Organization)<br>Add organization item. |
| `void` | [`Assign`](#assign)([`SMOrganizations`](smcodesystem-SMOrganizations) _OtherInstance)<br>Assign instance properties from another. |
| `void` | [`Clear`](#clear)()<br>Clear item. |
| `int` | [`Find`](#find)(`int` _IdOrganization)<br>Find organization by id. |
| [`SMOrganization`](./smcodesystem-SMOrganization) | [`Get`](#get)(`int` _IdOrganization, `bool` _ReturnNewInstanceIfNotFound)<br>Get organization by id. |
| `bool` | [`Has`](#has-12)(`...`)<br>Return true if user has organization with specified id. |
| `string` | [`Keys`](#keys)(`string` _Quote, `string` _Separator)<br>Return keys list as a string with separator and quote specified. |
| `int` | [`Load`](#load)(`bool` _OnlyByDefault)<br>Load organization collection. Return 1 if success, 0 if fail or -1 if error. |

## Details
### Summary
SMCode organizations collection class.

### Constructors
#### SMOrganizations [1/2]
```csharp
public SMOrganizations(SMCode _SM)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| [`SMCode`](./smcodesystem-SMCode) | _SM |   |

##### Summary
Class constructor.

#### SMOrganizations [2/2]
```csharp
public SMOrganizations(SMOrganizations _OtherInstance, SMCode _SM)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| [`SMOrganizations`](smcodesystem-SMOrganizations) | _OtherInstance |   |
| [`SMCode`](./smcodesystem-SMCode) | _SM |   |

##### Summary
Class constructor.

### Methods
#### Add
```csharp
public int Add(SMOrganization _Organization)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| [`SMOrganization`](./smcodesystem-SMOrganization) | _Organization |   |

##### Summary
Add organization item.

#### Assign
```csharp
public void Assign(SMOrganizations _OtherInstance)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| [`SMOrganizations`](smcodesystem-SMOrganizations) | _OtherInstance |   |

##### Summary
Assign instance properties from another.

#### Clear
```csharp
public void Clear()
```
##### Summary
Clear item.

#### Find
```csharp
public int Find(int _IdOrganization)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| `int` | _IdOrganization |   |

##### Summary
Find organization by id.

#### Get
```csharp
public SMOrganization Get(int _IdOrganization, bool _ReturnNewInstanceIfNotFound)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| `int` | _IdOrganization |   |
| `bool` | _ReturnNewInstanceIfNotFound |   |

##### Summary
Get organization by id.

#### Has [1/2]
```csharp
public bool Has(int _IdOrganization)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| `int` | _IdOrganization |   |

##### Summary
Return true if user has organization with specified id.

#### Has [2/2]
```csharp
public bool Has(int[] _IdOrganizations)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| `int``[]` | _IdOrganizations |   |

##### Summary
Return true if user has at least one of organization with specified id.

#### Keys
```csharp
public string Keys(string _Quote, string _Separator)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| `string` | _Quote |   |
| `string` | _Separator |   |

##### Summary
Return keys list as a string with separator and quote specified.

#### Load
```csharp
public int Load(bool _OnlyByDefault)
```
##### Arguments
| Type | Name | Description |
| --- | --- | --- |
| `bool` | _OnlyByDefault |   |

##### Summary
Load organization collection. Return 1 if success, 0 if fail or -1 if error.

### Properties
#### Item
```csharp
public SMOrganization Item { get; }
```

#### Count
```csharp
public int Count { get; }
```
##### Summary
Get users count.

*Generated with* [*ModularDoc*](https://github.com/hailstorm75/ModularDoc)
