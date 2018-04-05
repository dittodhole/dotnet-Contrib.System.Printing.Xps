using System;
using System.Linq;
using System.Xml.Linq;
using JetBrains.Annotations;

namespace Contrib.System.Printing.Xps
{
  internal static class XmlHelper
  {
    [CanBeNull]
    public delegate XNamespace GetNamespaceOfPrefix([NotNull] string prefix);

    [NotNull]
    public static XNamespace PrinterSchemaFrameworkXNamespace { get; } = XNamespace.Get("http://schemas.microsoft.com/windows/2003/08/printing/printschemaframework");

    [NotNull]
    public static XNamespace PrinterSchemaKeywordsXNamespace { get; } = XNamespace.Get("http://schemas.microsoft.com/windows/2003/08/printing/printschemakeywords");

    /// <code>
    ///   using Contrib.System.Printing.Xps;
    ///
    ///   var namespacePrefix = XmlHelper.GetNamespacePrefix(<paramref name="str"/>: "psk:JobInputBin");
    ///   if (namespacePrefix == "psk") ...
    /// </code>
    [Pure]
    [CanBeNull]
    public static string GetNamespacePrefix([CanBeNull] string str)
    {
      string namespacePrefix;
      if (str == null)
      {
        namespacePrefix = null;
      }
      else if (str.Contains(':'))
      {
        namespacePrefix = str.Split(':')
                             .ElementAt(0);
      }
      else
      {
        namespacePrefix = null;
      }

      return namespacePrefix;
    }

    /// <code>
    ///   using Contrib.System.Printing.Xps;
    ///
    ///   var xname = XmlHelper.GetXName(<paramref name="str"/>: "psk:JobInputBin",
    ///                                  <paramref name="getNamespaceOfPrefix"/>: xelement.GetNamespaceOfPrefix);
    ///   if (xname.NamespaceName == "http://schemas.microsoft.com/windows/2003/08/printing/printschemakeywords") ...
    ///   if (xname.LocalName == "JobInputBin") ...
    /// </code>
    [CanBeNull]
    public static XName GetXName([CanBeNull] string str,
                                 [NotNull] GetNamespaceOfPrefix getNamespaceOfPrefix)
    {
      XName xname;
      if (str == null)
      {
        xname = null;
      }
      else
      {
        string namespacePrefix;
        string localName;
        if (str.Contains(':'))
        {
          var parts = str.Split(':');
          namespacePrefix = parts.ElementAtOrDefault(0);
          localName = parts.ElementAtOrDefault(1);
        }
        else
        {
          namespacePrefix = null;
          localName = null;
        }

        if (namespacePrefix == null)
        {
          xname = XName.Get(str);
        }
        else if (localName == null)
        {
          xname = XName.Get(str);
        }
        else
        {
          var xnamespace = getNamespaceOfPrefix.Invoke(namespacePrefix);
          if (xnamespace == null)
          {
            xname = null;
          }
          else
          {
            xname = xnamespace + localName;
          }
        }
      }

      return xname;
    }
  }
}
