/** @pp
 * rootnamespace: Contrib.System
 */
namespace Contrib.System.Printing.Xps.ExtensionMethods
{
  using global::System;
  using global::System.IO;
  using global::System.Printing;
  using global::System.Xml.Linq;
  using global::Anotar.LibLog;
  using global::JetBrains.Annotations;

  /// <summary>
  ///   Provides extensions for <see cref="T:System.Printing.PrintQueue"/> objects.
  /// </summary>
#if CONTRIB_SYSTEM_PRINTING_XPS
  public
#else
  internal
#endif
  static partial class PrintQueueExtensions
  {
    /// <summary>
    ///   Gets an <see cref="T:System.Xml.Linq.XDocument"/> object that specifies the printer's capabilities that complies with the Print Schema (see https://msdn.microsoft.com/en-us/library/windows/desktop/ms715274).
    /// </summary>
    /// <param name="printQueue"/>
    /// <param name="printTicket"/>
    /// <seealso cref="M:System.Printing.PrintQueue.GetPrintCapabilitiesAsXml(System.Printing.PrintTicket)"/>
    /// <exception cref="T:System.ArgumentNullException"><paramref name="printQueue"/> is <see langword="null"/>.</exception>
    /// <exception cref="T:System.Exception"/>
    [Pure]
    [CanBeNull]
    public static XDocument GetPrintCapabilitiesAsXDocument([NotNull] this PrintQueue printQueue,
                                                            [CanBeNull] PrintTicket printTicket)
    {
      if (printQueue == null)
      {
        throw new ArgumentNullException(nameof(printQueue));
      }

      XDocument result;
      try
      {
        MemoryStream memoryStream;
        if (printTicket == null)
        {
          memoryStream = printQueue.GetPrintCapabilitiesAsXml();
        }
        else
        {
          memoryStream = printQueue.GetPrintCapabilitiesAsXml(printTicket);
        }

        using (memoryStream)
        {
          result = XDocument.Load(memoryStream);
        }
      }
      catch (Exception exception)
      {
        LogTo.WarnException($"Could not query {nameof(PrintQueue)} '{printQueue.FullName}' for {nameof(PrintCapabilities)}.",
                            exception);
        result = null;
      }

      return result;
    }

    /// <summary>
    ///   Gets an <see cref="T:System.Xml.Linq.XDocument"/> object that represent the default print ticket.
    /// </summary>
    /// <param name="printQueue"/>
    /// <seealso cref="M:System.Printing.PrintQueue.UserPrintTicket"/>
    /// <exception cref="T:System.ArgumentNullException"><paramref name="printQueue"/> is <see langword="null"/>.</exception>
    /// <exception cref="T:System.Exception"/>
    [Pure]
    [CanBeNull]
    public static XDocument GetPrintTicketAsXDocument([NotNull] this PrintQueue printQueue)
    {
      if (printQueue == null)
      {
        throw new ArgumentNullException(nameof(printQueue));
      }

      XDocument result;
      try
      {
        using (var memoryStream = printQueue.UserPrintTicket.GetXmlStream())
        {
          result = XDocument.Load(memoryStream);
        }
      }
      catch (Exception exception)
      {
        LogTo.WarnException($"Could not query {nameof(PrintQueue)} '{printQueue.FullName}' for {nameof(PrintTicket)}.",
                            exception);
        result = null;
      }

      return result;
    }
  }
}
