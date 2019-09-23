/** @pp
 * rootnamespace: Contrib.System
 */
namespace Contrib.System.Printing.Xps.ExtensionMethods
{
  using global::System;
  using global::System.Printing;
  using global::System.Xml.Linq;
  using global::JetBrains.Annotations;

  /// <summary>
  ///   Provides extensions for <see cref="T:System.Printing.PrintQueue"/> objects.
  /// </summary>
#if CONTRIB_SYSTEM_PRINTING_XPS
  public
#else
  internal
#endif
  static partial class PrintTicketExtensions
  {
    /// <summary>
    ///   Gets an <see cref="T:System.Xml.Linq.XDocument"/> object that represent the print ticket.
    /// </summary>
    /// <param name="printTicket"/>
    /// <exception cref="T:System.ArgumentNullException"><paramref name="printTicket"/> is <see langword="null"/>.</exception>
    /// <exception cref="T:System.Exception"/>
    [Pure]
    [NotNull]
    public static XDocument GetXDocument([NotNull] this PrintTicket printTicket)
    {
      if (printTicket == null)
      {
        throw new ArgumentNullException(nameof(printTicket));
      }

      XDocument result;
      using (var memoryStream = printTicket.GetXmlStream())
      {
        result = XDocument.Load(memoryStream);
      }

      return result;
    }
  }
}
