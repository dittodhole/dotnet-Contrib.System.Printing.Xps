using System.Xml.Linq;
using JetBrains.Annotations;

namespace Contrib.System.Printing.Xps
{
  public partial class XpsServer
  {
    /// <returns>xsi:</returns>
    [NotNull]
    public static XNamespace XmlSchemaInstanceXNamespace { get; } = XNamespace.Get("http://www.w3.org/2001/XMLSchema-instance");

    /// <returns>xsd:</returns>
    [NotNull]
    public static XNamespace XmlSchemaXNamespace { get; } = XNamespace.Get("http://www.w3.org/2001/XMLSchema");

    /// <returns>psf:</returns>
    [NotNull]
    public static XNamespace PrinterSchemaFrameworkXNamespace { get; } = XNamespace.Get("http://schemas.microsoft.com/windows/2003/08/printing/printschemaframework");

    /// <returns>psk:</returns>
    [NotNull]
    public static XNamespace PrinterSchemaKeywordsXNamespace { get; } = XNamespace.Get("http://schemas.microsoft.com/windows/2003/08/printing/printschemakeywords");
  }
}
