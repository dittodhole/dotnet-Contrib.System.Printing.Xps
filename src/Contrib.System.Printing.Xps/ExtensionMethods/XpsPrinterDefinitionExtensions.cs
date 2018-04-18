﻿using System;
using System.Printing;
using System.Windows.Documents;
using Anotar.LibLog;
using JetBrains.Annotations;

namespace Contrib.System.Printing.Xps.ExtensionMethods
{
  /// <summary>
  ///  Provides extensions to query <see cref="IXpsPrinterDefinition"/> instances
  /// </summary>
  [PublicAPI]
  public static partial class XpsPrinterDefinitionExtensions
  {
    /// <summary>
    /// 
    /// </summary>
    /// <param name="printQueue" />
    /// <exception cref="ArgumentNullException"><paramref name="printQueue" /> is <see langword="null" />.</exception>
    [CanBeNull]
    public delegate PrintTicket PrintTicketFactory([NotNull] PrintQueue printQueue);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="xpsPrinterDefinition" />
    /// <param name="documentPaginatorSource" />
    /// <param name="printTicketFactory" />
    /// <exception cref="ArgumentNullException"><paramref name="xpsPrinterDefinition" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="documentPaginatorSource" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="printTicketFactory" /> is <see langword="null" />.</exception>
    /// <exception cref="Exception" />
    public static void Print([NotNull] this IXpsPrinterDefinition xpsPrinterDefinition,
                             [NotNull] IDocumentPaginatorSource documentPaginatorSource,
                             [NotNull] [InstantHandle] PrintTicketFactory printTicketFactory)
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

      using (var printServer = new PrintServer())
      using (var printQueues = printServer.GetLocalAndRemotePrintQueues())
      {
        var printQueue = printQueues.FindPrintQueue(xpsPrinterDefinition);
        if (printQueue == null)
        {
          LogTo.Error($"Could not get {nameof(PrintQueue)} '{xpsPrinterDefinition.FullName}'.");
        }
        else
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
}
