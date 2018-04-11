using System;
using System.Xml.Linq;
using JetBrains.Annotations;

namespace Contrib.System.Printing.Xps.ExtensionMethods
{
  public static class HasXpsPropertiesExtensions
  {
    /// <exception cref="ArgumentNullException"><paramref name="hasXpsProperties"/> is <see langword="null"/></exception>
    /// <exception cref="ArgumentNullException"><paramref name="name"/> is <see langword="null"/></exception>
    [CanBeNull]
    public static IXpsProperty FindXpsProperty([NotNull] this IHasXpsProperties hasXpsProperties,
                                               [NotNull] XName name)
    {
      if (hasXpsProperties == null)
      {
        throw new ArgumentNullException(nameof(hasXpsProperties));
      }
      if (name == null)
      {
        throw new ArgumentNullException(nameof(name));
      }

      var result = hasXpsProperties.GetXpsProperty(name);
      if (result == null)
      {
        var xpsProperties = hasXpsProperties.GetXpsProperties();
        foreach (var xpsProperty in xpsProperties)
        {
          result = xpsProperty.FindXpsProperty(name);
          if (result != null)
          {
            break;
          }
        }
      }

      return result;
    }
  }
}
