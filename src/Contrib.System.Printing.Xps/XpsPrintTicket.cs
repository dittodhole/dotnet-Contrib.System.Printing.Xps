using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using JetBrains.Annotations;

namespace Contrib.System.Printing.Xps
{
  public interface IXpsPrintTicket
  {
    /// <exception cref="ArgumentNullException"><paramref name="name" /> is <see langword="null" />.</exception>
    [CanBeNull]
    IXpsFeature GetXpsFeature([NotNull] XName name);

    [NotNull]
    [ItemNotNull]
    IXpsFeature[] GetXpsFeatures();

    /// <exception cref="ArgumentNullException"><paramref name="xpsFeatures" /> is <see langword="null" />.</exception>
    void AddXpsFeatures([NotNull] [ItemNotNull] [InstantHandle] IEnumerable<IXpsFeature> xpsFeatures);
  }

  public interface IXpsPrintTicketFactory
  {
    [NotNull]
    IXpsPrintTicket Create();
  }

  public sealed class XpsPrintTicketFactory : IXpsPrintTicketFactory
  {
    private sealed class XpsPrintTicket : IXpsPrintTicket
    {
      public XpsPrintTicket() { }

      [NotNull]
      private IDictionary<XName, IXpsFeature> Features { get; } = new Dictionary<XName, IXpsFeature>();

      /// <inheritdoc />
      public IXpsFeature GetXpsFeature(XName name)
      {
        this.Features.TryGetValue(name,
                                  out var xpsFeature);

        return xpsFeature;
      }

      /// <inheritdoc />
      public IXpsFeature[] GetXpsFeatures()
      {
        return this.Features.Values.ToArray();
      }

      /// <inheritdoc />
      public void AddXpsFeatures(IEnumerable<IXpsFeature> xpsFeatures)
      {
        foreach (var xpsFeature in xpsFeatures)
        {
          this.Features[xpsFeature.Name] = xpsFeature;
        }
      }
    }

    /// <inheritdoc />
    public IXpsPrintTicket Create()
    {
      var xpsPrintTicket = new XpsPrintTicket();

      return xpsPrintTicket;
    }
  }
}
