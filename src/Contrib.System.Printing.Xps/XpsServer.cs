/** @pp
 * rootnamespace: Contrib.System.Printing.Xps
 */
namespace Contrib.System.Printing.Xps
{
  using global::System;
  using global::System.Linq;
  using global::System.Printing;
  using global::System.Xml.Linq;
  using global::Anotar.LibLog;
  using global::Contrib.System.Printing.Xps.ExtensionMethods;
  using global::JetBrains.Annotations;

  /// <summary>
  ///   Provides <typeparamref name="TXpsPrinterDefinition"/> and <typeparamref name="TXpsInputBinDefinition"/> objects.
  /// </summary>
  /// <typeparam name="TXpsPrinterDefinition"/>
  /// <typeparam name="TXpsInputBinDefinition"/>
  /// <seealso cref="XpsServer{TXpsPrinterDefinition,TXpsInputBinDefinition}"/>
  [PublicAPI]
#if CONTRIB_SYSTEM_PRINTING_XPS
  public
#else
  internal
#endif
  partial interface IXpsServer<TXpsPrinterDefinition, TXpsInputBinDefinition>
    where TXpsPrinterDefinition : IXpsPrinterDefinition
    where TXpsInputBinDefinition : IXpsInputBinDefinition
  {
    /// <summary>
    ///   Gets the collection of printers hosted by the print server.
    /// </summary>
    /// <exception cref="Exception"/>
    [Pure]
    [NotNull]
    [ItemNotNull]
    TXpsPrinterDefinition[] GetXpsPrinterDefinitions();

    /// <summary>
    ///   Gets the collection of input bins for the specified printer hosted by the print server.
    /// </summary>
    /// <param name="xpsPrinterDefinition"/>
    /// <exception cref="ArgumentNullException"><paramref name="xpsPrinterDefinition"/> is <see langword="null"/>.</exception>
    /// <exception cref="Exception"/>
    [Pure]
    [NotNull]
    [ItemNotNull]
    TXpsInputBinDefinition[] GetXpsInputBinDefinitions([NotNull] TXpsPrinterDefinition xpsPrinterDefinition);
  }

  /// <inheritdoc />
  /// <seealso cref="XpsServer"/>
  [PublicAPI]
#if CONTRIB_SYSTEM_PRINTING_XPS
  public
#else
  internal
#endif
  partial interface IXpsServer : IXpsServer<IXpsPrinterDefinition, IXpsInputBinDefinition> { }

