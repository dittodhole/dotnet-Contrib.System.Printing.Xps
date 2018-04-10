using System;
using JetBrains.Annotations;

namespace Contrib.System.Printing.Xps.ExtensionMethods
{
  public static class XpsPrintCapabilitiesExtensions
  {
    /// <exception cref="ArgumentNullException"><paramref name="xpsPrintCapabilities"/> is <see langword="null"/></exception>
    [Pure]
    [CanBeNull]
    public static IXpsFeature GetInputBinXpsFeature([NotNull] this IXpsPrintCapabilities xpsPrintCapabilities)
    {
      if (xpsPrintCapabilities == null)
      {
        throw new ArgumentNullException(nameof(xpsPrintCapabilities));
      }

      {
        var pageInputBinXpsFeature = xpsPrintCapabilities.GetXpsFeature(PrintCapabilitiesReader.PageInputBinXName);
        if (pageInputBinXpsFeature != null)
        {
          return pageInputBinXpsFeature;
        }
      }

      {
        var documentInputBinXpsFeature = xpsPrintCapabilities.GetXpsFeature(PrintCapabilitiesReader.DocumentInputBinXName);
        if (documentInputBinXpsFeature != null)
        {
          return documentInputBinXpsFeature;
        }
      }

      {
        var jobInputBinXpsFeature = xpsPrintCapabilities.GetXpsFeature(PrintCapabilitiesReader.JobInputBinXName);
        if (jobInputBinXpsFeature != null)
        {
          return jobInputBinXpsFeature;
        }
      }

      return null;
    }
  }
}
