namespace Contrib.System.Printing.Xps.Tests.ExtensionMethods
{
  using global::System.Collections.Generic;
  using global::System.Xml.Linq;
  using global::Contrib.System.Printing.Xps.ExtensionMethods;
  using global::NUnit.Framework;

  [TestFixture]
  public class XElementExtensionsTests
  {
    private static string DefaultXmlContent => @"
<root xmlns:ns0000=""http://my.scheme.com"">
  <child1 name=""child1""/>
  <child2 name=""16bpcSupport""/>
  <child3 name=""ns0000:foo""/>
</root>";

    [TestCaseSource(nameof(XElementExtensionsTests.ReduceName_TestCases))]
    public string ReduceName(XName name)
    {
      var element = new XElement("element");
      var reducedName = element.ReduceName(name);

      return reducedName;
    }

    private static IEnumerable<TestCaseData> ReduceName_TestCases
    {
      get
      {
        yield return new TestCaseData(XName.Get("name")).Returns("name");
        yield return new TestCaseData(XName.Get("{http://my.scheme.com}name")).Returns("ns0000:name");
      }
    }

    [TestCaseSource(nameof(XElementExtensionsTests.EnsurePrefixRegistrationOfNamespace_TestCases))]
    public string EnsurePrefixRegistrationOfNamespace(XNamespace @namespace)
    {
      var element = new XElement("element");
      var prefix = element.EnsurePrefixRegistrationOfNamespace(@namespace);

      return prefix;
    }

    private static IEnumerable<TestCaseData> EnsurePrefixRegistrationOfNamespace_TestCases
    {
      get
      {
        yield return new TestCaseData(XNamespace.None).Returns(null);
        yield return new TestCaseData(XNamespace.Get("http://my.scheme.com")).Returns("ns0000");
      }
    }

    [TestCaseSource(nameof(XElementExtensionsTests.FindElementByNameAttribute_Should_Succeed_TestCases))]
    public void FindElementByNameAttribute_Should_Succeed(XpsName name)
    {
      var document = XDocument.Parse(XElementExtensionsTests.DefaultXmlContent);
      var root = document.Root;

      var result = root.FindElementByNameAttribute(name);

      Assert.IsNotNull(result);
    }

    private static IEnumerable<TestCaseData> FindElementByNameAttribute_Should_Succeed_TestCases
    {
      get
      {
        yield return new TestCaseData(XNamespace.None.GetXpsName("child1"));
        yield return new TestCaseData(XNamespace.None.GetXpsName("16bpcSupport"));
        yield return new TestCaseData(XNamespace.Get("http://my.scheme.com").GetXpsName("foo"));
      }
    }

    [TestCaseSource(nameof(XElementExtensionsTests.GetXpsName_TestCases))]
    public void GetXpsName(string str,
                           string expectedNamespace,
                           string expectedLocalName)
    {
      var document = XDocument.Parse(XElementExtensionsTests.DefaultXmlContent);
      var root = document.Root;

      var xpsName = root.GetXpsName(str);
      if (xpsName == null)
      {
        Assert.IsNull(expectedNamespace);
        Assert.IsNull(expectedLocalName);
      }
      else
      {
        var actualNamespace = xpsName.Namespace.NamespaceName;
        var actualLocalName = xpsName.LocalName;

        Assert.AreEqual(expectedNamespace,
                        actualNamespace);
        Assert.AreEqual(expectedLocalName,
                        actualLocalName);
      }
    }

    private static IEnumerable<TestCaseData> GetXpsName_TestCases
    {
      get
      {
        yield return new TestCaseData("ns0000:foo",
                                      "http://my.scheme.com",
                                      "foo");
        yield return new TestCaseData("foo",
                                      string.Empty,
                                      "foo");
        yield return new TestCaseData("ns0001:foo",
                                      null,
                                      null);
        yield return new TestCaseData("ns0000:foo:",
                                      "http://my.scheme.com",
                                      "foo:");
      }
    }

    [TestCaseSource(nameof(XElementExtensionsTests.GetXName_TestCases))]
    public void GetXName(string str,
                         string expectedNamespace,
                         string expectedLocalName)
    {
      var document = XDocument.Parse(XElementExtensionsTests.DefaultXmlContent);
      var root = document.Root;

      var name = root.GetXName(str);
      if (name == null)
      {
        Assert.IsNull(expectedNamespace);
        Assert.IsNull(expectedLocalName);
      }
      else
      {
        var actualNamespace = name.Namespace.NamespaceName;
        var actualLocalName = name.LocalName;

        Assert.AreEqual(expectedNamespace,
                        actualNamespace);
        Assert.AreEqual(expectedLocalName,
                        actualLocalName);
      }
    }

    private static IEnumerable<TestCaseData> GetXName_TestCases
    {
      get
      {
        yield return new TestCaseData("ns0000:foo",
                                      "http://my.scheme.com",
                                      "foo");
        yield return new TestCaseData("foo",
                                      string.Empty,
                                      "foo");
        yield return new TestCaseData("ns0001:foo",
                                      null,
                                      null);
        yield return new TestCaseData("ns0000:foo:",
                                      null,
                                      null);
      }
    }

  }
}
