using System.Linq;
using JetBrains.Annotations;

namespace Contrib.System.Printing.Xps
{
  public static class NumberHelper
  {
    [Pure]
    [CanBeNull]
    public static long? GetDimension([CanBeNull] long? dimension0,
                                     [CanBeNull] long? dimension1,
                                     bool returnMax)
    {
      // TODO remove LINQ stuff, opt here!

      long? result;
      if (dimension0.HasValue
          && dimension1.HasValue)
      {
        var dimensions = new[]
                         {
                           dimension0.Value,
                           dimension1.Value
                         };
        if (returnMax)
        {
          result = dimensions.Max();
        }
        else
        {
          result = dimensions.Min();
        }
      }
      else
      {
        result = null;
      }

      return result;
    }
  }
}
