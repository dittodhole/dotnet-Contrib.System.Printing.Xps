﻿using System.Collections.Generic;
using System.Xml.Linq;
using Contrib.System.Printing.Xps.ExtensionMethods;
using NUnit.Framework;

namespace Contrib.System.Printing.Xps.Tests.ExtensionMethods
{
  [TestFixture]
  public class XElementExtensionsTests
  {
    [TestCaseSource(nameof(XElementExtensionsTests.ReduceName_Should_Succeed_TestCases))]
    public string ReduceName_Should_Succeed(XName name)
    {
      var element = new XElement("element");
      var reducedName = element.ReduceName(name);

      return reducedName;
    }

    private static IEnumerable<TestCaseData> ReduceName_Should_Succeed_TestCases
    {
      get
      {
        yield return new TestCaseData(XName.Get("name")).Returns("name");
        yield return new TestCaseData(XName.Get("{http://my.scheme.com}name")).Returns("ns0000:name");
      }
    }

    [TestCaseSource(nameof(XElementExtensionsTests.EnsurePrefixRegistrationOfNamespace_Should_Succeed_TestCases))]
    public string EnsurePrefixRegistrationOfNamespace_Should_Succeed(XNamespace @namespace)
    {
      var element = new XElement("element");
      var prefix = element.EnsurePrefixRegistrationOfNamespace(@namespace);

      return prefix;
    }

    private static IEnumerable<TestCaseData> EnsurePrefixRegistrationOfNamespace_Should_Succeed_TestCases
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
      const string content = @"
<root xmlns:ns0000=""http://my.scheme.com"">
  <child1 name=""child1""/>
  <child2 name=""16bpcSupport""/>
  <child3 name=""ns0000:foo""/>
</root>";

      var document = XDocument.Parse(content);
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
  }
}
