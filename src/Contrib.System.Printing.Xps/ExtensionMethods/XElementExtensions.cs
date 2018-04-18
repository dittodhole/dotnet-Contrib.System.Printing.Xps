using System;
using System.Linq;
using System.Xml.Linq;
using Anotar.LibLog;
using JetBrains.Annotations;

namespace Contrib.System.Printing.Xps.ExtensionMethods
{
  /// <summary>
  ///   Provides extensions to query <see cref="XElement"/> instances.
  /// </summary>
  [PublicAPI]
  public static partial class XElementExtensions
  {
    /// <summary>
    ///   Ensures and gets the prefix of the namespace registration of <paramref name="name"/>.
    /// </summary>
    /// <param name="xelement"/>
    /// <param name="name"/>
    /// <seealso cref="XElement.GetPrefixOfNamespace"/>
    /// <seealso cref="FindUnusedPrefixForNamespace"/>
    /// <exception cref="ArgumentNullException"><paramref name="xelement"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="name"/> is <see langword="null"/>.</exception>
    /// <exception cref="Exception"/>
    [MustUseReturnValue]
    [NotNull]
    public static string EnsurePrefixRegistrationOfNamespace([NotNull] this XElement xelement,
                                                             [NotNull] XName name)
    {
      if (xelement == null)
      {
        throw new ArgumentNullException(nameof(xelement));
      }
      if (name == null)
      {
        throw new ArgumentNullException(nameof(name));
      }

      var prefix = xelement.GetPrefixOfNamespace(name.Namespace);
      if (prefix == null)
      {
        prefix = xelement.FindUnusedPrefixForNamespace();

        var namespaceXName = XNamespace.Xmlns + prefix;
        xelement.SetAttributeValue(namespaceXName,
                                   name.NamespaceName);
      }

      return prefix;
    }

    /// <summary>
    ///   Finds an unused prefix for namespace registration.
    /// </summary>
    /// <param name="xelement"/>
    /// <seealso cref="XElement.GetNamespaceOfPrefix"/>
    /// <remarks>The prefix is constructed via following pattern: "ns{0000}"</remarks>
    /// <exception cref="ArgumentNullException"><paramref name="xelement"/> is <see langword="null"/>.</exception>
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

    /// <summary>
    ///   Gets the value from descending value-<see cref="XElement"/>.
    /// </summary>
    /// <param name="xelement"/>
    /// <seealso cref="XpsServer.ValueElementXName"/>
    /// <seealso cref="GetValue"/>
    /// <exception cref="ArgumentNullException"><paramref name="xelement"/> is <see langword="null"/>.</exception>
    [Pure]
    [CanBeNull]
    public static object GetValueFromValueElement([NotNull] this XElement xelement)
    {
      if (xelement == null)
      {
        throw new ArgumentNullException(nameof(xelement));
      }

      var valueXElement = xelement.Element(XpsServer.ValueElementXName);
      var value = valueXElement?.GetValue();

      return value;
    }

    /// <summary>
    ///   Gets the value by taking type-<see cref="XAttribute"/> into account.
    /// </summary>
    /// <param name="xelement"/>
    /// <seealso cref="XpsServer.TypeXName"/>
    /// <seealso cref="XpsServer.StringTypeXName"/>
    /// <seealso cref="XpsServer.IntegerTypeXName"/>
    /// <seealso cref="XpsServer.QNameTypeXName"/>
    /// <seealso cref="GetXName"/>
    /// <exception cref="ArgumentNullException"><paramref name="xelement"/> is <see langword="null"/>.</exception>
    [Pure]
    [CanBeNull]
    public static object GetValue([NotNull] this XElement xelement)
    {
      if (xelement == null)
      {
        throw new ArgumentNullException(nameof(xelement));
      }

      object result;
      // TODO there *MUST* be a better way for handling xsi:type

      var rawValue = xelement.Value;

