/** @pp
 * rootnamespace: Contrib.System.Printing.Xps
 */
namespace Contrib.System.Printing.Xps
{
  using global::System.Xml.Linq;
  using global::JetBrains.Annotations;

  public partial class XpsServer
  {
    /// <summary>
    ///   xsi
    /// </summary>
    [PublicAPI]
    [NotNull]
    public static XNamespace XmlSchemaInstanceNamespace { get; } = XNamespace.Get("http://www.w3.org/2001/XMLSchema-instance");

    /// <summary>
    ///   xsd
    /// </summary>
    [PublicAPI]
    [NotNull]
    public static XNamespace XmlSchemaNamespace { get; } = XNamespace.Get("http://www.w3.org/2001/XMLSchema");

    /// <summary>
    ///   psf
    /// </summary>
    [PublicAPI]
    [NotNull]
    public static XNamespace PrinterSchemaFrameworkNamespace { get; } = XNamespace.Get("http://schemas.microsoft.com/windows/2003/08/printing/printschemaframework");

    /// <summary>
    ///   psk
    /// </summary>
    [PublicAPI]
    [NotNull]
    public static XNamespace PrinterSchemaKeywordsNamespace { get; } = XNamespace.Get("http://schemas.microsoft.com/windows/2003/08/printing/printschemakeywords");
  }
}
