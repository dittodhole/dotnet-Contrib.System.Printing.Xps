using System.Collections.Generic;
using System.Xml.Linq;

namespace Contrib.System.Printing.Xps.Tests
{
  using global::NUnit.Framework;

  [TestFixture]
  public class XpsNameTests
  {
    [TestCaseSource(nameof(XpsNameTests.Get_TestCases))]
    public void Get(string expandedName,
                    XNamespace @namespace,
                    string localName)
    {
      var xpsName = XpsName.Get(expandedName);

      Assert.AreEqual(@namespace,
                      xpsName.Namespace);
      Assert.AreEqual(localName,
                      xpsName.LocalName);
    }

    private static IEnumerable<TestCaseData> Get_TestCases
    {
      get
      {
        yield return new TestCaseData("{http://schemas.microsoft.com/windows/2003/08/printing/printschemaframework}PrintCapabilities",
                                      XpsServer.PrinterSchemaFrameworkNamespace,
                                      "PrintCapabilities");
        yield return new TestCaseData("{http://schemas.microsoft.com/windows/2003/08/printing/printschemaframework}",
                                      XpsServer.PrinterSchemaFrameworkNamespace,
                                      string.Empty);
        yield return new TestCaseData("PrintCapabilities",
                                      XNamespace.None,
                                      "PrintCapabilities");
      }
    }
  }
}