      var typeXAttribute = xelement.Attribute(XpsServer.TypeXName);
      if (typeXAttribute == null)
      {
        result = rawValue;
      }
      else
      {
        var type = typeXAttribute.Value;
        var typeXName = xelement.GetXName(type);
        if (typeXName == null)
        {
          LogTo.Warn($"Could not get {nameof(XName)} from {nameof(XAttribute)} '{XpsServer.TypeXName}': {xelement}");
          result = null;
        }
        else if (typeXName == XpsServer.StringTypeXName)
        {
          result = rawValue;
        }
        else if (typeXName == XpsServer.IntegerTypeXName)
        {
          if (long.TryParse(rawValue,
                            out var longValue))
          {
            result = longValue;
          }
          else
          {
            result = null;
          }
        }
        else if (typeXName == XpsServer.QNameTypeXName)
        {
          var xnameValue = xelement.GetXName(rawValue);
          if (xnameValue == null)
          {
            LogTo.Warn($"Could not get {nameof(XName)} from {nameof(XElement)}: {xelement}");
            result = null;
          }
          else
          {
            result = xnameValue;
          }
        }
        else
        {
          // TODO support more types :beers:
          result = rawValue;
        }
      }

      return result;
    }

    /// <summary>
    ///   Finds the descending <paramref name="name"/>-<see cref="XElement"/>.
    /// </summary>
    /// <param name="xelement"/>
    /// <param name="name"/>
    /// <seealso cref="GetNameFromNameAttribute"/>
    /// <exception cref="ArgumentNullException"><paramref name="xelement"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="name"/> is <see langword="null"/>.</exception>
    [Pure]
    [CanBeNull]
    public static XElement FindElementByNameAttribute([NotNull] this XElement xelement,
                                                      [NotNull] XName name)
    {
      if (xelement == null)
      {
        throw new ArgumentNullException(nameof(xelement));
      }
      if (name == null)
      {
        throw new ArgumentNullException(nameof(name));
      }

      foreach (var element in xelement.Elements())
      {
        var xname = element.GetNameFromNameAttribute();
        if (xname == name)
        {
          return element;
        }
      }

      return null;
    }

    /// <summary>
    ///   Gets the <see cref="XName"/> from name-<see cref="XAttribute"/> of <paramref name="xelement"/>.
    /// </summary>
    /// <param name="xelement"/>
    /// <seealso cref="GetXName"/>
    /// <exception cref="ArgumentNullException"><paramref name="xelement"/> is <see langword="null"/>.</exception>
    [Pure]
    [CanBeNull]
    public static XName GetNameFromNameAttribute([NotNull] this XElement xelement)
    {
      if (xelement == null)
      {
        throw new ArgumentNullException(nameof(xelement));
      }

      var nameXAttribute = xelement.Attribute(XpsServer.NameAttributeXName);
      var result = xelement.GetXName(nameXAttribute?.Value);

      return result;
    }

    /// <summary>
    ///   Gets the <see cref="XName"/> from <paramref name="str"/>.
    /// </summary>
    /// <param name="xelement"/>
    /// <param name="str"/>
    /// <remarks><paramref name="xelement"/> is used to find the namespace for the prefix, contained in <paramref name="str"/>.</remarks>
    /// <exception cref="ArgumentNullException"><paramref name="xelement"/> is <see langword="null"/>.</exception>
    [Pure]
    [CanBeNull]
    public static XName GetXName([NotNull] this XElement xelement,
                                 [CanBeNull] string str)
    {
      if (xelement == null)
      {
        throw new ArgumentNullException(nameof(xelement));
      }

      XName result;
      if (str == null)
      {
        result = null;
      }
      else
      {
        string prefix;
        string localName;
        if (str.Contains(':'))
        {
          var parts = str.Split(':');
          prefix = parts.ElementAtOrDefault(0);
          localName = parts.ElementAtOrDefault(1);
        }
        else
        {
          prefix = null;
          localName = null;
        }

        if (prefix == null)
        {
          try
          {
            result = XName.Get(str);
          }
          catch (Exception exception)
          {
            LogTo.WarnException($"Could not get {nameof(XName)} from '{str}'.",
                                exception);
            result = null;
          }
        }
        else if (localName == null)
        {
          try
          {
            result = XName.Get(str);
          }
          catch (Exception exception)
          {
            LogTo.WarnException($"Could not get {nameof(XName)} from '{str}'.",
                                exception);
            result = null;
          }
        }
        else
        {
          var xnamespace = xelement.GetNamespaceOfPrefix(prefix);
          if (xnamespace == null)
          {
            LogTo.Warn($"Could not get {nameof(XNamespace)} for {nameof(prefix)} '{prefix}'.");
            result = null;
          }
          else
          {
            try
            {
              result = xnamespace.GetName(localName);
            }
            catch (Exception exception)
            {
              LogTo.WarnException($"Could not get {nameof(XName)} from '{localName}'.",
                                  exception);
              result = null;
            }
          }
        }
      }

      return result;
    }
  }
}
