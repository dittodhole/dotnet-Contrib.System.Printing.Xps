using System.Printing;
using JetBrains.Annotations;

namespace Contrib.System.Printing.Xps
{
  public interface IComplexXpsPrinterDefinition
  {
    [NotNull]
    PrintQueue PrintQueue { get; }

    [NotNull]
    IXpsPrinterDefinition XpsPrinterDefinition { get; }
  }
}
