/** @pp
 * rootnamespace: Contrib.System.Printing.Xps
 */
namespace Contrib.System.Printing.Xps.ExtensionMethods
{
  using global::System;
  using global::System.Xml.Linq;
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
  static partial class XNamespaceExtensions
  {
    /// <summary>
    ///   Returns an <see cref="XpsName"/> object created from this <see cref="XNamespace"/> and the specified local name.
    /// </summary>
    /// <param name="namespace"/>
    /// <param name="localName"/>
    /// <exception cref="ArgumentNullException"><paramref name="namespace"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="localName"/> is <see langword="null"/>.</exception>
    public static XpsName GetXpsName([NotNull] this XNamespace @namespace,
                                     [NotNull] string localName)
    {
      if (@namespace == null)
      {
        throw new ArgumentNullException(nameof(@namespace));
      }
      if (localName == null)
      {
        throw new ArgumentNullException(nameof(localName));
      }

      var result = new XpsName(@namespace,
                               localName);

      return result;
    }
  }
}
