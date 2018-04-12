﻿using System;
using System.IO;
using System.Printing;
using System.Xml.Linq;
using JetBrains.Annotations;

namespace Contrib.System.Printing.Xps.ExtensionMethods
{
  public static class XpsInputBinDefinitionExtensions
  {
    /// <exception cref="ArgumentNullException"><paramref name="xpsInputBinDefinition"/> is <see langword="null"/></exception>
    /// <exception cref="Exception" />
    [Pure]
    [NotNull]
    public static PrintTicket GetPrintTicket([NotNull] this IXpsInputBinDefinition xpsInputBinDefinition)
    {
      if (xpsInputBinDefinition == null)
      {
        throw new ArgumentNullException(nameof(xpsInputBinDefinition));
      }

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
      //                  xmlns:{prefix0}="{FeatureName.NamespaceName}"
      //                  xmlns:{prefix1}="{FeatureName.NamespaceName}"
      //                  xmlns:{prefix2}="{XpsPrintCapabilitiesReader.FeedTypeXName.NamespaceName}"
      //                  xmlns:{prefix3}="{FeedType.NamespaceName}"
      //                  version="1">
      //   <psf:Feature name="{prefix0}:{FeatureName.LocalName}">
      //     <psf:Option name="{prefix1}:{DisplayName.LocalName}">
      //       <psf:ScoredProperty name="{prefix2}:{XpsPrintCapabilitiesReader.FeedTypeXName.LocalName}">
      //         <psf:Value>{prefix3}:{FeedType.LocalName}</psf:Value>
      //       </psf:ScoredProperty>
      //     </psf:Option>
      //   </psf:Feature>
      // </psf:PrintTicket>
      // === === === === ===

      XElement featureXElement;
      {
        var featureXName = xpsInputBinDefinition.FeatureName;

        var prefix0 = printTicketXElement.EnsurePrefixRegistrationOfNamespace(featureXName);

        featureXElement = new XElement(XpsServer.FeatureElementXName);
        featureXElement.SetAttributeValue(XpsServer.NameAttributeXName,
                                          $"{prefix0}:{featureXName.LocalName}");
        printTicketXElement.Add(featureXElement);
      }

      XElement optionXElement;
      {
        var inputBinXName = xpsInputBinDefinition.Name;

        var prefix1 = printTicketXElement.EnsurePrefixRegistrationOfNamespace(inputBinXName);

        optionXElement = new XElement(XpsServer.OptionElementXName);
        optionXElement.SetAttributeValue(XpsServer.NameAttributeXName,
                                         $"{prefix1}:{inputBinXName.LocalName}");
        featureXElement.Add(optionXElement);
      }

      {
        var feedTypeXName = xpsInputBinDefinition.GetValue(XpsServer.FeedTypeXName) as XName;
        if (feedTypeXName != null)
        {
          var prefix2 = printTicketXElement.EnsurePrefixRegistrationOfNamespace(XpsServer.FeedTypeXName);

          var scoredPropertyXElement = new XElement(XpsServer.ScoredPropertyElementXName);
          scoredPropertyXElement.SetAttributeValue(XpsServer.NameAttributeXName,
                                                   $"{prefix2}:{XpsServer.FeedTypeXName.LocalName}");
          optionXElement.Add(scoredPropertyXElement);

          var prefix3 = printTicketXElement.EnsurePrefixRegistrationOfNamespace(feedTypeXName);

          // TODO add xsi:type="xsd:QName" :beers:
          var valueXElement = new XElement(XpsServer.ValueElementXName);
          valueXElement.Value = $"{prefix3}:{feedTypeXName.LocalName}";
          scoredPropertyXElement.Add(valueXElement);
        }
      }

      PrintTicket result;
      using (var memoryStream = new MemoryStream())
      {
        xdocument.Save(memoryStream);
        memoryStream.Seek(0L,
                          SeekOrigin.Begin);

        result = new PrintTicket(memoryStream);
      }

      return result;
    }
  }
}
