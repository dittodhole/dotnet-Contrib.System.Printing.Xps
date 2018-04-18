![](assets/noun_1230289_cc.png)

# dotnet-Contrib.System.Printing.Xps

| [![Build status](https://img.shields.io/appveyor/ci/dittodhole/dotnet-contrib-system-printing-xps.svg)](https://ci.appveyor.com/project/dittodhole/dotnet-contrib-system-printing-xps) | [![Test status](https://img.shields.io/appveyor/tests/dittodhole/dotnet-contrib-system-printing-xps.svg)](https://ci.appveyor.com/project/dittodhole/dotnet-contrib-system-printing-xps) | [![NuGet Status](https://img.shields.io/nuget/v/Contrib.System.Printing.Xps.svg)](https://www.nuget.org/packages/Contrib.System.Printing.Xps) | [![MyGet Pre Release](https://img.shields.io/myget/dittodhole/vpre/Contrib.System.Printing.Xps.svg)](https://www.myget.org/feed/dittodhole/package/nuget/Contrib.System.Printing.Xps)
|:-|:-|:-|:-|

> Easily access and use [XPS](https://en.wikipedia.org/wiki/Open_XML_Paper_Specification) printers.

## Table of Contents

- [Example](#example)
- [Installing](#installing)
- [API Reference](#api-reference)
- [License](#license)
- [Icon](#icon)

## Example

```csharp
using Contrib.System.Printing.Xps;
using System.Windows.Documents;

IDocumentPaginatorSource documentPaginatorSource;

var xpsServer = new XpsServer();
var xpsPrinterDefinitions = xpsServer.GetXpsPrinterDefinitions();

foreach (var xpsPrinterDefinition in xpsPrinterDefinition)
{
  xpsPrinterDefinition.Print(documentPaginatorSource,
                             printQueue =>
                             {
                               var printTicket = new PrintTicket();

                               return printTicket;
                             });

  var xpsInputBinDefinitions = xpsServer.GetXpsInputBinDefinitions(xpsPrinterDefinition);
  foreach (var xpsInputBinDefinition in xpsInputBinDefinitions)
  {
    xpsPrinterDefinition.Print(documentPaginatorSource,
                               printQueue =>
                               {
                                 var printTicket = xpsInputBinDefinition.GetPrintTicket();

                                 return printTicket;
                               });
  }
}
```

## Installing

### [Releases](https://www.nuget.org/packages/Contrib.System.Printing.Xps)

```powershell
PM> Install-Package -Id Contrib.System.Printing.Xps
```

### [Pre Releases](https://www.myget.org/feed/dittodhole/package/nuget/Contrib.System.Printing.Xps)

```powershell
PM> Install-Package -Id Contrib.System.Printing.Xps -Source https://www.myget.org/F/dittodhole/api/v3/index.json -IncludePrerelease
```

## API Reference

- [IXpsInputBinDefinition](#T-Contrib-System-Printing-Xps-IXpsInputBinDefinition 'Contrib.System.Printing.Xps.IXpsInputBinDefinition')
  - [DisplayName](#P-Contrib-System-Printing-Xps-IXpsInputBinDefinition-DisplayName 'Contrib.System.Printing.Xps.IXpsInputBinDefinition.DisplayName')
  - [FeatureName](#P-Contrib-System-Printing-Xps-IXpsInputBinDefinition-FeatureName 'Contrib.System.Printing.Xps.IXpsInputBinDefinition.FeatureName')
  - [FeedType](#P-Contrib-System-Printing-Xps-IXpsInputBinDefinition-FeedType 'Contrib.System.Printing.Xps.IXpsInputBinDefinition.FeedType')
  - [Name](#P-Contrib-System-Printing-Xps-IXpsInputBinDefinition-Name 'Contrib.System.Printing.Xps.IXpsInputBinDefinition.Name')
- [IXpsInputBinDefinitionFactory\`1](#T-Contrib-System-Printing-Xps-IXpsInputBinDefinitionFactory`1 'Contrib.System.Printing.Xps.IXpsInputBinDefinitionFactory`1')
  - [Create(optionXElement,printCapabilitiesXElement)](#M-Contrib-System-Printing-Xps-IXpsInputBinDefinitionFactory`1-Create-System-Xml-Linq-XElement,System-Xml-Linq-XElement- 'Contrib.System.Printing.Xps.IXpsInputBinDefinitionFactory`1.Create(System.Xml.Linq.XElement,System.Xml.Linq.XElement)')
- [IXpsPrinterDefinition](#T-Contrib-System-Printing-Xps-IXpsPrinterDefinition 'Contrib.System.Printing.Xps.IXpsPrinterDefinition')
  - [DisplayName](#P-Contrib-System-Printing-Xps-IXpsPrinterDefinition-DisplayName 'Contrib.System.Printing.Xps.IXpsPrinterDefinition.DisplayName')
  - [DriverName](#P-Contrib-System-Printing-Xps-IXpsPrinterDefinition-DriverName 'Contrib.System.Printing.Xps.IXpsPrinterDefinition.DriverName')
  - [FullName](#P-Contrib-System-Printing-Xps-IXpsPrinterDefinition-FullName 'Contrib.System.Printing.Xps.IXpsPrinterDefinition.FullName')
  - [PortName](#P-Contrib-System-Printing-Xps-IXpsPrinterDefinition-PortName 'Contrib.System.Printing.Xps.IXpsPrinterDefinition.PortName')
- [IXpsPrinterDefinitionFactory\`1](#T-Contrib-System-Printing-Xps-IXpsPrinterDefinitionFactory`1 'Contrib.System.Printing.Xps.IXpsPrinterDefinitionFactory`1')
  - [Create(displayName,fullName,portName,driverName,printCapabilitiesXElement)](#M-Contrib-System-Printing-Xps-IXpsPrinterDefinitionFactory`1-Create-System-String,System-String,System-String,System-String,System-Xml-Linq-XElement- 'Contrib.System.Printing.Xps.IXpsPrinterDefinitionFactory`1.Create(System.String,System.String,System.String,System.String,System.Xml.Linq.XElement)')
- [IXpsServer](#T-Contrib-System-Printing-Xps-IXpsServer 'Contrib.System.Printing.Xps.IXpsServer')
  - [GetXpsInputBinDefinitions(xpsPrinterDefinition)](#M-Contrib-System-Printing-Xps-IXpsServer-GetXpsInputBinDefinitions-Contrib-System-Printing-Xps-IXpsPrinterDefinition- 'Contrib.System.Printing.Xps.IXpsServer.GetXpsInputBinDefinitions(Contrib.System.Printing.Xps.IXpsPrinterDefinition)')
  - [GetXpsPrinterDefinitions()](#M-Contrib-System-Printing-Xps-IXpsServer-GetXpsPrinterDefinitions 'Contrib.System.Printing.Xps.IXpsServer.GetXpsPrinterDefinitions')
- [IXpsServerEx\`2](#T-Contrib-System-Printing-Xps-IXpsServerEx`2 'Contrib.System.Printing.Xps.IXpsServerEx`2')
  - [GetXpsInputBinDefinitions(xpsPrinterDefinition)](#M-Contrib-System-Printing-Xps-IXpsServerEx`2-GetXpsInputBinDefinitions-`0- 'Contrib.System.Printing.Xps.IXpsServerEx`2.GetXpsInputBinDefinitions(`0)')
  - [GetXpsPrinterDefinitions()](#M-Contrib-System-Printing-Xps-IXpsServerEx`2-GetXpsPrinterDefinitions 'Contrib.System.Printing.Xps.IXpsServerEx`2.GetXpsPrinterDefinitions')
- [PrintQueueCollectionExtensions](#T-Contrib-System-Printing-Xps-ExtensionMethods-PrintQueueCollectionExtensions 'Contrib.System.Printing.Xps.ExtensionMethods.PrintQueueCollectionExtensions')
  - [FindPrintQueue(printQueueCollection,xpsPrinterDefinition)](#M-Contrib-System-Printing-Xps-ExtensionMethods-PrintQueueCollectionExtensions-FindPrintQueue-System-Printing-PrintQueueCollection,Contrib-System-Printing-Xps-IXpsPrinterDefinition- 'Contrib.System.Printing.Xps.ExtensionMethods.PrintQueueCollectionExtensions.FindPrintQueue(System.Printing.PrintQueueCollection,Contrib.System.Printing.Xps.IXpsPrinterDefinition)')
- [PrintQueueExtensions](#T-Contrib-System-Printing-Xps-ExtensionMethods-PrintQueueExtensions 'Contrib.System.Printing.Xps.ExtensionMethods.PrintQueueExtensions')
  - [GetPrintCapabilitiesAsXDocument(printQueue)](#M-Contrib-System-Printing-Xps-ExtensionMethods-PrintQueueExtensions-GetPrintCapabilitiesAsXDocument-System-Printing-PrintQueue- 'Contrib.System.Printing.Xps.ExtensionMethods.PrintQueueExtensions.GetPrintCapabilitiesAsXDocument(System.Printing.PrintQueue)')
  - [GetPrintCapabilitiesAsXDocument(printQueue,printTicket)](#M-Contrib-System-Printing-Xps-ExtensionMethods-PrintQueueExtensions-GetPrintCapabilitiesAsXDocument-System-Printing-PrintQueue,System-Printing-PrintTicket- 'Contrib.System.Printing.Xps.ExtensionMethods.PrintQueueExtensions.GetPrintCapabilitiesAsXDocument(System.Printing.PrintQueue,System.Printing.PrintTicket)')
- [PrintServerExtensions](#T-Contrib-System-Printing-Xps-ExtensionMethods-PrintServerExtensions 'Contrib.System.Printing.Xps.ExtensionMethods.PrintServerExtensions')
  - [GetLocalAndRemotePrintQueues(printServer)](#M-Contrib-System-Printing-Xps-ExtensionMethods-PrintServerExtensions-GetLocalAndRemotePrintQueues-System-Printing-PrintServer- 'Contrib.System.Printing.Xps.ExtensionMethods.PrintServerExtensions.GetLocalAndRemotePrintQueues(System.Printing.PrintServer)')
- [PrintTicketFactory](#T-Contrib-System-Printing-Xps-ExtensionMethods-XpsPrinterDefinitionExtensions-PrintTicketFactory 'Contrib.System.Printing.Xps.ExtensionMethods.XpsPrinterDefinitionExtensions.PrintTicketFactory')
- [UnitConverter](#T-Contrib-System-Printing-Xps-UnitConverter 'Contrib.System.Printing.Xps.UnitConverter')
  - [LengthValueFromDIPToMicron(dipValue)](#M-Contrib-System-Printing-Xps-UnitConverter-LengthValueFromDIPToMicron-System-Double- 'Contrib.System.Printing.Xps.UnitConverter.LengthValueFromDIPToMicron(System.Double)')
  - [LengthValueFromMicronToDIP(micronValue)](#M-Contrib-System-Printing-Xps-UnitConverter-LengthValueFromMicronToDIP-System-Int32- 'Contrib.System.Printing.Xps.UnitConverter.LengthValueFromMicronToDIP(System.Int32)')
- [XElementExtensions](#T-Contrib-System-Printing-Xps-ExtensionMethods-XElementExtensions 'Contrib.System.Printing.Xps.ExtensionMethods.XElementExtensions')
  - [EnsurePrefixRegistrationOfNamespace(xelement,name)](#M-Contrib-System-Printing-Xps-ExtensionMethods-XElementExtensions-EnsurePrefixRegistrationOfNamespace-System-Xml-Linq-XElement,System-Xml-Linq-XName- 'Contrib.System.Printing.Xps.ExtensionMethods.XElementExtensions.EnsurePrefixRegistrationOfNamespace(System.Xml.Linq.XElement,System.Xml.Linq.XName)')
  - [FindElementByNameAttribute(xelement,name)](#M-Contrib-System-Printing-Xps-ExtensionMethods-XElementExtensions-FindElementByNameAttribute-System-Xml-Linq-XElement,System-Xml-Linq-XName- 'Contrib.System.Printing.Xps.ExtensionMethods.XElementExtensions.FindElementByNameAttribute(System.Xml.Linq.XElement,System.Xml.Linq.XName)')
  - [FindUnusedPrefixForNamespace(xelement)](#M-Contrib-System-Printing-Xps-ExtensionMethods-XElementExtensions-FindUnusedPrefixForNamespace-System-Xml-Linq-XElement- 'Contrib.System.Printing.Xps.ExtensionMethods.XElementExtensions.FindUnusedPrefixForNamespace(System.Xml.Linq.XElement)')
  - [GetNameFromNameAttribute(xelement)](#M-Contrib-System-Printing-Xps-ExtensionMethods-XElementExtensions-GetNameFromNameAttribute-System-Xml-Linq-XElement- 'Contrib.System.Printing.Xps.ExtensionMethods.XElementExtensions.GetNameFromNameAttribute(System.Xml.Linq.XElement)')
  - [GetValue(xelement)](#M-Contrib-System-Printing-Xps-ExtensionMethods-XElementExtensions-GetValue-System-Xml-Linq-XElement- 'Contrib.System.Printing.Xps.ExtensionMethods.XElementExtensions.GetValue(System.Xml.Linq.XElement)')
  - [GetValueFromValueElement(xelement)](#M-Contrib-System-Printing-Xps-ExtensionMethods-XElementExtensions-GetValueFromValueElement-System-Xml-Linq-XElement- 'Contrib.System.Printing.Xps.ExtensionMethods.XElementExtensions.GetValueFromValueElement(System.Xml.Linq.XElement)')
  - [GetXName(xelement,str)](#M-Contrib-System-Printing-Xps-ExtensionMethods-XElementExtensions-GetXName-System-Xml-Linq-XElement,System-String- 'Contrib.System.Printing.Xps.ExtensionMethods.XElementExtensions.GetXName(System.Xml.Linq.XElement,System.String)')
- [XpsInputBinDefinition](#T-Contrib-System-Printing-Xps-XpsInputBinDefinitionFactory-XpsInputBinDefinition 'Contrib.System.Printing.Xps.XpsInputBinDefinitionFactory.XpsInputBinDefinition')
  - [DisplayName](#P-Contrib-System-Printing-Xps-XpsInputBinDefinitionFactory-XpsInputBinDefinition-DisplayName 'Contrib.System.Printing.Xps.XpsInputBinDefinitionFactory.XpsInputBinDefinition.DisplayName')
  - [FeatureName](#P-Contrib-System-Printing-Xps-XpsInputBinDefinitionFactory-XpsInputBinDefinition-FeatureName 'Contrib.System.Printing.Xps.XpsInputBinDefinitionFactory.XpsInputBinDefinition.FeatureName')
  - [FeedType](#P-Contrib-System-Printing-Xps-XpsInputBinDefinitionFactory-XpsInputBinDefinition-FeedType 'Contrib.System.Printing.Xps.XpsInputBinDefinitionFactory.XpsInputBinDefinition.FeedType')
  - [Name](#P-Contrib-System-Printing-Xps-XpsInputBinDefinitionFactory-XpsInputBinDefinition-Name 'Contrib.System.Printing.Xps.XpsInputBinDefinitionFactory.XpsInputBinDefinition.Name')
- [XpsInputBinDefinitionExtensions](#T-Contrib-System-Printing-Xps-ExtensionMethods-XpsInputBinDefinitionExtensions 'Contrib.System.Printing.Xps.ExtensionMethods.XpsInputBinDefinitionExtensions')
  - [GetPrintTicket(xpsInputBinDefinition)](#M-Contrib-System-Printing-Xps-ExtensionMethods-XpsInputBinDefinitionExtensions-GetPrintTicket-Contrib-System-Printing-Xps-IXpsInputBinDefinition- 'Contrib.System.Printing.Xps.ExtensionMethods.XpsInputBinDefinitionExtensions.GetPrintTicket(Contrib.System.Printing.Xps.IXpsInputBinDefinition)')
- [XpsInputBinDefinitionFactory](#T-Contrib-System-Printing-Xps-XpsInputBinDefinitionFactory 'Contrib.System.Printing.Xps.XpsInputBinDefinitionFactory')
  - [#ctor()](#M-Contrib-System-Printing-Xps-XpsInputBinDefinitionFactory-#ctor 'Contrib.System.Printing.Xps.XpsInputBinDefinitionFactory.#ctor')
  - [Create()](#M-Contrib-System-Printing-Xps-XpsInputBinDefinitionFactory-Create-System-Xml-Linq-XElement,System-Xml-Linq-XElement- 'Contrib.System.Printing.Xps.XpsInputBinDefinitionFactory.Create(System.Xml.Linq.XElement,System.Xml.Linq.XElement)')
- [XpsPrinterDefinition](#T-Contrib-System-Printing-Xps-XpsPrinterDefinitionFactory-XpsPrinterDefinition 'Contrib.System.Printing.Xps.XpsPrinterDefinitionFactory.XpsPrinterDefinition')
  - [DisplayName](#P-Contrib-System-Printing-Xps-XpsPrinterDefinitionFactory-XpsPrinterDefinition-DisplayName 'Contrib.System.Printing.Xps.XpsPrinterDefinitionFactory.XpsPrinterDefinition.DisplayName')
  - [DriverName](#P-Contrib-System-Printing-Xps-XpsPrinterDefinitionFactory-XpsPrinterDefinition-DriverName 'Contrib.System.Printing.Xps.XpsPrinterDefinitionFactory.XpsPrinterDefinition.DriverName')
  - [FullName](#P-Contrib-System-Printing-Xps-XpsPrinterDefinitionFactory-XpsPrinterDefinition-FullName 'Contrib.System.Printing.Xps.XpsPrinterDefinitionFactory.XpsPrinterDefinition.FullName')
  - [PortName](#P-Contrib-System-Printing-Xps-XpsPrinterDefinitionFactory-XpsPrinterDefinition-PortName 'Contrib.System.Printing.Xps.XpsPrinterDefinitionFactory.XpsPrinterDefinition.PortName')
- [XpsPrinterDefinitionExtensions](#T-Contrib-System-Printing-Xps-ExtensionMethods-XpsPrinterDefinitionExtensions 'Contrib.System.Printing.Xps.ExtensionMethods.XpsPrinterDefinitionExtensions')
  - [Print(xpsPrinterDefinition,documentPaginatorSource,printTicketFactory)](#M-Contrib-System-Printing-Xps-ExtensionMethods-XpsPrinterDefinitionExtensions-Print-Contrib-System-Printing-Xps-IXpsPrinterDefinition,System-Windows-Documents-IDocumentPaginatorSource,Contrib-System-Printing-Xps-ExtensionMethods-XpsPrinterDefinitionExtensions-PrintTicketFactory- 'Contrib.System.Printing.Xps.ExtensionMethods.XpsPrinterDefinitionExtensions.Print(Contrib.System.Printing.Xps.IXpsPrinterDefinition,System.Windows.Documents.IDocumentPaginatorSource,Contrib.System.Printing.Xps.ExtensionMethods.XpsPrinterDefinitionExtensions.PrintTicketFactory)')
- [XpsPrinterDefinitionFactory](#T-Contrib-System-Printing-Xps-XpsPrinterDefinitionFactory 'Contrib.System.Printing.Xps.XpsPrinterDefinitionFactory')
  - [#ctor()](#M-Contrib-System-Printing-Xps-XpsPrinterDefinitionFactory-#ctor 'Contrib.System.Printing.Xps.XpsPrinterDefinitionFactory.#ctor')
  - [Create()](#M-Contrib-System-Printing-Xps-XpsPrinterDefinitionFactory-Create-System-String,System-String,System-String,System-String,System-Xml-Linq-XElement- 'Contrib.System.Printing.Xps.XpsPrinterDefinitionFactory.Create(System.String,System.String,System.String,System.String,System.Xml.Linq.XElement)')
- [XpsServer](#T-Contrib-System-Printing-Xps-XpsServer 'Contrib.System.Printing.Xps.XpsServer')
  - [#ctor()](#M-Contrib-System-Printing-Xps-XpsServer-#ctor 'Contrib.System.Printing.Xps.XpsServer.#ctor')
  - [#ctor(xpsPrinterDefinitionFactory,xpsInputBinDefinitionFactory)](#M-Contrib-System-Printing-Xps-XpsServer-#ctor-Contrib-System-Printing-Xps-IXpsPrinterDefinitionFactory{Contrib-System-Printing-Xps-IXpsPrinterDefinition},Contrib-System-Printing-Xps-IXpsInputBinDefinitionFactory{Contrib-System-Printing-Xps-IXpsInputBinDefinition}- 'Contrib.System.Printing.Xps.XpsServer.#ctor(Contrib.System.Printing.Xps.IXpsPrinterDefinitionFactory{Contrib.System.Printing.Xps.IXpsPrinterDefinition},Contrib.System.Printing.Xps.IXpsInputBinDefinitionFactory{Contrib.System.Printing.Xps.IXpsInputBinDefinition})')
  - [DisplayNameXName](#P-Contrib-System-Printing-Xps-XpsServer-DisplayNameXName 'Contrib.System.Printing.Xps.XpsServer.DisplayNameXName')
  - [DocumentInputBinXName](#P-Contrib-System-Printing-Xps-XpsServer-DocumentInputBinXName 'Contrib.System.Printing.Xps.XpsServer.DocumentInputBinXName')
  - [FeatureElementXName](#P-Contrib-System-Printing-Xps-XpsServer-FeatureElementXName 'Contrib.System.Printing.Xps.XpsServer.FeatureElementXName')
  - [FeedTypeXName](#P-Contrib-System-Printing-Xps-XpsServer-FeedTypeXName 'Contrib.System.Printing.Xps.XpsServer.FeedTypeXName')
  - [ImageableSizeHeightXName](#P-Contrib-System-Printing-Xps-XpsServer-ImageableSizeHeightXName 'Contrib.System.Printing.Xps.XpsServer.ImageableSizeHeightXName')
  - [ImageableSizeWidthXName](#P-Contrib-System-Printing-Xps-XpsServer-ImageableSizeWidthXName 'Contrib.System.Printing.Xps.XpsServer.ImageableSizeWidthXName')
  - [IntegerTypeXName](#P-Contrib-System-Printing-Xps-XpsServer-IntegerTypeXName 'Contrib.System.Printing.Xps.XpsServer.IntegerTypeXName')
  - [JobInputBinXName](#P-Contrib-System-Printing-Xps-XpsServer-JobInputBinXName 'Contrib.System.Printing.Xps.XpsServer.JobInputBinXName')
  - [MediaSizeHeightXName](#P-Contrib-System-Printing-Xps-XpsServer-MediaSizeHeightXName 'Contrib.System.Printing.Xps.XpsServer.MediaSizeHeightXName')
  - [MediaSizeWidthXName](#P-Contrib-System-Printing-Xps-XpsServer-MediaSizeWidthXName 'Contrib.System.Printing.Xps.XpsServer.MediaSizeWidthXName')
  - [NameAttributeXName](#P-Contrib-System-Printing-Xps-XpsServer-NameAttributeXName 'Contrib.System.Printing.Xps.XpsServer.NameAttributeXName')
  - [OptionElementXName](#P-Contrib-System-Printing-Xps-XpsServer-OptionElementXName 'Contrib.System.Printing.Xps.XpsServer.OptionElementXName')
  - [PageImageableSizeXName](#P-Contrib-System-Printing-Xps-XpsServer-PageImageableSizeXName 'Contrib.System.Printing.Xps.XpsServer.PageImageableSizeXName')
  - [PageInputBinXName](#P-Contrib-System-Printing-Xps-XpsServer-PageInputBinXName 'Contrib.System.Printing.Xps.XpsServer.PageInputBinXName')
  - [PageMediaSizeXName](#P-Contrib-System-Printing-Xps-XpsServer-PageMediaSizeXName 'Contrib.System.Printing.Xps.XpsServer.PageMediaSizeXName')
  - [PrintCapabilitiesXName](#P-Contrib-System-Printing-Xps-XpsServer-PrintCapabilitiesXName 'Contrib.System.Printing.Xps.XpsServer.PrintCapabilitiesXName')
  - [PrinterSchemaFrameworkXNamespace](#P-Contrib-System-Printing-Xps-XpsServer-PrinterSchemaFrameworkXNamespace 'Contrib.System.Printing.Xps.XpsServer.PrinterSchemaFrameworkXNamespace')
  - [PrinterSchemaKeywordsXNamespace](#P-Contrib-System-Printing-Xps-XpsServer-PrinterSchemaKeywordsXNamespace 'Contrib.System.Printing.Xps.XpsServer.PrinterSchemaKeywordsXNamespace')
  - [PropertyElementXName](#P-Contrib-System-Printing-Xps-XpsServer-PropertyElementXName 'Contrib.System.Printing.Xps.XpsServer.PropertyElementXName')
  - [QNameTypeXName](#P-Contrib-System-Printing-Xps-XpsServer-QNameTypeXName 'Contrib.System.Printing.Xps.XpsServer.QNameTypeXName')
  - [ScoredPropertyElementXName](#P-Contrib-System-Printing-Xps-XpsServer-ScoredPropertyElementXName 'Contrib.System.Printing.Xps.XpsServer.ScoredPropertyElementXName')
  - [StringTypeXName](#P-Contrib-System-Printing-Xps-XpsServer-StringTypeXName 'Contrib.System.Printing.Xps.XpsServer.StringTypeXName')
  - [TypeXName](#P-Contrib-System-Printing-Xps-XpsServer-TypeXName 'Contrib.System.Printing.Xps.XpsServer.TypeXName')
  - [ValueElementXName](#P-Contrib-System-Printing-Xps-XpsServer-ValueElementXName 'Contrib.System.Printing.Xps.XpsServer.ValueElementXName')
  - [XmlSchemaInstanceXNamespace](#P-Contrib-System-Printing-Xps-XpsServer-XmlSchemaInstanceXNamespace 'Contrib.System.Printing.Xps.XpsServer.XmlSchemaInstanceXNamespace')
  - [XmlSchemaXNamespace](#P-Contrib-System-Printing-Xps-XpsServer-XmlSchemaXNamespace 'Contrib.System.Printing.Xps.XpsServer.XmlSchemaXNamespace')
- [XpsServerEx\`2](#T-Contrib-System-Printing-Xps-XpsServerEx`2 'Contrib.System.Printing.Xps.XpsServerEx`2')
  - [#ctor(xpsPrinterDefinitionFactory,xpsInputBinDefinitionFactory)](#M-Contrib-System-Printing-Xps-XpsServerEx`2-#ctor-Contrib-System-Printing-Xps-IXpsPrinterDefinitionFactory{`0},Contrib-System-Printing-Xps-IXpsInputBinDefinitionFactory{`1}- 'Contrib.System.Printing.Xps.XpsServerEx`2.#ctor(Contrib.System.Printing.Xps.IXpsPrinterDefinitionFactory{`0},Contrib.System.Printing.Xps.IXpsInputBinDefinitionFactory{`1})')
  - [GetPrintTicketImpl(featureXName,inputBinXName)](#M-Contrib-System-Printing-Xps-XpsServerEx`2-GetPrintTicketImpl-System-Xml-Linq-XName,System-Xml-Linq-XName- 'Contrib.System.Printing.Xps.XpsServerEx`2.GetPrintTicketImpl(System.Xml.Linq.XName,System.Xml.Linq.XName)')
  - [GetXpsInputBinDefinitions()](#M-Contrib-System-Printing-Xps-XpsServerEx`2-GetXpsInputBinDefinitions-`0- 'Contrib.System.Printing.Xps.XpsServerEx`2.GetXpsInputBinDefinitions(`0)')
  - [GetXpsPrinterDefinitions()](#M-Contrib-System-Printing-Xps-XpsServerEx`2-GetXpsPrinterDefinitions 'Contrib.System.Printing.Xps.XpsServerEx`2.GetXpsPrinterDefinitions')

<a name='T-Contrib-System-Printing-Xps-IXpsInputBinDefinition'></a>
## IXpsInputBinDefinition [#](#T-Contrib-System-Printing-Xps-IXpsInputBinDefinition 'Go To Here') [=](#contents 'Back To Contents')

##### Namespace

Contrib.System.Printing.Xps

##### Summary

Holds information of an input bin.

##### See Also

- [Contrib.System.Printing.Xps.XpsInputBinDefinitionFactory.XpsInputBinDefinition](#T-Contrib-System-Printing-Xps-XpsInputBinDefinitionFactory-XpsInputBinDefinition 'Contrib.System.Printing.Xps.XpsInputBinDefinitionFactory.XpsInputBinDefinition')

<a name='P-Contrib-System-Printing-Xps-IXpsInputBinDefinition-DisplayName'></a>
### DisplayName `property` [#](#P-Contrib-System-Printing-Xps-IXpsInputBinDefinition-DisplayName 'Go To Here') [=](#contents 'Back To Contents')

##### Summary

The display name of the input bin.

##### Example

Automatically Select

<a name='P-Contrib-System-Printing-Xps-IXpsInputBinDefinition-FeatureName'></a>
### FeatureName `property` [#](#P-Contrib-System-Printing-Xps-IXpsInputBinDefinition-FeatureName 'Go To Here') [=](#contents 'Back To Contents')

##### Summary

The [XName](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Xml.Linq.XName 'System.Xml.Linq.XName') of the feature for the input bin.

##### Example

{http://schemas.microsoft.com/windows/2003/08/printing/printschemakeywords}JobInputBin

<a name='P-Contrib-System-Printing-Xps-IXpsInputBinDefinition-FeedType'></a>
### FeedType `property` [#](#P-Contrib-System-Printing-Xps-IXpsInputBinDefinition-FeedType 'Go To Here') [=](#contents 'Back To Contents')

##### Summary

The [XName](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Xml.Linq.XName 'System.Xml.Linq.XName') of the feed of the input bin.

##### Example

{http://schemas.microsoft.com/windows/2003/08/printing/printschemakeywords}Automatic

<a name='P-Contrib-System-Printing-Xps-IXpsInputBinDefinition-Name'></a>
### Name `property` [#](#P-Contrib-System-Printing-Xps-IXpsInputBinDefinition-Name 'Go To Here') [=](#contents 'Back To Contents')

##### Summary

The [XName](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Xml.Linq.XName 'System.Xml.Linq.XName') of the input bin.

##### Example

{http://schemas.microsoft.com/windows/2003/08/printing/printschemakeywords}AutoSelect

<a name='T-Contrib-System-Printing-Xps-IXpsInputBinDefinitionFactory`1'></a>
## IXpsInputBinDefinitionFactory\`1 [#](#T-Contrib-System-Printing-Xps-IXpsInputBinDefinitionFactory`1 'Go To Here') [=](#contents 'Back To Contents')

##### Namespace

Contrib.System.Printing.Xps

##### Summary

Factory class for `TXpsInputBinDefinition`.

##### Generic Types

| Name | Description |
| ---- | ----------- |
| TXpsInputBinDefinition |  |

##### Example

```
var customXpsInputBinDefinitionFactory = new CustomXpsInputBinDefinitionFactory();
            
             using Contrib.System.Printing.Xps;
             using System.Xml.Linq;
            
             public interface ICustomXpsInputBinDefinition : IXpsInputBinDefinition { }
            
             public class CustomXpsInputBinDefinitionFactory : IXpsInputBinDefinitionFactoryEx&lt;ICustomXpsInputBinDefinition&gt;
             {
               private class CustomXpsInputBinDefinition : ICustomXpsInputBinDefinition
               {
                 public XName FeatureName { get; set; }
                 public string DisplayName { get; set; }
                 public XName Name { get; set; }
                 public XName FeedType { get; set; }
               }
            
               private IXpsInputBinDefinitionFactory XpsInputBinDefinitionFactory { get; } = new XpsInputBinDefinitionFactory();
            
               public ICustomXpsInputBinDefinition Create(XElement optionXElement,
                                                          XElement printCapabilitiesXElement)
               {
                 var xpsInputBinDefinition = this.XpsInputBinDefinitionFactory.Create(optionXElement,
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

##### See Also

- [Contrib.System.Printing.Xps.XpsInputBinDefinitionFactory](#T-Contrib-System-Printing-Xps-XpsInputBinDefinitionFactory 'Contrib.System.Printing.Xps.XpsInputBinDefinitionFactory')

<a name='M-Contrib-System-Printing-Xps-IXpsInputBinDefinitionFactory`1-Create-System-Xml-Linq-XElement,System-Xml-Linq-XElement-'></a>
### Create(optionXElement,printCapabilitiesXElement) `method` [#](#M-Contrib-System-Printing-Xps-IXpsInputBinDefinitionFactory`1-Create-System-Xml-Linq-XElement,System-Xml-Linq-XElement- 'Go To Here') [=](#contents 'Back To Contents')

##### Summary

Factory method for `TXpsInputBinDefinition`.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| optionXElement | [System.Xml.Linq.XElement](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Xml.Linq.XElement 'System.Xml.Linq.XElement') |  |
| printCapabilitiesXElement | [System.Xml.Linq.XElement](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Xml.Linq.XElement 'System.Xml.Linq.XElement') |  |

<a name='T-Contrib-System-Printing-Xps-IXpsPrinterDefinition'></a>
## IXpsPrinterDefinition [#](#T-Contrib-System-Printing-Xps-IXpsPrinterDefinition 'Go To Here') [=](#contents 'Back To Contents')

##### Namespace

Contrib.System.Printing.Xps

##### Summary

Holds information of a printer.

##### See Also

- [Contrib.System.Printing.Xps.XpsPrinterDefinitionFactory.XpsPrinterDefinition](#T-Contrib-System-Printing-Xps-XpsPrinterDefinitionFactory-XpsPrinterDefinition 'Contrib.System.Printing.Xps.XpsPrinterDefinitionFactory.XpsPrinterDefinition')

<a name='P-Contrib-System-Printing-Xps-IXpsPrinterDefinition-DisplayName'></a>
### DisplayName `property` [#](#P-Contrib-System-Printing-Xps-IXpsPrinterDefinition-DisplayName 'Go To Here') [=](#contents 'Back To Contents')

##### Summary

The display name of the printer.

##### Example

Microsoft XPS Document Writer

<a name='P-Contrib-System-Printing-Xps-IXpsPrinterDefinition-DriverName'></a>
### DriverName `property` [#](#P-Contrib-System-Printing-Xps-IXpsPrinterDefinition-DriverName 'Go To Here') [=](#contents 'Back To Contents')

##### Summary

The driver name of the printer.

##### Example

Microsoft XPS Document Writer v4

<a name='P-Contrib-System-Printing-Xps-IXpsPrinterDefinition-FullName'></a>
### FullName `property` [#](#P-Contrib-System-Printing-Xps-IXpsPrinterDefinition-FullName 'Go To Here') [=](#contents 'Back To Contents')

##### Summary

The full name of the printer.

##### Example

Microsoft XPS Document Writer

<a name='P-Contrib-System-Printing-Xps-IXpsPrinterDefinition-PortName'></a>
### PortName `property` [#](#P-Contrib-System-Printing-Xps-IXpsPrinterDefinition-PortName 'Go To Here') [=](#contents 'Back To Contents')

##### Summary

The port name of the printer.

##### Example

PORTPROMPT:

<a name='T-Contrib-System-Printing-Xps-IXpsPrinterDefinitionFactory`1'></a>
## IXpsPrinterDefinitionFactory\`1 [#](#T-Contrib-System-Printing-Xps-IXpsPrinterDefinitionFactory`1 'Go To Here') [=](#contents 'Back To Contents')

##### Namespace

Contrib.System.Printing.Xps

##### Summary

Factory class for `TXpsPrinterDefinition`.

##### Generic Types

| Name | Description |
| ---- | ----------- |
| TXpsPrinterDefinition |  |

##### Example

```
var customXpsPrinterDefinitionFactory = new CustomXpsPrinterDefinitionFactory();
            
             using Contrib.System.Printing.Xps;
             using System.Xml.Linq;
            
             public interface ICustomXpsPrinterDefinition : IXpsPrinterDefinition { }
            
             public class CustomXpsPrinterDefinitionFactory : IXpsPrinterDefinitionFactoryEx&lt;ICustomXpsPrinterDefinition&gt;
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

##### See Also

- [Contrib.System.Printing.Xps.XpsPrinterDefinitionFactory](#T-Contrib-System-Printing-Xps-XpsPrinterDefinitionFactory 'Contrib.System.Printing.Xps.XpsPrinterDefinitionFactory')

<a name='M-Contrib-System-Printing-Xps-IXpsPrinterDefinitionFactory`1-Create-System-String,System-String,System-String,System-String,System-Xml-Linq-XElement-'></a>
### Create(displayName,fullName,portName,driverName,printCapabilitiesXElement) `method` [#](#M-Contrib-System-Printing-Xps-IXpsPrinterDefinitionFactory`1-Create-System-String,System-String,System-String,System-String,System-Xml-Linq-XElement- 'Go To Here') [=](#contents 'Back To Contents')

##### Summary

Factory method for `TXpsPrinterDefinition`.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| displayName | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') |  |
| fullName | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') |  |
| portName | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') |  |
| driverName | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') |  |
| printCapabilitiesXElement | [System.Xml.Linq.XElement](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Xml.Linq.XElement 'System.Xml.Linq.XElement') |  |

<a name='T-Contrib-System-Printing-Xps-IXpsServer'></a>
## IXpsServer [#](#T-Contrib-System-Printing-Xps-IXpsServer 'Go To Here') [=](#contents 'Back To Contents')

##### Namespace

Contrib.System.Printing.Xps

##### Summary

Provides [IXpsPrinterDefinition](#T-Contrib-System-Printing-Xps-IXpsPrinterDefinition 'Contrib.System.Printing.Xps.IXpsPrinterDefinition') and [IXpsInputBinDefinition](#T-Contrib-System-Printing-Xps-IXpsInputBinDefinition 'Contrib.System.Printing.Xps.IXpsInputBinDefinition') instances.

##### See Also

- [Contrib.System.Printing.Xps.XpsServer](#T-Contrib-System-Printing-Xps-XpsServer 'Contrib.System.Printing.Xps.XpsServer')

<a name='M-Contrib-System-Printing-Xps-IXpsServer-GetXpsInputBinDefinitions-Contrib-System-Printing-Xps-IXpsPrinterDefinition-'></a>
### GetXpsInputBinDefinitions(xpsPrinterDefinition) `method` [#](#M-Contrib-System-Printing-Xps-IXpsServer-GetXpsInputBinDefinitions-Contrib-System-Printing-Xps-IXpsPrinterDefinition- 'Go To Here') [=](#contents 'Back To Contents')

##### Summary

Gets all [IXpsInputBinDefinition](#T-Contrib-System-Printing-Xps-IXpsInputBinDefinition 'Contrib.System.Printing.Xps.IXpsInputBinDefinition') instances for `xpsPrinterDefinition`.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| xpsPrinterDefinition | [Contrib.System.Printing.Xps.IXpsPrinterDefinition](#T-Contrib-System-Printing-Xps-IXpsPrinterDefinition 'Contrib.System.Printing.Xps.IXpsPrinterDefinition') |  |

##### Exceptions

| Name | Description |
| ---- | ----------- |
| [System.ArgumentNullException](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.ArgumentNullException 'System.ArgumentNullException') | `xpsPrinterDefinition` is `null`. |
| [System.Exception](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Exception 'System.Exception') |  |

<a name='M-Contrib-System-Printing-Xps-IXpsServer-GetXpsPrinterDefinitions'></a>
### GetXpsPrinterDefinitions() `method` [#](#M-Contrib-System-Printing-Xps-IXpsServer-GetXpsPrinterDefinitions 'Go To Here') [=](#contents 'Back To Contents')

##### Summary

Gets all [IXpsPrinterDefinition](#T-Contrib-System-Printing-Xps-IXpsPrinterDefinition 'Contrib.System.Printing.Xps.IXpsPrinterDefinition') instances.

##### Parameters

This method has no parameters.

##### Exceptions

| Name | Description |
| ---- | ----------- |
| [System.Exception](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Exception 'System.Exception') |  |

<a name='T-Contrib-System-Printing-Xps-IXpsServerEx`2'></a>
## IXpsServerEx\`2 [#](#T-Contrib-System-Printing-Xps-IXpsServerEx`2 'Go To Here') [=](#contents 'Back To Contents')

##### Namespace

Contrib.System.Printing.Xps

##### Summary

Provides `TXpsPrinterDefinition` and `TXpsInputBinDefinition` instances.

##### Generic Types

| Name | Description |
| ---- | ----------- |
| TXpsPrinterDefinition |  |
| TXpsInputBinDefinition |  |

##### See Also

- [Contrib.System.Printing.Xps.XpsServerEx\`2](#T-Contrib-System-Printing-Xps-XpsServerEx`2 'Contrib.System.Printing.Xps.XpsServerEx`2')

<a name='M-Contrib-System-Printing-Xps-IXpsServerEx`2-GetXpsInputBinDefinitions-`0-'></a>
### GetXpsInputBinDefinitions(xpsPrinterDefinition) `method` [#](#M-Contrib-System-Printing-Xps-IXpsServerEx`2-GetXpsInputBinDefinitions-`0- 'Go To Here') [=](#contents 'Back To Contents')

##### Summary

Gets all `TXpsInputBinDefinition` instances.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| xpsPrinterDefinition | [\`0](#T-`0 '`0') |  |

##### Exceptions

| Name | Description |
| ---- | ----------- |
| [System.ArgumentNullException](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.ArgumentNullException 'System.ArgumentNullException') | `xpsPrinterDefinition` is `null`. |
| [System.Exception](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Exception 'System.Exception') |  |

<a name='M-Contrib-System-Printing-Xps-IXpsServerEx`2-GetXpsPrinterDefinitions'></a>
### GetXpsPrinterDefinitions() `method` [#](#M-Contrib-System-Printing-Xps-IXpsServerEx`2-GetXpsPrinterDefinitions 'Go To Here') [=](#contents 'Back To Contents')

##### Summary

Gets all `TXpsPrinterDefinition` instances.

##### Parameters

This method has no parameters.

##### Exceptions

| Name | Description |
| ---- | ----------- |
| [System.Exception](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Exception 'System.Exception') |  |

<a name='T-Contrib-System-Printing-Xps-ExtensionMethods-PrintQueueCollectionExtensions'></a>
## PrintQueueCollectionExtensions [#](#T-Contrib-System-Printing-Xps-ExtensionMethods-PrintQueueCollectionExtensions 'Go To Here') [=](#contents 'Back To Contents')

##### Namespace

Contrib.System.Printing.Xps.ExtensionMethods

##### Summary

Provides extensions to query [PrintQueueCollection](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Printing.PrintQueueCollection 'System.Printing.PrintQueueCollection') instances.

<a name='M-Contrib-System-Printing-Xps-ExtensionMethods-PrintQueueCollectionExtensions-FindPrintQueue-System-Printing-PrintQueueCollection,Contrib-System-Printing-Xps-IXpsPrinterDefinition-'></a>
### FindPrintQueue(printQueueCollection,xpsPrinterDefinition) `method` [#](#M-Contrib-System-Printing-Xps-ExtensionMethods-PrintQueueCollectionExtensions-FindPrintQueue-System-Printing-PrintQueueCollection,Contrib-System-Printing-Xps-IXpsPrinterDefinition- 'Go To Here') [=](#contents 'Back To Contents')

##### Summary

Finds the [PrintQueue](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Printing.PrintQueue 'System.Printing.PrintQueue') instance with matching  for `xpsPrinterDefinition`.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| printQueueCollection | [System.Printing.PrintQueueCollection](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Printing.PrintQueueCollection 'System.Printing.PrintQueueCollection') |  |
| xpsPrinterDefinition | [Contrib.System.Printing.Xps.IXpsPrinterDefinition](#T-Contrib-System-Printing-Xps-IXpsPrinterDefinition 'Contrib.System.Printing.Xps.IXpsPrinterDefinition') |  |

##### Exceptions

| Name | Description |
| ---- | ----------- |
| [System.ArgumentNullException](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.ArgumentNullException 'System.ArgumentNullException') | `printQueueCollection` is `null`. |
| [System.ArgumentNullException](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.ArgumentNullException 'System.ArgumentNullException') | `xpsPrinterDefinition` is `null`. |
| [System.Exception](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Exception 'System.Exception') |  |

<a name='T-Contrib-System-Printing-Xps-ExtensionMethods-PrintQueueExtensions'></a>
## PrintQueueExtensions [#](#T-Contrib-System-Printing-Xps-ExtensionMethods-PrintQueueExtensions 'Go To Here') [=](#contents 'Back To Contents')

##### Namespace

Contrib.System.Printing.Xps.ExtensionMethods

##### Summary

Provides extensions to query [PrintQueue](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Printing.PrintQueue 'System.Printing.PrintQueue') instances.

<a name='M-Contrib-System-Printing-Xps-ExtensionMethods-PrintQueueExtensions-GetPrintCapabilitiesAsXDocument-System-Printing-PrintQueue-'></a>
### GetPrintCapabilitiesAsXDocument(printQueue) `method` [#](#M-Contrib-System-Printing-Xps-ExtensionMethods-PrintQueueExtensions-GetPrintCapabilitiesAsXDocument-System-Printing-PrintQueue- 'Go To Here') [=](#contents 'Back To Contents')

##### Summary

Gets the print capabilities (see https://msdn.microsoft.com/en-us/library/windows/desktop/ms715274).

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| printQueue | [System.Printing.PrintQueue](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Printing.PrintQueue 'System.Printing.PrintQueue') |  |

##### Exceptions

| Name | Description |
| ---- | ----------- |
| [System.ArgumentNullException](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.ArgumentNullException 'System.ArgumentNullException') | `printQueue` is `null`. |

##### See Also

- [System.Printing.PrintQueue.GetPrintCapabilitiesAsXml](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Printing.PrintQueue.GetPrintCapabilitiesAsXml 'System.Printing.PrintQueue.GetPrintCapabilitiesAsXml')

<a name='M-Contrib-System-Printing-Xps-ExtensionMethods-PrintQueueExtensions-GetPrintCapabilitiesAsXDocument-System-Printing-PrintQueue,System-Printing-PrintTicket-'></a>
### GetPrintCapabilitiesAsXDocument(printQueue,printTicket) `method` [#](#M-Contrib-System-Printing-Xps-ExtensionMethods-PrintQueueExtensions-GetPrintCapabilitiesAsXDocument-System-Printing-PrintQueue,System-Printing-PrintTicket- 'Go To Here') [=](#contents 'Back To Contents')

##### Summary

Gets the print capabilities (see https://msdn.microsoft.com/en-us/library/windows/desktop/ms715274).

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| printQueue | [System.Printing.PrintQueue](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Printing.PrintQueue 'System.Printing.PrintQueue') |  |
| printTicket | [System.Printing.PrintTicket](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Printing.PrintTicket 'System.Printing.PrintTicket') |  |

##### Exceptions

| Name | Description |
| ---- | ----------- |
| [System.ArgumentNullException](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.ArgumentNullException 'System.ArgumentNullException') | `printQueue` is `null`. |
| [System.ArgumentNullException](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.ArgumentNullException 'System.ArgumentNullException') | `printTicket` is `null`. |

##### See Also

- [System.Printing.PrintQueue.GetPrintCapabilitiesAsXml](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Printing.PrintQueue.GetPrintCapabilitiesAsXml 'System.Printing.PrintQueue.GetPrintCapabilitiesAsXml(System.Printing.PrintTicket)')

<a name='T-Contrib-System-Printing-Xps-ExtensionMethods-PrintServerExtensions'></a>
## PrintServerExtensions [#](#T-Contrib-System-Printing-Xps-ExtensionMethods-PrintServerExtensions 'Go To Here') [=](#contents 'Back To Contents')

##### Namespace

Contrib.System.Printing.Xps.ExtensionMethods

##### Summary

Provides extensions to query [PrintServer](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Printing.PrintServer 'System.Printing.PrintServer') instances.

<a name='M-Contrib-System-Printing-Xps-ExtensionMethods-PrintServerExtensions-GetLocalAndRemotePrintQueues-System-Printing-PrintServer-'></a>
### GetLocalAndRemotePrintQueues(printServer) `method` [#](#M-Contrib-System-Printing-Xps-ExtensionMethods-PrintServerExtensions-GetLocalAndRemotePrintQueues-System-Printing-PrintServer- 'Go To Here') [=](#contents 'Back To Contents')

##### Summary

Gets local and remote [PrintQueue](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Printing.PrintQueue 'System.Printing.PrintQueue')-instances, wrapped in [PrintQueueCollection](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Printing.PrintQueueCollection 'System.Printing.PrintQueueCollection').

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| printServer | [System.Printing.PrintServer](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Printing.PrintServer 'System.Printing.PrintServer') |  |

##### Exceptions

| Name | Description |
| ---- | ----------- |
| [System.ArgumentNullException](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.ArgumentNullException 'System.ArgumentNullException') | `printServer` is `null`. |
| [System.Exception](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Exception 'System.Exception') |  |

<a name='T-Contrib-System-Printing-Xps-ExtensionMethods-XpsPrinterDefinitionExtensions-PrintTicketFactory'></a>
## PrintTicketFactory [#](#T-Contrib-System-Printing-Xps-ExtensionMethods-XpsPrinterDefinitionExtensions-PrintTicketFactory 'Go To Here') [=](#contents 'Back To Contents')

##### Namespace

Contrib.System.Printing.Xps.ExtensionMethods.XpsPrinterDefinitionExtensions

##### Summary

Factory for [PrintTicket](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Printing.PrintTicket 'System.Printing.PrintTicket')-instance.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| printQueue | [T:Contrib.System.Printing.Xps.ExtensionMethods.XpsPrinterDefinitionExtensions.PrintTicketFactory](#T-T-Contrib-System-Printing-Xps-ExtensionMethods-XpsPrinterDefinitionExtensions-PrintTicketFactory 'T:Contrib.System.Printing.Xps.ExtensionMethods.XpsPrinterDefinitionExtensions.PrintTicketFactory') |  |

##### Exceptions

| Name | Description |
| ---- | ----------- |
| [System.ArgumentNullException](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.ArgumentNullException 'System.ArgumentNullException') | `printQueue` is `null`. |

<a name='T-Contrib-System-Printing-Xps-UnitConverter'></a>
## UnitConverter [#](#T-Contrib-System-Printing-Xps-UnitConverter 'Go To Here') [=](#contents 'Back To Contents')

##### Namespace

Contrib.System.Printing.Xps

##### Summary

A transparent proxy for [UnitConverter](#T-MS-Internal-Printing-Configuration-UnitConverter 'MS.Internal.Printing.Configuration.UnitConverter').

##### See Also

- [MS.Internal.Printing.Configuration.UnitConverter](#T-MS-Internal-Printing-Configuration-UnitConverter 'MS.Internal.Printing.Configuration.UnitConverter')

<a name='M-Contrib-System-Printing-Xps-UnitConverter-LengthValueFromDIPToMicron-System-Double-'></a>
### LengthValueFromDIPToMicron(dipValue) `method` [#](#M-Contrib-System-Printing-Xps-UnitConverter-LengthValueFromDIPToMicron-System-Double- 'Go To Here') [=](#contents 'Back To Contents')

##### Summary

Converts device-independent pixels to micron.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| dipValue | [System.Double](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Double 'System.Double') |  |

##### Exceptions

| Name | Description |
| ---- | ----------- |
| [System.Exception](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Exception 'System.Exception') |  |

##### See Also

- [MS.Internal.Printing.Configuration.UnitConverter.LengthValueFromDIPToMicron](#M-MS-Internal-Printing-Configuration-UnitConverter-LengthValueFromDIPToMicron-System-Double- 'MS.Internal.Printing.Configuration.UnitConverter.LengthValueFromDIPToMicron(System.Double)')

<a name='M-Contrib-System-Printing-Xps-UnitConverter-LengthValueFromMicronToDIP-System-Int32-'></a>
### LengthValueFromMicronToDIP(micronValue) `method` [#](#M-Contrib-System-Printing-Xps-UnitConverter-LengthValueFromMicronToDIP-System-Int32- 'Go To Here') [=](#contents 'Back To Contents')

##### Summary

Converts micron to device-independent pixels.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| micronValue | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') |  |

##### Exceptions

| Name | Description |
| ---- | ----------- |
| [System.Exception](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Exception 'System.Exception') |  |

##### See Also

- [MS.Internal.Printing.Configuration.UnitConverter.LengthValueFromMicronToDIP](#M-MS-Internal-Printing-Configuration-UnitConverter-LengthValueFromMicronToDIP-System-Int32- 'MS.Internal.Printing.Configuration.UnitConverter.LengthValueFromMicronToDIP(System.Int32)')

<a name='T-Contrib-System-Printing-Xps-ExtensionMethods-XElementExtensions'></a>
## XElementExtensions [#](#T-Contrib-System-Printing-Xps-ExtensionMethods-XElementExtensions 'Go To Here') [=](#contents 'Back To Contents')

##### Namespace

Contrib.System.Printing.Xps.ExtensionMethods

##### Summary

Provides extensions to query [XElement](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Xml.Linq.XElement 'System.Xml.Linq.XElement') instances.

<a name='M-Contrib-System-Printing-Xps-ExtensionMethods-XElementExtensions-EnsurePrefixRegistrationOfNamespace-System-Xml-Linq-XElement,System-Xml-Linq-XName-'></a>
### EnsurePrefixRegistrationOfNamespace(xelement,name) `method` [#](#M-Contrib-System-Printing-Xps-ExtensionMethods-XElementExtensions-EnsurePrefixRegistrationOfNamespace-System-Xml-Linq-XElement,System-Xml-Linq-XName- 'Go To Here') [=](#contents 'Back To Contents')

##### Summary

Ensures and gets the prefix of the namespace registration of `name`.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| xelement | [System.Xml.Linq.XElement](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Xml.Linq.XElement 'System.Xml.Linq.XElement') |  |
| name | [System.Xml.Linq.XName](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Xml.Linq.XName 'System.Xml.Linq.XName') |  |

##### Exceptions

| Name | Description |
| ---- | ----------- |
| [System.ArgumentNullException](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.ArgumentNullException 'System.ArgumentNullException') | `xelement` is `null`. |
| [System.ArgumentNullException](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.ArgumentNullException 'System.ArgumentNullException') | `name` is `null`. |
| [System.Exception](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Exception 'System.Exception') |  |

<a name='M-Contrib-System-Printing-Xps-ExtensionMethods-XElementExtensions-FindElementByNameAttribute-System-Xml-Linq-XElement,System-Xml-Linq-XName-'></a>
### FindElementByNameAttribute(xelement,name) `method` [#](#M-Contrib-System-Printing-Xps-ExtensionMethods-XElementExtensions-FindElementByNameAttribute-System-Xml-Linq-XElement,System-Xml-Linq-XName- 'Go To Here') [=](#contents 'Back To Contents')

##### Summary

Finds the descending `name`-[XElement](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Xml.Linq.XElement 'System.Xml.Linq.XElement').

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| xelement | [System.Xml.Linq.XElement](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Xml.Linq.XElement 'System.Xml.Linq.XElement') |  |
| name | [System.Xml.Linq.XName](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Xml.Linq.XName 'System.Xml.Linq.XName') |  |

##### Exceptions

| Name | Description |
| ---- | ----------- |
| [System.ArgumentNullException](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.ArgumentNullException 'System.ArgumentNullException') | `xelement` is `null`. |
| [System.ArgumentNullException](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.ArgumentNullException 'System.ArgumentNullException') | `name` is `null`. |

<a name='M-Contrib-System-Printing-Xps-ExtensionMethods-XElementExtensions-FindUnusedPrefixForNamespace-System-Xml-Linq-XElement-'></a>
### FindUnusedPrefixForNamespace(xelement) `method` [#](#M-Contrib-System-Printing-Xps-ExtensionMethods-XElementExtensions-FindUnusedPrefixForNamespace-System-Xml-Linq-XElement- 'Go To Here') [=](#contents 'Back To Contents')

##### Summary

Finds an unused prefix for namespace registration.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| xelement | [System.Xml.Linq.XElement](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Xml.Linq.XElement 'System.Xml.Linq.XElement') |  |

##### Exceptions

| Name | Description |
| ---- | ----------- |
| [System.ArgumentNullException](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.ArgumentNullException 'System.ArgumentNullException') | `xelement` is `null`. |

##### Remarks

The prefix is constructed via following pattern: "ns{0000}"

<a name='M-Contrib-System-Printing-Xps-ExtensionMethods-XElementExtensions-GetNameFromNameAttribute-System-Xml-Linq-XElement-'></a>
### GetNameFromNameAttribute(xelement) `method` [#](#M-Contrib-System-Printing-Xps-ExtensionMethods-XElementExtensions-GetNameFromNameAttribute-System-Xml-Linq-XElement- 'Go To Here') [=](#contents 'Back To Contents')

##### Summary

Gets the [XName](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Xml.Linq.XName 'System.Xml.Linq.XName') from name-[XAttribute](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Xml.Linq.XAttribute 'System.Xml.Linq.XAttribute') of `xelement`.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| xelement | [System.Xml.Linq.XElement](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Xml.Linq.XElement 'System.Xml.Linq.XElement') |  |

##### Exceptions

| Name | Description |
| ---- | ----------- |
| [System.ArgumentNullException](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.ArgumentNullException 'System.ArgumentNullException') | `xelement` is `null`. |

<a name='M-Contrib-System-Printing-Xps-ExtensionMethods-XElementExtensions-GetValue-System-Xml-Linq-XElement-'></a>
### GetValue(xelement) `method` [#](#M-Contrib-System-Printing-Xps-ExtensionMethods-XElementExtensions-GetValue-System-Xml-Linq-XElement- 'Go To Here') [=](#contents 'Back To Contents')

##### Summary

Gets the boxed value by taking type-[XAttribute](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Xml.Linq.XAttribute 'System.Xml.Linq.XAttribute') into account.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| xelement | [System.Xml.Linq.XElement](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Xml.Linq.XElement 'System.Xml.Linq.XElement') |  |

##### Exceptions

| Name | Description |
| ---- | ----------- |
| [System.ArgumentNullException](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.ArgumentNullException 'System.ArgumentNullException') | `xelement` is `null`. |

<a name='M-Contrib-System-Printing-Xps-ExtensionMethods-XElementExtensions-GetValueFromValueElement-System-Xml-Linq-XElement-'></a>
### GetValueFromValueElement(xelement) `method` [#](#M-Contrib-System-Printing-Xps-ExtensionMethods-XElementExtensions-GetValueFromValueElement-System-Xml-Linq-XElement- 'Go To Here') [=](#contents 'Back To Contents')

##### Summary

Gets the boxed value from descending value-[XElement](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Xml.Linq.XElement 'System.Xml.Linq.XElement').

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| xelement | [System.Xml.Linq.XElement](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Xml.Linq.XElement 'System.Xml.Linq.XElement') |  |

##### Exceptions

| Name | Description |
| ---- | ----------- |
| [System.ArgumentNullException](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.ArgumentNullException 'System.ArgumentNullException') | `xelement` is `null`. |

<a name='M-Contrib-System-Printing-Xps-ExtensionMethods-XElementExtensions-GetXName-System-Xml-Linq-XElement,System-String-'></a>
### GetXName(xelement,str) `method` [#](#M-Contrib-System-Printing-Xps-ExtensionMethods-XElementExtensions-GetXName-System-Xml-Linq-XElement,System-String- 'Go To Here') [=](#contents 'Back To Contents')

##### Summary

Gets the [XName](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Xml.Linq.XName 'System.Xml.Linq.XName') from `str`.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| xelement | [System.Xml.Linq.XElement](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Xml.Linq.XElement 'System.Xml.Linq.XElement') |  |
| str | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') |  |

##### Exceptions

| Name | Description |
| ---- | ----------- |
| [System.ArgumentNullException](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.ArgumentNullException 'System.ArgumentNullException') | `xelement` is `null`. |

##### Remarks

`xelement` is used to find the namespace for the prefix, contained in `str`.

<a name='T-Contrib-System-Printing-Xps-XpsInputBinDefinitionFactory-XpsInputBinDefinition'></a>
## XpsInputBinDefinition [#](#T-Contrib-System-Printing-Xps-XpsInputBinDefinitionFactory-XpsInputBinDefinition 'Go To Here') [=](#contents 'Back To Contents')

##### Namespace

Contrib.System.Printing.Xps.XpsInputBinDefinitionFactory

##### Summary

*Inherit from parent.*

<a name='P-Contrib-System-Printing-Xps-XpsInputBinDefinitionFactory-XpsInputBinDefinition-DisplayName'></a>
### DisplayName `property` [#](#P-Contrib-System-Printing-Xps-XpsInputBinDefinitionFactory-XpsInputBinDefinition-DisplayName 'Go To Here') [=](#contents 'Back To Contents')

##### Summary

*Inherit from parent.*

<a name='P-Contrib-System-Printing-Xps-XpsInputBinDefinitionFactory-XpsInputBinDefinition-FeatureName'></a>
### FeatureName `property` [#](#P-Contrib-System-Printing-Xps-XpsInputBinDefinitionFactory-XpsInputBinDefinition-FeatureName 'Go To Here') [=](#contents 'Back To Contents')

##### Summary

*Inherit from parent.*

<a name='P-Contrib-System-Printing-Xps-XpsInputBinDefinitionFactory-XpsInputBinDefinition-FeedType'></a>
### FeedType `property` [#](#P-Contrib-System-Printing-Xps-XpsInputBinDefinitionFactory-XpsInputBinDefinition-FeedType 'Go To Here') [=](#contents 'Back To Contents')

##### Summary

*Inherit from parent.*

<a name='P-Contrib-System-Printing-Xps-XpsInputBinDefinitionFactory-XpsInputBinDefinition-Name'></a>
### Name `property` [#](#P-Contrib-System-Printing-Xps-XpsInputBinDefinitionFactory-XpsInputBinDefinition-Name 'Go To Here') [=](#contents 'Back To Contents')

##### Summary

*Inherit from parent.*

<a name='T-Contrib-System-Printing-Xps-ExtensionMethods-XpsInputBinDefinitionExtensions'></a>
## XpsInputBinDefinitionExtensions [#](#T-Contrib-System-Printing-Xps-ExtensionMethods-XpsInputBinDefinitionExtensions 'Go To Here') [=](#contents 'Back To Contents')

##### Namespace

Contrib.System.Printing.Xps.ExtensionMethods

##### Summary

Provides extensions to query [IXpsInputBinDefinition](#T-Contrib-System-Printing-Xps-IXpsInputBinDefinition 'Contrib.System.Printing.Xps.IXpsInputBinDefinition') instances.

<a name='M-Contrib-System-Printing-Xps-ExtensionMethods-XpsInputBinDefinitionExtensions-GetPrintTicket-Contrib-System-Printing-Xps-IXpsInputBinDefinition-'></a>
### GetPrintTicket(xpsInputBinDefinition) `method` [#](#M-Contrib-System-Printing-Xps-ExtensionMethods-XpsInputBinDefinitionExtensions-GetPrintTicket-Contrib-System-Printing-Xps-IXpsInputBinDefinition- 'Go To Here') [=](#contents 'Back To Contents')

##### Summary

Gets the [PrintTicket](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Printing.PrintTicket 'System.Printing.PrintTicket') for `xpsInputBinDefinition`.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| xpsInputBinDefinition | [Contrib.System.Printing.Xps.IXpsInputBinDefinition](#T-Contrib-System-Printing-Xps-IXpsInputBinDefinition 'Contrib.System.Printing.Xps.IXpsInputBinDefinition') |  |

##### Exceptions

| Name | Description |
| ---- | ----------- |
| [System.ArgumentNullException](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.ArgumentNullException 'System.ArgumentNullException') | `xpsInputBinDefinition` is `null`. |
| [System.Exception](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Exception 'System.Exception') |  |

<a name='T-Contrib-System-Printing-Xps-XpsInputBinDefinitionFactory'></a>
## XpsInputBinDefinitionFactory [#](#T-Contrib-System-Printing-Xps-XpsInputBinDefinitionFactory 'Go To Here') [=](#contents 'Back To Contents')

##### Namespace

Contrib.System.Printing.Xps

##### Summary

*Inherit from parent.*

<a name='M-Contrib-System-Printing-Xps-XpsInputBinDefinitionFactory-#ctor'></a>
### #ctor() `constructor` [#](#M-Contrib-System-Printing-Xps-XpsInputBinDefinitionFactory-#ctor 'Go To Here') [=](#contents 'Back To Contents')

##### Summary

Initializes a new instance of the [XpsInputBinDefinitionFactory](#T-Contrib-System-Printing-Xps-XpsInputBinDefinitionFactory 'Contrib.System.Printing.Xps.XpsInputBinDefinitionFactory') class.

##### Parameters

This constructor has no parameters.

<a name='M-Contrib-System-Printing-Xps-XpsInputBinDefinitionFactory-Create-System-Xml-Linq-XElement,System-Xml-Linq-XElement-'></a>
### Create() `method` [#](#M-Contrib-System-Printing-Xps-XpsInputBinDefinitionFactory-Create-System-Xml-Linq-XElement,System-Xml-Linq-XElement- 'Go To Here') [=](#contents 'Back To Contents')

##### Summary

*Inherit from parent.*

##### Parameters

This method has no parameters.

<a name='T-Contrib-System-Printing-Xps-XpsPrinterDefinitionFactory-XpsPrinterDefinition'></a>
## XpsPrinterDefinition [#](#T-Contrib-System-Printing-Xps-XpsPrinterDefinitionFactory-XpsPrinterDefinition 'Go To Here') [=](#contents 'Back To Contents')

##### Namespace

Contrib.System.Printing.Xps.XpsPrinterDefinitionFactory

##### Summary

*Inherit from parent.*

<a name='P-Contrib-System-Printing-Xps-XpsPrinterDefinitionFactory-XpsPrinterDefinition-DisplayName'></a>
### DisplayName `property` [#](#P-Contrib-System-Printing-Xps-XpsPrinterDefinitionFactory-XpsPrinterDefinition-DisplayName 'Go To Here') [=](#contents 'Back To Contents')

##### Summary

*Inherit from parent.*

<a name='P-Contrib-System-Printing-Xps-XpsPrinterDefinitionFactory-XpsPrinterDefinition-DriverName'></a>
### DriverName `property` [#](#P-Contrib-System-Printing-Xps-XpsPrinterDefinitionFactory-XpsPrinterDefinition-DriverName 'Go To Here') [=](#contents 'Back To Contents')

##### Summary

*Inherit from parent.*

<a name='P-Contrib-System-Printing-Xps-XpsPrinterDefinitionFactory-XpsPrinterDefinition-FullName'></a>
### FullName `property` [#](#P-Contrib-System-Printing-Xps-XpsPrinterDefinitionFactory-XpsPrinterDefinition-FullName 'Go To Here') [=](#contents 'Back To Contents')

##### Summary

*Inherit from parent.*

<a name='P-Contrib-System-Printing-Xps-XpsPrinterDefinitionFactory-XpsPrinterDefinition-PortName'></a>
### PortName `property` [#](#P-Contrib-System-Printing-Xps-XpsPrinterDefinitionFactory-XpsPrinterDefinition-PortName 'Go To Here') [=](#contents 'Back To Contents')

##### Summary

*Inherit from parent.*

<a name='T-Contrib-System-Printing-Xps-ExtensionMethods-XpsPrinterDefinitionExtensions'></a>
## XpsPrinterDefinitionExtensions [#](#T-Contrib-System-Printing-Xps-ExtensionMethods-XpsPrinterDefinitionExtensions 'Go To Here') [=](#contents 'Back To Contents')

##### Namespace

Contrib.System.Printing.Xps.ExtensionMethods

##### Summary

Provides extensions to query [IXpsPrinterDefinition](#T-Contrib-System-Printing-Xps-IXpsPrinterDefinition 'Contrib.System.Printing.Xps.IXpsPrinterDefinition') instances.

<a name='M-Contrib-System-Printing-Xps-ExtensionMethods-XpsPrinterDefinitionExtensions-Print-Contrib-System-Printing-Xps-IXpsPrinterDefinition,System-Windows-Documents-IDocumentPaginatorSource,Contrib-System-Printing-Xps-ExtensionMethods-XpsPrinterDefinitionExtensions-PrintTicketFactory-'></a>
### Print(xpsPrinterDefinition,documentPaginatorSource,printTicketFactory) `method` [#](#M-Contrib-System-Printing-Xps-ExtensionMethods-XpsPrinterDefinitionExtensions-Print-Contrib-System-Printing-Xps-IXpsPrinterDefinition,System-Windows-Documents-IDocumentPaginatorSource,Contrib-System-Printing-Xps-ExtensionMethods-XpsPrinterDefinitionExtensions-PrintTicketFactory- 'Go To Here') [=](#contents 'Back To Contents')

##### Summary

Prints `documentPaginatorSource` to the printer, defined by `xpsPrinterDefinition`.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| xpsPrinterDefinition | [Contrib.System.Printing.Xps.IXpsPrinterDefinition](#T-Contrib-System-Printing-Xps-IXpsPrinterDefinition 'Contrib.System.Printing.Xps.IXpsPrinterDefinition') |  |
| documentPaginatorSource | [System.Windows.Documents.IDocumentPaginatorSource](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Windows.Documents.IDocumentPaginatorSource 'System.Windows.Documents.IDocumentPaginatorSource') |  |
| printTicketFactory | [Contrib.System.Printing.Xps.ExtensionMethods.XpsPrinterDefinitionExtensions.PrintTicketFactory](#T-Contrib-System-Printing-Xps-ExtensionMethods-XpsPrinterDefinitionExtensions-PrintTicketFactory 'Contrib.System.Printing.Xps.ExtensionMethods.XpsPrinterDefinitionExtensions.PrintTicketFactory') |  |

##### Exceptions

| Name | Description |
| ---- | ----------- |
| [System.ArgumentNullException](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.ArgumentNullException 'System.ArgumentNullException') | `xpsPrinterDefinition` is `null`. |
| [System.ArgumentNullException](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.ArgumentNullException 'System.ArgumentNullException') | `documentPaginatorSource` is `null`. |
| [System.ArgumentNullException](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.ArgumentNullException 'System.ArgumentNullException') | `printTicketFactory` is `null`. |
| [System.Exception](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Exception 'System.Exception') |  |

<a name='T-Contrib-System-Printing-Xps-XpsPrinterDefinitionFactory'></a>
## XpsPrinterDefinitionFactory [#](#T-Contrib-System-Printing-Xps-XpsPrinterDefinitionFactory 'Go To Here') [=](#contents 'Back To Contents')

##### Namespace

Contrib.System.Printing.Xps

##### Summary

*Inherit from parent.*

<a name='M-Contrib-System-Printing-Xps-XpsPrinterDefinitionFactory-#ctor'></a>
### #ctor() `constructor` [#](#M-Contrib-System-Printing-Xps-XpsPrinterDefinitionFactory-#ctor 'Go To Here') [=](#contents 'Back To Contents')

##### Summary

Initializes a new instance of the [XpsPrinterDefinitionFactory](#T-Contrib-System-Printing-Xps-XpsPrinterDefinitionFactory 'Contrib.System.Printing.Xps.XpsPrinterDefinitionFactory') class.

##### Parameters

This constructor has no parameters.

<a name='M-Contrib-System-Printing-Xps-XpsPrinterDefinitionFactory-Create-System-String,System-String,System-String,System-String,System-Xml-Linq-XElement-'></a>
### Create() `method` [#](#M-Contrib-System-Printing-Xps-XpsPrinterDefinitionFactory-Create-System-String,System-String,System-String,System-String,System-Xml-Linq-XElement- 'Go To Here') [=](#contents 'Back To Contents')

##### Summary

*Inherit from parent.*

##### Parameters

This method has no parameters.

<a name='T-Contrib-System-Printing-Xps-XpsServer'></a>
## XpsServer [#](#T-Contrib-System-Printing-Xps-XpsServer 'Go To Here') [=](#contents 'Back To Contents')

##### Namespace

Contrib.System.Printing.Xps

##### Summary

*Inherit from parent.*

<a name='M-Contrib-System-Printing-Xps-XpsServer-#ctor'></a>
### #ctor() `constructor` [#](#M-Contrib-System-Printing-Xps-XpsServer-#ctor 'Go To Here') [=](#contents 'Back To Contents')

##### Summary

Initializes a new instance of the [XpsServer](#T-Contrib-System-Printing-Xps-XpsServer 'Contrib.System.Printing.Xps.XpsServer') class.

##### Parameters

This constructor has no parameters.

<a name='M-Contrib-System-Printing-Xps-XpsServer-#ctor-Contrib-System-Printing-Xps-IXpsPrinterDefinitionFactory{Contrib-System-Printing-Xps-IXpsPrinterDefinition},Contrib-System-Printing-Xps-IXpsInputBinDefinitionFactory{Contrib-System-Printing-Xps-IXpsInputBinDefinition}-'></a>
### #ctor(xpsPrinterDefinitionFactory,xpsInputBinDefinitionFactory) `constructor` [#](#M-Contrib-System-Printing-Xps-XpsServer-#ctor-Contrib-System-Printing-Xps-IXpsPrinterDefinitionFactory{Contrib-System-Printing-Xps-IXpsPrinterDefinition},Contrib-System-Printing-Xps-IXpsInputBinDefinitionFactory{Contrib-System-Printing-Xps-IXpsInputBinDefinition}- 'Go To Here') [=](#contents 'Back To Contents')

##### Summary

Initializes a new instance of the [XpsServer](#T-Contrib-System-Printing-Xps-XpsServer 'Contrib.System.Printing.Xps.XpsServer') class.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| xpsPrinterDefinitionFactory | [Contrib.System.Printing.Xps.IXpsPrinterDefinitionFactory{Contrib.System.Printing.Xps.IXpsPrinterDefinition}](#T-Contrib-System-Printing-Xps-IXpsPrinterDefinitionFactory{Contrib-System-Printing-Xps-IXpsPrinterDefinition} 'Contrib.System.Printing.Xps.IXpsPrinterDefinitionFactory{Contrib.System.Printing.Xps.IXpsPrinterDefinition}') |  |
| xpsInputBinDefinitionFactory | [Contrib.System.Printing.Xps.IXpsInputBinDefinitionFactory{Contrib.System.Printing.Xps.IXpsInputBinDefinition}](#T-Contrib-System-Printing-Xps-IXpsInputBinDefinitionFactory{Contrib-System-Printing-Xps-IXpsInputBinDefinition} 'Contrib.System.Printing.Xps.IXpsInputBinDefinitionFactory{Contrib.System.Printing.Xps.IXpsInputBinDefinition}') |  |

<a name='P-Contrib-System-Printing-Xps-XpsServer-DisplayNameXName'></a>
### DisplayNameXName `property` [#](#P-Contrib-System-Printing-Xps-XpsServer-DisplayNameXName 'Go To Here') [=](#contents 'Back To Contents')

##### Summary

psk:DisplayName

<a name='P-Contrib-System-Printing-Xps-XpsServer-DocumentInputBinXName'></a>
### DocumentInputBinXName `property` [#](#P-Contrib-System-Printing-Xps-XpsServer-DocumentInputBinXName 'Go To Here') [=](#contents 'Back To Contents')

##### Summary

psk:DocumentInputBin

<a name='P-Contrib-System-Printing-Xps-XpsServer-FeatureElementXName'></a>
### FeatureElementXName `property` [#](#P-Contrib-System-Printing-Xps-XpsServer-FeatureElementXName 'Go To Here') [=](#contents 'Back To Contents')

##### Summary

psf:Feature

<a name='P-Contrib-System-Printing-Xps-XpsServer-FeedTypeXName'></a>
### FeedTypeXName `property` [#](#P-Contrib-System-Printing-Xps-XpsServer-FeedTypeXName 'Go To Here') [=](#contents 'Back To Contents')

##### Summary

psk:FeedType

<a name='P-Contrib-System-Printing-Xps-XpsServer-ImageableSizeHeightXName'></a>
### ImageableSizeHeightXName `property` [#](#P-Contrib-System-Printing-Xps-XpsServer-ImageableSizeHeightXName 'Go To Here') [=](#contents 'Back To Contents')

##### Summary

psk:ImageableSizeHeight

<a name='P-Contrib-System-Printing-Xps-XpsServer-ImageableSizeWidthXName'></a>
### ImageableSizeWidthXName `property` [#](#P-Contrib-System-Printing-Xps-XpsServer-ImageableSizeWidthXName 'Go To Here') [=](#contents 'Back To Contents')

##### Summary

psk:ImageableSizeWidth

<a name='P-Contrib-System-Printing-Xps-XpsServer-IntegerTypeXName'></a>
### IntegerTypeXName `property` [#](#P-Contrib-System-Printing-Xps-XpsServer-IntegerTypeXName 'Go To Here') [=](#contents 'Back To Contents')

##### Summary

xsd:integer

<a name='P-Contrib-System-Printing-Xps-XpsServer-JobInputBinXName'></a>
### JobInputBinXName `property` [#](#P-Contrib-System-Printing-Xps-XpsServer-JobInputBinXName 'Go To Here') [=](#contents 'Back To Contents')

##### Summary

psk:JobInputBin

<a name='P-Contrib-System-Printing-Xps-XpsServer-MediaSizeHeightXName'></a>
### MediaSizeHeightXName `property` [#](#P-Contrib-System-Printing-Xps-XpsServer-MediaSizeHeightXName 'Go To Here') [=](#contents 'Back To Contents')

##### Summary

psk:MediaSizeHeight

<a name='P-Contrib-System-Printing-Xps-XpsServer-MediaSizeWidthXName'></a>
### MediaSizeWidthXName `property` [#](#P-Contrib-System-Printing-Xps-XpsServer-MediaSizeWidthXName 'Go To Here') [=](#contents 'Back To Contents')

##### Summary

psk:MediaSizeWidth

<a name='P-Contrib-System-Printing-Xps-XpsServer-NameAttributeXName'></a>
### NameAttributeXName `property` [#](#P-Contrib-System-Printing-Xps-XpsServer-NameAttributeXName 'Go To Here') [=](#contents 'Back To Contents')

##### Summary

name

<a name='P-Contrib-System-Printing-Xps-XpsServer-OptionElementXName'></a>
### OptionElementXName `property` [#](#P-Contrib-System-Printing-Xps-XpsServer-OptionElementXName 'Go To Here') [=](#contents 'Back To Contents')

##### Summary

psf:Option

<a name='P-Contrib-System-Printing-Xps-XpsServer-PageImageableSizeXName'></a>
### PageImageableSizeXName `property` [#](#P-Contrib-System-Printing-Xps-XpsServer-PageImageableSizeXName 'Go To Here') [=](#contents 'Back To Contents')

##### Summary

psk:PageImageableSize

<a name='P-Contrib-System-Printing-Xps-XpsServer-PageInputBinXName'></a>
### PageInputBinXName `property` [#](#P-Contrib-System-Printing-Xps-XpsServer-PageInputBinXName 'Go To Here') [=](#contents 'Back To Contents')

##### Summary

psk:PageInputBin

<a name='P-Contrib-System-Printing-Xps-XpsServer-PageMediaSizeXName'></a>
### PageMediaSizeXName `property` [#](#P-Contrib-System-Printing-Xps-XpsServer-PageMediaSizeXName 'Go To Here') [=](#contents 'Back To Contents')

##### Summary

psk:PageMediaSize

<a name='P-Contrib-System-Printing-Xps-XpsServer-PrintCapabilitiesXName'></a>
### PrintCapabilitiesXName `property` [#](#P-Contrib-System-Printing-Xps-XpsServer-PrintCapabilitiesXName 'Go To Here') [=](#contents 'Back To Contents')

##### Summary

psf:PrintCapabilities

<a name='P-Contrib-System-Printing-Xps-XpsServer-PrinterSchemaFrameworkXNamespace'></a>
### PrinterSchemaFrameworkXNamespace `property` [#](#P-Contrib-System-Printing-Xps-XpsServer-PrinterSchemaFrameworkXNamespace 'Go To Here') [=](#contents 'Back To Contents')

##### Summary

psf

<a name='P-Contrib-System-Printing-Xps-XpsServer-PrinterSchemaKeywordsXNamespace'></a>
### PrinterSchemaKeywordsXNamespace `property` [#](#P-Contrib-System-Printing-Xps-XpsServer-PrinterSchemaKeywordsXNamespace 'Go To Here') [=](#contents 'Back To Contents')

##### Summary

psk

<a name='P-Contrib-System-Printing-Xps-XpsServer-PropertyElementXName'></a>
### PropertyElementXName `property` [#](#P-Contrib-System-Printing-Xps-XpsServer-PropertyElementXName 'Go To Here') [=](#contents 'Back To Contents')

##### Summary

psf:Property

<a name='P-Contrib-System-Printing-Xps-XpsServer-QNameTypeXName'></a>
### QNameTypeXName `property` [#](#P-Contrib-System-Printing-Xps-XpsServer-QNameTypeXName 'Go To Here') [=](#contents 'Back To Contents')

##### Summary

xsd:QName

<a name='P-Contrib-System-Printing-Xps-XpsServer-ScoredPropertyElementXName'></a>
### ScoredPropertyElementXName `property` [#](#P-Contrib-System-Printing-Xps-XpsServer-ScoredPropertyElementXName 'Go To Here') [=](#contents 'Back To Contents')

##### Summary

psf:ScoredProperty

<a name='P-Contrib-System-Printing-Xps-XpsServer-StringTypeXName'></a>
### StringTypeXName `property` [#](#P-Contrib-System-Printing-Xps-XpsServer-StringTypeXName 'Go To Here') [=](#contents 'Back To Contents')

##### Summary

xsd:string

<a name='P-Contrib-System-Printing-Xps-XpsServer-TypeXName'></a>
### TypeXName `property` [#](#P-Contrib-System-Printing-Xps-XpsServer-TypeXName 'Go To Here') [=](#contents 'Back To Contents')

##### Summary

xsi:type

<a name='P-Contrib-System-Printing-Xps-XpsServer-ValueElementXName'></a>
### ValueElementXName `property` [#](#P-Contrib-System-Printing-Xps-XpsServer-ValueElementXName 'Go To Here') [=](#contents 'Back To Contents')

##### Summary

psf:Value

<a name='P-Contrib-System-Printing-Xps-XpsServer-XmlSchemaInstanceXNamespace'></a>
### XmlSchemaInstanceXNamespace `property` [#](#P-Contrib-System-Printing-Xps-XpsServer-XmlSchemaInstanceXNamespace 'Go To Here') [=](#contents 'Back To Contents')

##### Summary

xsi

<a name='P-Contrib-System-Printing-Xps-XpsServer-XmlSchemaXNamespace'></a>
### XmlSchemaXNamespace `property` [#](#P-Contrib-System-Printing-Xps-XpsServer-XmlSchemaXNamespace 'Go To Here') [=](#contents 'Back To Contents')

##### Summary

xsd

<a name='T-Contrib-System-Printing-Xps-XpsServerEx`2'></a>
## XpsServerEx\`2 [#](#T-Contrib-System-Printing-Xps-XpsServerEx`2 'Go To Here') [=](#contents 'Back To Contents')

##### Namespace

Contrib.System.Printing.Xps

##### Summary

*Inherit from parent.*

<a name='M-Contrib-System-Printing-Xps-XpsServerEx`2-#ctor-Contrib-System-Printing-Xps-IXpsPrinterDefinitionFactory{`0},Contrib-System-Printing-Xps-IXpsInputBinDefinitionFactory{`1}-'></a>
### #ctor(xpsPrinterDefinitionFactory,xpsInputBinDefinitionFactory) `constructor` [#](#M-Contrib-System-Printing-Xps-XpsServerEx`2-#ctor-Contrib-System-Printing-Xps-IXpsPrinterDefinitionFactory{`0},Contrib-System-Printing-Xps-IXpsInputBinDefinitionFactory{`1}- 'Go To Here') [=](#contents 'Back To Contents')

##### Summary

Initializes a new instance of the [XpsServerEx\`2](#T-Contrib-System-Printing-Xps-XpsServerEx`2 'Contrib.System.Printing.Xps.XpsServerEx`2') class.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| xpsPrinterDefinitionFactory | [Contrib.System.Printing.Xps.IXpsPrinterDefinitionFactory{\`0}](#T-Contrib-System-Printing-Xps-IXpsPrinterDefinitionFactory{`0} 'Contrib.System.Printing.Xps.IXpsPrinterDefinitionFactory{`0}') |  |
| xpsInputBinDefinitionFactory | [Contrib.System.Printing.Xps.IXpsInputBinDefinitionFactory{\`1}](#T-Contrib-System-Printing-Xps-IXpsInputBinDefinitionFactory{`1} 'Contrib.System.Printing.Xps.IXpsInputBinDefinitionFactory{`1}') |  |

##### Exceptions

| Name | Description |
| ---- | ----------- |
| [System.ArgumentNullException](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.ArgumentNullException 'System.ArgumentNullException') | `xpsPrinterDefinitionFactory` is `null`. |
| [System.ArgumentNullException](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.ArgumentNullException 'System.ArgumentNullException') | `xpsInputBinDefinitionFactory` is `null`. |

<a name='M-Contrib-System-Printing-Xps-XpsServerEx`2-GetPrintTicketImpl-System-Xml-Linq-XName,System-Xml-Linq-XName-'></a>
### GetPrintTicketImpl(featureXName,inputBinXName) `method` [#](#M-Contrib-System-Printing-Xps-XpsServerEx`2-GetPrintTicketImpl-System-Xml-Linq-XName,System-Xml-Linq-XName- 'Go To Here') [=](#contents 'Back To Contents')

##### Summary

Gets a plain [PrintTicket](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Printing.PrintTicket 'System.Printing.PrintTicket'), which is bound to `inputBinXName`, to retrieve the print capabilities of the input bin.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| featureXName | [System.Xml.Linq.XName](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Xml.Linq.XName 'System.Xml.Linq.XName') |  |
| inputBinXName | [System.Xml.Linq.XName](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Xml.Linq.XName 'System.Xml.Linq.XName') |  |

<a name='M-Contrib-System-Printing-Xps-XpsServerEx`2-GetXpsInputBinDefinitions-`0-'></a>
### GetXpsInputBinDefinitions() `method` [#](#M-Contrib-System-Printing-Xps-XpsServerEx`2-GetXpsInputBinDefinitions-`0- 'Go To Here') [=](#contents 'Back To Contents')

##### Summary

*Inherit from parent.*

##### Parameters

This method has no parameters.

<a name='M-Contrib-System-Printing-Xps-XpsServerEx`2-GetXpsPrinterDefinitions'></a>
### GetXpsPrinterDefinitions() `method` [#](#M-Contrib-System-Printing-Xps-XpsServerEx`2-GetXpsPrinterDefinitions 'Go To Here') [=](#contents 'Back To Contents')

##### Summary

*Inherit from parent.*

##### Parameters

This method has no parameters.

## License

dotnet-Contrib.System.Printing.Xps is published under [WTFNMFPLv3](https://github.com/dittodhole/WTFNMFPLv3).

## Icon

[Printer](https://thenounproject.com/term/printer/1230289/) by [Wira](https://thenounproject.com/wirawizinda097) from the Noun Project.
