using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace Contrib.System.Printing.Xps.ExtensionMethods
{
  public static class XpsPrintCapabilitiesExtensions
  {
    /// <exception cref="ArgumentNullException"><paramref name="xpsPrintCapabilities"/> is <see langword="null"/></exception>
    [NotNull]
    [ItemNotNull]
    public static IXpsFeature[] GetInputBinXpsFeatures([NotNull] this IXpsPrintCapabilities xpsPrintCapabilities)
    {
      if (xpsPrintCapabilities == null)
      {
        throw new ArgumentNullException(nameof(xpsPrintCapabilities));
      }

      var inputBinXpsFeatures = xpsPrintCapabilities.GetInputBinXpsFeaturesImpl();
      var result = inputBinXpsFeatures.ToArray();

      return result;
    }

    [NotNull]
    [ItemNotNull]
    internal static IEnumerable<IXpsFeature> GetInputBinXpsFeaturesImpl([NotNull] this IXpsPrintCapabilities xpsPrintCapabilities)
    {
      {
        var pageInputBinXpsFeature = xpsPrintCapabilities.GetXpsFeature(PrintCapabilitiesReader.PageInputBinXName);
        if (pageInputBinXpsFeature != null)
        {
          yield return pageInputBinXpsFeature;
        }
      }

      {
        var documentInputBinXpsFeature = xpsPrintCapabilities.GetXpsFeature(PrintCapabilitiesReader.DocumentInputBinXName);
        if (documentInputBinXpsFeature != null)
        {
          yield return documentInputBinXpsFeature;
        }
      }

      {
        var jobInputBinXpsFeature = xpsPrintCapabilities.GetXpsFeature(PrintCapabilitiesReader.JobInputBinXName);
        if (jobInputBinXpsFeature != null)
        {
          yield return jobInputBinXpsFeature;
        }
      }
    }
  }
}
