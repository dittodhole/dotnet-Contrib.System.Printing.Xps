using JetBrains.Annotations;

namespace Contrib.System.Printing.Xps
{
  public interface IXpsPrinterDefinition
  {
    [NotNull]
    string HostingMachineName { get; }

    [NotNull]
    string Name { get; }

    [NotNull]
    string FullName { get; }

    [NotNull]
    string CustomNamespaceUri { get; }

    [NotNull]
    string CustomNamespacePrefix { get; }

    [CanBeNull]
    double? DefaultPageWidth { get; }

    [CanBeNull]
    double? DefaultPageHeight { get; }
  }
}
