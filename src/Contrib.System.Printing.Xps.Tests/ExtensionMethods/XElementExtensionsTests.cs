using System.Xml.Linq;
using Contrib.System.Printing.Xps.ExtensionMethods;
using NUnit.Framework;

namespace Contrib.System.Printing.Xps.Tests.ExtensionMethods
{
  [TestFixture]
  public class XElementExtensionsTests
  {
    [TestCase("name", "name")]
    [TestCase("{http://my.scheme.com}name", "ns0000:name")]
    public void ReduceName_Should_Succeed(string expandedName,
                                          string expectedShortName)
    {
      var element = new XElement("element");
      var name = XName.Get(expandedName);
      var actualReducedName = element.ReduceName(name);

      Assert.AreEqual(expectedShortName,
                      actualReducedName);
    }

    [Test]
    public void EnsurePrefixRegistrationOfNamespace_Should_Succeed()
    {
      var element = new XElement("foo");
      var name = XName.Get("bar");
      var prefix = element.EnsurePrefixRegistrationOfNamespace(name);

      Assert.IsNull(prefix);
    }
  }
}
