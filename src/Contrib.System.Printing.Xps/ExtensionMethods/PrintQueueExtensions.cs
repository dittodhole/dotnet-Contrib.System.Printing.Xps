/** @pp
 * rootnamespace: Contrib.System.Printing.Xps
 */
namespace Contrib.System.Printing.Xps.ExtensionMethods
{
  using global::System;
  using global::System.Printing;
  using global::System.Xml.Linq;
  using global::Anotar.LibLog;
  using global::JetBrains.Annotations;

  /// <summary>
  ///   Provides extensions for <see cref="PrintQueue"/> objects.
  /// </summary>
  [PublicAPI]
  public static partial class PrintQueueExtensions
  {
    /// <summary>
    ///   Gets an <see cref="XDocument"/> object that specifies the printer's capabilities that complies with the Print Schema (see https://msdn.microsoft.com/en-us/library/windows/desktop/ms715274).
    /// </summary>
    /// <param name="printQueue"/>
    /// <seealso cref="PrintQueue.GetPrintCapabilitiesAsXml()"/>
    /// <exception cref="ArgumentNullException"><paramref name="printQueue"/> is <see langword="null"/>.</exception>
    [Pure]
    [NotNull]
    public static XDocument GetPrintCapabilitiesAsXDocument([NotNull] this PrintQueue printQueue)
    {
      if (printQueue == null)
      {
        throw new ArgumentNullException(nameof(printQueue));
      }

      XDocument result;
      try
      {
        using (var memoryStream = printQueue.GetPrintCapabilitiesAsXml())
        {
          result = XDocument.Load(memoryStream);
        }
      }
      catch (Exception exception)
      {
        LogTo.WarnException($"Could not query {nameof(PrintQueue)} '{printQueue.FullName}' for {nameof(PrintCapabilities)}.",
                            exception);
        result = new XDocument();
      }

      return result;
    }

    /// <summary>
    ///   Gets an <see cref="XDocument"/> object that specifies the printer's capabilities that complies with the Print Schema (see https://msdn.microsoft.com/en-us/library/windows/desktop/ms715274).
    /// </summary>
    /// <param name="printQueue"/>
    /// <param name="printTicket"/>
    /// <seealso cref="PrintQueue.GetPrintCapabilitiesAsXml(PrintTicket)"/>
    /// <exception cref="ArgumentNullException"><paramref name="printQueue"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="printTicket"/> is <see langword="null"/>.</exception>
    [Pure]
    [NotNull]
    public static XDocument GetPrintCapabilitiesAsXDocument([NotNull] this PrintQueue printQueue,
                                                            [NotNull] PrintTicket printTicket)
    {
      if (printQueue == null)
      {
        throw new ArgumentNullException(nameof(printQueue));
      }
      if (printTicket == null)
      {
        throw new ArgumentNullException(nameof(printTicket));
      }

      XDocument result;
      try
      {
        using (var memoryStream = printQueue.GetPrintCapabilitiesAsXml(printTicket))
        {
          result = XDocument.Load(memoryStream);
        }
      }
      catch (Exception exception)
      {
        LogTo.WarnException($"Could not query {nameof(PrintQueue)} '{printQueue.FullName}' for {nameof(PrintCapabilities)}.",
                            exception);
        result = new XDocument();
      }

      return result;
    }
  }
}
