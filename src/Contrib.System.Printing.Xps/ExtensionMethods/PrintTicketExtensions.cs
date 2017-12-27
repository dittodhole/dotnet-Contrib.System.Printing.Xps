using System;
using System.IO;
using System.Printing;
using System.Xml;
using JetBrains.Annotations;

namespace Contrib.System.Printing.Xps.ExtensionMethods
{
  public static class PrintTicketExtensions
  {
    /// <exception cref="Exception" />
    [NotNull]
    internal static PrintTicket With([NotNull] this PrintTicket printTicket,
                                     [NotNull] IXpsInputBinDefinition xpsInputBinDefinition)
    {
      var namespacePrefix = XpsPrinter.GetNamespacePrefix(xpsInputBinDefinition.Name);

      printTicket = printTicket.With(xpsInputBinDefinition.Name,
                                     xpsInputBinDefinition.FeatureName,
                                     namespacePrefix,
                                     xpsInputBinDefinition.NamespaceUri);

      return printTicket;
    }

    /// <exception cref="Exception" />
    [NotNull]
    internal static PrintTicket With([NotNull] this PrintTicket printTicket,
                                     [NotNull] string inputBinName,
                                     [NotNull] string featureName,
                                     [CanBeNull] string namespacePrefix = null,
                                     [CanBeNull] string namespaceUri = null)
    {
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
