using System.Xml.Linq;
using Contrib.System.Printing.Xps.ExtensionMethods;
using NUnit.Framework;

namespace Contrib.System.Printing.Xps.Tests.ExtensionMethods
{
  [TestFixture]
  public class XElementExtensionsTests
  {
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
