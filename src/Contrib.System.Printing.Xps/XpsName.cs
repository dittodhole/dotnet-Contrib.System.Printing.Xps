/** @pp
 * rootnamespace: Contrib.System
 */
namespace Contrib.System.Printing.Xps
{
  using global::System;
  using global::System.Runtime.Serialization;
  using global::System.Security.Permissions;
  using global::System.Xml;
  using global::System.Xml.Linq;
  using global::JetBrains.Annotations;

  /// <summary>
  ///   Represents a name of an XPS option, feature, or property, which is stored
  ///   in the "name"-attribute.
  /// </summary>
  /// <remarks>
  ///   The Print Schema (see https://msdn.microsoft.com/en-us/library/windows/desktop/ms715274)
  ///   specifies the usage of the "name"-attribute as follows:
  ///
  ///   > The name attribute value MUST also be a qualified name.
  ///
  ///   As some vendors do not obey the rules, the verification of the local name
  ///   must be skipped, thus introducing <see cref="XpsName"/>.
  /// </remarks>
  /// <seealso cref="T:System.Xml.Linq.XName"/>
  /// <seealso cref="M:System.Xml.XmlConvert.VerifyNCName(string)"/>
  [KnownType(typeof(XpsNameSerializer))]
  [Serializable]
  [PublicAPI]
#if CONTRIB_SYSTEM_PRINTING_XPS
  public sealed
#else
  internal
#endif
  partial class XpsName : ISerializable
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

    /// <param name="expandedName"/>
    /// <exception cref="T:System.ArgumentNullException"><paramref name="expandedName"/> is <see langword="null"/>.</exception>
    /// <exception cref="T:System.Exception"/>
    [NotNull]
    public static XpsName Get([NotNull] string expandedName)
    {
      if (expandedName == null)
      {
        throw new ArgumentNullException(nameof(expandedName));
      }

      XNamespace @namespace;
      string localName;
      if (expandedName.StartsWith("{",
                                  StringComparison.Ordinal))
      {
        var index = expandedName.LastIndexOf("}",
                                             StringComparison.Ordinal);
        @namespace = XNamespace.Get(expandedName.Substring(1,
                                                           index - 1));
        localName = expandedName.Substring(index + 1);
      }
      else
      {
        @namespace = XNamespace.None;
        localName = expandedName;
      }

      var result = new XpsName(@namespace,
                               localName);

      return result;
    }

    /// <inheritdoc/>
    [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
    void ISerializable.GetObjectData(SerializationInfo info,
                                     StreamingContext context)
    {
      if (info == null)
      {
        throw new ArgumentNullException(nameof(info));
      }

      info.AddValue(nameof(XpsNameSerializer.ExpandedName),
                    this.ToString());
      info.SetType(typeof(XpsNameSerializer));
    }
  }
}
