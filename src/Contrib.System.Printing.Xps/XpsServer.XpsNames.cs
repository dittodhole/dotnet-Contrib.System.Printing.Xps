/** @pp
 * rootnamespace: Contrib.System
 */
namespace Contrib.System.Printing.Xps
{
  using global::JetBrains.Annotations;
  using global::Contrib.System.Printing.Xps.ExtensionMethods;

  partial class XpsServer
  {
    /// <summary>
    ///   psk:DisplayName
    /// </summary>
    [NotNull]
    public static XpsName DisplayNameName => XpsServer.PrinterSchemaKeywordsNamespace.GetXpsName("DisplayName");

    /// <summary>
    ///   psk:PageInputBin
    /// </summary>
    [NotNull]
    public static XpsName PageInputBinName => XpsServer.PrinterSchemaKeywordsNamespace.GetXpsName("PageInputBin");

    /// <summary>
    ///   psk:DocumentInputBin
    /// </summary>
    [NotNull]
    public static XpsName DocumentInputBinName => XpsServer.PrinterSchemaKeywordsNamespace.GetXpsName("DocumentInputBin");

    /// <summary>
    ///   psk:JobInputBin
    /// </summary>
    [NotNull]
    public static XpsName JobInputBinName => XpsServer.PrinterSchemaKeywordsNamespace.GetXpsName("JobInputBin");

    /// <summary>
    ///   psk:PageImageableSize
    /// </summary>
    [NotNull]
    public static XpsName PageImageableSizeName => XpsServer.PrinterSchemaKeywordsNamespace.GetXpsName("PageImageableSize");

    /// <summary>
    ///   psk:ImageableSizeWidth
    /// </summary>
    [NotNull]
    public static XpsName ImageableSizeWidthName => XpsServer.PrinterSchemaKeywordsNamespace.GetXpsName("ImageableSizeWidth");

    /// <summary>
    ///   psk:ImageableSizeHeight
    /// </summary>
    [NotNull]
    public static XpsName ImageableSizeHeightName => XpsServer.PrinterSchemaKeywordsNamespace.GetXpsName("ImageableSizeHeight");

    /// <summary>
    ///   psk:PageMediaSize
    /// </summary>
    [NotNull]
    public static XpsName PageMediaSizeName => XpsServer.PrinterSchemaKeywordsNamespace.GetXpsName("PageMediaSize");

    /// <summary>
    ///   psk:MediaSizeWidth
    /// </summary>
    [NotNull]
    public static XpsName MediaSizeWidthName => XpsServer.PrinterSchemaKeywordsNamespace.GetXpsName("MediaSizeWidth");

    /// <summary>
    ///   psk:MediaSizeHeight
    /// </summary>
    [NotNull]
    public static XpsName MediaSizeHeightName => XpsServer.PrinterSchemaKeywordsNamespace.GetXpsName("MediaSizeHeight");

    /// <summary>
    ///   psk:FeedType
    /// </summary>
    [NotNull]
    public static XpsName FeedTypeName => XpsServer.PrinterSchemaKeywordsNamespace.GetXpsName("FeedType");

    /// <summary>
    ///   psk:PageResolution
    /// </summary>
    [NotNull]
    public static XpsName PageResolutionName => XpsServer.PrinterSchemaKeywordsNamespace.GetXpsName("PageResolution");

    /// <summary>
    ///   psk:ResolutionX
    /// </summary>
    [NotNull]
    public static XpsName ResolutionXName => XpsServer.PrinterSchemaKeywordsNamespace.GetXpsName("ResolutionX");

    /// <summary>
    ///   psk:ResolutionY
    /// </summary>
    [NotNull]
    public static XpsName ResolutionYName => XpsServer.PrinterSchemaKeywordsNamespace.GetXpsName("ResolutionY");

    /// <summary>
    ///   psk:ImageableArea
    /// </summary>
    [NotNull]
    public static XpsName ImageableAreaName => XpsServer.PrinterSchemaKeywordsNamespace.GetXpsName("ImageableArea");

    /// <summary>
    ///   psk:OriginWidth
    /// </summary>
    [NotNull]
    public static XpsName OriginWidthName => XpsServer.PrinterSchemaKeywordsNamespace.GetXpsName("OriginWidth");

    /// <summary>
    ///   psk:OriginHeight
    /// </summary>
    [NotNull]
    public static XpsName OriginHeightName => XpsServer.PrinterSchemaKeywordsNamespace.GetXpsName("OriginHeight");
  }
}
