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
  /// <seealso cref="XpsServerEx{TXpsPrinterDefinition,TXpsInputBinDefinition}"/>
  [PublicAPI]
  public partial interface IXpsServerEx<TXpsPrinterDefinition, TXpsInputBinDefinition>
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

  /// <inheritdoc cref="IXpsServerEx{TXpsPrinterDefinition,TXpsInputBinDefinition}"/>
  public partial class XpsServerEx<TXpsPrinterDefinition, TXpsInputBinDefinition> : IXpsServerEx<TXpsPrinterDefinition, TXpsInputBinDefinition>
    where TXpsPrinterDefinition : IXpsPrinterDefinition
    where TXpsInputBinDefinition : IXpsInputBinDefinition
  {
    /// <summary>
    ///   Initializes a new instance of the <see cref="XpsServerEx{TXpsPrinterDefinition,TXpsInputBinDefinition}"/> class.
    /// </summary>
    /// <param name="xpsPrinterDefinitionFactory"/>
    /// <param name="xpsInputBinDefinitionFactory"/>
    /// <exception cref="ArgumentNullException"><paramref name="xpsPrinterDefinitionFactory"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="xpsInputBinDefinitionFactory"/> is <see langword="null"/>.</exception>
    [PublicAPI]
    public XpsServerEx([NotNull] IXpsPrinterDefinitionFactory<TXpsPrinterDefinition> xpsPrinterDefinitionFactory,
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

    /// <summary>
    ///   Gets a plain <see cref="PrintTicket"/>, which is bound to <paramref name="inputBinXName"/>, to retrieve the print capabilities of the input bin.
    /// </summary>
    /// <param name="featureXName"/>
    /// <param name="inputBinXName"/>
    [NotNull]
    protected virtual PrintTicket GetPrintTicketImpl([NotNull] XName featureXName,
                                                     [NotNull] XName inputBinXName)
    {
      // === SOURCE ===
      // <?xml version="1.0" encoding="UTF-8"?>
      // <psf:PrintTicket xmlns:psf="http://schemas.microsoft.com/windows/2003/08/printing/printschemaframework"
      //                  xmlns:psk="http://schemas.microsoft.com/windows/2003/08/printing/printschemakeywords"
      //                  xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
      //                  xmlns:xsd="http://www.w3.org/2001/XMLSchema"
      //                  version="1">
      // </psf:PrintTicket>
      // === === === ===

      XDocument xdocument;

      {
        var printTicket = new PrintTicket();

        using (var memoryStream = printTicket.GetXmlStream())
        {
          xdocument = XDocument.Load(memoryStream);
        }
      }

      var printTicketXElement = xdocument.Root;
      if (printTicketXElement == null)
      {
        throw new Exception($"Could not get {nameof(XDocument.Root)}: {xdocument}");
      }

      // === DESTINATION ===
      // <?xml version="1.0" encoding="UTF-8"?>
      // <psf:PrintTicket xmlns:psf="http://schemas.microsoft.com/windows/2003/08/printing/printschemaframework"
      //                  xmlns:psk="http://schemas.microsoft.com/windows/2003/08/printing/printschemakeywords"
      //                  xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
      //                  xmlns:xsd="http://www.w3.org/2001/XMLSchema"
      //                  xmlns:{prefix0}="{featureXName.NamespaceName}"
      //                  xmlns:{prefix1}="{inputBinXName.NamespaceName}"
      //                  version="1">
      //   <psf:Feature name="{prefix0}:{featureXName.LocalName}">
      //     <psf:Option name="{prefix1}:{inputBinXName.LocalName}"/>
      //   </psf:Feature>
      // </psf:PrintTicket>
      // === === === === ===

      XElement featureXElement;
      {
        var prefix0 = printTicketXElement.EnsurePrefixRegistrationOfNamespace(featureXName);

        featureXElement = new XElement(XpsServer.FeatureElementXName);
        featureXElement.SetAttributeValue(XpsServer.NameAttributeXName,
                                          $"{prefix0}:{featureXName.LocalName}");
        printTicketXElement.Add(featureXElement);
      }

      {
        var prefix1 = printTicketXElement.EnsurePrefixRegistrationOfNamespace(inputBinXName);

        var optionXElement = new XElement(XpsServer.OptionElementXName);
        optionXElement.SetAttributeValue(XpsServer.NameAttributeXName,
                                         $"{prefix1}:{inputBinXName.LocalName}");
        featureXElement.Add(optionXElement);
      }

      PrintTicket result;
      using (var memoryStream = new MemoryStream())
      {
        xdocument.Save(memoryStream);
        memoryStream.Seek(0L,
                          SeekOrigin.Begin);

        result = new PrintTicket(memoryStream);
      }

      return result;
    }
  }
}
