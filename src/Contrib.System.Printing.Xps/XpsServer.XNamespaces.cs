using System.Xml.Linq;
using JetBrains.Annotations;

namespace Contrib.System.Printing.Xps
{
  public partial class XpsServer
  {
    /// <summary>
    /// 
    /// </summary>
    /// <returns>xsi:</returns>
    [PublicAPI]
    [NotNull]
    public static XNamespace XmlSchemaInstanceXNamespace { get; } = XNamespace.Get("http://www.w3.org/2001/XMLSchema-instance");

    /// <summary>
    /// 
    /// </summary>
    /// <returns>xsd:</returns>
    [PublicAPI]
    [NotNull]
    public static XNamespace XmlSchemaXNamespace { get; } = XNamespace.Get("http://www.w3.org/2001/XMLSchema");

    /// <summary>
    /// 
    /// </summary>
    /// <returns>psf:</returns>
    [PublicAPI]
    [NotNull]
    public static XNamespace PrinterSchemaFrameworkXNamespace { get; } = XNamespace.Get("http://schemas.microsoft.com/windows/2003/08/printing/printschemaframework");

    /// <summary>
    /// 
    /// </summary>
    /// <returns>psk:</returns>
    [PublicAPI]
    [NotNull]
    public static XNamespace PrinterSchemaKeywordsXNamespace { get; } = XNamespace.Get("http://schemas.microsoft.com/windows/2003/08/printing/printschemakeywords");
  }
}
