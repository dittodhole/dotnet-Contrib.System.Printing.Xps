using System;
using System.Printing;
using System.Windows.Documents;
using JetBrains.Annotations;

namespace Contrib.System.Printing.Xps.ExtensionMethods
{
  public static class XpsPrinterDefinitionExtensions
  {
    /// <exception cref="ArgumentNullException"><paramref name="xpsPrinterDefinition" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="documentPaginatorSource" /> is <see langword="null" />.</exception>
    /// <exception cref="Exception" />
    public static void Print([NotNull] this IXpsPrinterDefinition xpsPrinterDefinition,
                             [NotNull] IDocumentPaginatorSource documentPaginatorSource)
    {
      if (xpsPrinterDefinition == null)
      {
        throw new ArgumentNullException(nameof(xpsPrinterDefinition));
      }
      if (documentPaginatorSource == null)
      {
        throw new ArgumentNullException(nameof(documentPaginatorSource));
      }

      xpsPrinterDefinition.Print(documentPaginatorSource,
                                 printQueue => printQueue.UserPrintTicket ?? printQueue.DefaultPrintTicket);
    }

    /// <exception cref="ArgumentNullException"><paramref name="xpsPrinterDefinition" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="documentPaginatorSource" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="printTicketFactory" /> is <see langword="null" />.</exception>
    /// <exception cref="Exception" />
    public static void Print([NotNull] this IXpsPrinterDefinition xpsPrinterDefinition,
                             [NotNull] IDocumentPaginatorSource documentPaginatorSource,
                             [NotNull] PrintTicketFactory printTicketFactory)
    {
      if (xpsPrinterDefinition == null)
      {
        throw new ArgumentNullException(nameof(xpsPrinterDefinition));
      }
      if (documentPaginatorSource == null)
      {
        throw new ArgumentNullException(nameof(documentPaginatorSource));
      }
      if (printTicketFactory == null)
      {
        throw new ArgumentNullException(nameof(printTicketFactory));
      }

      using (var printServer = new PrintServer(xpsPrinterDefinition.HostingMachineName))
      using (var printQueue = printServer.GetPrintQueue(xpsPrinterDefinition.Name))
      {
        var xpsDocumentWriter = PrintQueue.CreateXpsDocumentWriter(printQueue);
        var printTicket = printTicketFactory.Invoke(printQueue);

        if (documentPaginatorSource is FixedDocumentSequence fixedDocumentSequence)
        {
          if (printTicket == null)
          {
            xpsDocumentWriter.Write(fixedDocumentSequence);
          }
          else
          {
            fixedDocumentSequence.PrintTicket = printTicket;
            xpsDocumentWriter.Write(fixedDocumentSequence,
                                    printTicket);
          }
        }
        else if (documentPaginatorSource is FixedDocument fixedDocument)
        {
          if (printTicket == null)
          {
            xpsDocumentWriter.Write(fixedDocument);
          }
          else
          {
            fixedDocument.PrintTicket = printTicket;
            xpsDocumentWriter.Write(fixedDocument,
                                    printTicket);
          }
        }
        else
        {
          if (printTicket == null)
          {
            xpsDocumentWriter.Write(documentPaginatorSource.DocumentPaginator);
          }
          else
          {
            xpsDocumentWriter.Write(documentPaginatorSource.DocumentPaginator,
                                    printTicket);
          }
        }
      }
    }
  }
}
