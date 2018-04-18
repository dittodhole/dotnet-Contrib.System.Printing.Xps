using System;
using System.Linq;
using System.Printing;
using JetBrains.Annotations;

namespace Contrib.System.Printing.Xps.ExtensionMethods
{
  /// <summary>
  ///   Provides extensions to query <see cref="PrintQueueCollection"/> instances.
  /// </summary>
  [PublicAPI]
  public static partial class PrintQueueCollectionExtensions
  {
    /// <summary>
    ///   Finds the <see cref="PrintQueue"/> instance with matching <seealso cref="PrintQueue.FullName"/> for <paramref name="xpsPrinterDefinition"/>.
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
