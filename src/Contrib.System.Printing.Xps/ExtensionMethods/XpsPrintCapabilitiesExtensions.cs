using System;
using JetBrains.Annotations;

namespace Contrib.System.Printing.Xps.ExtensionMethods
{
  public static class XpsPrintCapabilitiesExtensions
  {
    /// <remarks>
    ///   The first input bin feature is returned:
    ///   <list type="number">
    ///     <item>
    ///       <description><see cref="PrintCapabilitiesReader.PageInputBinXName"/></description>
    ///     </item>
    ///     <item>
    ///       <description><see cref="PrintCapabilitiesReader.DocumentInputBinXName"/></description>
    ///     </item>
    ///     <item>
    ///       <description><see cref="PrintCapabilitiesReader.JobInputBinXName"/></description>
    ///     </item>
    ///   </list>
    /// </remarks>
    /// <exception cref="ArgumentNullException"><paramref name="xpsPrintCapabilities"/> is <see langword="null"/></exception>
    [Pure]
    [CanBeNull]
    public static IXpsFeature FindInputBinXpsFeature([NotNull] this IXpsPrintCapabilities xpsPrintCapabilities)
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
