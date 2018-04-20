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
    ///   <psf:PrintCapabilities/>
    /// </summary>
    [PublicAPI]
    [NotNull]
    public static XElement PrintCapabilitiesElement => new XElement(XpsServer.PrintCapabilitiesName);

    /// <summary>
    ///   <psf:PrintTicket/>
    /// </summary>
    [PublicAPI]
    [NotNull]
    public static XElement PrintTicketElement => new XElement(XpsServer.PrintTicketName);
  }
}
