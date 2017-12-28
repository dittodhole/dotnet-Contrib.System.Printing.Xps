using System;
using System.IO;
using System.Printing;
using System.Xml;
using JetBrains.Annotations;

namespace Contrib.System.Printing.Xps.ExtensionMethods
{
  public static class PrintTicketExtensions
  {
    /// <exception cref="InvalidOperationException">If <paramref name="xpsInputBinDefinition" /> holds a prefix in <see cref="IXpsInputBinDefinition.Name" />, but does not provide a <see cref="IXpsInputBinDefinition.NamespaceUri" />.</exception>
    /// <exception cref="Exception" />
    [NotNull]
    public static PrintTicket With([NotNull] this PrintTicket printTicket,
                                   [NotNull] IXpsInputBinDefinition xpsInputBinDefinition)
    {
      var inputBinName = xpsInputBinDefinition.Name;
      var featureName = xpsInputBinDefinition.FeatureName;
      var namespacePrefix = XpsPrinter.GetNamespacePrefix(inputBinName);
      var namespaceUri = xpsInputBinDefinition.NamespaceUri;

      printTicket = printTicket.With(inputBinName,
                                     featureName,
                                     namespacePrefix,
                                     namespaceUri);

      return printTicket;
    }

    /// <exception cref="InvalidOperationException">If <paramref name="namespacePrefix" /> is not <see langword="null" />, and <paramref name="namespaceUri" /> is <see langword="null" />.</exception>
    /// <exception cref="Exception" />
    [NotNull]
    [ContractAnnotation("namespacePrefix: notnull, namespaceUri: null => halt")]
    public static PrintTicket With([NotNull] this PrintTicket printTicket,
                                   [NotNull] string inputBinName,
                                   [NotNull] string featureName,
                                   [CanBeNull] string namespacePrefix,
                                   [CanBeNull] string namespaceUri)
    {
      if (namespacePrefix != null)
      {
        if (namespaceUri == null)
        {
          throw new InvalidOperationException($"Providing a {nameof(namespacePrefix)} ({namespacePrefix}) makes {namespaceUri} mandatory.");
        }
      }

      var xmlDocument = new XmlDocument();
      using (var memoryStream = printTicket.GetXmlStream())
      {
        xmlDocument.Load(memoryStream);
      }

      var documentXmlElement = xmlDocument.DocumentElement;
      if (documentXmlElement != null)
      {
        var xmlNamespaceManager = new XmlNamespaceManager(xmlDocument.NameTable);
        xmlNamespaceManager.AddNamespace(documentXmlElement.Prefix,
                                         documentXmlElement.NamespaceURI);

        var xmlNode = xmlDocument.SelectSingleNode($"//psf:Feature[@name='{featureName}']/psf:Option",
                                                   xmlNamespaceManager);
        if (xmlNode != null)
        {
          if (namespacePrefix != null)
          {
            var xmlAttribute = xmlDocument.CreateAttribute($"xmlns:{namespacePrefix}");
            xmlAttribute.Value = namespaceUri;
            documentXmlElement.Attributes.Append(xmlAttribute);
          }

          var xmlAttributeCollection = xmlNode.Attributes;
          if (xmlAttributeCollection != null)
          {
            var nameXmlAttribute = xmlAttributeCollection["name"];
            if (nameXmlAttribute != null)
            {
              nameXmlAttribute.Value = inputBinName;
            }
          }
        }
      }

      using (var memoryStream = new MemoryStream())
      {
        xmlDocument.Save(memoryStream);
        memoryStream.Seek(0L,
                          SeekOrigin.Begin);

        printTicket = new PrintTicket(memoryStream);
      }

      return printTicket;
    }
  }
}
