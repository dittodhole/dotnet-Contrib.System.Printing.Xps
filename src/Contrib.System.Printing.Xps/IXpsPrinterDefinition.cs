using JetBrains.Annotations;

namespace Contrib.System.Printing.Xps
{
  public interface IXpsPrinterDefinition
  {
    [NotNull]
    string Name { get; }

    [NotNull]
    string FullName { get; }

    [CanBeNull]
    double? DefaultPageWidth { get; }

    [CanBeNull]
    double? DefaultPageHeight { get; }

    [CanBeNull]
    string PortName { get; }

    [CanBeNull]
    string DriverName { get; }
  }
}
