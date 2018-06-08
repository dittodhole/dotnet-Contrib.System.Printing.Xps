/** @pp
 * rootnamespace: Contrib.System
 */
namespace Contrib.System.Printing.Xps
{
  using global::JetBrains.Annotations;
  using global::Contrib.System.Printing.Xps.ExtensionMethods;

#if CONTRIB_SYSTEM_PRINTING_XPS
  public
#else
  internal
#endif
  partial class XpsServer
  {
    /// <summary>
    ///   psk:DisplayName
    /// </summary>
    [PublicAPI]
    [NotNull]
    public static XpsName DisplayNameName => XpsServer.PrinterSchemaKeywordsNamespace.GetXpsName("DisplayName");

    /// <summary>
    ///   psk:PageInputBin
    /// </summary>
    [PublicAPI]
    [NotNull]
    public static XpsName PageInputBinName => XpsServer.PrinterSchemaKeywordsNamespace.GetXpsName("PageInputBin");

    /// <summary>
    ///   psk:DocumentInputBin
    /// </summary>
    [PublicAPI]
    [NotNull]
    public static XpsName DocumentInputBinName => XpsServer.PrinterSchemaKeywordsNamespace.GetXpsName("DocumentInputBin");

    /// <summary>
    ///   psk:JobInputBin
    /// </summary>
    [PublicAPI]
    [NotNull]
    public static XpsName JobInputBinName => XpsServer.PrinterSchemaKeywordsNamespace.GetXpsName("JobInputBin");

    /// <summary>
    ///   psk:PageImageableSize
    /// </summary>
    [PublicAPI]
    [NotNull]
    public static XpsName PageImageableSizeName => XpsServer.PrinterSchemaKeywordsNamespace.GetXpsName("PageImageableSize");

    /// <summary>
    ///   psk:ImageableSizeWidth
    /// </summary>
    [PublicAPI]
    [NotNull]
    public static XpsName ImageableSizeWidthName => XpsServer.PrinterSchemaKeywordsNamespace.GetXpsName("ImageableSizeWidth");

    /// <summary>
    ///   psk:ImageableSizeHeight
    /// </summary>
    [PublicAPI]
    [NotNull]
    public static XpsName ImageableSizeHeightName => XpsServer.PrinterSchemaKeywordsNamespace.GetXpsName("ImageableSizeHeight");

    /// <summary>
    ///   psk:PageMediaSize
    /// </summary>
    [PublicAPI]
    [NotNull]
    public static XpsName PageMediaSizeName => XpsServer.PrinterSchemaKeywordsNamespace.GetXpsName("PageMediaSize");

    /// <summary>
    ///   psk:MediaSizeWidth
    /// </summary>
    [PublicAPI]
    [NotNull]
    public static XpsName MediaSizeWidthName => XpsServer.PrinterSchemaKeywordsNamespace.GetXpsName("MediaSizeWidth");

    /// <summary>
    ///   psk:MediaSizeHeight
    /// </summary>
    [PublicAPI]
    [NotNull]
    public static XpsName MediaSizeHeightName => XpsServer.PrinterSchemaKeywordsNamespace.GetXpsName("MediaSizeHeight");

    /// <summary>
    ///   psk:FeedType
    /// </summary>
    [PublicAPI]
    [NotNull]
    public static XpsName FeedTypeName => XpsServer.PrinterSchemaKeywordsNamespace.GetXpsName("FeedType");

    /// <summary>
    ///   psk:PageResolution
    /// </summary>
    [PublicAPI]
    [NotNull]
    public static XpsName PageResolutionName => XpsServer.PrinterSchemaKeywordsNamespace.GetXpsName("PageResolution");

    /// <summary>
    ///   psk:ResolutionX
    /// </summary>
    [PublicAPI]
    [NotNull]
    public static XpsName ResolutionXName => XpsServer.PrinterSchemaKeywordsNamespace.GetXpsName("ResolutionX");

    /// <summary>
    ///   psk:ResolutionY
    /// </summary>
    [PublicAPI]
    [NotNull]
    public static XpsName ResolutionYName => XpsServer.PrinterSchemaKeywordsNamespace.GetXpsName("ResolutionY");
  }
}
