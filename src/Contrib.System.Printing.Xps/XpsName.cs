using System;
using System.Xml.Linq;
using JetBrains.Annotations;

namespace Contrib.System.Printing.Xps
{
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

    [NotNull]
    public XNamespace Namespace { get; }

    [NotNull]
    public string LocalName { get; }

    [NotNull]
    public string ToString([CanBeNull] string prefix)
    {
      string result;
      if (string.IsNullOrEmpty(prefix))
      {
        result = this.LocalName;
      }
      else
      {
        result = $"{prefix}:{this.LocalName}";
      }

      return result;
    }

    [CanBeNull]
    public static implicit operator XpsName([CanBeNull] XName name)
    {
      XpsName result;
      if (name == null)
      {
        result = null;
      }
      else
      {
        result = new XpsName(name.Namespace,
                             name.LocalName);
      }

      return result;
    }
  }
}
