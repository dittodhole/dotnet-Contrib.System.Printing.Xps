using System;
using System.Collections.Generic;
using System.Linq;
using System.Printing;
using System.Xml;
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
      using (var printServer = new PrintServer())
      {
        var printerDefinitions = this.GetXpsPrinterDefinitionsImpl(printServer);
        var printerDefinition = printerDefinitions.FirstOrDefault();

        return printerDefinition;
      }
    }

    /// <inheritdoc />
    public virtual IXpsPrinterDefinition[] GetXpsPrinterDefinitions()
    {
      using (var printServer = new PrintServer())
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
        using (var printServer = new PrintServer(xpsPrinterDefinition.HostingMachineName))
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

      using (var printServer = new PrintServer(xpsPrinterDefinition.HostingMachineName))
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
      PrintTicket printTicket;
      XmlDocument xmlDocument;
      using (var printQueue = printServer.GetPrintQueue(xpsPrinterDefinition.Name))
      {
        printTicket = printQueue.UserPrintTicket ?? printQueue.DefaultPrintTicket;

        using (var memoryStream = printQueue.GetPrintCapabilitiesAsXml())
        {
          xmlDocument = new XmlDocument();
          xmlDocument.Load(memoryStream);
        }
      }

      var xmlNamespaceManager = new XmlNamespaceManager(xmlDocument.NameTable);
      var documentXmlElement = xmlDocument.DocumentElement;
      if (documentXmlElement != null)
      {
        xmlNamespaceManager.AddNamespace(documentXmlElement.Prefix,
                                         documentXmlElement.NamespaceURI);
      }

      var inputBinFeatureNames = this.GetInputBinFeatureNamesInHierarchicalOrder();
      foreach (var inputBinFeatureName in inputBinFeatureNames)
      {
        var xmlNodeList = xmlDocument.SelectNodes($"//psf:Feature[@name='{inputBinFeatureName}']/psf:Option",
                                                  xmlNamespaceManager);
        if (xmlNodeList == null)
        {
          continue;
        }

        foreach (XmlNode xmlNode in xmlNodeList)
        {
          var xmlAttributeCollection = xmlNode.Attributes;
          if (xmlAttributeCollection == null)
          {
            continue;
          }
          var nameXmlAttribute = xmlAttributeCollection["name"];
          if (nameXmlAttribute == null)
          {
            continue;
          }

          var name = nameXmlAttribute.Value;
          var namespacePrefix = XpsPrinter.GetNamespacePrefix(name);

          string namespaceUri;
          if (namespacePrefix == null)
          {
            namespaceUri = null;
          }
          else
          {
            namespaceUri = xmlDocument.ChildNodes[1]
                                      .GetNamespaceOfPrefix(namespacePrefix);
          }

          var displayName = xmlNode.SelectSingleNode("psf:Property[@name='psk:DisplayName']/psf:Value",
                                                     xmlNamespaceManager)
                                   ?.InnerText;

          var scopedPrintTicket = printTicket.With(name,
                                                   inputBinFeatureName,
                                                   namespacePrefix,
                                                   namespaceUri);

          var pageWidth = scopedPrintTicket.PageMediaSize.Width;
          var pageHeight = scopedPrintTicket.PageMediaSize.Height;

          var inputBinDefinition = new XpsInputBinDefinition
                                   {
                                     XpsPrinterDefinition = xpsPrinterDefinition,
                                     Name = name,
                                     DisplayName = displayName ?? "unkown",
                                     FeatureName = inputBinFeatureName,
                                     PageWidth = pageWidth,
                                     PageHeight = pageHeight,
                                     NamespaceUri = namespaceUri
                                   };

          yield return inputBinDefinition;
        }

        if (xmlNodeList.Count > 0)
        {
          // only the first feature that returns a non-empty collection, should be used.
          break;
        }
      }
    }

    [Pure]
    [NotNull]
    [ItemNotNull]
    protected virtual IEnumerable<string> GetInputBinFeatureNamesInHierarchicalOrder()
    {
      yield return "psk:JobInputBin"; // PrintTicketScope.JobScope
      yield return "psk:DocumentInputBin"; // PrintTicketScope.DocumentScope
      yield return "psk:PageInputBin"; // PrintTicketScope.PageScope
    }

    [Pure]
    [CanBeNull]
    internal static string GetNamespacePrefix([NotNull] string str)
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
  }
}
