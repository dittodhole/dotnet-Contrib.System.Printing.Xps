using System;
using System.Linq;
using System.Printing;
using JetBrains.Annotations;

namespace Contrib.System.Printing.Xps.ExtensionMethods
{
  public static class PrintQueueCollectionExtensions
  {
    /// <exception cref="ArgumentNullException"><paramref name="printQueueCollection" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="xpsPrinterDefinition" /> is <see langword="null" />.</exception>
    /// <exception cref="Exception" />
    [Pure]
    [CanBeNull]
    public static PrintQueue GetPrintQueue([NotNull] this PrintQueueCollection printQueueCollection,
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
