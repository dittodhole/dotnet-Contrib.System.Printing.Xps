/** @pp
 * rootnamespace: Contrib.System.Printing.Xps
 */
namespace Contrib.System.Printing.Xps.ExtensionMethods
{
  using global::System;
  using global::System.Printing;
  using global::JetBrains.Annotations;

  /// <summary>
  ///   Provides extensions for <see cref="PrintServer"/> objects.
  /// </summary>
  [PublicAPI]
#if CONTRIB_SYSTEM_PRINTING_XPS
  public
#else
  internal
#endif
  static partial class PrintServerExtensions
  {
    /// <summary>
    ///   Gets the collection of local and remote print queues that are hosted by the print server.
    /// </summary>
    /// <param name="printServer"/>
    /// <exception cref="ArgumentNullException"><paramref name="printServer"/> is <see langword="null"/>.</exception>
    /// <exception cref="Exception"/>
    [Pure]
    [NotNull]
    [ItemNotNull]
    public static PrintQueueCollection GetLocalAndRemotePrintQueues([NotNull] this PrintServer printServer)
    {
      if (printServer == null)
      {
        throw new ArgumentNullException(nameof(printServer));
      }

      var result = printServer.GetPrintQueues(new[]
                                              {
                                                EnumeratedPrintQueueTypes.Connections,
                                                EnumeratedPrintQueueTypes.Local
                                              });

      return result;
    }
  }
}
