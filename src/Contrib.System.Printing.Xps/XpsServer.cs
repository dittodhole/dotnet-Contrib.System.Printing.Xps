using System;
using System.IO;
using System.Linq;
using System.Printing;
using System.Xml.Linq;
using Anotar.LibLog;
using Contrib.System.Printing.Xps.ExtensionMethods;
using JetBrains.Annotations;

namespace Contrib.System.Printing.Xps
{
  /// <summary>
  ///   Provides <typeparamref name="TXpsPrinterDefinition"/> and <typeparamref name="TXpsInputBinDefinition"/> instances.
  /// </summary>
  /// <typeparam name="TXpsPrinterDefinition"/>
  /// <typeparam name="TXpsInputBinDefinition"/>
  /// <seealso cref="XpsServer{TXpsPrinterDefinition,TXpsInputBinDefinition}"/>
  [PublicAPI]
  public partial interface IXpsServer<TXpsPrinterDefinition, TXpsInputBinDefinition>
    where TXpsPrinterDefinition : IXpsPrinterDefinition
    where TXpsInputBinDefinition : IXpsInputBinDefinition
  {
    /// <summary>
    ///   Gets all <typeparamref name="TXpsPrinterDefinition"/> instances.
    /// </summary>
    /// <exception cref="Exception"/>
    [Pure]
    [NotNull]
    [ItemNotNull]
    TXpsPrinterDefinition[] GetXpsPrinterDefinitions();

    /// <summary>
    ///   Gets all <typeparamref name="TXpsInputBinDefinition"/> instances.
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
  [PublicAPI]
  public partial interface IXpsServer : IXpsServer<IXpsPrinterDefinition, IXpsInputBinDefinition> { }

  /// <inheritdoc cref="IXpsServer{TXpsPrinterDefinition,TXpsInputBinDefinition}"/>
  public partial class XpsServer<TXpsPrinterDefinition, TXpsInputBinDefinition> : IXpsServer<TXpsPrinterDefinition, TXpsInputBinDefinition>
    where TXpsPrinterDefinition : IXpsPrinterDefinition
    where TXpsInputBinDefinition : IXpsInputBinDefinition
  {
    /// <summary>
    ///   Initializes a new instance of the <see cref="XpsServer{TXpsPrinterDefinition,TXpsInputBinDefinition}"/> class.
    /// </summary>
    /// <param name="xpsPrinterDefinitionFactory"/>
    /// <param name="xpsInputBinDefinitionFactory"/>
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
                                      XElement printCapabilitiesXElement;
                                      {
                                        var printCapabilitiesXDocument = printQueue.GetPrintCapabilitiesAsXDocument();
                                        printCapabilitiesXElement = printCapabilitiesXDocument.Root ?? new XElement(XpsServer.PrintCapabilitiesXName);
                                      }

                                      var xpsPrinterDefinition = this.XpsPrinterDefinitionFactory.Create(printQueue.Name,
                                                                                                         printQueue.FullName,
                                                                                                         printQueue.QueuePort?.Name,
                                                                                                         printQueue.QueueDriver?.Name,
                                                                                                         printCapabilitiesXElement);

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
          XElement inputBinXElement;
          {
            var printCapabilitiesXDocument = printQueue.GetPrintCapabilitiesAsXDocument();
            var printCapabilitiesXElement = printCapabilitiesXDocument.Root ?? new XElement(XpsServer.PrintCapabilitiesXName);
            inputBinXElement = printCapabilitiesXElement.FindElementByNameAttribute(XpsServer.PageInputBinXName)
                               ?? printCapabilitiesXElement.FindElementByNameAttribute(XpsServer.DocumentInputBinXName)
                               ?? printCapabilitiesXElement.FindElementByNameAttribute(XpsServer.JobInputBinXName);
          }

          if (inputBinXElement == null)
          {
            result = new TXpsInputBinDefinition[0];
          }
          else
          {
            result = inputBinXElement.Elements(XpsServer.OptionElementXName)
                                     .Select(optionXElement =>
                                             {
                                               var featureXName = inputBinXElement.Name;
                                               var inputBinXName = optionXElement.Name;
                                               var printTicket = this.GetPrintTicketImpl(featureXName,
                                                                                         inputBinXName);
                                               var printCapabilitiesXDocument = printQueue.GetPrintCapabilitiesAsXDocument(printTicket);
                                               var printCapabilitiesXElement = printCapabilitiesXDocument.Root ?? new XElement(XpsServer.PrintCapabilitiesXName);

                                               var xpsInputBinDefinition = this.XpsInputBinDefinitionFactory.Create(optionXElement,
                                                                                                                    printCapabilitiesXElement);

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
  public partial class XpsServer : XpsServer<IXpsPrinterDefinition, IXpsInputBinDefinition>,
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
