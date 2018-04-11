using System.Collections.Generic;
using System.Xml.Linq;
using NUnit.Framework;

namespace Contrib.System.Printing.Xps.Tests
{
  [TestFixture]
  public sealed class XpsPrintCapabilitiesReaderTests
  {
    [Test]
    [TestCaseSource(nameof(XpsPrintCapabilitiesReaderTests.Get_ReadXpsPrintCapabilities_TestCaseSources))]
    public void ReadXpsPrintCapabilities_Should_Succeed(string printCapabilities)
    {
      var printCapabilitiesXDocument = XDocument.Parse(printCapabilities);
      var printCapabilitiesXElement = printCapabilitiesXDocument.Root;

      var xpsPrintCapabilitiesReader = new XpsPrintCapabilitiesReader();

      var xpsPrintCapabilities = xpsPrintCapabilitiesReader.ReadXpsPrintCapabilities(printCapabilitiesXElement);

      Assert.NotNull(xpsPrintCapabilities);
    }

    public static IEnumerable<string> Get_ReadXpsPrintCapabilities_TestCaseSources()
    {
      yield return PrintCapabilities.Datamax_O_Neil_E_4204B_Mark_III;
      yield return PrintCapabilities.Fax;
      yield return PrintCapabilities.Foxit_PhantomPDF_Printer;
      yield return PrintCapabilities.HP_ePrint;
      yield return PrintCapabilities.Lexmark_X790_Series;
      yield return PrintCapabilities.Microsoft_Print_to_PDF;
      yield return PrintCapabilities.Microsoft_XPS_Document_Writer;
      yield return PrintCapabilities.PDF_Architect_6;
      yield return PrintCapabilities.Send_To_OneNote_2013;
      yield return PrintCapabilities.Send_To_OneNote_2016;
      yield return PrintCapabilities.Zebra_ZP_450_CTP;
    }
  }
}
