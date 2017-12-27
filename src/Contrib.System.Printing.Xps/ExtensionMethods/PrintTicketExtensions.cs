using System.IO;
using System.Printing;
using System.Xml;
using JetBrains.Annotations;

namespace Contrib.System.Printing.Xps.ExtensionMethods
{
  public static class PrintTicketExtensions
  {
    [NotNull]
    internal static PrintTicket With([NotNull] this PrintTicket printTicket,
                                     [NotNull] IXpsInputBinDefinition xpsInputBinDefinition)
    {
      printTicket = printTicket.With(xpsInputBinDefinition.XpsPrinterDefinition.CustomNamespacePrefix,
                                     xpsInputBinDefinition.XpsPrinterDefinition.CustomNamespaceUri,
                                     xpsInputBinDefinition.FeatureName,
                                     xpsInputBinDefinition.Name);

      return printTicket;
    }

    [NotNull]
    internal static PrintTicket With([NotNull] this PrintTicket printTicket,
                                     [NotNull] string customNamespacePrefix,
                                     [NotNull] string customNamespaceUri,
                                     [NotNull] string featureName,
                                     [NotNull] string inputBinName)
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
          if (inputBinName.StartsWith($"{customNamespacePrefix}:"))
          {
            var xmlAttribute = xmlDocument.CreateAttribute($"xmlns:{customNamespacePrefix}");
            xmlAttribute.Value = customNamespaceUri;
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
