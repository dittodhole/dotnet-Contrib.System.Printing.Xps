﻿/** @pp
 * rootnamespace: Contrib.System.Printing.Xps
 */
namespace Contrib.System.Printing.Xps.ExtensionMethods
{
  using global::System;
  using global::System.Linq;
  using global::System.Xml;
  using global::System.Xml.Linq;
  using global::Anotar.LibLog;
  using global::JetBrains.Annotations;

  /// <summary>
  ///   Provides extensions for <see cref="XElement"/> objects.
  /// </summary>
  [PublicAPI]
#if CONTRIB_SYSTEM_PRINTING_XPS
  public
#else
  internal
#endif
  static partial class XElementExtensions
  {
    /// <summary>
    ///   Registers a prefix for the namespace of <paramref name="name"/> if needed, and reduces it accordingly.
    /// </summary>
    /// <param name="element"/>
    /// <param name="name"/>
    /// <exception cref="ArgumentNullException"><paramref name="element"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="name"/> is <see langword="null"/>.</exception>
    [MustUseReturnValue]
    [NotNull]
    public static string ReduceName([NotNull] this XElement element,
                                    [NotNull] XName name)
    {
      if (element == null)
      {
        throw new ArgumentNullException(nameof(element));
      }
      if (name == null)
      {
        throw new ArgumentNullException(nameof(name));
      }

      string result;

      var prefix = element.EnsurePrefixRegistrationOfNamespace(name.Namespace);
      if (prefix == null)
      {
        result = name.LocalName;
      }
      else
      {
        result = XmlQualifiedName.ToString(name.LocalName,
                                           prefix);
      }

      return result;
    }

    /// <summary>
    ///   Ensures and gets the prefix of the namespace registration for <paramref name="name"/>.
    /// </summary>
    /// <param name="element"/>
    /// <param name="namespace"></param>
    /// <exception cref="ArgumentNullException"><paramref name="element"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="namespace"/> is <see langword="null"/>.</exception>
    [MustUseReturnValue]
    [CanBeNull]
    public static string EnsurePrefixRegistrationOfNamespace([NotNull] this XElement element,
                                                             [NotNull] XNamespace @namespace)
    {
      if (element == null)
      {
        throw new ArgumentNullException(nameof(element));
      }
      if (@namespace == null)
      {
        throw new ArgumentNullException(nameof(@namespace));
      }

      string result;

      if (@namespace == XNamespace.None)
      {
        result = null;
      }
      else
      {
        var document = element.Document;
        var root = document?.Root ?? element;

        result = root.GetPrefixOfNamespace(@namespace);

        if (result == null)
        {
          result = root.FindUnusedPrefixForNamespace();

          root.SetAttributeValue(XNamespace.Xmlns + result,
                                 @namespace);
        }
      }

      return result;
    }

    /// <summary>
    ///   Finds an unused prefix for namespace registration.
    /// </summary>
    /// <param name="element"/>
    /// <remarks>The prefix is constructed via following pattern: "ns{0000}"</remarks>
    /// <exception cref="ArgumentNullException"><paramref name="element"/> is <see langword="null"/>.</exception>
    [Pure]
    [NotNull]
    public static string FindUnusedPrefixForNamespace([NotNull] this XElement element)
    {
      if (element == null)
      {
        throw new ArgumentNullException(nameof(element));
      }

      string result;

      var counter = 0;
      do
      {
        result = $"ns{counter++:0000}";
      } while (element.GetNamespaceOfPrefix(result) != null);

      return result;
    }

    /// <summary>
    ///   Gets the boxed value by taking "xsi:Type" attribute into account.
    /// </summary>
    /// <param name="element"/>
    /// <exception cref="ArgumentNullException"><paramref name="element"/> is <see langword="null"/>.</exception>
    [Pure]
    [CanBeNull]
    public static object GetValue([NotNull] this XElement element)
    {
      if (element == null)
      {
        throw new ArgumentNullException(nameof(element));
      }

