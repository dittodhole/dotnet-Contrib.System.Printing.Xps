/** @pp
 * rootnamespace: Contrib.System
 */
namespace Contrib.System.Printing.Xps
{
  using global::System;
  using global::System.IO;
  using global::System.Printing;
  using global::System.Xml.Linq;
  using global::Contrib.System.Printing.Xps.ExtensionMethods;
  using global::JetBrains.Annotations;

#if CONTRIB_SYSTEM_PRINTING_XPS
  public
#else
  internal
#endif
  partial class XpsServer
  {
    /// <summary>
    ///   Gets a plain print ticket, which is bound to the specified input bin.
    /// </summary>
    /// <param name="featureName"/>
    /// <param name="inputBinName"/>
    /// <exception cref="T:System.ArgumentNullException"><paramref name="featureName"/> is <see langword="null"/>.</exception>
    /// <exception cref="T:System.ArgumentNullException"><paramref name="inputBinName"/> is <see langword="null"/>.</exception>
    /// <exception cref="T:System.Exception"/>
    [PublicAPI]
    [NotNull]
    public static PrintTicket GetPrintTicket([NotNull] XpsName featureName,
                                             [NotNull] XpsName inputBinName)
    {
      if (featureName == null)
      {
        throw new ArgumentNullException(nameof(featureName));
      }
      if (inputBinName == null)
      {
        throw new ArgumentNullException(nameof(inputBinName));
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

      XDocument document;
      using (var memoryStream = new PrintTicket().GetXmlStream())
      {
        document = XDocument.Load(memoryStream);
      }

      var printTicket = document.Root ?? XpsServer.PrintTicketElement;

      // === DESTINATION ===
      // <?xml version="1.0" encoding="UTF-8"?>
      // <psf:PrintTicket xmlns:psf="http://schemas.microsoft.com/windows/2003/08/printing/printschemaframework"
      //                  xmlns:psk="http://schemas.microsoft.com/windows/2003/08/printing/printschemakeywords"
      //                  xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
      //                  xmlns:xsd="http://www.w3.org/2001/XMLSchema"
      //                  xmlns:{prefix0}="{featureName.NamespaceName}"
      //                  xmlns:{prefix1}="{inputBinName.NamespaceName}"
      //                  version="1">
      //   <psf:Feature name="{prefix0}:{featureName.LocalName}">
      //     <psf:Option name="{prefix1}:{inputBinName.LocalName}"/>
      //   </psf:Feature>
      // </psf:PrintTicket>
      // === === === === ===

      var feature = printTicket.AddElement(XpsServer.FeatureName);
      var prefix = feature.GetPrefixOfNamespace(featureName.Namespace);
      feature.SetAttributeValue(XpsServer.NameName,
                                featureName.ToString(prefix));

      var option = feature.AddElement(XpsServer.OptionName);
      prefix = option.GetPrefixOfNamespace(inputBinName.Namespace);
      option.SetAttributeValue(XpsServer.NameName,
                               inputBinName.ToString(prefix));

      PrintTicket result;
      using (var memoryStream = new MemoryStream())
      {
        document.Save(memoryStream);
        memoryStream.Seek(0L,
                          SeekOrigin.Begin);

        result = new PrintTicket(memoryStream);
      }

      return result;
    }
  }
}
