using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Printing;
using System.Xml.Linq;
using Anotar.LibLog;
using Contrib.System.Printing.Xps.ExtensionMethods;
using JetBrains.Annotations;

namespace Contrib.System.Printing.Xps
{
  public interface IXpsPrinter
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

  public partial class XpsPrinter : IXpsPrinter
  {
    public XpsPrinter()
    {
      this.PrintCapabilitiesReader = new PrintCapabilitiesReader();
    }

    /// <exception cref="ArgumentNullException"><paramref name="printCapabilitiesReader" /> is <see langword="null" />.</exception>
    public XpsPrinter([NotNull] IPrintCapabilitiesReader printCapabilitiesReader)
    {
      this.PrintCapabilitiesReader = printCapabilitiesReader ?? throw new ArgumentNullException(nameof(printCapabilitiesReader));
    }

    [NotNull]
    private IPrintCapabilitiesReader PrintCapabilitiesReader { get; }

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
                                      var xpsPrinterDefinition = XpsPrinterDefinition.Create(printQueue,
                                                                                             xpsPrintCapabilities);

                                      return xpsPrinterDefinition;
                                    })
                            .Cast<IXpsPrinterDefinition>()
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
          LogTo.Warn($"Could not find {nameof(PrintQueue)} '{xpsPrinterDefinition.FullName}'.");
          result = new IXpsInputBinDefinition[0];
        }
        else
        {
          result = this.GetXpsInputBinDefinitionsImpl(printQueue)
                       .ToArray();
        }
      }

      return result;
    }

    /// <exception cref="Exception" />
    [Pure]
    [NotNull]
    [ItemNotNull]
    protected virtual IEnumerable<IXpsInputBinDefinition> GetXpsInputBinDefinitionsImpl([NotNull] PrintQueue printQueue)
    {
      var xpsPrintCapabilities = this.GetXpsPrintCapabilitiesImpl(printQueue);

      var pageInputBinXpsFeature = xpsPrintCapabilities.GetXpsFeature(Xps.PrintCapabilitiesReader.PageInputBinXName);
      if (pageInputBinXpsFeature != null)
      {
        foreach (var xpsOption in pageInputBinXpsFeature.GetXpsOptions())
        {
          var xpsPrintTicket = this.GetXpsPrintTicketImpl(pageInputBinXpsFeature.Name,
                                                          xpsOption,
                                                          printQueue);
          var xpsInputBinDefinition = XpsInputBinDefinition.Create(pageInputBinXpsFeature.Name,
                                                                   xpsOption,
                                                                   xpsPrintTicket);

          yield return xpsInputBinDefinition;
        }
      }

      var documentInputBinXpsFeature = xpsPrintCapabilities.GetXpsFeature(Xps.PrintCapabilitiesReader.DocumentInputBinXName);
      if (documentInputBinXpsFeature != null)
      {
        foreach (var xpsOption in documentInputBinXpsFeature.GetXpsOptions())
        {
          var xpsPrintTicket = this.GetXpsPrintTicketImpl(documentInputBinXpsFeature.Name,
                                                          xpsOption,
                                                          printQueue);
          var xpsInputBinDefinition = XpsInputBinDefinition.Create(documentInputBinXpsFeature.Name,
                                                                   xpsOption,
                                                                   xpsPrintTicket);

          yield return xpsInputBinDefinition;
        }
      }

      var jobInputBinXpsFeature = xpsPrintCapabilities.GetXpsFeature(Xps.PrintCapabilitiesReader.JobInputBinXName);
      if (jobInputBinXpsFeature != null)
      {
        foreach (var xpsOption in jobInputBinXpsFeature.GetXpsOptions())
        {
          var xpsPrintTicket = this.GetXpsPrintTicketImpl(jobInputBinXpsFeature.Name,
                                                          xpsOption,
                                                          printQueue);
          var xpsInputBinDefinition = XpsInputBinDefinition.Create(jobInputBinXpsFeature.Name,
                                                                   xpsOption,
                                                                   xpsPrintTicket);

          yield return xpsInputBinDefinition;
        }
      }
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

      IXpsPrintCapabilities xpsPrintCapabilities;

      var printCapabilitiesXElement = xdocument.Root;
      if (printCapabilitiesXElement == null)
      {
        xpsPrintCapabilities = NullXpsPrintCapabilities.Instance;
      }
      else
      {
        xpsPrintCapabilities = this.PrintCapabilitiesReader.ReadXpsPrintCapabilities(printCapabilitiesXElement);
      }

      return xpsPrintCapabilities;
    }

    [NotNull]
    protected virtual IXpsPrintTicket GetXpsPrintTicketImpl([NotNull] XName featureXName,
                                                            [NotNull] IXpsOption xpsOption,
                                                            [NotNull] PrintQueue printQueue)
    {
      XDocument xdocument;

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

      IXpsPrintTicket xpsPrintTicket;

      var printTicketXElement = xdocument.Root;
      if (printTicketXElement == null)
      {
        xpsPrintTicket = NullXpsPrintTicket.Instance;
      }
      else
      {
        xpsPrintTicket = this.PrintCapabilitiesReader.ReadXpsPrintTicket(printTicketXElement);
      }

      return xpsPrintTicket;
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

        featureXElement = new XElement(Xps.PrintCapabilitiesReader.FeatureElementXName);
        featureXElement.SetAttributeValue(Xps.PrintCapabilitiesReader.NameAttributeXName,
                                          $"{prefix0}:{featureXName.LocalName}");
        printTicketXElement.Add(featureXElement);
      }

      {
        var prefix1 = printTicketXElement.EnsurePrefixRegistrationOfNamespace(inputBinXName);

        var optionXElement = new XElement(Xps.PrintCapabilitiesReader.OptionElementXName);
        optionXElement.SetAttributeValue(Xps.PrintCapabilitiesReader.NameAttributeXName,
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