  /// <inheritdoc/>
#if CONTRIB_SYSTEM_PRINTING_XPS
  public
#else
  internal
#endif
  partial class XpsServer<TXpsPrinterDefinition, TXpsInputBinDefinition> : IXpsServer<TXpsPrinterDefinition, TXpsInputBinDefinition>
    where TXpsPrinterDefinition : IXpsPrinterDefinition
    where TXpsInputBinDefinition : IXpsInputBinDefinition
  {
    /// <summary>
    ///   Initializes a new instance of the <see cref="XpsServer{TXpsPrinterDefinition,TXpsInputBinDefinition}"/> class.
    /// </summary>
    /// <param name="xpsPrinterDefinitionFactory">Factory for <typeparamref name="TXpsPrinterDefinition"/>.</param>
    /// <param name="xpsInputBinDefinitionFactory">Factory for <typeparamref name="TXpsInputBinDefinition"/>.</param>
    /// <exception cref="ArgumentNullException"><paramref name="xpsPrinterDefinitionFactory"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="xpsInputBinDefinitionFactory"/> is <see langword="null"/>.</exception>
    [PublicAPI]
    public XpsServer([NotNull] IXpsPrinterDefinitionFactory<TXpsPrinterDefinition> xpsPrinterDefinitionFactory,
                     [NotNull] IXpsInputBinDefinitionFactory<TXpsInputBinDefinition> xpsInputBinDefinitionFactory)
    {
      this.XpsPrinterDefinitionFactory = xpsPrinterDefinitionFactory ?? throw new ArgumentNullException(nameof(xpsPrinterDefinitionFactory));
      this.XpsInputBinDefinitionFactory = xpsInputBinDefinitionFactory ?? throw new ArgumentNullException(nameof(xpsInputBinDefinitionFactory));
    }

    [NotNull]
    private IXpsPrinterDefinitionFactory<TXpsPrinterDefinition> XpsPrinterDefinitionFactory { get; }

    [NotNull]
    private IXpsInputBinDefinitionFactory<TXpsInputBinDefinition> XpsInputBinDefinitionFactory { get; }

    /// <inheritdoc/>
    public virtual TXpsPrinterDefinition[] GetXpsPrinterDefinitions()
    {
      TXpsPrinterDefinition[] result;
      using (var printServer = new PrintServer())
      using (var printQueues = printServer.GetLocalAndRemotePrintQueues())
      {
        result = printQueues.Select(printQueue =>
                                    {
                                      var printCapabilities = printQueue.GetPrintCapabilitiesAsXDocument(new PrintTicket())
                                                                        ?.Root ?? XpsServer.PrintCapabilitiesElement;

                                      var xpsPrinterDefinition = this.XpsPrinterDefinitionFactory.Create(printQueue.Name,
                                                                                                         printQueue.FullName,
                                                                                                         printQueue.QueuePort?.Name,
                                                                                                         printQueue.QueueDriver?.Name,
                                                                                                         printCapabilities);

                                      return xpsPrinterDefinition;
                                    })
                            .ToArray();
      }

      return result;
    }

    /// <inheritdoc/>
    public virtual TXpsInputBinDefinition[] GetXpsInputBinDefinitions(TXpsPrinterDefinition xpsPrinterDefinition)
    {
      if (xpsPrinterDefinition == null)
      {
        throw new ArgumentNullException(nameof(xpsPrinterDefinition));
      }

      TXpsInputBinDefinition[] result;
      using (var printServer = new PrintServer())
      using (var printQueues = printServer.GetLocalAndRemotePrintQueues())
      {
        var printQueue = printQueues.FindPrintQueue(xpsPrinterDefinition);
        if (printQueue == null)
        {
          LogTo.Warn($"Could not get {nameof(PrintQueue)} '{xpsPrinterDefinition.FullName}'.");
          result = new TXpsInputBinDefinition[0];
        }
        else
        {
          XElement feature;
          {
            var printCapabilities = printQueue.GetPrintCapabilitiesAsXDocument(new PrintTicket())
                                              ?.Root ?? XpsServer.PrintCapabilitiesElement;

            var prefix = printCapabilities.GetPrefixOfNamespace(XpsServer.PageInputBinName.Namespace);
            feature = printCapabilities.FindElementByNameAttribute(XpsServer.PageInputBinName.ToString(prefix));

            if (feature == null)
            {
              prefix = printCapabilities.GetPrefixOfNamespace(XpsServer.DocumentInputBinName.Namespace);
              feature = printCapabilities.FindElementByNameAttribute(XpsServer.DocumentInputBinName.ToString(prefix));
            }

            if (feature == null)
            {
              prefix = printCapabilities.GetPrefixOfNamespace(XpsServer.JobInputBinName.Namespace);
              feature = printCapabilities.FindElementByNameAttribute(XpsServer.JobInputBinName.ToString(prefix));
            }
          }

          if (feature == null)
          {
            result = new TXpsInputBinDefinition[0];
          }
          else
          {
            result = feature.Elements(XpsServer.OptionName)
                            .Select(option => new
                                              {
                                                Option = option,
                                                InputBinName = option.GetXpsName(option.Attribute(XpsServer.NameName)
                                                                                       ?.Value),
                                                Feature = feature,
                                                FeatureName = feature.GetXpsName(feature.Attribute(XpsServer.NameName)
                                                                                        ?.Value)
                                              })
                            .Where(arg => arg.InputBinName != null)
                            .Where(arg => arg.FeatureName != null)
                            .Select(arg =>
                                    {
                                      var printTicket = XpsServer.GetPrintTicket(arg.FeatureName,
                                                                                 arg.InputBinName);
                                      var printCapabilities = printQueue.GetPrintCapabilitiesAsXDocument(printTicket)
                                                                        ?.Root ?? XpsServer.PrintCapabilitiesElement;

                                      var xpsInputBinDefinition = this.XpsInputBinDefinitionFactory.Create(arg.FeatureName,
                                                                                                           arg.InputBinName,
                                                                                                           arg.Option,
                                                                                                           printCapabilities);

                                      return xpsInputBinDefinition;
                                    })
                            .ToArray();
          }
        }
      }

      return result;
    }
  }

  /// <inheritdoc cref="IXpsServer"/>
#if CONTRIB_SYSTEM_PRINTING_XPS
  public
#else
  internal
#endif
  partial class XpsServer : XpsServer<IXpsPrinterDefinition, IXpsInputBinDefinition>,
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
    /// <param name="xpsPrinterDefinitionFactory">Factory for <see cref="IXpsPrinterDefinition"/>.</param>
    /// <param name="xpsInputBinDefinitionFactory">Factory for <see cref="IXpsInputBinDefinition"/>.</param>
    [PublicAPI]
    public XpsServer([NotNull] IXpsPrinterDefinitionFactory<IXpsPrinterDefinition> xpsPrinterDefinitionFactory,
                     [NotNull] IXpsInputBinDefinitionFactory<IXpsInputBinDefinition> xpsInputBinDefinitionFactory)
      : base(xpsPrinterDefinitionFactory,
             xpsInputBinDefinitionFactory) { }
  }
}
