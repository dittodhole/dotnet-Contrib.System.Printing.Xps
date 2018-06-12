/** @pp
 * rootnamespace: Contrib.System
 */
namespace Contrib.System.Printing.Xps.ExtensionMethods
{
  using global::System;
  using global::System.IO;
  using global::System.Printing;
  using global::System.Xml.Linq;
  using global::JetBrains.Annotations;

  /// <summary>
  ///   Provides extensions for <see cref="T:Contrib.System.Printing.Xps.IXpsInputBinDefinition"/> objects.
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
    /// <exception cref="T:System.ArgumentNullException"><paramref name="xpsInputBinDefinition"/> is <see langword="null"/>.</exception>
    /// <exception cref="T:System.Exception"/>
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
      var prefix = feature.EnsurePrefixRegistrationOfNamespace(xpsInputBinDefinition.Feature.Namespace);
      feature.SetAttributeValue(XpsServer.NameName,
                                xpsInputBinDefinition.Feature.ToString(prefix));

      var option = feature.AddElement(XpsServer.OptionName);
      prefix = option.EnsurePrefixRegistrationOfNamespace(xpsInputBinDefinition.Name.Namespace);
      option.SetAttributeValue(XpsServer.NameName,
                               xpsInputBinDefinition.Name.ToString(prefix));

      var feedType = xpsInputBinDefinition.FeedType;
      if (feedType != null)
      {
        var scoredProperty = option.AddElement(XpsServer.ScoredPropertyName);
        prefix = scoredProperty.EnsurePrefixRegistrationOfNamespace(XpsServer.FeedTypeName.Namespace);
        scoredProperty.SetAttributeValue(XpsServer.NameName,
                                         XpsServer.FeedTypeName.ToString(prefix));

        var value = scoredProperty.AddElement(XpsServer.ValueName);
        prefix = value.EnsurePrefixRegistrationOfNamespace(feedType.Namespace);
        value.SetValue(feedType.ToString(prefix));
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
