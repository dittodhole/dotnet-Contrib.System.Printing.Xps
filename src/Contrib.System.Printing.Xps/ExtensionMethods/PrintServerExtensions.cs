/** @pp
 * rootnamespace: Contrib.System.Printing.Xps
 */
namespace Contrib.System.Printing.Xps.ExtensionMethods
{
  using global::System;
  using global::System.Printing;
  using global::JetBrains.Annotations;

  /// <summary>
  ///   Provides extensions to query <see cref="PrintServer"/> instances.
  /// </summary>
  [PublicAPI]
  public static partial class PrintServerExtensions
  {
    /// <summary>
    ///   Gets local and remote <see cref="PrintQueue"/>-instances, wrapped in <see cref="PrintQueueCollection"/>.
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
