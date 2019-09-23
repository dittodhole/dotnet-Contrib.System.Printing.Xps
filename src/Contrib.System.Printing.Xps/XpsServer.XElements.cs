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
    ///   &lt;psf:PrintCapabilities/&gt;
    /// </summary>
    [NotNull]
    public static XElement PrintCapabilitiesElement => new XElement(XpsServer.PrintCapabilitiesName);

    /// <summary>
    ///   &lt;psf:PrintTicket/&gt;
    /// </summary>
    [NotNull]
    public static XElement PrintTicketElement => new XElement(XpsServer.PrintTicketName);
  }
}