      object result;
      // TODO there *MUST* be a better way for handling xsi:type

      var rawValue = element.Value;

      var typeAttribute = element.Attribute(XpsServer.TypeName);
      if (typeAttribute == null)
      {
        result = rawValue;
      }
      else
      {
        var type = typeAttribute.Value;
        var typeXName = element.GetXName(type);
        if (typeXName == null)
        {
          LogTo.Warn($"Could not get {nameof(XName)} from {nameof(XAttribute)} '{XpsServer.TypeName}': {element}");
          result = null;
        }
        else if (typeXName == XpsServer.StringName)
        {
          result = rawValue;
        }
        else if (typeXName == XpsServer.IntegerName)
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
        else if (typeXName == XpsServer.QNameName)
        {
          var xnameValue = element.GetXName(rawValue);
          if (xnameValue == null)
          {
            LogTo.Warn($"Could not get {nameof(XName)} from {nameof(XElement)}: {element}");
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
    ///   Finds the matching child element.
    /// </summary>
    /// <param name="element"/>
    /// <param name="name"/>
    /// <exception cref="ArgumentNullException"><paramref name="element"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="name"/> is <see langword="null"/>.</exception>
    [Pure]
    [CanBeNull]
    public static XElement FindElementByNameAttribute([NotNull] this XElement element,
                                                      [NotNull] string name)
    {
      if (element == null)
      {
        throw new ArgumentNullException(nameof(element));
      }
      if (name == null)
      {
        throw new ArgumentNullException(nameof(name));
      }

      foreach (var child in element.Elements())
      {
        var childName = child.Attribute(XpsServer.NameName)
                             ?.Value;
        if (string.Equals(name,
                          childName,
                          StringComparison.Ordinal))
        {
          return child;
        }
      }

      return null;
    }

    /// <summary>
    ///   Gets the <see cref="XName"/> from <paramref name="str"/>.
    /// </summary>
    /// <param name="element"/>
    /// <param name="str"/>
    /// <remarks><paramref name="element"/> is used to find the namespace for the prefix, contained in <paramref name="str"/>.</remarks>
    /// <exception cref="ArgumentNullException"><paramref name="element"/> is <see langword="null"/>.</exception>
    [Pure]
    [CanBeNull]
    public static XName GetXName([NotNull] this XElement element,
                                 [CanBeNull] string str)
    {
      if (element == null)
      {
        throw new ArgumentNullException(nameof(element));
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
            LogTo.WarnException($"Could not get {nameof(XName)} from '{str}': {element}",
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
            LogTo.WarnException($"Could not get {nameof(XName)} from '{str}': {element}",
                                exception);
            result = null;
          }
        }
        else
        {
          var xnamespace = element.GetNamespaceOfPrefix(prefix);
          if (xnamespace == null)
          {
            LogTo.Warn($"Could not get {nameof(XNamespace)} from '{str}': {element}");
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
              LogTo.WarnException($"Could not get {nameof(XName)} from '{str}': {element}",
                                  exception);
              result = null;
            }
          }
        }
      }

      return result;
    }

    /// <summary>
    ///   Adds a <paramref name="name"/> named child element to <paramref name="element"/>, and returns it.
    /// </summary>
    /// <param name="element"/>
    /// <param name="name"/>
    /// <exception cref="ArgumentNullException"><paramref name="element"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="name"/> is <see langword="null"/>.</exception>
    [MustUseReturnValue]
    [NotNull]
    public static XElement AddElement([NotNull] this XElement element,
                                      [NotNull] XName name)
    {
      if (element == null)
      {
        throw new ArgumentNullException(nameof(element));
      }
      if (name == null)
      {
        throw new ArgumentNullException(nameof(name));
      }

      var result = new XElement(name);

      element.Add(result);

      return result;
    }
  }
}
