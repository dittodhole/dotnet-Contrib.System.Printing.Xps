using System;
using System.Xml.Linq;
using JetBrains.Annotations;

namespace Contrib.System.Printing.Xps.ExtensionMethods
{
  public static class HasValuesExtensions
  {
    /// <exception cref="ArgumentNullException"><paramref name="hasValues"/> is <see langword="null"/></exception>
    /// <exception cref="ArgumentNullException"><paramref name="name"/> is <see langword="null"/></exception>
    [Pure]
    [ContractAnnotation("=> true, value:notnull; => false, value:null")]
    public static bool TryGetValue<T>([NotNull] this IHasValues hasValues,
                                      [NotNull] XName name,
                                      out T value)
    {
      if (hasValues == null)
      {
        throw new ArgumentNullException(nameof(hasValues));
      }
      if (name == null)
      {
        throw new ArgumentNullException(nameof(name));
      }

      bool result;

      var obj = hasValues.GetValue(name);
      if (obj == null)
      {
        value = default(T);
        result = false;
      }
      else
        if (obj is T variable)
        {
          value = variable;
          result = true;
        }
      else
      {
        value = default(T);
        result = false;
      }

      return result;
    }
  }
}
