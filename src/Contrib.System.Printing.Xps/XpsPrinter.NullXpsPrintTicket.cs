using System.Collections.Generic;
using System.Xml.Linq;
using JetBrains.Annotations;

namespace Contrib.System.Printing.Xps
{
  public partial class XpsPrinter
  {
    protected sealed class NullXpsPrintTicket : IXpsPrintTicket
    {
      [NotNull]
      public static NullXpsPrintTicket Instance { get; } = new NullXpsPrintTicket();

      private NullXpsPrintTicket() { }

      /// <inheritdoc />
      public IXpsFeature GetXpsFeature(XName name) => null;

      /// <inheritdoc />
      public IXpsFeature[] GetXpsFeatures() => new IXpsFeature[0];

      /// <inheritdoc />
      public void AddXpsFeatures(IEnumerable<IXpsFeature> xpsFeatures) { }
    }
  }
}
