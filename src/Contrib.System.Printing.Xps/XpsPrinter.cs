using System;
using System.Collections.Generic;
using System.Linq;
using System.Printing;
using System.Xml.Linq;
using Contrib.System.Printing.Xps.ExtensionMethods;
using JetBrains.Annotations;

namespace Contrib.System.Printing.Xps
{
  public interface IXpsPrinter
  {
    /// <exception cref="Exception" />
    [Pure]
    [CanBeNull]
    IXpsPrinterDefinition GetDefaultXpsPrinterDefinition();

    /// <exception cref="Exception" />
    [Pure]
    [NotNull]
    [ItemNotNull]
    IXpsPrinterDefinition[] GetXpsPrinterDefinitions();

    /// <exception cref="Exception" />
    [Pure]
    [NotNull]
    [ItemNotNull]
    IXpsInputBinDefinition[] GetXpsInputBinDefinitions();

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
      public string HostingMachineName { get; set; }

      /// <inheritdoc />
      public string Name { get; set; }

      /// <inheritdoc />
      public string FullName { get; set; }

      /// <inheritdoc />
      public double? DefaultPageWidth { get; set; }

      /// <inheritdoc />
      public double? DefaultPageHeight { get; set; }

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
    }

    [Serializable]
    private sealed class XpsInputBinDefinition : IXpsInputBinDefinition,
                                                 IEquatable<XpsInputBinDefinition>
    {
      /// <inheritdoc />
      public IXpsPrinterDefinition XpsPrinterDefinition { get; set; }

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

        return this.XpsPrinterDefinition.Equals(other.XpsPrinterDefinition) && string.Equals(this.Name,
                                                                                             other.Name);
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
        unchecked
        {
          return this.XpsPrinterDefinition.GetHashCode() * 397 ^ this.Name.GetHashCode();
        }
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
    }

    /// <inheritdoc />
    public virtual IXpsPrinterDefinition GetDefaultXpsPrinterDefinition()
    {
      using (var printServer = new PrintServer(PrintSystemDesiredAccess.EnumerateServer))
      {
        var printerDefinitions = this.GetXpsPrinterDefinitionsImpl(printServer);
        var printerDefinition = printerDefinitions.FirstOrDefault();

        return printerDefinition;
      }
    }

    /// <inheritdoc />
    public virtual IXpsPrinterDefinition[] GetXpsPrinterDefinitions()
    {
      using (var printServer = new PrintServer(PrintSystemDesiredAccess.EnumerateServer))
      {
        var printerDefinitions = this.GetXpsPrinterDefinitionsImpl(printServer);
        var result = printerDefinitions.ToArray();

        return result;
      }
    }

    /// <exception cref="Exception" />
    [NotNull]
    [ItemNotNull]
    protected virtual IEnumerable<IXpsPrinterDefinition> GetXpsPrinterDefinitionsImpl([NotNull] PrintServer printServer)
    {
      var printQueues = printServer.GetPrintQueues(new[]
                                                   {
                                                     EnumeratedPrintQueueTypes.Connections,
                                                     EnumeratedPrintQueueTypes.Local
                                                   });

      foreach (var printQueue in printQueues)
      {
        IXpsPrinterDefinition xpsPrinterDefinition;
        using (printQueue)
        {
          var printCapabilities = printQueue.GetPrintCapabilities();

          var defaultPageWidth = printCapabilities.OrientedPageMediaWidth;
          var defaultPageHeight = printCapabilities.OrientedPageMediaHeight;

          xpsPrinterDefinition = new XpsPrinterDefinition
                                 {
                                   HostingMachineName = printQueue.HostingPrintServer.Name,
                                   Name = printQueue.Name,
                                   FullName = printQueue.FullName,
                                   DefaultPageWidth = defaultPageWidth,
                                   DefaultPageHeight = defaultPageHeight
                                 };
        }

        yield return xpsPrinterDefinition;
      }
    }

    /// <inheritdoc />
    public virtual IXpsInputBinDefinition[] GetXpsInputBinDefinitions()
    {
      var inputBinDefinitions = this.GetXpsInputBinDefinitionsImpl();
      var result = inputBinDefinitions.ToArray();

      return result;
    }

    /// <exception cref="Exception" />
    [NotNull]
    [ItemNotNull]
    protected virtual IEnumerable<IXpsInputBinDefinition> GetXpsInputBinDefinitionsImpl()
    {
      var xpsPrinterDefinitions = this.GetXpsPrinterDefinitions();
      foreach (var xpsPrinterDefinition in xpsPrinterDefinitions)
      {
        using (var printServer = new PrintServer(xpsPrinterDefinition.HostingMachineName,
                                                 PrintSystemDesiredAccess.EnumerateServer))
        {
          var inputBinDefinitions = this.GetXpsInputBinDefinitionsImpl(printServer,
                                                                       xpsPrinterDefinition);
          foreach (var inputBinDefinition in inputBinDefinitions)
          {
            yield return inputBinDefinition;
          }
        }
      }
    }

