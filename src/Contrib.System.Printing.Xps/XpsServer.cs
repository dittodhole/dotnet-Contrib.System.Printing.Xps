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
  public interface IXpsServer
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

  public partial class XpsServer : IXpsServer
  {
    public XpsServer()
      : this(new XpsPrintCapabilitiesReader(),
             new XpsPrinterDefinitionFactory(),
             new XpsInputBinDefinitionFactory()) { }

    /// <exception cref="ArgumentNullException"><paramref name="xpsPrintCapabilitiesReader" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="xpsPrinterDefinitionFactory" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="xpsInputBinDefinitionFactory" /> is <see langword="null" />.</exception>
    public XpsServer([NotNull] IXpsPrintCapabilitiesReader xpsPrintCapabilitiesReader,
                     [NotNull] IXpsPrinterDefinitionFactory xpsPrinterDefinitionFactory,
                     [NotNull] IXpsInputBinDefinitionFactory xpsInputBinDefinitionFactory)
    {
      this.XpsPrintCapabilitiesReader = xpsPrintCapabilitiesReader ?? throw new ArgumentNullException(nameof(xpsPrintCapabilitiesReader));
      this.XpsPrinterDefinitionFactory = xpsPrinterDefinitionFactory;
      this.XpsInputBinDefinitionFactory = xpsInputBinDefinitionFactory;
    }

    [NotNull]
    private IXpsPrintCapabilitiesReader XpsPrintCapabilitiesReader { get; }

    [NotNull]
    private IXpsPrinterDefinitionFactory XpsPrinterDefinitionFactory { get; }

    [NotNull]
    private IXpsInputBinDefinitionFactory XpsInputBinDefinitionFactory { get; }

    /// <inheritdoc />
    public virtual IXpsPrinterDefinition[] GetXpsPrinterDefinitions()
    {
      IXpsPrinterDefinition[] result;
      using (var printServer = new PrintServer())
      using (var printQueues = printServer.GetLocalAndRemotePrintQueues())
      {
        result = printQueues.Select(printQueue =>
                                    {
                                      var xpsPrintCapabilities = this.GetXpsPrintCapabilitiesImpl(printQueue);
                                      var xpsPrinterDefinition = this.XpsPrinterDefinitionFactory.Create(printQueue.Name,
                                                                                                         printQueue.FullName,
                                                                                                         printQueue.QueuePort?.Name,
                                                                                                         printQueue.QueueDriver?.Name,
                                                                                                         xpsPrintCapabilities);

                                      return xpsPrinterDefinition;
                                    })
                            .ToArray();
      }

      return result;
    }

    /// <inheritdoc />
    public virtual IXpsInputBinDefinition[] GetXpsInputBinDefinitions(IXpsPrinterDefinition xpsPrinterDefinition)
    {
      if (xpsPrinterDefinition == null)
      {
        throw new ArgumentNullException(nameof(xpsPrinterDefinition));
      }

      IXpsInputBinDefinition[] result;
      using (var printServer = new PrintServer())
      using (var printQueues = printServer.GetLocalAndRemotePrintQueues())
      {
        var printQueue = printQueues.GetPrintQueue(xpsPrinterDefinition);
        if (printQueue == null)
        {
          LogTo.Warn($"Could not get {nameof(PrintQueue)} '{xpsPrinterDefinition.FullName}'.");
          result = new IXpsInputBinDefinition[0];
        }
        else
        {
          var inputBinXpsFeature = this.GetXpsPrintCapabilitiesImpl(printQueue)
                                       .FindInputBinXpsFeature();
          if (inputBinXpsFeature == null)
          {
            result = new IXpsInputBinDefinition[0];
          }
          else
          {
            result = inputBinXpsFeature.GetXpsOptions()
                                       .Select(xpsOption =>
                                               {
                                                 var featureName = inputBinXpsFeature.Name;
                                                 var displayName = xpsOption.FindXpsProperty(Xps.XpsPrintCapabilitiesReader.DisplayNameXName)?.Value as string;
                                                 var name = xpsOption.Name;
                                                 var xpsPrintCapabilities = this.GetXpsPrintCapabilitiesImpl(printQueue,
                                                                                                             inputBinXpsFeature,
                                                                                                             xpsOption);

                                                 var xpsInputBinDefinition = this.XpsInputBinDefinitionFactory.Create(featureName,
                                                                                                                      displayName,
                                                                                                                      name,
                                                                                                                      xpsOption,
                                                                                                                      xpsPrintCapabilities);

                                                 return xpsInputBinDefinition;
                                               })
                                       .ToArray();
          }
        }
      }

      return result;
    }

    [NotNull]
    protected virtual IXpsPrintCapabilities GetXpsPrintCapabilitiesImpl([NotNull] PrintQueue printQueue)
    {
      XDocument xdocument;
      try
      {
        using (var memoryStream = printQueue.GetPrintCapabilitiesAsXml())
        {
          xdocument = XDocument.Load(memoryStream);
        }
      }
      catch (Exception exception)
      {
        LogTo.WarnException($"Could not query {nameof(PrintQueue)} '{printQueue.FullName}' for {nameof(PrintCapabilities)}.",
                            exception);
        xdocument = new XDocument();
      }

      var xpsPrintCapabilities = this.GetXpsPrintCapabilitiesImpl(xdocument);

      return xpsPrintCapabilities;
    }

    [NotNull]
    protected virtual IXpsPrintCapabilities GetXpsPrintCapabilitiesImpl([NotNull] PrintQueue printQueue,
                                                                        [NotNull] IXpsFeature xpsFeature,
                                                                        [NotNull] IXpsOption xpsOption)
    {
      XDocument xdocument;

      var featureXName = xpsFeature.Name;
      var inputBinXName = xpsOption.Name;
      if (inputBinXName == null)
      {
        xdocument = new XDocument();
      }
      else
      {
        try
        {
          var printTicket = this.GetPrintTicketImpl(featureXName,
                                                    inputBinXName);
          using (var memoryStream = printQueue.GetPrintCapabilitiesAsXml(printTicket))
          {
            xdocument = XDocument.Load(memoryStream);
          }
        }
        catch (Exception exception)
        {
          LogTo.WarnException($"Could not query {nameof(PrintQueue)} '{printQueue.FullName}' for {nameof(PrintCapabilities)} (input bin '{inputBinXName}').",
                              exception);

          xdocument = new XDocument();
        }
      }

      var xpsPrintCapabilities = this.GetXpsPrintCapabilitiesImpl(xdocument);

      return xpsPrintCapabilities;
    }

    [NotNull]
    protected virtual IXpsPrintCapabilities GetXpsPrintCapabilitiesImpl([NotNull] XDocument xdocument)
    {
      IXpsPrintCapabilities xpsPrintCapabilities;

      var printCapabilitiesXElement = xdocument.Root;
      if (printCapabilitiesXElement == null)
      {
        xpsPrintCapabilities = NullXpsPrintCapabilities.Instance;
      }
      else
      {
        xpsPrintCapabilities = this.XpsPrintCapabilitiesReader.ReadXpsPrintCapabilities(printCapabilitiesXElement);
      }

      return xpsPrintCapabilities;
    }

    /// <exception cref="Exception" />
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
      //     <psf:Option name="{prefix1}:{inputBinXName.LocalName}" />
      //   </psf:Feature>
      // </psf:PrintTicket>
      // === === === === ===

      XElement featureXElement;
      {
        var prefix0 = printTicketXElement.EnsurePrefixRegistrationOfNamespace(featureXName);

        featureXElement = new XElement(Xps.XpsPrintCapabilitiesReader.FeatureElementXName);
        featureXElement.SetAttributeValue(Xps.XpsPrintCapabilitiesReader.NameAttributeXName,
                                          $"{prefix0}:{featureXName.LocalName}");
        printTicketXElement.Add(featureXElement);
      }

      {
        var prefix1 = printTicketXElement.EnsurePrefixRegistrationOfNamespace(inputBinXName);

        var optionXElement = new XElement(Xps.XpsPrintCapabilitiesReader.OptionElementXName);
        optionXElement.SetAttributeValue(Xps.XpsPrintCapabilitiesReader.NameAttributeXName,
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
