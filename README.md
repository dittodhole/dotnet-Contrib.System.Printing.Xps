![](assets/noun_1230289_cc.png)

# dotnet-Contrib.System.Printing.Xps

> Easily access and use [XPS](https://en.wikipedia.org/wiki/Open_XML_Paper_Specification) printers.

| Hosting                                                                                      | Build                                                                                                                                                                                             | Version                                                                                                                                                              | Installation                                                                                                  |
|:---------------------------------------------------------------------------------------------|:--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|:--------------------------------------------------------------------------------------------------------------------------------------------------------------------:|:--------------------------------------------------------------------------------------------------------------|
| [myget.org](https://www.myget.org/feed/dittodhole/package/nuget/Contrib.System.Printing.Xps) | [![](https://img.shields.io/appveyor/ci/dittodhole/dotnet-contrib-system-printing-xps/develop.svg)](https://ci.appveyor.com/project/dittodhole/dotnet-contrib-system-printing-xps/branch/develop) | [![](https://img.shields.io/myget/dittodhole/vpre/Contrib.System.Printing.Xps.svg)](https://www.myget.org/feed/dittodhole/package/nuget/Contrib.System.Printing.Xps) | `PM> Install-Package -Id Contrib.System.Printing.Xps -pre --source https://www.myget.org/F/dittodhole/api/v2` |
| [nuget.org](https://www.nuget.org/packages/Contrib.System.Printing.Xps)                      | [![](https://img.shields.io/appveyor/ci/dittodhole/dotnet-contrib-system-printing-xps/master.svg)](https://ci.appveyor.com/project/dittodhole/dotnet-contrib-system-printing-xps/branch/master)   | [![](https://img.shields.io/nuget/v/Contrib.System.Printing.Xps.svg)](https://www.nuget.org/packages/Contrib.System.Printing.Xps)                                    | `PM> Install-Package -Id Contrib.System.Printing.Xps`                                                         |


## Example

```csharp
using Contrib.System.Printing.Xps;
using System.Windows.Documents;

IDocumentPaginatorSource documentPaginatorSource;

var xpsServer = new XpsServer();
var xpsPrinterDefinitions = xpsServer.GetXpsPrinterDefinitions();

foreach (var xpsPrinterDefinition in xpsPrinterDefinitions)
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

## Developing & Building

```cmd
> git clone https://github.com/dittodhole/dotnet-Contrib.System.Printing.Xps.git
> cd dotnet-Contrib.System.Printing.Xps/
dotnet-Contrib.System.Printing.Xps> cd build
dotnet-Contrib.System.Printing.Xps/build> npm install
dotnet-Contrib.System.Printing.Xps/build> npm run-script build
```

## License

dotnet-Contrib.System.Printing.Xps is published under [WTFNMFPLv3](https://github.com/dittodhole/WTFNMFPLv3).

## Icon

[Printer](https://thenounproject.com/term/printer/1230289/) by [Wira](https://thenounproject.com/wirawizinda097) from the Noun Project.