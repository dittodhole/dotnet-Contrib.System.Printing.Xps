![](assets/noun_1230289_cc.png)

# dotnet-Contrib.System.Printing.Xps
> Easily access and use [XPS](https://en.wikipedia.org/wiki/Open_XML_Paper_Specification) printers.

[![Build status](https://img.shields.io/appveyor/ci/dittodhole/dotnet-contrib-system-printing-xps.svg)](https://ci.appveyor.com/project/dittodhole/dotnet-contrib-system-printing-xps)
[![Test status](https://img.shields.io/appveyor/tests/dittodhole/dotnet-contrib-system-printing-xps.svg)](https://ci.appveyor.com/project/dittodhole/dotnet-contrib-system-printing-xps)
[![NuGet Status](https://img.shields.io/nuget/v/Contrib.System.Printing.Xps.svg)](https://www.nuget.org/packages/Contrib.System.Printing.Xps)
[![MyGet Pre Release](https://img.shields.io/myget/dittodhole/vpre/Contrib.System.Printing.Xps.svg)](https://www.myget.org/feed/dittodhole/package/nuget/Contrib.System.Printing.Xps)

## Table of Contents

- [Example](#example)
- [Installing](#installing)
- [API Reference](#api-reference)
- [License](#license)
- [Icon](#icon)

## Example

```csharp
using Contrib.System.Printing.Xps;

var xpsServer = new XpsServer();
var xpsPrinterDefinitions = xpsServer.GetXpsPrinterDefinitions();

foreach (var xpsPrinterDefinition in xpsPrinterDefinition)
{
  var xpsInputBinDefinitions = xpsServer.GetXpsInputBinDefinitions(xpsPrinterDefinition);
}
```

## Installing

### [Releases](https://www.nuget.org/packages/Contrib.System.Printing.Xps)

```powershell
PM> Install-Package Contrib.System.Printing.Xps
```

### [Pre Releases](https://www.myget.org/feed/dittodhole/package/nuget/Contrib.System.Printing.Xps)

```powershell
PM> nuget sources Add "dittodhole" https://www.myget.org/F/dittodhole/api/v3/index.json
PM> Install-Package Contrib.System.Printing.Xps -pre
```

## API Reference

### `Contrib.System.Printing.Xps.IXpsServer`

| Method                      | Return type
|:----------------------------|:-|
| `GetXpsPrinterDefinitions`  | `Contrib.System.Printing.Xps.IXpsPrinterDefinition[]`
| `GetXpsInputBinDefinitions` | `Contrib.System.Printing.Xps.IXpsInputBinDefinition[]`

Default implementation: `Contrib.System.Printing.Xps.XpsServer`

### `Contrib.System.Printing.Xps.IXpsServerEx<TXpsPrinterDefinition, TXpsInputBinDefinition>`

This interface provides full customization of the return types via generics, to easily extend the default layout of `Contrib.System.Printing.Xps.IXpsPrinterDefinition` and `Contrib.System.Printing.Xps.IXpsInputBinDefinition`.

| Method                      | Return type
|:----------------------------|:-|
| `GetXpsPrinterDefinitions`  | `TXpsPrinterDefinition[] : Contrib.System.Printing.Xps.IXpsPrinterDefinition`
| `GetXpsInputBinDefinitions` | `TXpsInputBinDefinition[] : Contrib.System.Printing.Xps.IXpsInputBinDefinition`

Default implementation: `Contrib.System.Printing.Xps.XpsServerEx<TXpsPrinterDefinition, TXpsInputBinDefinition`

---

### `Contrib.System.Printing.Xps.IXpsPrinterDefinition`

This interface provides basic properties of a printer. You can further customize the interface by extending the interface, and creating your own implementation of `Contrib.System.Printing.Xps.IXpsPrinterDefinitionFactoryEx<TXpsPrinterDefinition>`.

| Property      | Type     | Example
|:--------------|:---------|:-|
| `DisplayName` | `string` | Microsoft XPS Document Writer
| `FullName`    | `string` | Microsoft XPS Document Writer
| `PortName`    | `string` | PORTPROMPT:
| `DriverName`  | `string` | Microsoft XPS Document Writer v4

Default implementation: `Contrib.System.Printing.Xps.XpsPrinterDefinitionFactory+XpsPrinterDefinition`

### `Contrib.System.Printing.Xps.IXpsPrinterDefinitionFactory`

| Method   | Return type
|:---------|:-|
| `Create` | `Contrib.System.Printing.Xps.IXpsPrinterDefinition`

Default implementation: `Contrib.System.Printing.Xps.XpsPrinterDefinitionFactory`

### `Contrib.System.Printing.Xps.IXpsPrinterDefinitionFactoryEx<TXpsPrinterDefinition>`

This extended interface is used to provide generic extension and offer full customization of `IXpsServerEx<TXpsPrinterDefinition, TXpsInputBinDefinition>.GetXpsPrinterDefinitions()`.

| Method   | Return type
|:---------|:-|
| `Create` | `TXpsPrinterDefinition : Contrib.System.Printing.Xps.IXpsPrinterDefinition`

Default implementation: `Contrib.System.Printing.Xps.XpsPrinterDefinitionFactory`

#### Example

```csharp
using Contrib.System.Printing.Xps;
using System.Xml.Linq;

public interface ICustomXpsPrinterDefinition : IXpsPrinterDefinition { }

public class CustomXpsPrinterDefinitionFactory : IXpsPrinterDefinitionFactoryEx<ICustomXpsPrinterDefinition>
{
  private class CustomXpsPrinterDefinition : ICustomXpsPrinterDefinition
  {
    public string DisplayName { get; set; }
    public string FullName { get; set; }
    public string PortName { get; set; }
    public string DriverName { get; set; }
  }

  private IXpsPrinterDefinitionFactory XpsPrinterDefinitionFactory { get; } = new XpsPrinterDefinitionFactory();

  public ICustomXpsPrinterDefinition Create(string displayName,
                                            string fullName,
                                            string portName,
                                            string driverName,
                                            XElement printCapabilitiesXElement)
  {
    var xpsPrinterDefinition = this.XpsPrinterDefinitionFactory.Create(displayName,
                                                                       fullName,
                                                                       portName,
                                                                       driverName,
                                                                       printCapabilitiesXElement);
    var customXpsPrinterDefinition = new CustomXpsPrinterDefinition
                                     {
                                       DisplayName = xpsPrinterDefinition.DisplayName,
                                       FullName = xpsPrinterDefinition.FullName,
                                       PortName = xpsPrinterDefinition.PortName,
                                       DriverName = xpsPrinterDefinition.DriverName
                                     };

    // TODO use printCapabilitiesXElement with Contrib.System.Printing.Xps.ExtensionMethods.XElementExtensions to extract needed values

    return customXpsPrinterDefinition;
  }
}
```

---

### `Contrib.System.Printing.Xps.IXpsInputBinDefinition`

This interface provides basic properties of an input bin. You can further customize the interface by extending the interface, and creating your own implementation of `Contrib.System.Printing.Xps.IXpsInputBinDefinitionFactoryEx<TXpsInputBinDefinition>`.

| Property      | Type                    | Example
|:--------------|:------------------------|:-|
| `FeatureName` | `System.Xml.Linq.XName` | {http://schemas.microsoft.com/windows/2003/08/printing/printschemakeywords}JobInputBin
| `Name`        | `System.Xml.Linq.XName` | {http://schemas.microsoft.com/windows/2003/08/printing/printschemakeywords}AutoSelect
| `DisplayName` | `string`                | Automatically Select
| `FeedType`    | `System.Xml.Linq.XName` | {http://schemas.microsoft.com/windows/2003/08/printing/printschemakeywords}Automatic

Default implementation: `Contrib.System.Printing.Xps.XpsInputBinDefinitionFactory+XpsInputBinDefinition`

### `Contrib.System.Printing.Xps.IXpsInputBinDefinitionFactory`

| Method   | Return type
|:---------|:-|
| `Create` | `Contrib.System.Printing.Xps.IXpsInputBinDefinition`

Default implementation: `Contrib.System.Printing.Xps.XpsInputBinDefinitionFactory`

### `Contrib.System.Printing.Xps.IXpsInputBinDefinitionFactoryEx<TXpsInputBinDefinition>`

This extended interface is used to provide generic extension and offer full customization of `IXpsServerEx<TXpsPrinterDefinition, TXpsInputBinDefinition>.GetXpsInputBinDefinitions(TXpsPrinterDefinition)`.

| Method   | Return type
|:---------|:-|
| `Create` | `TXpsInputBinDefinition : Contrib.System.Printing.Xps.IXpsInputBinDefinition`

Default implementation: `Contrib.System.Printing.Xps.XpsInputBinDefinitionFactory`

#### Example

```csharp
using Contrib.System.Printing.Xps;
using System.Xml.Linq;

public interface ICustomXpsInputBinDefinition : IXpsInputBinDefinition { }

public class CustomXpsInputBinDefinitionFactory : IXpsInputBinDefinitionFactoryEx<ICustomXpsInputBinDefinition>
{
  private class CustomXpsInputBinDefinition : ICustomXpsInputBinDefinition
  {
    public XName FeatureName { get; set; }
    public string DisplayName { get; set; }
    public XName Name { get; set; }
    public XName FeedType { get; set; }
  }

  private IXpsInputBinDefinitionFactory XpsInputBinFactory { get; } = new XpsInputBinDefinitionFactory();

  public ICustomXpsInputBinDefinition Create(XElement optionXElement,
                                             XElement printCapabilitiesXElement)
  {
    var xpsInputBinDefinition = this.XpsInputBinFactory.Create(optionXElement,
                                                               printCapabilitiesXElement);
    var customXpsInputBinDefinition = new CustomXpsInputBinDefinition
                                     {
                                       FeatureName = xpsInputBinDefinition.FeatureName,
                                       DisplayName = xpsInputBinDefinition.DisplayName,
                                       Name = xpsInputBinDefinition.Name,
                                       FeedType = xpsInputBinDefinition.FeedType
                                     };

    // TODO use printCapabilitiesXElement with Contrib.System.Printing.Xps.ExtensionMethods.XElementExtensions to extract needed values

    return customXpsInputBinDefinition;
  }
}
```

## License

dotnet-Contrib.System.Printing.Xps is published under [WTFNMFPLv3](https://github.com/dittodhole/WTFNMFPLv3).

## Icon

[Printer](https://thenounproject.com/term/printer/1230289/) by [Wira](https://thenounproject.com/wirawizinda097) from the Noun Project.
