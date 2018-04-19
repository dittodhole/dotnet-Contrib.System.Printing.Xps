using JetBrains.Annotations;

namespace Contrib.System.Printing.Xps
{
  /// <summary>
  ///   Provides <see cref="IXpsPrinterDefinition"/> and <see cref="IXpsInputBinDefinition"/> instances.
  /// </summary>
  /// <seealso cref="XpsServer"/>
  [PublicAPI]
  public partial interface IXpsServer : IXpsServerEx<IXpsPrinterDefinition, IXpsInputBinDefinition> { }

  /// <inheritdoc cref="IXpsServer"/>
  public partial class XpsServer : XpsServerEx<IXpsPrinterDefinition, IXpsInputBinDefinition>,
                                   IXpsServer
  {
    /// <summary>
    ///   Initializes a new instance of the <see cref="XpsServer"/> class.
    /// </summary>
    [PublicAPI]
    public XpsServer()
      : base(new XpsPrinterDefinitionFactory(),
             new XpsInputBinDefinitionFactory()) { }


    /// <summary>
    ///   Initializes a new instance of the <see cref="XpsServer"/> class.
    /// </summary>
    /// <param name="xpsPrinterDefinitionFactory"/>
    /// <param name="xpsInputBinDefinitionFactory"/>
    [PublicAPI]
    public XpsServer([NotNull] IXpsPrinterDefinitionFactory<IXpsPrinterDefinition> xpsPrinterDefinitionFactory,
                     [NotNull] IXpsInputBinDefinitionFactory<IXpsInputBinDefinition> xpsInputBinDefinitionFactory)
      : base(xpsPrinterDefinitionFactory,
             xpsInputBinDefinitionFactory) { }
  }
}
