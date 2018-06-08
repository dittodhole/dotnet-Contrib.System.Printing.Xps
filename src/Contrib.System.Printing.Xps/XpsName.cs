/** @pp
 * rootnamespace: Contrib.System
 */
namespace Contrib.System.Printing.Xps
{
  using global::System;
  using global::System.Xml;
  using global::System.Xml.Linq;
  using global::JetBrains.Annotations;

  /// <summary>
  ///
  /// </summary>
  [PublicAPI]
  [Serializable]
#if CONTRIB_SYSTEM_PRINTING_XPS
  public sealed
#else
  internal
#endif
  partial class XpsName
  {
    /// <summary>
    ///   Initializes a new instance of the <see cref="T:Contrib.System.Printing.Xps.XpsName"/> class.
    /// </summary>
    /// <param name="namespace"/>
    /// <param name="localName"/>
    /// <exception cref="T:System.ArgumentNullException"><paramref name="namespace"/> is <see langword="null"/>.</exception>
    /// <exception cref="T:System.ArgumentNullException"><paramref name="localName"/> is <see langword="null"/>.</exception>
    internal XpsName([NotNull] XNamespace @namespace,
                     [NotNull] string localName)
    {
      this.Namespace = @namespace ?? throw new ArgumentNullException(nameof(@namespace));
      this.LocalName = localName ?? throw new ArgumentNullException(nameof(localName));
    }

    /// <summary>
    ///   Gets the namespace of the current instance.
    /// </summary>
    [NotNull]
    public XNamespace Namespace { get; }

    /// <summary>
    ///   Gets the local name of the current instance.
    /// </summary>
    /// <remarks>The local name may start with a digit (eg "16bpcSupport" - this behavior differs from <see cref="T:System.Xml.Linq.XName"/> validation.</remarks>
    [NotNull]
    public string LocalName { get; }

    /// <summary>
    ///   Returns the prefixed <see cref="M:Contrib.System.Printing.Xps.XpsName.LocalName"/>.
    /// </summary>
    /// <param name="prefix"/>
    /// <remarks>The string representation follows the pattern "<paramref name="prefix"/>:<see cref="M:Contrib.System.Printing.Xps.XpsName.LocalName"/>"</remarks>
    [NotNull]
    public string ToString([CanBeNull] string prefix)
    {
      var result = XmlQualifiedName.ToString(this.LocalName,
                                             prefix);

      return result;
    }

    /// <inheritdoc/>
    public override string ToString()
    {
      string result;
      if (string.IsNullOrEmpty(this.Namespace.NamespaceName))
      {
        result = this.LocalName;
      }
      else
      {
        result = $"{{{this.Namespace}}}{this.LocalName}";
      }

      return result;
    }
  }
}
