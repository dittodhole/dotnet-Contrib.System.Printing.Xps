﻿using System;
using System.Printing;
using JetBrains.Annotations;

namespace Contrib.System.Printing.Xps.ExtensionMethods
{
  public static class PrintServerExtensions
  {
    /// <exception cref="ArgumentNullException"><paramref name="printServer" /> is <see langword="null" /></exception>
    /// <exception cref="Exception" />
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
