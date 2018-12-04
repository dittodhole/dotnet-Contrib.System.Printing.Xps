﻿/** @pp
 * rootnamespace: Contrib.System
 */
namespace Contrib.System.Printing.Xps.ExtensionMethods
{
  using global::System;
  using global::System.Printing;
  using global::JetBrains.Annotations;

  /// <summary>
  ///   Provides extensions for <see cref="T:System.Printing.PrintQueueCollection"/> objects.
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
    /// <exception cref="T:System.ArgumentNullException"><paramref name="printQueueCollection"/> is <see langword="null"/>.</exception>
    /// <exception cref="T:System.ArgumentNullException"><paramref name="xpsPrinterDefinition"/> is <see langword="null"/>.</exception>
    /// <exception cref="T:System.Exception"/>
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

      var result = default(PrintQueue);
      foreach (var printQueue in printQueueCollection)
      {
        var success = string.Equals(printQueue.FullName,
                                    xpsPrinterDefinition.FullName,
                                    StringComparison.Ordinal);
        if (success)
        {
          result = printQueue;
          break;
        }
      }

      return result;
    }
  }
}
