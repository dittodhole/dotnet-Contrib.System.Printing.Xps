using System.Collections.Generic;
using System.Xml.Linq;
using Contrib.System.Printing.Xps.ExtensionMethods;
using NUnit.Framework;

namespace Contrib.System.Printing.Xps.Tests.ExtensionMethods
{
  [TestFixture]
  public class HasXpsPropertiesExtensionsTests
  {
    [Test]
    [TestCaseSource(nameof(HasXpsPropertiesExtensionsTests.Get_FindXpsProperty_TestCaseSources))]
    public void FindXpsProperty_Should_Succeed(string printCapabilities)
    {
      var printCapabilitiesXDocument = XDocument.Parse(printCapabilities);
      var printCapabilitiesXElement = printCapabilitiesXDocument.Root;

      var xpsPrintCapabilitiesReader = new XpsPrintCapabilitiesReader();

      var xpsPrintCapabilities = xpsPrintCapabilitiesReader.ReadXpsPrintCapabilities(printCapabilitiesXElement);

      var xpsProperty = xpsPrintCapabilities.FindXpsProperty(XpsPrintCapabilitiesReader.ImageableSizeWidthXName);

      Assert.NotNull(xpsProperty);
    }

    public static IEnumerable<string> Get_FindXpsProperty_TestCaseSources()
    {
      yield return XpsPrintCapabilitiesReaderTests_PrintCapabilities.Datamax_O_Neil_E_4204B_Mark_III;
      yield return XpsPrintCapabilitiesReaderTests_PrintCapabilities.Fax;
      yield return XpsPrintCapabilitiesReaderTests_PrintCapabilities.Foxit_PhantomPDF_Printer;
      yield return XpsPrintCapabilitiesReaderTests_PrintCapabilities.HP_ePrint;
      yield return XpsPrintCapabilitiesReaderTests_PrintCapabilities.Lexmark_X790_Series;
      yield return XpsPrintCapabilitiesReaderTests_PrintCapabilities.Microsoft_Print_to_PDF;
      yield return XpsPrintCapabilitiesReaderTests_PrintCapabilities.Microsoft_XPS_Document_Writer;
      yield return XpsPrintCapabilitiesReaderTests_PrintCapabilities.PDF_Architect_6;
      yield return XpsPrintCapabilitiesReaderTests_PrintCapabilities.Send_To_OneNote_2013;
      yield return XpsPrintCapabilitiesReaderTests_PrintCapabilities.Send_To_OneNote_2016;
      yield return XpsPrintCapabilitiesReaderTests_PrintCapabilities.Zebra_ZP_450_CTP;
    }
  }
}
