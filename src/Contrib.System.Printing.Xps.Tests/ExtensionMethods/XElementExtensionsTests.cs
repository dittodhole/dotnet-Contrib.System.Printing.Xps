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

    [TestCase("name", null)]
    [TestCase("{http://my.scheme.com}name", "ns0000")]
    public void EnsurePrefixRegistrationOfNamespace_Should_Succeed(string expandedName,
                                                                   string expectedPrefix)
    {
      var element = new XElement("element");
      var name = XName.Get(expandedName);
      var actualPrefix = element.EnsurePrefixRegistrationOfNamespace(name);

      Assert.AreEqual(expectedPrefix,
                      actualPrefix);
    }

    [TestCase("child1")]
    [TestCase("16bpcSupport")]
    public void FindElementByNameAttribute_Should_Succeed(string name)
    {
      var root = new XElement("root");
      var child1 = new XElement("child1");
      child1.SetAttributeValue(XpsServer.NameName,
                               "child1");
      root.Add(child1);
      var child2 = new XElement("child2");
      child2.SetAttributeValue(XpsServer.NameName,
                               "16bpcSupport");
      root.Add(child2);

      var result = root.FindElementByNameAttribute(name);

      Assert.IsNotNull(result);
    }
  }
}
