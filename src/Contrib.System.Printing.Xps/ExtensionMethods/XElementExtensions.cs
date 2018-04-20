/** @pp
 * rootnamespace: Contrib.System.Printing.Xps
 */
namespace Contrib.System.Printing.Xps.ExtensionMethods
{
  using global::System;
  using global::System.Linq;
  using global::System.Xml.Linq;
  using global::Anotar.LibLog;
  using global::JetBrains.Annotations;

  /// <summary>
  ///   Provides extensions to query <see cref="XElement"/> instances.
  /// </summary>
  [PublicAPI]
  public static partial class XElementExtensions
  {
    /// <summary>
    ///   Ensures and gets the prefix of the namespace registration of <paramref name="name"/>.
    /// </summary>
    /// <param name="element"/>
    /// <param name="name"/>
    /// <exception cref="ArgumentNullException"><paramref name="element"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="name"/> is <see langword="null"/>.</exception>
    /// <exception cref="Exception"/>
    [MustUseReturnValue]
    [NotNull]
    public static string EnsurePrefixRegistrationOfNamespace([NotNull] this XElement element,
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

      var result = element.GetPrefixOfNamespace(name.Namespace);
      if (result == null)
      {
        result = element.FindUnusedPrefixForNamespace();

        var @namespace = XNamespace.Xmlns + result;
        element.SetAttributeValue(@namespace,
                                  name.NamespaceName);
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
    ///   Gets the boxed value from descending value-<see cref="XElement"/>.
    /// </summary>
    /// <param name="element"/>
    /// <exception cref="ArgumentNullException"><paramref name="element"/> is <see langword="null"/>.</exception>
    [Pure]
    [CanBeNull]
    public static object GetValueFromValueElement([NotNull] this XElement element)
    {
      if (element == null)
      {
        throw new ArgumentNullException(nameof(element));
      }

      element = element.Element(XpsServer.ValueName);

      var result = element?.GetValue();

      return result;
    }

    /// <summary>
    ///   Gets the boxed value by taking type-<see cref="XAttribute"/> into account.
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
    ///   Finds the descending <paramref name="name"/>-<see cref="XElement"/>.
    /// </summary>
    /// <param name="element"/>
    /// <param name="name"/>
    /// <exception cref="ArgumentNullException"><paramref name="element"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="name"/> is <see langword="null"/>.</exception>
    [Pure]
    [CanBeNull]
    public static XElement FindElementByNameAttribute([NotNull] this XElement element,
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

      foreach (var child in element.Elements())
      {
        var xname = child.GetNameFromNameAttribute();
        if (xname == name)
        {
          return child;
        }
      }

      return null;
    }

    /// <summary>
    ///   Gets the <see cref="XName"/> from name-<see cref="XAttribute"/> of <paramref name="element"/>.
    /// </summary>
    /// <param name="element"/>
    /// <exception cref="ArgumentNullException"><paramref name="element"/> is <see langword="null"/>.</exception>
    [Pure]
    [CanBeNull]
    public static XName GetNameFromNameAttribute([NotNull] this XElement element)
    {
      if (element == null)
      {
        throw new ArgumentNullException(nameof(element));
      }

      var attribute = element.Attribute(XpsServer.NameName);
      var result = element.GetXName(attribute?.Value);

      return result;
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
          var xnamespace = element.GetNamespaceOfPrefix(prefix);
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