    /// <inheritdoc />
    public virtual IXpsInputBinDefinition[] GetXpsInputBinDefinitions(IXpsPrinterDefinition xpsPrinterDefinition)
    {
      if (xpsPrinterDefinition == null)
      {
        throw new ArgumentNullException(nameof(xpsPrinterDefinition));
      }

      using (var printServer = new PrintServer(xpsPrinterDefinition.HostingMachineName,
                                               PrintSystemDesiredAccess.EnumerateServer))
      {
        var inputBinDefinitions = this.GetXpsInputBinDefinitionsImpl(printServer,
                                                                     xpsPrinterDefinition);
        var result = inputBinDefinitions.ToArray();

        return result;
      }
    }

    /// <exception cref="Exception" />
    [Pure]
    [NotNull]
    [ItemNotNull]
    protected virtual IEnumerable<IXpsInputBinDefinition> GetXpsInputBinDefinitionsImpl([NotNull] PrintServer printServer,
                                                                                        [NotNull] IXpsPrinterDefinition xpsPrinterDefinition)
    {
      using (var printQueue = printServer.GetPrintQueue(xpsPrinterDefinition.Name))
      {
        XDocument xdocument;
        using (var memoryStream = printQueue.GetPrintCapabilitiesAsXml())
        {
          xdocument = XDocument.Load(memoryStream);
        }

        var rootXElement = xdocument.Root;
        if (rootXElement == null)
        {
          throw new Exception($"Could not get {nameof(XDocument.Root)}: {xdocument}");
        }

        var printerSchemaFrameworkXNamespace = XpsPrinter.GetPrinterSchemaFrameworkXNamespace();
        var inputBinFeatureXElements = rootXElement.Elements(printerSchemaFrameworkXNamespace + "Feature")
                                                   .Where(arg => new[]
                                                                 {
                                                                   "psk:PageInputBin",
                                                                   "psk:DocumentInputBin",
                                                                   "psk:JobInputBin"
                                                                 }.Contains(arg.Attribute("name")?.Value,
                                                                            StringComparer.Ordinal));
        foreach (var inputBinFeatureXElement in inputBinFeatureXElements)
        {
          var inputBinFeatureNameXAttribute = inputBinFeatureXElement.Attribute("name");

          var inputBinOptionXElements = inputBinFeatureXElement.Elements(printerSchemaFrameworkXNamespace + "Option");
          foreach (var inputBinOptionXElement in inputBinOptionXElements)
          {
            var inputBinNameXAttribute = inputBinOptionXElement.Attribute("name");
            var inputBinName = inputBinNameXAttribute?.Value;
            if (inputBinName == null)
            {
              continue;
            }

            var inputBinNamespacePrefix = XpsPrinter.GetNamespacePrefix(inputBinName);
            if (inputBinNamespacePrefix == null)
            {
              throw new Exception($"'name'-{nameof(XAttribute)} for {nameof(IXpsInputBinDefinition.NamespacePrefix)} does not contain a valid name: {inputBinOptionXElement}");
            }

            var inputBinXNamespace = rootXElement.GetNamespaceOfPrefix(inputBinNamespacePrefix);
            if (inputBinXNamespace == null)
            {
              throw new Exception($"Could not get {nameof(XNamespace)} for '{inputBinNamespacePrefix}': {xdocument}");
            }

            var inputBinDisplayNameXElement = inputBinOptionXElement.Elements(printerSchemaFrameworkXNamespace + "Property")
                                                                    .Where(arg => string.Equals(arg.Attribute("name")?.Value,
                                                                                                "psk:DisplayName",
                                                                                                StringComparison.Ordinal))
                                                                    .FirstOrDefault();
            var inputBinDisplayNameValueXElement = inputBinDisplayNameXElement?.Element(printerSchemaFrameworkXNamespace + "Value");

            var inputBinDefinition = new XpsInputBinDefinition
                                     {
                                       XpsPrinterDefinition = xpsPrinterDefinition,
                                       Name = inputBinName,
                                       DisplayName = inputBinDisplayNameValueXElement?.Value ?? "unkown",
                                       NamespacePrefix = inputBinNamespacePrefix,
                                       NamespaceUri = inputBinXNamespace.NamespaceName,
                                       FeatureName = inputBinFeatureNameXAttribute?.Value ?? "psk:JobInputBin"
                                     };
            var printTicket = inputBinDefinition.CreatePrintTicket();

            var printCapabilities = printQueue.GetPrintCapabilities(printTicket);
            inputBinDefinition.PageWidth = printCapabilities.OrientedPageMediaWidth;
            inputBinDefinition.PageHeight = printCapabilities.OrientedPageMediaHeight;

            yield return inputBinDefinition;
          }
        }
      }
    }

    [Pure]
    [CanBeNull]
    public static string GetNamespacePrefix([NotNull] string str)
    {
      string namespacePrefix;
      if (str.Contains(':'))
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
  }
}
