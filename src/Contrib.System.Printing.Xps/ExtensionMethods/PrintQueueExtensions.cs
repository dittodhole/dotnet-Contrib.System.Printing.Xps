/** @pp
 * rootnamespace: Contrib.System
 */
namespace Contrib.System.Printing.Xps.ExtensionMethods
{
  using global::System;
  using global::System.Printing;
  using global::System.Xml.Linq;
  using global::Anotar.LibLog;
  using global::JetBrains.Annotations;

  /// <summary>
  ///   Provides extensions for <see cref="T:System.Printing.PrintQueue"/> objects.
  /// </summary>
  [PublicAPI]
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
    /// <seealso cref="M:System.Printing.PrintQueue.GetPrintCapabilitiesAsXml()"/>
    /// <exception cref="T:System.ArgumentNullException"><paramref name="printQueue"/> is <see langword="null"/>.</exception>
    [Pure]
    [CanBeNull]
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
        result = null;
      }

      return result;
    }

    /// <summary>
    ///   Gets an <see cref="T:System.Xml.Linq.XDocument"/> object that specifies the printer's capabilities that complies with the Print Schema (see https://msdn.microsoft.com/en-us/library/windows/desktop/ms715274).
    /// </summary>
    /// <param name="printQueue"/>
    /// <param name="printTicket"/>
    /// <seealso cref="M:System.Printing.PrintQueue.GetPrintCapabilitiesAsXml(PrintTicket)"/>
    /// <exception cref="T:System.ArgumentNullException"><paramref name="printQueue"/> is <see langword="null"/>.</exception>
    /// <exception cref="T:System.ArgumentNullException"><paramref name="printTicket"/> is <see langword="null"/>.</exception>
    [Pure]
    [CanBeNull]
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
        result = null;
      }

      return result;
    }
  }
}
