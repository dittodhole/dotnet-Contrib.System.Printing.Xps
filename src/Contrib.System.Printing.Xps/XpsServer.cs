using System;
using JetBrains.Annotations;

namespace Contrib.System.Printing.Xps
{
  /// <summary>
  ///   Provides <see cref="IXpsPrinterDefinition"/> and <see cref="IXpsInputBinDefinition"/> instances.
  /// </summary>
  /// <seealso cref="XpsServer"/>
  [PublicAPI]
  public partial interface IXpsServer
  {
    /// <summary>
    ///   Gets all <see cref="IXpsPrinterDefinition"/> instances.
    /// </summary>
    /// <exception cref="Exception"/>
    [Pure]
    [NotNull]
    [ItemNotNull]
    IXpsPrinterDefinition[] GetXpsPrinterDefinitions();

    /// <summary>
    ///   Gets all <see cref="IXpsInputBinDefinition"/> instances for <paramref name="xpsPrinterDefinition"/>.
    /// </summary>
    /// <param name="xpsPrinterDefinition"/>
    /// <exception cref="ArgumentNullException"><paramref name="xpsPrinterDefinition"/> is <see langword="null"/>.</exception>
    /// <exception cref="Exception"/>
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
    public XpsServer([NotNull] IXpsPrinterDefinitionFactoryEx<IXpsPrinterDefinition> xpsPrinterDefinitionFactory,
                     [NotNull] IXpsInputBinDefinitionFactoryEx<IXpsInputBinDefinition> xpsInputBinDefinitionFactory)
      : base(xpsPrinterDefinitionFactory,
             xpsInputBinDefinitionFactory) { }
  }
}
