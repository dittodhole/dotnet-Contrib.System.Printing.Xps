using System;
using System.Xml.Linq;
using JetBrains.Annotations;

namespace Contrib.System.Printing.Xps.ExtensionMethods
{
  public static class XpsPrintCapabilitiesExtensions
  {
    /// <remarks>
    ///   The first input bin feature is returned:
    ///   <list type="number">
    ///     <item>
    ///       <description><see cref="XpsPrintCapabilitiesReader.PageInputBinXName"/></description>
    ///     </item>
    ///     <item>
    ///       <description><see cref="XpsPrintCapabilitiesReader.DocumentInputBinXName"/></description>
    ///     </item>
    ///     <item>
    ///       <description><see cref="XpsPrintCapabilitiesReader.JobInputBinXName"/></description>
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

      var result = xpsPrintCapabilities.GetXpsFeature(XpsPrintCapabilitiesReader.PageInputBinXName)
                   ?? xpsPrintCapabilities.GetXpsFeature(XpsPrintCapabilitiesReader.DocumentInputBinXName)
                   ?? xpsPrintCapabilities.GetXpsFeature(XpsPrintCapabilitiesReader.JobInputBinXName);

      return result;
    }

    /// <exception cref="ArgumentNullException"><paramref name="xpsPrintCapabilities"/> is <see langword="null"/></exception>
    /// <exception cref="ArgumentNullException"><paramref name="name"/> is <see langword="null"/></exception>
    [Pure]
    [CanBeNull]
    public static IXpsProperty FindXpsProperty([NotNull] this IXpsPrintCapabilities xpsPrintCapabilities,
                                               [NotNull] XName name)
    {
      if (xpsPrintCapabilities == null)
      {
        throw new ArgumentNullException(nameof(xpsPrintCapabilities));
      }
      if (name == null)
      {
        throw new ArgumentNullException(nameof(name));
      }

      var result = HasXpsPropertiesExtensions.FindXpsProperty(xpsPrintCapabilities,
                                                              name);
      if (result == null)
      {
        var xpsFeatures = xpsPrintCapabilities.GetXpsFeatures();
        foreach (var xpsFeature in xpsFeatures)
        {
          result = xpsFeature.FindXpsProperty(name);
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
