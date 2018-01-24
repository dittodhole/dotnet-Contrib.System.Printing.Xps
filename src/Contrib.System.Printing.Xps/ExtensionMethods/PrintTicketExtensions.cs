using System;
using System.IO;
using System.Printing;
using System.Xml.Linq;
using JetBrains.Annotations;

namespace Contrib.System.Printing.Xps.ExtensionMethods
{
  public static class PrintTicketExtensions
  {
    /// <exception cref="Exception" />
    /// <code>
    ///   using Contrib.System.Printing.Xps.ExtensionMethods;
    ///
    ///   PrintTicketExtensions.CreatePrintTicket(<paramref name="featureName"/>: "psk:JobInputBin",
    ///                                           <paramref name="inputBinName"/>: "psk:AutoSelect",
    ///                                           <paramref name="namespacePrefix"/>: "psk",
    ///                                           <paramref name="namespaceUri"/>: "http://schemas.microsoft.com/windows/2003/08/printing/printschemakeywords");
    /// </code>
    [Pure]
    [NotNull]
    public static PrintTicket CreatePrintTicket([NotNull] string featureName,
                                                [NotNull] string inputBinName,
                                                [NotNull] string namespacePrefix,
                                                [NotNull] string namespaceUri)
    {
      // === SOURCE ===
      // <?xml version="1.0" encoding="UTF-8"?>
      // <psf:PrintTicket xmlns:psf="http://schemas.microsoft.com/windows/2003/08/printing/printschemaframework"
      //                  xmlns:psk="http://schemas.microsoft.com/windows/2003/08/printing/printschemakeywords"
      //                  xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
      //                  xmlns:xsd="http://www.w3.org/2001/XMLSchema"
      //                  version="1">
      // </psf:PrintTicket>
      // === === === ===

      XDocument xdocument;

      {
        var printTicket = new PrintTicket();

        using (var memoryStream = printTicket.GetXmlStream())
        {
          xdocument = XDocument.Load(memoryStream);
        }
      }

      var printTicketXElement = xdocument.Root;
      if (printTicketXElement == null)
      {
        throw new Exception($"Could not get {nameof(XDocument.Root)}: {xdocument}");
      }

      // === DESTINATION ===
      // <?xml version="1.0" encoding="UTF-8"?>
      // <psf:PrintTicket xmlns:psf="http://schemas.microsoft.com/windows/2003/08/printing/printschemaframework"
      //                  xmlns:psk="http://schemas.microsoft.com/windows/2003/08/printing/printschemakeywords"
      //                  xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
      //                  xmlns:xsd="http://www.w3.org/2001/XMLSchema"
      //                  xmlns:{namespacePrefix}="{namespaceUri}"
      //                  version="1">
      //   <psf:Feature name="{featureName}">
      //     <psf:Option name="{inputBinName}" />
      //   </psf:Feature>
      // </psf:PrintTicket>
      // === === === === ===

      var printerSchemaFrameworkXNamespace = XpsPrinter.GetPrinterSchemaFrameworkXNamespace();

      var featureXElement = new XElement(printerSchemaFrameworkXNamespace + "Feature");
      featureXElement.SetAttributeValue("name", featureName);
      printTicketXElement.Add(featureXElement);

      var optionXElement = new XElement(printerSchemaFrameworkXNamespace + "Option");
      optionXElement.SetAttributeValue("name", inputBinName);
      featureXElement.Add(optionXElement);

      var namespaceXName = XNamespace.Xmlns + namespacePrefix;

      if (printTicketXElement.Attribute(namespaceXName) == null)
      {
        printTicketXElement.SetAttributeValue(namespaceXName,
                                              namespaceUri);
      }

      using (var memoryStream = new MemoryStream())
      {
        xdocument.Save(memoryStream);
        memoryStream.Seek(0L,
                          SeekOrigin.Begin);

        var printTicket = new PrintTicket(memoryStream);

        return printTicket;
      }
    }
  }
}
