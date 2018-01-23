using JetBrains.Annotations;

namespace Contrib.System.Printing.Xps
{
  public interface IXpsInputBinDefinition
  {
    [NotNull]
    IXpsPrinterDefinition XpsPrinterDefinition { get; }

    [NotNull]
    string Name { get; }

    [NotNull]
    string DisplayName { get; }

    [CanBeNull]
    double? PageWidth { get; }

    [CanBeNull]
    double? PageHeight { get; }

    [NotNull]
    string NamespaceUri { get; }
  }
}
