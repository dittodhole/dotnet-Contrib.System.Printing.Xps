/** @pp
 * rootnamespace: Contrib.System.Printing.Xps
 */
namespace Contrib.System.Printing.Xps
{
  using global::System.Xml.Linq;
  using global::JetBrains.Annotations;

#if CONTRIB_SYSTEM_PRINTING_XPS
  public
#else
  internal
#endif
  partial class XpsServer
  {
    /// <summary>
    ///   &lt;psf:PrintCapabilities/&gt;
    /// </summary>
    [PublicAPI]
    [NotNull]
    public static XElement PrintCapabilitiesElement => new XElement(XpsServer.PrintCapabilitiesName);

    /// <summary>
    ///   &lt;psf:PrintTicket/&gt;
    /// </summary>
    [PublicAPI]
    [NotNull]
    public static XElement PrintTicketElement => new XElement(XpsServer.PrintTicketName);
  }
}
