/** @pp
 * rootnamespace: Contrib.System
 */
namespace Contrib.System.Printing.Xps
{
  using global::System;
  using global::System.IO;
  using global::System.Linq;
  using global::System.Printing;
  using global::System.Xml.Linq;
  using global::Contrib.System.Printing.Xps.ExtensionMethods;
  using global::JetBrains.Annotations;

  /// <summary>
  ///   Provides <typeparamref name="TXpsPrinterDefinition"/> and <typeparamref name="TXpsInputBinDefinition"/> objects.
  /// </summary>
  /// <typeparam name="TXpsPrinterDefinition"/>
  /// <typeparam name="TXpsInputBinDefinition"/>
  /// <seealso cref="T:Contrib.System.Printing.Xps.XpsServer{TXpsPrinterDefinition,TXpsInputBinDefinition}"/>
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
    /// <exception cref="T:System.Exception"/>
    [Pure]
    [NotNull]
    [ItemNotNull]
    TXpsPrinterDefinition[] GetXpsPrinterDefinitions();

    /// <summary>
    ///   Gets the named printer hosted by the print server.
    /// </summary>
    /// <param name="fullName"/>
    /// <exception cref="T:System.ArgumentNullException"><paramref name="fullName"/> is <see langword="null"/>.</exception>
    /// <exception cref="T:System.Exception"/>
    [Pure]
    [CanBeNull]
    TXpsPrinterDefinition GetXpsPrinterDefinition([NotNull] string fullName);

    /// <summary>
    ///   Gets the collection of input bins for the specified printer hosted by the print server.
    /// </summary>
    /// <param name="xpsPrinterDefinition"/>
    /// <exception cref="T:System.ArgumentNullException"><paramref name="xpsPrinterDefinition"/> is <see langword="null"/>.</exception>
    /// <exception cref="T:System.Exception"/>
    [Pure]
    [NotNull]
    [ItemNotNull]
    TXpsInputBinDefinition[] GetXpsInputBinDefinitions([NotNull] TXpsPrinterDefinition xpsPrinterDefinition);
  }

  /// <inheritdoc />
  /// <seealso cref="T:Contrib.System.Printing.Xps.XpsServer"/>
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
    ///   Initializes a new instance of the <see cref="T:Contrib.System.Printing.Xps.XpsServer{TXpsPrinterDefinition,TXpsInputBinDefinition}"/> class.
    /// </summary>
    /// <param name="xpsPrinterDefinitionFactory">Factory for <typeparamref name="TXpsPrinterDefinition"/>.</param>
    /// <param name="xpsInputBinDefinitionFactory">Factory for <typeparamref name="TXpsInputBinDefinition"/>.</param>
    /// <exception cref="T:System.ArgumentNullException"><paramref name="xpsPrinterDefinitionFactory"/> is <see langword="null"/>.</exception>
    /// <exception cref="T:System.ArgumentNullException"><paramref name="xpsInputBinDefinitionFactory"/> is <see langword="null"/>.</exception>
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
      using (var localPrintServer = new LocalPrintServer())
      using (var printQueues = localPrintServer.GetLocalAndRemotePrintQueues())
      {
        result = printQueues.Select(printQueue =>
                                    {
                                      var printTicket = printQueue.UserPrintTicket;
                                      var printCapabilities = printQueue.GetPrintCapabilitiesAsXDocument(printTicket);
                                    
                                      var xpsPrinterDefinition = this.XpsPrinterDefinitionFactory.Create(printQueue,
                                                                                                         printCapabilities);
                                    
                                      return xpsPrinterDefinition;
                                    })
                            .ToArray();
      }

      return result;
    }

    /// <inheritdoc/>
    public virtual TXpsPrinterDefinition GetXpsPrinterDefinition(string fullName)
    {
      if (fullName == null)
      {
        throw new ArgumentNullException(nameof(fullName));
      }

      PrintServer printServer;
      string name;

      if (Uri.TryCreate(fullName,
                        UriKind.Absolute,
                        out var uri))
      {
        if (uri.IsUnc)
        {
          printServer = new PrintServer(@"\\" + uri.Host);
          name = Path.GetFileName(uri.LocalPath);
        }
        else
        {
          printServer = new LocalPrintServer();
          name = fullName;
        }
      }
      else
      {
        printServer = new LocalPrintServer();
        name = fullName;
      }

      TXpsPrinterDefinition result;
      using (printServer)
      using (var printQueue = printServer.GetPrintQueue(name))
      {
        var printTicket = printQueue.UserPrintTicket;
        var printCapabilities = printQueue.GetPrintCapabilitiesAsXDocument(printTicket);

        result = this.XpsPrinterDefinitionFactory.Create(printQueue,
                                                         printCapabilities);
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
      using (var printServer = new PrintServer(xpsPrinterDefinition.Host))
      using (var printQueue = printServer.GetPrintQueue(xpsPrinterDefinition.Name))
      {
        var printCapabilities = printQueue.GetPrintCapabilitiesAsXDocument(printQueue.DefaultPrintTicket);

        var feature = printCapabilities.Root.FindElementByNameAttribute(XpsServer.PageInputBinName)
                      ?? printCapabilities.Root.FindElementByNameAttribute(XpsServer.DocumentInputBinName)
                      ?? printCapabilities.Root.FindElementByNameAttribute(XpsServer.JobInputBinName);
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
                          .Select(arg => this.XpsInputBinDefinitionFactory.Create(arg.FeatureName,
                                                                                  arg.InputBinName,
                                                                                  arg.Option,
                                                                                  printCapabilities))
                          .ToArray();
        }
      }

      return result;
    }
  }

  /// <inheritdoc cref="T:Contrib.System.Printing.Xps.IXpsServer"/>
#if CONTRIB_SYSTEM_PRINTING_XPS
  public
#else
  internal
#endif
  partial class XpsServer : XpsServer<IXpsPrinterDefinition, IXpsInputBinDefinition>,
                            IXpsServer
  {
    /// <summary>
    ///   Initializes a new instance of the <see cref="T:Contrib.System.Printing.Xps.XpsServer"/> class.
    /// </summary>
    public XpsServer()
      : base(new XpsPrinterDefinitionFactory(),
             new XpsInputBinDefinitionFactory()) { }

    /// <summary>
    ///   Initializes a new instance of the <see cref="T:Contrib.System.Printing.Xps.XpsServer"/> class.
    /// </summary>
    /// <param name="xpsPrinterDefinitionFactory">Factory for <see cref="T:Contrib.System.Printing.Xps.IXpsPrinterDefinition"/>.</param>
    /// <param name="xpsInputBinDefinitionFactory">Factory for <see cref="T:Contrib.System.Printing.Xps.IXpsInputBinDefinition"/>.</param>
    public XpsServer([NotNull] IXpsPrinterDefinitionFactory<IXpsPrinterDefinition> xpsPrinterDefinitionFactory,
                     [NotNull] IXpsInputBinDefinitionFactory<IXpsInputBinDefinition> xpsInputBinDefinitionFactory)
      : base(xpsPrinterDefinitionFactory,
             xpsInputBinDefinitionFactory) { }
  }
}
