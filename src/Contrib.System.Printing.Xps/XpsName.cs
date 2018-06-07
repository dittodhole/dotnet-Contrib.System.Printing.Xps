using System;
using System.Xml;
using System.Xml.Linq;
using JetBrains.Annotations;

namespace Contrib.System.Printing.Xps
{
  /// <summary>
  ///
  /// </summary>
  [PublicAPI]
#if CONTRIB_SYSTEM_PRINTING_XPS
  public sealed
#else
  internal
#endif
  partial class XpsName
  {
    /// <summary>
    ///   Initializes a new instance of the <see cref="XpsName"/> class.
    /// </summary>
    /// <param name="namespace"/>
    /// <param name="localName"/>
    /// <exception cref="ArgumentNullException"><paramref name="namespace"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="localName"/> is <see langword="null"/>.</exception>
    public XpsName([NotNull] XNamespace @namespace,
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
    /// <remarks>The local name may start with a digit (eg "16bpcSupport" - this behavior differs from <see cref="XName"/> validation.</remarks>
    [NotNull]
    public string LocalName { get; }

    /// <summary>
    ///   Returns the prefixed <see cref="XpsName.LocalName"/>.
    /// </summary>
    /// <param name="prefix"/>
    /// <remarks>The string representation follows the pattern "<paramref name="prefix"/>:<see cref="XpsName.LocalName"/>"</remarks>
    [NotNull]
    public string ToString([CanBeNull] string prefix)
    {
      var result = XmlQualifiedName.ToString(this.LocalName,
                                             prefix);

      return result;
    }
  }
}
