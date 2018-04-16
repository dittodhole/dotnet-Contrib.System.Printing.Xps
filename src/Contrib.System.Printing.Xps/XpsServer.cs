using System;
using JetBrains.Annotations;

namespace Contrib.System.Printing.Xps
{
  public partial interface IXpsServer
  {
    /// <exception cref="Exception" />
    [Pure]
    [NotNull]
    [ItemNotNull]
    IXpsPrinterDefinition[] GetXpsPrinterDefinitions();

    /// <exception cref="ArgumentNullException"><paramref name="xpsPrinterDefinition" /> is <see langword="null" />.</exception>
    /// <exception cref="Exception" />
    [Pure]
    [NotNull]
    [ItemNotNull]
    IXpsInputBinDefinition[] GetXpsInputBinDefinitions([NotNull] IXpsPrinterDefinition xpsPrinterDefinition);
  }

  public partial class XpsServer : XpsServerEx<IXpsPrinterDefinition, IXpsInputBinDefinition>,
                                   IXpsServer
  {
    public XpsServer()
      : base(new XpsPrinterDefinitionFactory(),
             new XpsInputBinDefinitionFactory()) { }

    /// <inheritdoc />
    public XpsServer([NotNull] IXpsPrinterDefinitionFactoryEx<IXpsPrinterDefinition> xpsPrinterDefinitionFactory,
                     [NotNull] IXpsInputBinDefinitionFactoryEx<IXpsInputBinDefinition> xpsInputBinDefinitionFactory)
      : base(xpsPrinterDefinitionFactory,
             xpsInputBinDefinitionFactory) { }
  }
}
