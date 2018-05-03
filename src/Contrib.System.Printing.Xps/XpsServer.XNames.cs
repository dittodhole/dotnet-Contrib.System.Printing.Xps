/** @pp
 * rootnamespace: Contrib.System.Printing.Xps
 */
namespace Contrib.System.Printing.Xps
{
  using global::System.Xml.Linq;
  using global::JetBrains.Annotations;

#if CONTRIB_SYSTEM_PRINTING_XPS
  public
#else
  internal
#endif
  partial class XpsServer
  {
    /// <summary>
    ///   psf:PrintCapabilities
    /// </summary>
    [PublicAPI]
    [NotNull]
    public static XName PrintCapabilitiesName => XpsServer.PrinterSchemaFrameworkNamespace + "PrintCapabilities";

    /// <summary>
    ///   psf:PrintTicket
    /// </summary>
    [PublicAPI]
    [NotNull]
    public static XName PrintTicketName => XpsServer.PrinterSchemaFrameworkNamespace + "PrintTicket";

    /// <summary>
    ///   psf:Feature
    /// </summary>
    [PublicAPI]
    [NotNull]
    public static XName FeatureName => XpsServer.PrinterSchemaFrameworkNamespace + "Feature";

    /// <summary>
    ///   name
    /// </summary>
    [PublicAPI]
    [NotNull]
    public static XName NameName => XNamespace.None + "name";

    /// <summary>
    ///   psf:Property
    /// </summary>
    [PublicAPI]
    [NotNull]
    public static XName PropertyName => XpsServer.PrinterSchemaFrameworkNamespace + "Property";

    /// <summary>
    ///   psf:ScoredProperty
    /// </summary>
    [PublicAPI]
    [NotNull]
    public static XName ScoredPropertyName => XpsServer.PrinterSchemaFrameworkNamespace + "ScoredProperty";

    /// <summary>
    ///   psf:Option
    /// </summary>
    [PublicAPI]
    [NotNull]
    public static XName OptionName => XpsServer.PrinterSchemaFrameworkNamespace + "Option";

    /// <summary>
    ///   psf:Value
    /// </summary>
    [PublicAPI]
    [NotNull]
    public static XName ValueName => XpsServer.PrinterSchemaFrameworkNamespace + "Value";

    /// <summary>
    ///   psk:DisplayName
    /// </summary>
    [PublicAPI]
    [NotNull]
    public static XName DisplayNameName => XpsServer.PrinterSchemaKeywordsNamespace + "DisplayName";

    /// <summary>
    ///   psk:PageInputBin
    /// </summary>
    [PublicAPI]
    [NotNull]
    public static XName PageInputBinName => XpsServer.PrinterSchemaKeywordsNamespace + "PageInputBin";

    /// <summary>
    ///   psk:DocumentInputBin
    /// </summary>
    [PublicAPI]
    [NotNull]
    public static XName DocumentInputBinName => XpsServer.PrinterSchemaKeywordsNamespace + "DocumentInputBin";

    /// <summary>
    ///   psk:JobInputBin
    /// </summary>
    [PublicAPI]
    [NotNull]
    public static XName JobInputBinName => XpsServer.PrinterSchemaKeywordsNamespace + "JobInputBin";

    /// <summary>
    ///   psk:PageImageableSize
    /// </summary>
    [PublicAPI]
    [NotNull]
    public static XName PageImageableSizeName => XpsServer.PrinterSchemaKeywordsNamespace + "PageImageableSize";

    /// <summary>
    ///   psk:ImageableSizeWidth
    /// </summary>
    [PublicAPI]
    [NotNull]
    public static XName ImageableSizeWidthName => XpsServer.PrinterSchemaKeywordsNamespace + "ImageableSizeWidth";

    /// <summary>
    ///   psk:ImageableSizeHeight
    /// </summary>
    [PublicAPI]
    [NotNull]
    public static XName ImageableSizeHeightName => XpsServer.PrinterSchemaKeywordsNamespace + "ImageableSizeHeight";

    /// <summary>
    ///   psk:PageMediaSize
    /// </summary>
    [PublicAPI]
    [NotNull]
    public static XName PageMediaSizeName => XpsServer.PrinterSchemaKeywordsNamespace + "PageMediaSize";

    /// <summary>
    ///   psk:MediaSizeWidth
    /// </summary>
    [PublicAPI]
    [NotNull]
    public static XName MediaSizeWidthName => XpsServer.PrinterSchemaKeywordsNamespace + "MediaSizeWidth";

    /// <summary>
    ///   psk:MediaSizeHeight
    /// </summary>
    [PublicAPI]
    [NotNull]
    public static XName MediaSizeHeightName => XpsServer.PrinterSchemaKeywordsNamespace + "MediaSizeHeight";

    /// <summary>
    ///   psk:FeedType
    /// </summary>
    [PublicAPI]
    [NotNull]
    public static XName FeedTypeName => XpsServer.PrinterSchemaKeywordsNamespace + "FeedType";

    /// <summary>
    ///   xsi:type
    /// </summary>
    [PublicAPI]
    [NotNull]
    public static XName TypeName => XpsServer.XmlSchemaInstanceNamespace + "type";

    /// <summary>
    ///   xsd:integer
    /// </summary>
    [PublicAPI]
    [NotNull]
    public static XName IntegerName => XpsServer.XmlSchemaNamespace + "integer";

    /// <summary>
    ///   xsd:string
    /// </summary>
    [PublicAPI]
    [NotNull]
    public static XName StringName => XpsServer.XmlSchemaNamespace + "string";

    /// <summary>
    ///   xsd:QName
    /// </summary>
    [PublicAPI]
    [NotNull]
    public static XName QNameName => XpsServer.XmlSchemaNamespace + "QName";
  }
}
