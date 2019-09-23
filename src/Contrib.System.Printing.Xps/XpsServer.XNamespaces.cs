/** @pp
 * rootnamespace: Contrib.System
 */
namespace Contrib.System.Printing.Xps
{
  using global::System.Xml.Linq;
  using global::JetBrains.Annotations;

  partial class XpsServer
  {
    /// <summary>
    ///   xsi
    /// </summary>
    [NotNull]
    public static XNamespace XmlSchemaInstanceNamespace { get; } = XNamespace.Get("http://www.w3.org/2001/XMLSchema-instance");

    /// <summary>
    ///   xsd
    /// </summary>
    [NotNull]
    public static XNamespace XmlSchemaNamespace { get; } = XNamespace.Get("http://www.w3.org/2001/XMLSchema");

    /// <summary>
    ///   psf
    /// </summary>
    [NotNull]
    public static XNamespace PrinterSchemaFrameworkNamespace { get; } = XNamespace.Get("http://schemas.microsoft.com/windows/2003/08/printing/printschemaframework");

    /// <summary>
    ///   psk
    /// </summary>
    [NotNull]
    public static XNamespace PrinterSchemaKeywordsNamespace { get; } = XNamespace.Get("http://schemas.microsoft.com/windows/2003/08/printing/printschemakeywords");
  }
}
