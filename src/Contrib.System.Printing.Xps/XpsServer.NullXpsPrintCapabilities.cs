using System.Collections.Generic;
using System.Xml.Linq;
using JetBrains.Annotations;

namespace Contrib.System.Printing.Xps
{
  public partial class XpsServer
  {
    protected sealed class NullXpsPrintCapabilities : IXpsPrintCapabilities
    {
      [NotNull]
      public static NullXpsPrintCapabilities Instance = new NullXpsPrintCapabilities();

      private NullXpsPrintCapabilities() { }

      /// <inheritdoc />
      public IXpsFeature GetXpsFeature(XName name) => null;

      /// <inheritdoc />
      public IXpsFeature[] GetXpsFeatures() => new IXpsFeature[0];

      /// <inheritdoc />
      public IXpsProperty GetXpsProperty(XName name) => null;

      /// <inheritdoc />
      public IXpsProperty[] GetXpsProperties() => new IXpsProperty[0];

      /// <inheritdoc />
      public void AddXpsFeatures(IEnumerable<IXpsFeature> xpsFeatures) { }

      /// <inheritdoc />
      public void AddXpsProperties(IEnumerable<IXpsProperty> xpsProperties) { }
    }
  }
}
