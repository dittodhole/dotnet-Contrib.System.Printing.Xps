/** @pp
 * rootnamespace: Contrib.System
 */
namespace Contrib.System.Printing.Xps.ExtensionMethods
{
  using global::System;
  using global::System.Linq;
  using global::System.Printing;
  using global::JetBrains.Annotations;

  /// <summary>
  ///   Provides extensions for <see cref="PrintQueueCollection"/> objects.
  /// </summary>
  [PublicAPI]
#if CONTRIB_SYSTEM_PRINTING_XPS
  public
#else
  internal
#endif
  static partial class PrintQueueCollectionExtensions
  {
    /// <summary>
    ///   Finds the matching print queue.
    /// </summary>
    /// <param name="printQueueCollection"/>
    /// <param name="xpsPrinterDefinition"/>
    /// <exception cref="ArgumentNullException"><paramref name="printQueueCollection"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="xpsPrinterDefinition"/> is <see langword="null"/>.</exception>
    /// <exception cref="Exception"/>
    [Pure]
    [CanBeNull]
    public static PrintQueue FindPrintQueue([NotNull] this PrintQueueCollection printQueueCollection,
                                            [NotNull] IXpsPrinterDefinition xpsPrinterDefinition)
    {
      if (printQueueCollection == null)
      {
        throw new ArgumentNullException(nameof(printQueueCollection));
      }
      if (xpsPrinterDefinition == null)
      {
        throw new ArgumentNullException(nameof(xpsPrinterDefinition));
      }

      var result = printQueueCollection.FirstOrDefault(arg => string.Equals(arg.FullName,
                                                                            xpsPrinterDefinition.FullName,
                                                                            StringComparison.Ordinal));

      return result;
    }
  }
}
