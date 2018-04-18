using System.Xml.Linq;
using JetBrains.Annotations;

namespace Contrib.System.Printing.Xps
{
  public partial class XpsServer
  {
    /// <summary>
    /// 
    /// </summary>
    /// <returns>psf:PrintCapabilities</returns>
    [PublicAPI]
    [NotNull]
    public static XName PrintCapabilitiesXName => XpsServer.PrinterSchemaFrameworkXNamespace + "PrintCapabilities";

    /// <summary>
    /// 
    /// </summary>
    /// <returns>psf:Feature</returns>
    [PublicAPI]
    [NotNull]
    public static XName FeatureElementXName => XpsServer.PrinterSchemaFrameworkXNamespace + "Feature";

    /// <summary>
    /// 
    /// </summary>
    /// <returns>name</returns>
    [PublicAPI]
    [NotNull]
    public static XName NameAttributeXName => XNamespace.None + "name";

    /// <summary>
    /// 
    /// </summary>
    /// <returns>psf:Property</returns>
    [PublicAPI]
    [NotNull]
    public static XName PropertyElementXName => XpsServer.PrinterSchemaFrameworkXNamespace + "Property";

    /// <summary>
    /// 
    /// </summary>
    /// <returns>psf:ScoredProperty</returns>
    [PublicAPI]
    [NotNull]
    public static XName ScoredPropertyElementXName => XpsServer.PrinterSchemaFrameworkXNamespace + "ScoredProperty";

    /// <summary>
    /// 
    /// </summary>
    /// <returns>psf:Option</returns>
    [PublicAPI]
    [NotNull]
    public static XName OptionElementXName => XpsServer.PrinterSchemaFrameworkXNamespace + "Option";

    /// <summary>
    /// 
    /// </summary>
    /// <returns>psf:Value</returns>
    [PublicAPI]
    [NotNull]
    public static XName ValueElementXName => XpsServer.PrinterSchemaFrameworkXNamespace + "Value";

    /// <summary>
    /// 
    /// </summary>
    /// <returns>psk:DisplayName</returns>
    [PublicAPI]
    [NotNull]
    public static XName DisplayNameXName => XpsServer.PrinterSchemaKeywordsXNamespace + "DisplayName";

    /// <summary>
    /// 
    /// </summary>
    /// <returns>psk:PageInputBin</returns>
    [PublicAPI]
    [NotNull]
    public static XName PageInputBinXName => XpsServer.PrinterSchemaKeywordsXNamespace + "PageInputBin";

    /// <summary>
    /// 
    /// </summary>
    /// <returns>psk:DocumentInputBin</returns>
    [PublicAPI]
    [NotNull]
    public static XName DocumentInputBinXName => XpsServer.PrinterSchemaKeywordsXNamespace + "DocumentInputBin";

    /// <summary>
    /// 
    /// </summary>
    /// <returns>psk:JobInputBin</returns>
    [PublicAPI]
    [NotNull]
    public static XName JobInputBinXName => XpsServer.PrinterSchemaKeywordsXNamespace + "JobInputBin";

    /// <summary>
    /// 
    /// </summary>
    /// <returns>psk:PageImageableSize</returns>
    [PublicAPI]
    [NotNull]
    public static XName PageImageableSizeXName => XpsServer.PrinterSchemaKeywordsXNamespace + "PageImageableSize";

    /// <summary>
    /// 
    /// </summary>
    /// <returns>psk:ImageableSizeWidth</returns>
    [PublicAPI]
    [NotNull]
    public static XName ImageableSizeWidthXName => XpsServer.PrinterSchemaKeywordsXNamespace + "ImageableSizeWidth";

    /// <summary>
    /// 
    /// </summary>
    /// <returns>psk:ImageableSizeHeight</returns>
    [PublicAPI]
    [NotNull]
    public static XName ImageableSizeHeightXName => XpsServer.PrinterSchemaKeywordsXNamespace + "ImageableSizeHeight";

    /// <summary>
    /// 
    /// </summary>
    /// <returns>psk:PageMediaSize</returns>
    [PublicAPI]
    [NotNull]
    public static XName PageMediaSizeXName => XpsServer.PrinterSchemaKeywordsXNamespace + "PageMediaSize";

    /// <summary>
    /// 
    /// </summary>
    /// <returns>psk:MediaSizeWidth</returns>
    [PublicAPI]
    [NotNull]
    public static XName MediaSizeWidthXName => XpsServer.PrinterSchemaKeywordsXNamespace + "MediaSizeWidth";

    /// <summary>
    /// 
    /// </summary>
    /// <returns>psk:MediaSizeHeight</returns>
    [PublicAPI]
    [NotNull]
    public static XName MediaSizeHeightXName => XpsServer.PrinterSchemaKeywordsXNamespace + "MediaSizeHeight";

    /// <summary>
    /// 
    /// </summary>
    /// <returns>psk:FeedType</returns>
    [PublicAPI]
    [NotNull]
    public static XName FeedTypeXName => XpsServer.PrinterSchemaKeywordsXNamespace + "FeedType";

    /// <summary>
    /// 
    /// </summary>
    /// <returns>xsi:type</returns>
    [PublicAPI]
    [NotNull]
    public static XName TypeXName => XpsServer.XmlSchemaInstanceXNamespace + "type";

    /// <summary>
    /// 
    /// </summary>
    /// <returns>xsd:integer</returns>
    [PublicAPI]
    [NotNull]
    public static XName IntegerTypeXName => XpsServer.XmlSchemaXNamespace + "integer";

    /// <summary>
    /// 
    /// </summary>
    /// <returns>xsd:string</returns>
    [PublicAPI]
    [NotNull]
    public static XName StringTypeXName => XpsServer.XmlSchemaXNamespace + "string";

    /// <summary>
    /// 
    /// </summary>
    /// <returns>xsd:QName</returns>
    [PublicAPI]
    [NotNull]
    public static XName QNameTypeXName => XpsServer.XmlSchemaXNamespace + "QName";
  }
}
