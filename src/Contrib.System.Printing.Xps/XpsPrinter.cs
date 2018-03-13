using System;
using System.Collections.Generic;
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

  public class XpsPrinter : IXpsPrinter
  {
    [Serializable]
    private sealed class XpsPrinterDefinition : IXpsPrinterDefinition,
                                                IEquatable<XpsPrinterDefinition>
    {
      /// <inheritdoc />
      public string Name { get; set; }

      /// <inheritdoc />
      public string FullName { get; set; }

      /// <inheritdoc />
      public double? DefaultPageWidth { get; set; }

      /// <inheritdoc />
      public double? DefaultPageHeight { get; set; }

      /// <inheritdoc />
      public string PortName { get; set; }

      /// <inheritdoc />
      public string DriverName { get; set; }

      /// <inheritdoc />
      public bool Equals(XpsPrinterDefinition other)
      {
        if (object.ReferenceEquals(null,
                                   other))
        {
          return false;
        }

        if (object.ReferenceEquals(this,
                                   other))
        {
          return true;
        }

        return string.Equals(this.FullName,
                             other.FullName);
      }

      /// <inheritdoc />
      public override bool Equals(object obj)
      {
        if (object.ReferenceEquals(null,
                                   obj))
        {
          return false;
        }

        if (object.ReferenceEquals(this,
                                   obj))
        {
          return true;
        }

        return obj is XpsPrinterDefinition && this.Equals((XpsPrinterDefinition) obj);
      }

      /// <inheritdoc />
      public override int GetHashCode()
      {
        return this.FullName.GetHashCode();
      }

      public static bool operator ==(XpsPrinterDefinition left,
                                     XpsPrinterDefinition right)
      {
        return object.Equals(left,
                             right);
      }

      public static bool operator !=(XpsPrinterDefinition left,
                                     XpsPrinterDefinition right)
      {
        return !object.Equals(left,
                              right);
      }

      /// <inheritdoc />
      public override string ToString()
      {
        return this.FullName;
      }
    }

    [Serializable]
    private sealed class XpsInputBinDefinition : IXpsInputBinDefinition,
                                                 IEquatable<XpsInputBinDefinition>
    {
      /// <inheritdoc />
      public string FullName { get; set; }

      /// <inheritdoc />
      public string Name { get; set; }

      /// <inheritdoc />
      public string DisplayName { get; set; }

      /// <inheritdoc />
      public string FeatureName { get; set; }

      /// <inheritdoc />
      public double? PageWidth { get; set; }

      /// <inheritdoc />
      public double? PageHeight { get; set; }

      /// <inheritdoc />
      public string NamespacePrefix { get; set; }

      /// <inheritdoc />
      public string NamespaceUri { get; set; }

      /// <inheritdoc />
      public bool Equals(XpsInputBinDefinition other)
      {
        if (object.ReferenceEquals(null,
                                   other))
        {
          return false;
        }

        if (object.ReferenceEquals(this,
                                   other))
        {
          return true;
        }

        return string.Equals(this.FullName,
                             other.FullName);
      }

      /// <inheritdoc />
      public override bool Equals(object obj)
      {
        if (object.ReferenceEquals(null,
                                   obj))
        {
          return false;
        }

        if (object.ReferenceEquals(this,
                                   obj))
        {
          return true;
        }

        return obj is XpsInputBinDefinition && this.Equals((XpsInputBinDefinition) obj);
      }

      /// <inheritdoc />
      public override int GetHashCode()
      {
        return this.FullName.GetHashCode();
      }

      public static bool operator ==(XpsInputBinDefinition left,
                                     XpsInputBinDefinition right)
      {
        return object.Equals(left,
                             right);
      }

      public static bool operator !=(XpsInputBinDefinition left,
                                     XpsInputBinDefinition right)
      {
        return !object.Equals(left,
                              right);
      }

      /// <inheritdoc />
      public override string ToString()
      {
        return this.FullName;
      }
    }

    /// <inheritdoc />
    public virtual IXpsPrinterDefinition[] GetXpsPrinterDefinitions()
    {
      IXpsPrinterDefinition[] result;

      using (var printServer = new PrintServer())
      using (var printQueues = XpsPrinter.GetPrintQueues(printServer))
      {
        result = printQueues.Select(this.GetXpsPrinterDefinitionImpl)
                            .ToArray();
      }

      return result;
    }

    /// <exception cref="Exception" />
    [Pure]
    [NotNull]
    protected virtual IXpsPrinterDefinition GetXpsPrinterDefinitionImpl([NotNull] PrintQueue printQueue)
    {
      double? defaultPageWidth;
      double? defaultPageHeight;

      try
      {
        var printCapabilities = printQueue.GetPrintCapabilities();

        defaultPageWidth = printCapabilities.OrientedPageMediaWidth;
        defaultPageHeight = printCapabilities.OrientedPageMediaHeight;
      }
      catch (Exception exception)
      {
        defaultPageWidth = null;
        defaultPageHeight = null;

        LogTo.WarnException($"Could not query {nameof(PrintQueue)} '{printQueue.FullName}' for {nameof(PrintCapabilities)}.",
                            exception);
      }

      var xpsPrinterDefinition = new XpsPrinterDefinition
                                 {
                                   Name = printQueue.Name,
                                   FullName = printQueue.FullName,
                                   DefaultPageWidth = defaultPageWidth,
                                   DefaultPageHeight = defaultPageHeight,
                                   DriverName = printQueue.QueueDriver.Name,
                                   PortName = printQueue.QueuePort.Name
                                 };

      return xpsPrinterDefinition;
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
      using (var printQueues = XpsPrinter.GetPrintQueues(printServer))
      {
        var printQueue = printQueues.GetPrintQueue(xpsPrinterDefinition);
        if (printQueue == null)
        {
          LogTo.Error($"Could not get {nameof(PrintQueue)} for {nameof(IXpsPrinterDefinition)}: {xpsPrinterDefinition}");
          result = new IXpsInputBinDefinition[0];
        }
        else
        {
          result = this.GetXpsInputBinDefinitionsImpl(printQueue);
        }
      }

      return result;
    }

    /// <exception cref="ArgumentNullException"><paramref name="printQueue" /> is <see langword="null" />.</exception>
    /// <exception cref="Exception" />
    [Pure]
    [NotNull]
    [ItemNotNull]
    protected virtual IXpsInputBinDefinition[] GetXpsInputBinDefinitionsImpl([NotNull] PrintQueue printQueue)
    {
      if (printQueue == null)
      {
        throw new ArgumentNullException(nameof(printQueue));
      }

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
        LogTo.ErrorException($"Could not query {nameof(PrintQueue)} '{printQueue.FullName}' for {nameof(PrintCapabilities)}.",
                             exception);
        xdocument = null;
      }

      IXpsInputBinDefinition[] result;
      if (xdocument == null)
      {
        result = new IXpsInputBinDefinition[0];
      }
      else
      {
        var printCapabilitiesXElement = xdocument.Root;
        if (printCapabilitiesXElement == null)
        {
          LogTo.Error($"Could not get root {nameof(XElement)}: {xdocument}");
          result = new IXpsInputBinDefinition[0];
        }
        else
        {
          var xpsInputBinDefinitions = new List<IXpsInputBinDefinition>();

          var printerSchemaFrameworkXNamespace = XpsPrinter.GetPrinterSchemaFrameworkXNamespace();
          var inputBinFeatureXElements = printCapabilitiesXElement.Elements(printerSchemaFrameworkXNamespace + "Feature")
                                                                  .Where(arg => new[]
                                                                                {
                                                                                  "psk:PageInputBin",
                                                                                  "psk:DocumentInputBin",
                                                                                  "psk:JobInputBin"
                                                                                }.Contains(arg.Attribute("name")
                                                                                              ?.Value,
                                                                                           StringComparer.Ordinal));
          foreach (var inputBinFeatureXElement in inputBinFeatureXElements)
          {
            var inputBinFeatureNameXAttribute = inputBinFeatureXElement.Attribute("name");

            var inputBinOptionXElements = inputBinFeatureXElement.Elements(printerSchemaFrameworkXNamespace + "Option");
            foreach (var inputBinOptionXElement in inputBinOptionXElements)
            {
              var inputBinNameXAttribute = inputBinOptionXElement.Attribute("name");
              var inputBinName = inputBinNameXAttribute?.Value;
              var inputBinNamespacePrefix = XpsPrinter.GetNamespacePrefix(inputBinName);
              if (inputBinNamespacePrefix == null)
              {
                LogTo.Warn($"'name'-{nameof(XAttribute)} for {nameof(IXpsInputBinDefinition.NamespacePrefix)} does not contain a valid name: {inputBinOptionXElement}");
                continue;
              }

              var inputBinXNamespace = printCapabilitiesXElement.GetNamespaceOfPrefix(inputBinNamespacePrefix);
              if (inputBinXNamespace == null)
              {
                LogTo.Warn($"Could not get {nameof(XNamespace)} for '{inputBinNamespacePrefix}': {xdocument}");
                continue;
              }

              var inputBinDisplayNameXElement = inputBinOptionXElement.Elements(printerSchemaFrameworkXNamespace + "Property")
                                                                      .Where(arg => string.Equals(arg.Attribute("name")
                                                                                                     ?.Value,
                                                                                                  "psk:DisplayName",
                                                                                                  StringComparison.Ordinal))
                                                                      .FirstOrDefault();
              var inputBinDisplayNameValueXElement = inputBinDisplayNameXElement?.Element(printerSchemaFrameworkXNamespace + "Value");

              var xpsInputBinDefinition = new XpsInputBinDefinition
                                          {
                                            FullName = $@"{printQueue.FullName}\{{{inputBinName}}}",
                                            Name = inputBinName,
                                            DisplayName = inputBinDisplayNameValueXElement?.Value ?? "unkown",
                                            NamespacePrefix = inputBinNamespacePrefix,
                                            NamespaceUri = inputBinXNamespace.NamespaceName,
                                            FeatureName = inputBinFeatureNameXAttribute?.Value ?? "psk:JobInputBin"
                                          };

              try
              {
                var printTicket = xpsInputBinDefinition.GetPrintTicket();
                var printCapabilities = printQueue.GetPrintCapabilities(printTicket);
                xpsInputBinDefinition.PageWidth = printCapabilities.OrientedPageMediaWidth;
                xpsInputBinDefinition.PageHeight = printCapabilities.OrientedPageMediaHeight;
              }
              catch (Exception exception)
              {
                LogTo.WarnException($"Could not query {nameof(PrintQueue)} '{printQueue.FullName}' for {nameof(PrintCapabilities)}.",
                                    exception);
              }

              xpsInputBinDefinitions.Add(xpsInputBinDefinition);
            }
          }

          result = xpsInputBinDefinitions.ToArray();
        }
      }

      return result;
    }

    [Pure]
    [CanBeNull]
    public static string GetNamespacePrefix([CanBeNull] string str)
    {
      string namespacePrefix;
      if (str == null)
      {
        namespacePrefix = null;
      }
      else if (str.Contains(':'))
      {
        namespacePrefix = str.Split(':')
                             .ElementAt(0);
      }
      else
      {
        namespacePrefix = null;
      }

      return namespacePrefix;
    }

    [Pure]
    [NotNull]
    public static XNamespace GetPrinterSchemaFrameworkXNamespace()
    {
      return XNamespace.Get("http://schemas.microsoft.com/windows/2003/08/printing/printschemaframework");
    }

    /// <exception cref="Exception" />
    [Pure]
    [NotNull]
    [ItemNotNull]
    public static PrintQueueCollection GetPrintQueues([CanBeNull] PrintServer printServer)
    {
      PrintQueueCollection result;
      if (printServer == null)
      {
        result = new PrintQueueCollection();
      }
      else
      {
        result = printServer.GetPrintQueues(new[]
                                            {
                                              EnumeratedPrintQueueTypes.Connections,
                                              EnumeratedPrintQueueTypes.Local
                                            });
      }

      return result;
    }
  }
}
