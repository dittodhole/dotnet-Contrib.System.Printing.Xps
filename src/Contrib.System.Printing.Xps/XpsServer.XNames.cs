using System.Xml.Linq;
using JetBrains.Annotations;

namespace Contrib.System.Printing.Xps
{
  public partial class XpsServer
  {
    /// <summary>
    ///   psf:PrintCapabilities
    /// </summary>
    [PublicAPI]
    [NotNull]
    public static XName PrintCapabilitiesXName => XpsServer.PrinterSchemaFrameworkXNamespace + "PrintCapabilities";

    /// <summary>
    ///   psf:Feature
    /// </summary>
    [PublicAPI]
    [NotNull]
    public static XName FeatureElementXName => XpsServer.PrinterSchemaFrameworkXNamespace + "Feature";

    /// <summary>
    ///   name
    /// </summary>
    [PublicAPI]
    [NotNull]
    public static XName NameAttributeXName => XNamespace.None + "name";

    /// <summary>
    ///   psf:Property
    /// </summary>
    [PublicAPI]
    [NotNull]
    public static XName PropertyElementXName => XpsServer.PrinterSchemaFrameworkXNamespace + "Property";

    /// <summary>
    ///   psf:ScoredProperty
    /// </summary>
    [PublicAPI]
    [NotNull]
    public static XName ScoredPropertyElementXName => XpsServer.PrinterSchemaFrameworkXNamespace + "ScoredProperty";

    /// <summary>
    ///   psf:Option
    /// </summary>
    [PublicAPI]
    [NotNull]
    public static XName OptionElementXName => XpsServer.PrinterSchemaFrameworkXNamespace + "Option";

    /// <summary>
    ///   psf:Value
    /// </summary>
    [PublicAPI]
    [NotNull]
    public static XName ValueElementXName => XpsServer.PrinterSchemaFrameworkXNamespace + "Value";

    /// <summary>
    ///   psk:DisplayName
    /// </summary>
    [PublicAPI]
    [NotNull]
    public static XName DisplayNameXName => XpsServer.PrinterSchemaKeywordsXNamespace + "DisplayName";

    /// <summary>
    ///   psk:PageInputBin
    /// </summary>
    [PublicAPI]
    [NotNull]
    public static XName PageInputBinXName => XpsServer.PrinterSchemaKeywordsXNamespace + "PageInputBin";

    /// <summary>
    ///   psk:DocumentInputBin
    /// </summary>
    [PublicAPI]
    [NotNull]
    public static XName DocumentInputBinXName => XpsServer.PrinterSchemaKeywordsXNamespace + "DocumentInputBin";

    /// <summary>
    ///   psk:JobInputBin
    /// </summary>
    [PublicAPI]
    [NotNull]
    public static XName JobInputBinXName => XpsServer.PrinterSchemaKeywordsXNamespace + "JobInputBin";

    /// <summary>
    ///   psk:PageImageableSize
    /// </summary>
    [PublicAPI]
    [NotNull]
    public static XName PageImageableSizeXName => XpsServer.PrinterSchemaKeywordsXNamespace + "PageImageableSize";

    /// <summary>
    ///   psk:ImageableSizeWidth
    /// </summary>
    [PublicAPI]
    [NotNull]
    public static XName ImageableSizeWidthXName => XpsServer.PrinterSchemaKeywordsXNamespace + "ImageableSizeWidth";

    /// <summary>
    ///   psk:ImageableSizeHeight
    /// </summary>
    [PublicAPI]
    [NotNull]
    public static XName ImageableSizeHeightXName => XpsServer.PrinterSchemaKeywordsXNamespace + "ImageableSizeHeight";

    /// <summary>
    ///   psk:PageMediaSize
    /// </summary>
    [PublicAPI]
    [NotNull]
    public static XName PageMediaSizeXName => XpsServer.PrinterSchemaKeywordsXNamespace + "PageMediaSize";

    /// <summary>
    ///   psk:MediaSizeWidth
    /// </summary>
    [PublicAPI]
    [NotNull]
    public static XName MediaSizeWidthXName => XpsServer.PrinterSchemaKeywordsXNamespace + "MediaSizeWidth";

    /// <summary>
    ///   psk:MediaSizeHeight
    /// </summary>
    [PublicAPI]
    [NotNull]
    public static XName MediaSizeHeightXName => XpsServer.PrinterSchemaKeywordsXNamespace + "MediaSizeHeight";

    /// <summary>
    ///   psk:FeedType
    /// </summary>
    [PublicAPI]
    [NotNull]
    public static XName FeedTypeXName => XpsServer.PrinterSchemaKeywordsXNamespace + "FeedType";

    /// <summary>
    ///   xsi:type
    /// </summary>
    [PublicAPI]
    [NotNull]
    public static XName TypeXName => XpsServer.XmlSchemaInstanceXNamespace + "type";

    /// <summary>
    ///   xsd:integer
    /// </summary>
    [PublicAPI]
    [NotNull]
    public static XName IntegerTypeXName => XpsServer.XmlSchemaXNamespace + "integer";

    /// <summary>
    ///   xsd:string
    /// </summary>
    [PublicAPI]
    [NotNull]
    public static XName StringTypeXName => XpsServer.XmlSchemaXNamespace + "string";

    /// <summary>
    ///   xsd:QName
    /// </summary>
    [PublicAPI]
    [NotNull]
    public static XName QNameTypeXName => XpsServer.XmlSchemaXNamespace + "QName";
  }
}
