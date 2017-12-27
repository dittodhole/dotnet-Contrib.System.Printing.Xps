using System;
using System.Printing;
using System.Windows.Documents;
using JetBrains.Annotations;

namespace Contrib.System.Printing.Xps.ExtensionMethods
{
  public static class XpsInputBinDefinitionExtensions
  {
    /// <exception cref="ArgumentNullException"><paramref name="xpsInputBinDefinition" /> is <see langword="null" />.</exception>
    public static InputBin GetInputBin([NotNull] this IXpsInputBinDefinition xpsInputBinDefinition)
    {
      if (xpsInputBinDefinition == null)
      {
        throw new ArgumentNullException(nameof(xpsInputBinDefinition));
      }

      var name = xpsInputBinDefinition.Name;

      const string namespacePrefix = "psk:";
      if (name.StartsWith(namespacePrefix,
                          StringComparison.Ordinal))
      {
        name = name.Substring(namespacePrefix.Length);
      }

      InputBin inputBin;
      if (!Enum.TryParse(name,
                         out inputBin))
      {
        inputBin = InputBin.Unknown;
      }

      return inputBin;
    }

    /// <exception cref="ArgumentNullException"><paramref name="xpsInputBinDefinition" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="documentPaginatorSource" /> is <see langword="null" />.</exception>
    /// <exception cref="Exception" />
    public static void Print([NotNull] this IXpsInputBinDefinition xpsInputBinDefinition,
                             [NotNull] IDocumentPaginatorSource documentPaginatorSource)
    {
      if (xpsInputBinDefinition == null)
      {
        throw new ArgumentNullException(nameof(xpsInputBinDefinition));
      }
      if (documentPaginatorSource == null)
      {
        throw new ArgumentNullException(nameof(documentPaginatorSource));
      }

      var xpsPrinterDefinition = xpsInputBinDefinition.XpsPrinterDefinition;

      using (var printServer = new PrintServer(xpsPrinterDefinition.HostingMachineName))
      {
        using (var printQueue = printServer.GetPrintQueue(xpsPrinterDefinition.Name))
        {
          var printTicket = printQueue.UserPrintTicket ?? printQueue.DefaultPrintTicket;

          printTicket = printTicket.With(xpsInputBinDefinition);

          var xpsDocumentWriter = PrintQueue.CreateXpsDocumentWriter(printQueue);

          if (documentPaginatorSource is FixedDocumentSequence fixedDocumentSequence)
          {
            fixedDocumentSequence.PrintTicket = printTicket;
            xpsDocumentWriter.Write(fixedDocumentSequence,
                                    printTicket);
          }
          else if (documentPaginatorSource is FixedDocument fixedDocument)
          {
            fixedDocument.PrintTicket = printTicket;
            xpsDocumentWriter.Write(fixedDocument,
                                    printTicket);
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
