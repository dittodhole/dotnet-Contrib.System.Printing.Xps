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
    [NotNull]
    private static XName DisplayNameXName { get; } = XmlHelper.PrinterSchemaKeywordsXNamespace + "DisplayName";

    private sealed class XpsGenericProperty : IXpsGenericProperty
    {
      public XName Type { get; set; }
      public XName Name { get; set; }
      public string Value { get; set; }
    }

    private abstract class XpsObjectWithProperties
    {
      [NotNull]
      private IDictionary<XName, IXpsGenericProperty> Properties { get; } = new Dictionary<XName, IXpsGenericProperty>();

      public virtual void AddXpsGenericProperty([NotNull] IXpsGenericProperty xpsGenericProperty)
      {
        this.Properties[xpsGenericProperty.Name] = xpsGenericProperty;
      }

      [ContractAnnotation("=> true, xpsGenericProperty:notnull; => false, xpsGenericProperty:null")]
      public bool TryGetXpsGenericProperty([NotNull] XName xname,
                                           out IXpsGenericProperty xpsGenericProperty)
      {
        return this.Properties.TryGetValue(xname,
                                           out xpsGenericProperty);
      }
    }

    private sealed class XpsPrinterDefinition : XpsObjectWithProperties,
                                                IXpsPrinterDefinition,
                                                IEquatable<XpsPrinterDefinition>
    {
      /// <inheritdoc />
      public string Name { get; set; }

      /// <inheritdoc />
      public string FullName { get; set; }

      /// <inheritdoc />
      public double? MediaWidth { get; set; }

      /// <inheritdoc />
      public double? MediaHeight { get; set; }

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

    private sealed class XpsInputBinDefinition : XpsObjectWithProperties,
                                                 IXpsInputBinDefinition,
                                                 IEquatable<XpsInputBinDefinition>
    {

      /// <inheritdoc />
      public string FullName { get; set; }

      /// <inheritdoc />
      public string Name { get; set; }

      /// <inheritdoc />
      public string DisplayName
      {
        get
        {
          string displayName;
          if (this.TryGetXpsGenericProperty(XpsPrinter.DisplayNameXName,
                                             out var xpsGenericProperty))
          {
            displayName = xpsGenericProperty.Value;
          }
          else
          {
            displayName = "unknown";
          }

          return displayName;
        }
      }

      /// <inheritdoc />
      public string FeatureName { get; set; }

      /// <inheritdoc />
      public double? MediaWidth { get; set; }

      /// <inheritdoc />
      public double? MediaHeight { get; set; }

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
      double? mediaWidth;
      double? mediaHeight;

      try
      {
        var printCapabilities = printQueue.GetPrintCapabilities();

        mediaWidth = NumberHelper.GetDimension(printCapabilities.OrientedPageMediaWidth,
                                               printCapabilities.OrientedPageMediaHeight,
                                               false);
        mediaHeight = NumberHelper.GetDimension(printCapabilities.OrientedPageMediaWidth,
                                                printCapabilities.OrientedPageMediaHeight,
                                                true);
      }
      catch (Exception exception)
      {
        mediaWidth = null;
        mediaHeight = null;

        LogTo.WarnException($"Could not query {nameof(PrintQueue)} '{printQueue.FullName}' for {nameof(PrintCapabilities)}.",
                            exception);
      }

      var xpsPrinterDefinition = new XpsPrinterDefinition
                                 {
                                   Name = printQueue.Name,
                                   FullName = printQueue.FullName,
                                   MediaWidth = mediaWidth,
                                   MediaHeight = mediaHeight,
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

          var printerSchemaFrameworkXNamespace = XmlHelper.PrinterSchemaFrameworkXNamespace;
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
              var inputBinNamespacePrefix = XmlHelper.GetNamespacePrefix(inputBinName);
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

              var xpsInputBinDefinition = new XpsInputBinDefinition
                                          {
                                            FullName = $@"{printQueue.FullName}\{{{inputBinName}}}",
                                            Name = inputBinName,
                                            NamespacePrefix = inputBinNamespacePrefix,
                                            NamespaceUri = inputBinXNamespace.NamespaceName,
                                            FeatureName = inputBinFeatureNameXAttribute?.Value ?? "psk:JobInputBin"
                                          };

              try
              {
                var xpsGenericProperties = XpsPrinter.ReadXpsGenericProperties(inputBinOptionXElement,
                                                                               printerSchemaFrameworkXNamespace + "Property",
                                                                               printerSchemaFrameworkXNamespace + "Value");
                foreach (var xpsGenericProperty in xpsGenericProperties)
                {
                  xpsInputBinDefinition.AddXpsGenericProperty(xpsGenericProperty);
                }
              }
              catch (Exception exception)
              {
                LogTo.WarnException($"Could not read properties from {nameof(inputBinOptionXElement)}: {inputBinOptionXElement}",
                                    exception);
              }

              try
              {
                var xpsGenericProperties = XpsPrinter.ReadXpsGenericProperties(inputBinOptionXElement,
                                                                               printerSchemaFrameworkXNamespace + "ScoredProperty",
                                                                               printerSchemaFrameworkXNamespace + "Value");
                foreach (var xpsGenericProperty in xpsGenericProperties)
                {
                  xpsInputBinDefinition.AddXpsGenericProperty(xpsGenericProperty);
                }
              }
              catch (Exception exception)
              {
                LogTo.WarnException($"Could not read scored properties from {nameof(inputBinOptionXElement)}: {inputBinOptionXElement}",
                                    exception);
              }

              double? mediaWidth;
              double? mediaHeight;
              try
              {
                var printTicket = xpsInputBinDefinition.GetPrintTicket();
                var printCapabilities = printQueue.GetPrintCapabilities(printTicket);

                mediaWidth = NumberHelper.GetDimension(printCapabilities.OrientedPageMediaWidth,
                                                       printCapabilities.OrientedPageMediaHeight,
                                                       false);
                mediaHeight = NumberHelper.GetDimension(printCapabilities.OrientedPageMediaWidth,
                                                        printCapabilities.OrientedPageMediaHeight,
                                                        true);
              }
              catch (Exception exception)
              {
                mediaWidth = null;
                mediaHeight = null;

                LogTo.WarnException($"Could not query {nameof(PrintQueue)} '{printQueue.FullName}' for {nameof(PrintCapabilities)}.",
                                    exception);
              }

              xpsInputBinDefinition.MediaWidth = mediaWidth;
              xpsInputBinDefinition.MediaHeight = mediaHeight;

              xpsInputBinDefinitions.Add(xpsInputBinDefinition);
            }
          }

          result = xpsInputBinDefinitions.ToArray();
        }
      }

      return result;
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

    [NotNull]
    [ItemNotNull]
    private static IEnumerable<IXpsGenericProperty> ReadXpsGenericProperties([NotNull] XElement xelement,
                                                                             [NotNull] XName childElementName,
                                                                             [NotNull] XName valueElementName)
    {
      var childXElements = xelement.Elements(childElementName);
      foreach (var childXElement in childXElements)
      {
        var nameXAttribute = childXElement.Attribute("name");
        if (nameXAttribute == null)
        {
          LogTo.Warn($"Could not get {nameof(nameXAttribute)} from {nameof(childXElement)}: {childXElement}");
          continue;
        }

        var nameXName = XmlHelper.GetXName(nameXAttribute.Value,
                                           childXElement.GetNamespaceOfPrefix);
        if (nameXName == null)
        {
          LogTo.Warn($"Could not get {nameof(nameXName)} from {nameof(nameXAttribute)}: {nameXAttribute}");
          continue;
        }

        var valueXElement = childXElement.Element(valueElementName);
        if (valueXElement == null)
        {
          LogTo.Warn($"Could not get {nameof(valueXElement)} from {nameof(childXElement)}: {childXElement}");
          continue;
        }

        var xpsGenericProperty = new XpsGenericProperty
                                 {
                                   Type = childElementName,
                                   Name = nameXName,
                                   Value = valueXElement.Value
                                 };

        yield return xpsGenericProperty;
      }
    }
  }
}
