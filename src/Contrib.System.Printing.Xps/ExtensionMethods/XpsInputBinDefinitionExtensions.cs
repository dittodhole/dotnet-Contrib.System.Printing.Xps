/** @pp
 * rootnamespace: Contrib.System.Printing.Xps
 */
namespace Contrib.System.Printing.Xps.ExtensionMethods
{
  using global::System;
  using global::System.IO;
  using global::System.Printing;
  using global::System.Xml.Linq;
  using global::JetBrains.Annotations;

  /// <summary>
  ///   Provides extensions for <see cref="IXpsInputBinDefinition"/> objects.
  /// </summary>
  [PublicAPI]
#if CONTRIB_SYSTEM_PRINTING_XPS
  public
#else
  internal
#endif
  static partial class XpsInputBinDefinitionExtensions
  {
    /// <summary>
    ///   Gets the print ticket for <paramref name="xpsInputBinDefinition"/>.
    /// </summary>
    /// <param name="xpsInputBinDefinition"/>
    /// <exception cref="ArgumentNullException"><paramref name="xpsInputBinDefinition"/> is <see langword="null"/>.</exception>
    /// <exception cref="Exception"/>
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
      //                  xmlns:{xsi}="http://www.w3.org/2001/XMLSchema-instance"
      //                  xmlns:xsd="http://www.w3.org/2001/XMLSchema"
      //                  xmlns:{prefix0}="{featureXName.NamespaceName}"
      //                  xmlns:{prefix1}="{inputBinXName.NamespaceName}"
      //                  xmlns:{prefix2}="{XpsServer.FeedTypeName.NamespaceName}"
      //                  xmlns:{prefix3}="{feedTypeXName.NamespaceName}"
      //                  xmlns:{prefix4}="{XpsServer.QNameName.NamespaceName}"
      //                  version="1">
      //   <psf:Feature name="{prefix0}:{featureXName.LocalName}">
      //     <psf:Option name="{prefix1}:{inputBinXName.LocalName}">
      //       <psf:ScoredProperty name="{prefix2}:{XpsServer.FeedTypeName.LocalName}">
      //         <psf:Value {xsi}:{XpsServer.TypeName.LocalName}="{prefix4}:{XpsServer.QNameName.LocalName}">{prefix3}:{feedTypeXName.LocalName}</psf:Value>
      //       </psf:ScoredProperty>
      //     </psf:Option>
      //   </psf:Feature>
      // </psf:PrintTicket>
      // === === === === ===

      var feature = printTicket.AddElement(XpsServer.FeatureName);
      feature.SetAttributeValue(XpsServer.NameName,
                                feature.ReduceName(xpsInputBinDefinition.Feature));

      var option = feature.AddElement(XpsServer.OptionName);
      option.SetAttributeValue(XpsServer.NameName,
                               option.ReduceName(xpsInputBinDefinition.Name));

      var feedType = xpsInputBinDefinition.FeedType;
      if (feedType != null)
      {
        var scoredProperty = option.AddElement(XpsServer.ScoredPropertyName);
        scoredProperty.SetAttributeValue(XpsServer.NameName,
                                         scoredProperty.ReduceName(XpsServer.FeedTypeName));

        var value = scoredProperty.AddElement(XpsServer.ValueName);
        value.SetValue(value.ReduceName(feedType));
        value.SetAttributeValue(XpsServer.TypeName,
                                value.ReduceName(XpsServer.QNameName));
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
