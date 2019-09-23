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
  ///   Provides extensions for <see cref="T:System.Printing.PrintQueue"/> objects.
  /// </summary>
#if CONTRIB_SYSTEM_PRINTING_XPS
  public
#else
  internal
#endif
  static partial class PrintQueueExtensions
  {
    /// <summary>
    ///   Gets an <see cref="T:System.Xml.Linq.XDocument"/> object that specifies the printer's capabilities that complies with the Print Schema (see https://msdn.microsoft.com/en-us/library/windows/desktop/ms715274).
    /// </summary>
    /// <param name="printQueue"/>
    /// <seealso cref="M:System.Printing.PrintQueue.GetPrintCapabilitiesAsXml()"/>
    /// <exception cref="T:System.ArgumentNullException"><paramref name="printQueue"/> is <see langword="null"/>.</exception>
    /// <exception cref="T:System.InvalidOperationException"/>
    /// <exception cref="T:System.Exception"/>
    [Pure]
    [NotNull]
    public static XDocument GetPrintCapabilitiesAsXDocument([NotNull] this PrintQueue printQueue)
    {
      if (printQueue == null)
      {
        throw new ArgumentNullException(nameof(printQueue));
      }

      XDocument result;
      try
      {
        using (var memoryStream = printQueue.GetPrintCapabilitiesAsXml())
        {
          result = XDocument.Load(memoryStream);
        }
      }
      catch (PrintQueueException printQueueException)
      {
        throw new InvalidOperationException("Failed to get print capabilities",
                                            printQueueException);
      }

      return result;
    }

    /// <summary>
    ///   Applies the provided <paramref name="xpsInputBinDefinition"/> to a new <see cref="T:System.Printing.PrintTicket"/>, based on <paramref name="printTicket"/>.
    /// </summary>
    /// <param name="printQueue"/>
    /// <param name="printTicket"/>
    /// <param name="xpsInputBinDefinition"/>
    /// <exception cref="T:System.ArgumentNullException"><paramref name="printQueue"/> is <see langword="null"/>.</exception>
    /// <exception cref="T:System.ArgumentNullException"><paramref name="printTicket"/> is <see langword="null"/>.</exception>
    /// <exception cref="T:System.ArgumentNullException"><paramref name="xpsInputBinDefinition"/> is <see langword="null"/>.</exception>
    /// <exception cref="T:System.InvalidOperationException"/>
    /// <exception cref="T:System.Exception"/>
    [Pure]
    [NotNull]
    public static PrintTicket ApplyXpsInputBinDefinition([NotNull] this PrintQueue printQueue,
                                                         [NotNull] PrintTicket printTicket,
                                                         [NotNull] IXpsInputBinDefinition xpsInputBinDefinition)
    {
      if (printQueue == null)
      {
        throw new ArgumentNullException(nameof(printQueue));
      }
      if (xpsInputBinDefinition == null)
      {
        throw new ArgumentNullException(nameof(xpsInputBinDefinition));
      }

      // === result ===
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

      var deltaPrintTicket = new PrintTicket().GetXDocument();

      var feature = deltaPrintTicket.Root.AddElement(XpsServer.FeatureName);
      var prefix = feature.EnsurePrefixRegistrationOfNamespace(xpsInputBinDefinition.FeatureName.Namespace);
      feature.SetAttributeValue(XpsServer.NameName,
                                xpsInputBinDefinition.FeatureName.ToString(prefix));

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
        deltaPrintTicket.Save(memoryStream);

        memoryStream.Seek(0L,
                          SeekOrigin.Begin);

        try
        {
          result = printQueue.MergeAndValidatePrintTicket(printTicket,
                                                          new PrintTicket(memoryStream))
                             .ValidatedPrintTicket;
        }
        catch (PrintQueueException printQueueException)
        {
          throw new InvalidOperationException("Failed to merge print ticket",
                                              printQueueException);
        }
      }

      return result;
    }
  }
}
