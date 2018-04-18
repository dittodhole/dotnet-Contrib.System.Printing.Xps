using System;
using JetBrains.Annotations;

namespace Contrib.System.Printing.Xps
{
  /// <summary>
  ///   Default implementation: <see cref="XpsServer"/>
  /// </summary>
  [PublicAPI]
  public partial interface IXpsServer
  {
    /// <summary>
    /// 
    /// </summary>
    /// <exception cref="Exception" />
    [Pure]
    [NotNull]
    [ItemNotNull]
    IXpsPrinterDefinition[] GetXpsPrinterDefinitions();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="xpsPrinterDefinition" />
    /// <exception cref="ArgumentNullException"><paramref name="xpsPrinterDefinition" /> is <see langword="null" />.</exception>
    /// <exception cref="Exception" />
    [Pure]
    [NotNull]
    [ItemNotNull]
    IXpsInputBinDefinition[] GetXpsInputBinDefinitions([NotNull] IXpsPrinterDefinition xpsPrinterDefinition);
  }

  /// <inheritdoc cref="IXpsServer"/>
  public partial class XpsServer : XpsServerEx<IXpsPrinterDefinition, IXpsInputBinDefinition>,
                                   IXpsServer
  {
    /// <summary>
    /// 
    /// </summary>
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
