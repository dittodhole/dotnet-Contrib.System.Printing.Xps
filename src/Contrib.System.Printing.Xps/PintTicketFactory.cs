using System;
using System.Printing;
using JetBrains.Annotations;

namespace Contrib.System.Printing.Xps
{
  /// <exception cref="ArgumentNullException"><paramref name="printQueue" /> is <see langword="null" />.</exception>
  [NotNull]
  public delegate PrintTicket PrintTicketFactory([NotNull] PrintQueue printQueue);
}
