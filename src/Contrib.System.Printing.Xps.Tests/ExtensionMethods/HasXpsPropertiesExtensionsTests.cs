using System.Collections;
using System.Xml.Linq;
using Contrib.System.Printing.Xps.ExtensionMethods;
using NUnit.Framework;

namespace Contrib.System.Printing.Xps.Tests.ExtensionMethods
{
  [TestFixture]
  public class HasXpsPropertiesExtensionsTests
  {
    [TestCaseSource(typeof(HasXpsPropertiesExtensionsTests.FindXpsPropertyCases))]
    public void FindXpsProperty(string printCapabilities,
                                XName name)
    {
      var printCapabilitiesXDocument = XDocument.Parse(printCapabilities);
      var printCapabilitiesXElement = printCapabilitiesXDocument.Root;

      var xpsPrintCapabilitiesReader = new XpsPrintCapabilitiesReader();

      var xpsPrintCapabilities = xpsPrintCapabilitiesReader.ReadXpsPrintCapabilities(printCapabilitiesXElement);

      var xpsProperty = xpsPrintCapabilities.FindXpsProperty(name);

      Assert.NotNull(xpsProperty);
    }

    private sealed class FindXpsPropertyCases : IEnumerable
    {
      public IEnumerator GetEnumerator()
      {
        yield return new object[]
                     {
                       XpsPrintCapabilitiesReaderTests_PrintCapabilities.Datamax_O_Neil_E_4204B_Mark_III,
                       XpsPrintCapabilitiesReader.ImageableSizeWidthXName
                     };
        yield return new object[]
                     {
                       XpsPrintCapabilitiesReaderTests_PrintCapabilities.Fax,
                       XpsPrintCapabilitiesReader.ImageableSizeWidthXName
                     };
        yield return new object[]
                     {
                       XpsPrintCapabilitiesReaderTests_PrintCapabilities.Foxit_PhantomPDF_Printer,
                       XpsPrintCapabilitiesReader.ImageableSizeWidthXName
                     };
        yield return new object[]
                     {
                       XpsPrintCapabilitiesReaderTests_PrintCapabilities.HP_ePrint,
                       XpsPrintCapabilitiesReader.ImageableSizeWidthXName
                     };
        yield return new object[]
                     {
                       XpsPrintCapabilitiesReaderTests_PrintCapabilities.Lexmark_X790_Series,
                       XpsPrintCapabilitiesReader.ImageableSizeWidthXName
                     };
        yield return new object[]
                     {
                       XpsPrintCapabilitiesReaderTests_PrintCapabilities.Microsoft_Print_to_PDF,
                       XpsPrintCapabilitiesReader.ImageableSizeWidthXName
                     };
        yield return new object[]
                     {
                       XpsPrintCapabilitiesReaderTests_PrintCapabilities.Microsoft_XPS_Document_Writer,
                       XpsPrintCapabilitiesReader.ImageableSizeWidthXName
                     };
        yield return new object[]
                     {
                       XpsPrintCapabilitiesReaderTests_PrintCapabilities.PDF_Architect_6,
                       XpsPrintCapabilitiesReader.ImageableSizeWidthXName
                     };
        yield return new object[]
                     {
                       XpsPrintCapabilitiesReaderTests_PrintCapabilities.Send_To_OneNote_2013,
                       XpsPrintCapabilitiesReader.ImageableSizeWidthXName
                     };
        yield return new object[]
                     {
                       XpsPrintCapabilitiesReaderTests_PrintCapabilities.Send_To_OneNote_2016,
                       XpsPrintCapabilitiesReader.ImageableSizeWidthXName
                     };
        yield return new object[]
                     {
                       XpsPrintCapabilitiesReaderTests_PrintCapabilities.Zebra_ZP_450_CTP,
                       XpsPrintCapabilitiesReader.ImageableSizeWidthXName
                     };
      }
    }
  }
}
