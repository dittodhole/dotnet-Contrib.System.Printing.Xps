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
  using global::Anotar.LibLog;
  using global::Contrib.System.Printing.Xps.ExtensionMethods;
  using global::JetBrains.Annotations;

  /// <summary>
  ///   Provides <typeparamref name="TXpsPrinterDefinition"/> and <typeparamref name="TXpsInputBinDefinition"/> objects.
  /// </summary>
  /// <typeparam name="TXpsPrinterDefinition"/>
  /// <typeparam name="TXpsInputBinDefinition"/>
  /// <seealso cref="T:Contrib.System.Printing.Xps.XpsServer{TXpsPrinterDefinition,TXpsInputBinDefinition}"/>
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
    /// <exception cref="T:System.Exception"/>
    [NotNull]
    [ItemNotNull]
    TXpsPrinterDefinition[] GetXpsPrinterDefinitions();

    /// <summary>
    ///   Gets the named printer hosted by the print server.
    /// </summary>
    /// <param name="fullName"/>
    /// <exception cref="T:System.ArgumentNullException"><paramref name="fullName"/> is <see langword="null"/>.</exception>
    /// <exception cref="T:System.Exception"/>
    [CanBeNull]
    TXpsPrinterDefinition GetXpsPrinterDefinition([NotNull] string fullName);

    /// <summary>
    ///   Gets the collection of available input bins for the specified printer hosted by the print server.
    /// </summary>
    /// <param name="xpsPrinterDefinition"/>
    /// <exception cref="T:System.ArgumentNullException"><paramref name="xpsPrinterDefinition"/> is <see langword="null"/>.</exception>
    /// <exception cref="T:System.Exception"/>
    [NotNull]
    [ItemNotNull]
    TXpsInputBinDefinition[] GetAvailableXpsInputBinDefinitions([NotNull] TXpsPrinterDefinition xpsPrinterDefinition);

    /// <summary>
    ///   Gets the collection of input bins for the specified printer hosted by the print server.
    /// </summary>
    /// <param name="xpsPrinterDefinition"/>
    /// <exception cref="T:System.ArgumentNullException"><paramref name="xpsPrinterDefinition"/> is <see langword="null"/>.</exception>
    /// <exception cref="T:System.Exception"/>
    [NotNull]
    [ItemNotNull]
    TXpsInputBinDefinition[] GetXpsInputBinDefinitions([NotNull] TXpsPrinterDefinition xpsPrinterDefinition);
  }

  /// <inheritdoc />
  /// <seealso cref="T:Contrib.System.Printing.Xps.XpsServer"/>
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
    ///   Initializes a new instance of the <see cref="T:Contrib.System.Printing.Xps.XpsServer{TXpsPrinterDefinition,TXpsInputBinDefinition}"/> class.
    /// </summary>
    /// <param name="xpsPrinterDefinitionFactory">Factory for <typeparamref name="TXpsPrinterDefinition"/>.</param>
    /// <param name="xpsInputBinDefinitionFactory">Factory for <typeparamref name="TXpsInputBinDefinition"/>.</param>
    /// <exception cref="T:System.ArgumentNullException"><paramref name="xpsPrinterDefinitionFactory"/> is <see langword="null"/>.</exception>
    /// <exception cref="T:System.ArgumentNullException"><paramref name="xpsInputBinDefinitionFactory"/> is <see langword="null"/>.</exception>
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
        result = printQueues.Select(this.CreateXpsPrinterDefinition)
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

      TXpsPrinterDefinition result;
      using (var printServer = new PrintServer())
      using (var printQueue = printServer.GetPrintQueue(fullName))
      {
        if (printQueue == null)
        {
          result = default;
        }
        else
        {
          result = this.CreateXpsPrinterDefinition(printQueue);
        }
      }

      return result;
    }

    /// <inheritdoc/>
    public virtual TXpsInputBinDefinition[] GetAvailableXpsInputBinDefinitions(TXpsPrinterDefinition xpsPrinterDefinition)
    {
      if (xpsPrinterDefinition == null)
      {
        throw new ArgumentNullException(nameof(xpsPrinterDefinition));
      }

      var xpsInputBinDefinitions = this.GetXpsInputBinDefinitions(xpsPrinterDefinition);
      var result = xpsInputBinDefinitions.Where(arg => arg.IsAvailable)
                                         .ToArray();

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
            var printTicket = this.GetPrintTicketForQueryingPrintCapabilitiesOfPrintQueue(printQueue);
            var printCapabilities = printQueue.GetPrintCapabilitiesAsXDocument(printTicket)
                                              ?.Root ?? XpsServer.PrintCapabilitiesElement;

            feature = printCapabilities.FindElementByNameAttribute(XpsServer.PageInputBinName)
                      ?? printCapabilities.FindElementByNameAttribute(XpsServer.DocumentInputBinName)
                      ?? printCapabilities.FindElementByNameAttribute(XpsServer.JobInputBinName);
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
                                      var printTicket = this.GetPrintTicketForQueryingPrintCapabilitiesOfInputBin(printQueue,
                                                                                                                  arg.FeatureName,
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

    /// <summary>
    ///   Converts a print queue to a printer.
    /// </summary>
    /// <param name="printQueue"/>
    /// <exception cref="T:System.Exception"/>
    [NotNull]
    protected virtual TXpsPrinterDefinition CreateXpsPrinterDefinition([NotNull] PrintQueue printQueue)
    {
      var printTicket = this.GetPrintTicketForQueryingPrintCapabilitiesOfPrintQueue(printQueue);
      var printCapabilities = printQueue.GetPrintCapabilitiesAsXDocument(printTicket)
                                        ?.Root ?? XpsServer.PrintCapabilitiesElement;

      var result = this.XpsPrinterDefinitionFactory.Create(printQueue.Name,
                                                           printQueue.FullName,
                                                           printQueue.QueuePort?.Name,
                                                           printQueue.QueueDriver?.Name,
                                                           printCapabilities);

      return result;
    }

    /// <summary>
    ///   Gets the print ticket for querying the print capabilities of a print queue.
    /// </summary>
    /// <param name="printQueue"/>
    /// <exception cref="T:System.Exception"/>
    [CanBeNull]
    protected virtual PrintTicket GetPrintTicketForQueryingPrintCapabilitiesOfPrintQueue([NotNull] PrintQueue printQueue)
    {
      return null;
    }

    /// <summary>
    ///   Gets the print ticket for querying the print capabilities of an input bin.
    /// </summary>
    /// <param name="printQueue"/>
    /// <param name="featureName"/>
    /// <param name="inputBinName"/>
    /// <exception cref="T:System.Exception"/>
    [NotNull]
    protected virtual PrintTicket GetPrintTicketForQueryingPrintCapabilitiesOfInputBin([NotNull] PrintQueue printQueue,
                                                                                       [NotNull] XpsName featureName,
                                                                                       [NotNull] XpsName inputBinName)
    {
      // === result ===
      // <?xml version="1.0" encoding="UTF-8"?>
      // <psf:PrintTicket xmlns:psf="http://schemas.microsoft.com/windows/2003/08/printing/printschemaframework"
      //                  xmlns:psk="http://schemas.microsoft.com/windows/2003/08/printing/printschemakeywords"
      //                  xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
      //                  xmlns:xsd="http://www.w3.org/2001/XMLSchema"
      //                  xmlns:{prefix0}="{featureName.NamespaceName}"
      //                  xmlns:{prefix1}="{inputBinName.NamespaceName}"
      //                  version="1">
      //   <psf:Feature name="{prefix0}:{featureName.LocalName}">
      //     <psf:Option name="{prefix1}:{inputBinName.LocalName}"/>
      //   </psf:Feature>
      // </psf:PrintTicket>
      // === === === === ===

      XDocument document;
      using (var memoryStream = (this.GetPrintTicketForQueryingPrintCapabilitiesOfPrintQueue(printQueue) ?? new PrintTicket()).GetXmlStream())
      {
        document = XDocument.Load(memoryStream);
      }

      var printTicket = document.Root ?? XpsServer.PrintTicketElement;

      var feature = printTicket.AddElement(XpsServer.FeatureName);
      var prefix = feature.GetPrefixOfNamespace(featureName.Namespace);
      feature.SetAttributeValue(XpsServer.NameName,
                                featureName.ToString(prefix));

      var option = feature.AddElement(XpsServer.OptionName);
      prefix = option.GetPrefixOfNamespace(inputBinName.Namespace);
      option.SetAttributeValue(XpsServer.NameName,
                               inputBinName.ToString(prefix));

      PrintTicket result;
      using (var memoryStream = new MemoryStream())
      {
        document.Save(memoryStream);
        memoryStream.Seek(0L,
                          SeekOrigin.Begin);

        result = new PrintTicket(memoryStream);
      }

      return result;
    }

    /// <summary>
    ///   Gets the print ticket for printing with a printer.
    /// </summary>
    /// <param name="xpsPrinterDefinition"/>
    /// <exception cref="T:System.ArgumentNullException"><paramref name="xpsPrinterDefinition"/> is <see langword="null"/>.</exception>
    /// <exception cref="T:System.Exception"/>
    [NotNull]
    public virtual PrintTicket GetPrintTicketForPrinting([NotNull] TXpsPrinterDefinition xpsPrinterDefinition)
    {
      if (xpsPrinterDefinition == null)
      {
        throw new ArgumentNullException(nameof(xpsPrinterDefinition));
      }

      return new PrintTicket();
    }

    /// <summary>
    ///   Gets the print ticket for printing with an input bin.
    /// </summary>
    /// <param name="xpsPrinterDefinition"/>
    /// <param name="xpsInputBinDefinition"/>
    /// <exception cref="T:System.ArgumentNullException"><paramref name="xpsPrinterDefinition"/> is <see langword="null"/>.</exception>
    /// <exception cref="T:System.ArgumentNullException"><paramref name="xpsInputBinDefinition"/> is <see langword="null"/>.</exception>
    /// <exception cref="T:System.Exception"/>
    [NotNull]
    public virtual PrintTicket GetPrintTicketForPrinting([NotNull] TXpsPrinterDefinition xpsPrinterDefinition,
                                                         [NotNull] TXpsInputBinDefinition xpsInputBinDefinition)
    {
      if (xpsPrinterDefinition == null)
      {
        throw new ArgumentNullException(nameof(xpsPrinterDefinition));
      }
      if (xpsInputBinDefinition == null)
      {
        throw new ArgumentNullException(nameof(xpsInputBinDefinition));
      }

      // === result ===
      // <?xml version="1.0" encoding="UTF-8"?>
      // <psf:PrintTicket xmlns:psf="http://schemas.microsoft.com/windows/2003/08/printing/printschemaframework"
      //                  xmlns:psk="http://schemas.microsoft.com/windows/2003/08/printing/printschemakeywords"
      //                  xmlns:{xsi}="http://www.w3.org/2001/XMLSchema-instance"
      //                  xmlns:xsd="http://www.w3.org/2001/XMLSchema"
      //                  xmlns:{prefix0}="{featureXName.NamespaceName}"
      //                  xmlns:{prefix1}="{inputBinXName.NamespaceName}"
      //                  xmlns:{prefix2}="{XpsServer.FeedTypeName.NamespaceName}"
      //                  xmlns:{prefix3}="{feedTypeXName.NamespaceName}"
      //                  xmlns:{prefix4}="{XpsServer.QNameName.NamespaceName}"
      //                  version="1">
      //   <psf:Feature name="{prefix0}:{featureXName.LocalName}">
      //     <psf:Option name="{prefix1}:{inputBinXName.LocalName}">
      //       <psf:ScoredProperty name="{prefix2}:{XpsServer.FeedTypeName.LocalName}">
      //         <psf:Value {xsi}:{XpsServer.TypeName.LocalName}="{prefix4}:{XpsServer.QNameName.LocalName}">{prefix3}:{feedTypeXName.LocalName}</psf:Value>
      //       </psf:ScoredProperty>
      //     </psf:Option>
      //   </psf:Feature>
      // </psf:PrintTicket>
      // === === === === ===

      XDocument document;
      using (var memoryStream = this.GetPrintTicketForPrinting(xpsPrinterDefinition)
                                    .GetXmlStream())
      {
        document = XDocument.Load(memoryStream);
      }

      var printTicket = document.Root ?? XpsServer.PrintTicketElement;

      var feature = printTicket.AddElement(XpsServer.FeatureName);
      var prefix = feature.EnsurePrefixRegistrationOfNamespace(xpsInputBinDefinition.Feature.Namespace);
      feature.SetAttributeValue(XpsServer.NameName,
                                xpsInputBinDefinition.Feature.ToString(prefix));

      var option = feature.AddElement(XpsServer.OptionName);
      prefix = option.EnsurePrefixRegistrationOfNamespace(xpsInputBinDefinition.Name.Namespace);
      option.SetAttributeValue(XpsServer.NameName,
                               xpsInputBinDefinition.Name.ToString(prefix));

      var feedType = xpsInputBinDefinition.FeedType;
      if (feedType != null)
      {
        var scoredProperty = option.AddElement(XpsServer.ScoredPropertyName);
        prefix = scoredProperty.EnsurePrefixRegistrationOfNamespace(XpsServer.FeedTypeName.Namespace);
        scoredProperty.SetAttributeValue(XpsServer.NameName,
                                         XpsServer.FeedTypeName.ToString(prefix));

        var value = scoredProperty.AddElement(XpsServer.ValueName);
        prefix = value.EnsurePrefixRegistrationOfNamespace(feedType.Namespace);
        value.SetValue(feedType.ToString(prefix));
        value.SetAttributeValue(XpsServer.TypeName,
                                value.ReduceName(XpsServer.QNameName));
      }

      PrintTicket result;
      using (var memoryStream = new MemoryStream())
      {
        document.Save(memoryStream);
        memoryStream.Seek(0L,
                          SeekOrigin.Begin);

        result = new PrintTicket(memoryStream);
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
    [PublicAPI]
    public XpsServer()
      : base(new XpsPrinterDefinitionFactory(),
             new XpsInputBinDefinitionFactory()) { }

    /// <summary>
    ///   Initializes a new instance of the <see cref="T:Contrib.System.Printing.Xps.XpsServer"/> class.
    /// </summary>
    /// <param name="xpsPrinterDefinitionFactory">Factory for <see cref="T:Contrib.System.Printing.Xps.IXpsPrinterDefinition"/>.</param>
    /// <param name="xpsInputBinDefinitionFactory">Factory for <see cref="T:Contrib.System.Printing.Xps.IXpsInputBinDefinition"/>.</param>
    [PublicAPI]
    public XpsServer([NotNull] IXpsPrinterDefinitionFactory<IXpsPrinterDefinition> xpsPrinterDefinitionFactory,
                     [NotNull] IXpsInputBinDefinitionFactory<IXpsInputBinDefinition> xpsInputBinDefinitionFactory)
      : base(xpsPrinterDefinitionFactory,
             xpsInputBinDefinitionFactory) { }
  }
}
