/** @pp
 * rootnamespace: Contrib.System.Printing.Xps
 */
namespace Contrib.System.Printing.Xps
{
  using global::System;
  using global::System.IO;
  using global::System.Printing;
  using global::System.Xml.Linq;
  using global::Contrib.System.Printing.Xps.ExtensionMethods;
  using global::JetBrains.Annotations;

  public partial class XpsServer
  {
    /// <summary>
    ///   Gets a plain <see cref="PrintTicket"/>, which is bound to <paramref name="inputBinName"/>, to retrieve the print capabilities of the input bin.
    /// </summary>
    /// <param name="featureName"/>
    /// <param name="inputBinName"/>
    /// <exception cref="ArgumentNullException"><paramref name="featureName"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="inputBinName"/> is <see langword="null"/>.</exception>
    /// <exception cref="Exception"/>
    [PublicAPI]
    [NotNull]
    public static PrintTicket GetPrintTicket([NotNull] XName featureName,
                                             [NotNull] XName inputBinName)
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

      var printTicket = document.Root ?? new XElement(XpsServer.PrintTicketName);

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

      XElement feature;
      {
        var prefix0 = printTicket.EnsurePrefixRegistrationOfNamespace(featureName);

        feature = new XElement(XpsServer.FeatureName);
        feature.SetAttributeValue(XpsServer.NameName,
                                  $"{prefix0}:{featureName.LocalName}");
        printTicket.Add(feature);
      }

      {
        var prefix1 = printTicket.EnsurePrefixRegistrationOfNamespace(inputBinName);

        var option = new XElement(XpsServer.OptionName);
        option.SetAttributeValue(XpsServer.NameName,
                                 $"{prefix1}:{inputBinName.LocalName}");
        feature.Add(option);
      }

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
