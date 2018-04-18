using System.Xml.Linq;
using JetBrains.Annotations;

namespace Contrib.System.Printing.Xps
{
  public partial class XpsServer
  {
    /// <returns>psf:PrintCapabilities</returns>
    [PublicAPI]
    [NotNull]
    public static XName PrintCapabilitiesXName => XpsServer.PrinterSchemaFrameworkXNamespace + "PrintCapabilities";

    /// <returns>psf:Feature</returns>
    [PublicAPI]
    [NotNull]
    public static XName FeatureElementXName => XpsServer.PrinterSchemaFrameworkXNamespace + "Feature";

    /// <returns>name</returns>
    [PublicAPI]
    [NotNull]
    public static XName NameAttributeXName => XNamespace.None + "name";

    /// <returns>psf:Property</returns>
    [PublicAPI]
    [NotNull]
    public static XName PropertyElementXName => XpsServer.PrinterSchemaFrameworkXNamespace + "Property";

    /// <returns>psf:ScoredProperty</returns>
    [PublicAPI]
    [NotNull]
    public static XName ScoredPropertyElementXName => XpsServer.PrinterSchemaFrameworkXNamespace + "ScoredProperty";

    /// <returns>psf:Option</returns>
    [PublicAPI]
    [NotNull]
    public static XName OptionElementXName => XpsServer.PrinterSchemaFrameworkXNamespace + "Option";

    /// <returns>psf:Value</returns>
    [PublicAPI]
    [NotNull]
    public static XName ValueElementXName => XpsServer.PrinterSchemaFrameworkXNamespace + "Value";

    /// <returns>psk:DisplayName</returns>
    [PublicAPI]
    [NotNull]
    public static XName DisplayNameXName => XpsServer.PrinterSchemaKeywordsXNamespace + "DisplayName";

    /// <returns>psk:PageInputBin</returns>
    [PublicAPI]
    [NotNull]
    public static XName PageInputBinXName => XpsServer.PrinterSchemaKeywordsXNamespace + "PageInputBin";

    /// <returns>psk:DocumentInputBin</returns>
    [PublicAPI]
    [NotNull]
    public static XName DocumentInputBinXName => XpsServer.PrinterSchemaKeywordsXNamespace + "DocumentInputBin";

    /// <returns>psk:JobInputBin</returns>
    [PublicAPI]
    [NotNull]
    public static XName JobInputBinXName => XpsServer.PrinterSchemaKeywordsXNamespace + "JobInputBin";

    /// <returns>psk:PageImageableSize</returns>
    [PublicAPI]
    [NotNull]
    public static XName PageImageableSizeXName => XpsServer.PrinterSchemaKeywordsXNamespace + "PageImageableSize";

    /// <returns>psk:ImageableSizeWidth</returns>
    [PublicAPI]
    [NotNull]
    public static XName ImageableSizeWidthXName => XpsServer.PrinterSchemaKeywordsXNamespace + "ImageableSizeWidth";

    /// <returns>psk:ImageableSizeHeight</returns>
    [PublicAPI]
    [NotNull]
    public static XName ImageableSizeHeightXName => XpsServer.PrinterSchemaKeywordsXNamespace + "ImageableSizeHeight";

    /// <returns>psk:PageMediaSize</returns>
    [PublicAPI]
    [NotNull]
    public static XName PageMediaSizeXName => XpsServer.PrinterSchemaKeywordsXNamespace + "PageMediaSize";

    /// <returns>psk:MediaSizeWidth</returns>
    [PublicAPI]
    [NotNull]
    public static XName MediaSizeWidthXName => XpsServer.PrinterSchemaKeywordsXNamespace + "MediaSizeWidth";

    /// <returns>psk:MediaSizeHeight</returns>
    [PublicAPI]
    [NotNull]
    public static XName MediaSizeHeightXName => XpsServer.PrinterSchemaKeywordsXNamespace + "MediaSizeHeight";

    /// <returns>psk:FeedType</returns>
    [PublicAPI]
    [NotNull]
    public static XName FeedTypeXName => XpsServer.PrinterSchemaKeywordsXNamespace + "FeedType";

    /// <returns>xsi:type</returns>
    [PublicAPI]
    [NotNull]
    public static XName TypeXName => XpsServer.XmlSchemaInstanceXNamespace + "type";

    /// <returns>xsd:integer</returns>
    [PublicAPI]
    [NotNull]
    public static XName IntegerTypeXName => XpsServer.XmlSchemaXNamespace + "integer";

    /// <returns>xsd:string</returns>
    [PublicAPI]
    [NotNull]
    public static XName StringTypeXName => XpsServer.XmlSchemaXNamespace + "string";

    /// <returns>xsd:QName</returns>
    [PublicAPI]
    [NotNull]
    public static XName QNameTypeXName => XpsServer.XmlSchemaXNamespace + "QName";
  }
}
