using System;
using System.Xml.Linq;
using JetBrains.Annotations;

namespace Contrib.System.Printing.Xps.ExtensionMethods
{
  public static class XElementExtensions
  {
    /// <returns>The prefix of the namespace registration is returned.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="xelement"/> is <see langword="null"/></exception>
    /// <exception cref="ArgumentNullException"><paramref name="xname"/> is <see langword="null"/></exception>
    /// <exception cref="Exception" />
    [MustUseReturnValue]
    [NotNull]
    public static string EnsurePrefixRegistrationOfNamespace([NotNull] this XElement xelement,
                                                             [NotNull] XName xname)
    {
      if (xelement == null)
      {
        throw new ArgumentNullException(nameof(xelement));
      }
      if (xname == null)
      {
        throw new ArgumentNullException(nameof(xname));
      }

      var prefix = xelement.GetPrefixOfNamespace(xname.Namespace);
      if (prefix == null)
      {
        prefix = xelement.FindUnusedPrefixForNamespace();

        var namespaceXName = XNamespace.Xmlns + prefix;
        xelement.SetAttributeValue(namespaceXName,
                                   xname.NamespaceName);
      }

      return prefix;
    }

    /// <remarks>The prefix is constructed via following pattern: "ns{0000}"</remarks>
    /// <exception cref="ArgumentNullException"><paramref name="xelement"/> is <see langword="null"/></exception>
    [Pure]
    [NotNull]
    public static string FindUnusedPrefixForNamespace([NotNull] this XElement xelement)
    {
      if (xelement == null)
      {
        throw new ArgumentNullException(nameof(xelement));
      }

      string namespacePrefix;
      var counter = 0;
      do
      {
        namespacePrefix = $"ns{counter++:0000}";
      } while (xelement.GetNamespaceOfPrefix(namespacePrefix) != null);

      return namespacePrefix;
    }
  }
}
