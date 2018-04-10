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

      var result = xpsPrintCapabilities.GetXpsFeature(PrintCapabilitiesReader.PageInputBinXName)
                   ?? xpsPrintCapabilities.GetXpsFeature(PrintCapabilitiesReader.DocumentInputBinXName)
                   ?? xpsPrintCapabilities.GetXpsFeature(PrintCapabilitiesReader.JobInputBinXName);

      return result;
    }
  }
}
