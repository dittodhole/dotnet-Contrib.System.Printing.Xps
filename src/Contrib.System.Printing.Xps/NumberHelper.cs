using System.Linq;
using JetBrains.Annotations;

namespace Contrib.System.Printing.Xps
{
  internal static class NumberHelper
  {
    [Pure]
    [CanBeNull]
    public static double? GetDimension([CanBeNull] double? dimension0,
                                       [CanBeNull] double? dimension1,
                                       bool returnMax)
    {
      double? result;
      if (dimension0.HasValue
          && dimension1.HasValue)
      {
        var dimensions = new[]
                         {
                           dimension0.Value,
                           dimension1.Value
                         };
        if (dimensions.Any(double.IsNaN))
        {
          result = null;
        }
        else if (dimensions.Any(double.IsInfinity))
        {
          result = null;
        }
        else
        {
          if (returnMax)
          {
            result = dimensions.Max();
          }
          else
          {
            result = dimensions.Min();
          }

          if (result < double.Epsilon)
          {
            result = null;
          }
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
