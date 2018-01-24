using System;
using System.Printing;
using System.Windows.Documents;
using JetBrains.Annotations;

namespace Contrib.System.Printing.Xps.ExtensionMethods
{
  public static class XpsInputBinDefinitionExtensions
  {
    /// <exception cref="InvalidOperationException">If <paramref name="xpsInputBinDefinition" /> holds a prefix in <see cref="IXpsInputBinDefinition.Name" />, but does not provide a <see cref="IXpsInputBinDefinition.NamespaceUri" />.</exception>
    /// <exception cref="Exception" />
    [Pure]
    [NotNull]
    public static PrintTicket CreatePrintTicket([NotNull] this IXpsInputBinDefinition xpsInputBinDefinition)
    {
      var featureName = xpsInputBinDefinition.FeatureName;
      var inputBinName = xpsInputBinDefinition.Name;
      var namespacePrefix = xpsInputBinDefinition.NamespacePrefix;
      var namespaceUri = xpsInputBinDefinition.NamespaceUri;

      var printTicket = PrintTicketExtensions.CreatePrintTicket(featureName,
                                                                inputBinName,
                                                                namespacePrefix,
                                                                namespaceUri);

      return printTicket;
    }

    /// <exception cref="ArgumentNullException"><paramref name="xpsInputBinDefinition" /> is <see langword="null" />.</exception>
    [Pure]
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

      xpsPrinterDefinition.Print(documentPaginatorSource,
                                 printQueue => xpsInputBinDefinition.CreatePrintTicket());
    }
  }
}
