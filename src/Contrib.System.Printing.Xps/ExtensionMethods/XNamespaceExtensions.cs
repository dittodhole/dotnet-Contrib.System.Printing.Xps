/** @pp
 * rootnamespace: Contrib.System
 */
namespace Contrib.System.Printing.Xps.ExtensionMethods
{
  using global::System;
  using global::System.Xml.Linq;
  using global::JetBrains.Annotations;

  /// <summary>
  ///   Provides extensions for <see cref="T:System.Linq.Xml.XElement"/> objects.
  /// </summary>
#if CONTRIB_SYSTEM_PRINTING_XPS
  public
#else
  internal
#endif
  static partial class XNamespaceExtensions
  {
    /// <summary>
    ///   Returns an <see cref="T:Contrib.System.Printing.Xps.XpsName"/> object created from this <see cref="XNamespace"/> and the specified local name.
    /// </summary>
    /// <param name="namespace"/>
    /// <param name="localName"/>
    /// <exception cref="T:System.ArgumentNullException"><paramref name="namespace"/> is <see langword="null"/>.</exception>
    /// <exception cref="T:System.ArgumentNullException"><paramref name="localName"/> is <see langword="null"/>.</exception>
    [Pure]
    [NotNull]
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
