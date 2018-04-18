using System.Xml.Linq;
using JetBrains.Annotations;

namespace Contrib.System.Printing.Xps
{
  public partial class XpsServer
  {
    /// <summary>
    ///   xsi
    /// </summary>
    [PublicAPI]
    [NotNull]
    public static XNamespace XmlSchemaInstanceXNamespace { get; } = XNamespace.Get("http://www.w3.org/2001/XMLSchema-instance");

    /// <summary>
    ///   xsd
    /// </summary>
    [PublicAPI]
    [NotNull]
    public static XNamespace XmlSchemaXNamespace { get; } = XNamespace.Get("http://www.w3.org/2001/XMLSchema");

    /// <summary>
    ///   psf
    /// </summary>
    [PublicAPI]
    [NotNull]
    public static XNamespace PrinterSchemaFrameworkXNamespace { get; } = XNamespace.Get("http://schemas.microsoft.com/windows/2003/08/printing/printschemaframework");

    /// <summary>
    ///   psk
    /// </summary>
    [PublicAPI]
    [NotNull]
    public static XNamespace PrinterSchemaKeywordsXNamespace { get; } = XNamespace.Get("http://schemas.microsoft.com/windows/2003/08/printing/printschemakeywords");
  }
}
