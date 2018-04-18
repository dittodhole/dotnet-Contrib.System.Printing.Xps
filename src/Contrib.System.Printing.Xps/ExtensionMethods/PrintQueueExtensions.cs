using System;
using System.Printing;
using System.Xml.Linq;
using Anotar.LibLog;
using JetBrains.Annotations;

namespace Contrib.System.Printing.Xps.ExtensionMethods
{
  /// <summary>
  ///  Provides extensions to query <see cref="PrintQueue"/> instances
  /// </summary>
  [PublicAPI]
  public static partial class PrintQueueExtensions
  {
    /// <summary>
    ///   Gets the print capabilities (see https://msdn.microsoft.com/en-us/library/windows/desktop/ms715274) as <see cref="XDocument"/>-instance.
    /// </summary>
    /// <param name="printQueue" />
    /// <exception cref="ArgumentNullException"><paramref name="printQueue" /> is <see langword="null" /></exception>
    [Pure]
    [NotNull]
    public static XDocument GetPrintCapabilitiesAsXDocument([NotNull] this PrintQueue printQueue)
    {
      if (printQueue == null)
      {
        throw new ArgumentNullException(nameof(printQueue));
      }

      XDocument printCapabilitiesXDocument;
      try
      {
        using (var memoryStream = printQueue.GetPrintCapabilitiesAsXml())
        {
          printCapabilitiesXDocument = XDocument.Load(memoryStream);
        }
      }
      catch (Exception exception)
      {
        LogTo.WarnException($"Could not query {nameof(PrintQueue)} '{printQueue.FullName}' for {nameof(PrintCapabilities)}.",
                            exception);
        printCapabilitiesXDocument = new XDocument();
      }

      return printCapabilitiesXDocument;
    }

    /// <summary>
    ///   Gets the print capabilities (see https://msdn.microsoft.com/en-us/library/windows/desktop/ms715274) as <see cref="XDocument"/>-instance.
    /// </summary>
    /// <param name="printQueue" />
    /// <param name="printTicket" />
    /// <exception cref="ArgumentNullException"><paramref name="printQueue" /> is <see langword="null" /></exception>
    /// <exception cref="ArgumentNullException"><paramref name="printTicket" /> is <see langword="null" /></exception>
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

      XDocument printCapabilitiesXDocument;
      try
      {
        using (var memoryStream = printQueue.GetPrintCapabilitiesAsXml(printTicket))
        {
          printCapabilitiesXDocument = XDocument.Load(memoryStream);
        }
      }
      catch (Exception exception)
      {
        LogTo.WarnException($"Could not query {nameof(PrintQueue)} '{printQueue.FullName}' for {nameof(PrintCapabilities)}.",
                            exception);
        printCapabilitiesXDocument = new XDocument();
      }

      return printCapabilitiesXDocument;
    }
  }
}
