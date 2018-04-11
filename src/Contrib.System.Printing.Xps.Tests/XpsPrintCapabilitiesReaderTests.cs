using System.Collections;
using System.Xml.Linq;
using NUnit.Framework;

namespace Contrib.System.Printing.Xps.Tests
{
  [TestFixture]
  public sealed class XpsPrintCapabilitiesReaderTests
  {
    [TestCaseSource(typeof(XpsPrintCapabilitiesReaderTests.ReadXpsPrintCapabilitiesCases))]
    public void ReadXpsPrintCapabilities(string printCapabilities)
    {
      var printCapabilitiesXDocument = XDocument.Parse(printCapabilities);
      var printCapabilitiesXElement = printCapabilitiesXDocument.Root;

      var xpsPrintCapabilitiesReader = new XpsPrintCapabilitiesReader();

      var xpsPrintCapabilities = xpsPrintCapabilitiesReader.ReadXpsPrintCapabilities(printCapabilitiesXElement);

      Assert.NotNull(xpsPrintCapabilities);
    }

    private sealed class ReadXpsPrintCapabilitiesCases : IEnumerable
    {
      public IEnumerator GetEnumerator()
      {
        yield return new object[]
                     {
                       XpsPrintCapabilitiesReaderTests_PrintCapabilities.Datamax_O_Neil_E_4204B_Mark_III
                     };
        yield return new object[]
                     {
                       XpsPrintCapabilitiesReaderTests_PrintCapabilities.Fax
                     };
        yield return new object[]
                     {
                       XpsPrintCapabilitiesReaderTests_PrintCapabilities.Foxit_PhantomPDF_Printer
                     };
        yield return new object[]
                     {
                       XpsPrintCapabilitiesReaderTests_PrintCapabilities.HP_ePrint
                     };
        yield return new object[]
                     {
                       XpsPrintCapabilitiesReaderTests_PrintCapabilities.Lexmark_X790_Series
                     };
        yield return new object[]
                     {
                       XpsPrintCapabilitiesReaderTests_PrintCapabilities.Microsoft_Print_to_PDF
                     };
        yield return new object[]
                     {
                       XpsPrintCapabilitiesReaderTests_PrintCapabilities.Microsoft_XPS_Document_Writer
                     };
        yield return new object[]
                     {
                       XpsPrintCapabilitiesReaderTests_PrintCapabilities.PDF_Architect_6
                     };
        yield return new object[]
                     {
                       XpsPrintCapabilitiesReaderTests_PrintCapabilities.Send_To_OneNote_2013
                     };
        yield return new object[]
                     {
                       XpsPrintCapabilitiesReaderTests_PrintCapabilities.Send_To_OneNote_2016
                     };
        yield return new object[]
                     {
                       XpsPrintCapabilitiesReaderTests_PrintCapabilities.Zebra_ZP_450_CTP
                     };
      }
    }
  }
}
