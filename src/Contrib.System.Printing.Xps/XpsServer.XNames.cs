using System.Xml.Linq;
using JetBrains.Annotations;

namespace Contrib.System.Printing.Xps
{
  public partial class XpsServer
  {
    /// <returns>psf:PrintCapabilities</returns>
    [NotNull]
    public static XName PrintCapabilitiesXName => XpsServer.PrinterSchemaFrameworkXNamespace + "PrintCapabilities";

    /// <returns>psf:Feature</returns>
    [NotNull]
    public static XName FeatureElementXName => XpsServer.PrinterSchemaFrameworkXNamespace + "Feature";

    /// <returns>name</returns>
    [NotNull]
    public static XName NameAttributeXName => XNamespace.None + "name";

    /// <returns>psf:Property</returns>
    [NotNull]
    public static XName PropertyElementXName => XpsServer.PrinterSchemaFrameworkXNamespace + "Property";

    /// <returns>psf:ScoredProperty</returns>
    [NotNull]
    public static XName ScoredPropertyElementXName => XpsServer.PrinterSchemaFrameworkXNamespace + "ScoredProperty";

    /// <returns>psf:Option</returns>
    [NotNull]
    public static XName OptionElementXName => XpsServer.PrinterSchemaFrameworkXNamespace + "Option";

    /// <returns>psf:Value</returns>
    [NotNull]
    public static XName ValueElementXName => XpsServer.PrinterSchemaFrameworkXNamespace + "Value";

    /// <returns>psk:DisplayName</returns>
    [NotNull]
    public static XName DisplayNameXName => XpsServer.PrinterSchemaKeywordsXNamespace + "DisplayName";

    /// <returns>psk:PageInputBin</returns>
    [NotNull]
    public static XName PageInputBinXName => XpsServer.PrinterSchemaKeywordsXNamespace + "PageInputBin";

    /// <returns>psk:DocumentInputBin</returns>
    [NotNull]
    public static XName DocumentInputBinXName => XpsServer.PrinterSchemaKeywordsXNamespace + "DocumentInputBin";

    /// <returns>psk:JobInputBin</returns>
    [NotNull]
    public static XName JobInputBinXName => XpsServer.PrinterSchemaKeywordsXNamespace + "JobInputBin";

    /// <returns>psk:PageImageableSize</returns>
    [NotNull]
    public static XName PageImageableSizeXName => XpsServer.PrinterSchemaKeywordsXNamespace + "PageImageableSize";

    /// <returns>psk:ImageableSizeWidth</returns>
    [NotNull]
    public static XName ImageableSizeWidthXName => XpsServer.PrinterSchemaKeywordsXNamespace + "ImageableSizeWidth";

    /// <returns>psk:ImageableSizeHeight</returns>
    [NotNull]
    public static XName ImageableSizeHeightXName => XpsServer.PrinterSchemaKeywordsXNamespace + "ImageableSizeHeight";

    /// <returns>psk:PageMediaSize</returns>
    [NotNull]
    public static XName PageMediaSizeXName => XpsServer.PrinterSchemaKeywordsXNamespace + "PageMediaSize";

    /// <returns>psk:MediaSizeWidth</returns>
    [NotNull]
    public static XName MediaSizeWidthXName => XpsServer.PrinterSchemaKeywordsXNamespace + "MediaSizeWidth";

    /// <returns>psk:MediaSizeHeight</returns>
    [NotNull]
    public static XName MediaSizeHeightXName => XpsServer.PrinterSchemaKeywordsXNamespace + "MediaSizeHeight";

    /// <returns>psk:FeedType</returns>
    [NotNull]
    public static XName FeedTypeXName => XpsServer.PrinterSchemaKeywordsXNamespace + "FeedType";

    /// <returns>xsi:type</returns>
    [NotNull]
    public static XName TypeXName => XpsServer.XmlSchemaInstanceXNamespace + "type";

    /// <returns>xsd:integer</returns>
    [NotNull]
    public static XName IntegerTypeXName => XpsServer.XmlSchemaXNamespace + "integer";

    /// <returns>xsd:string</returns>
    [NotNull]
    public static XName StringTypeXName => XpsServer.XmlSchemaXNamespace + "string";

    /// <returns>xsd:QName</returns>
    [NotNull]
    public static XName QNameTypeXName => XpsServer.XmlSchemaXNamespace + "QName";
  }
}
