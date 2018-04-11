using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using JetBrains.Annotations;

namespace Contrib.System.Printing.Xps
{
  public interface IHasXpsFeatures
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

  public interface IHasXpsProperties
  {
    /// <exception cref="ArgumentNullException"><paramref name="name" /> is <see langword="null" />.</exception>
    [CanBeNull]
    IXpsProperty GetXpsProperty([NotNull] XName name);

    [NotNull]
    [ItemNotNull]
    IXpsProperty[] GetXpsProperties();

    /// <exception cref="ArgumentNullException"><paramref name="xpsProperties" /> is <see langword="null" />.</exception>
    void AddXpsProperties([NotNull] [ItemNotNull] [InstantHandle] IEnumerable<IXpsProperty> xpsProperties);
  }

  public interface IXpsPrintCapabilities : IHasXpsFeatures,
                                           IHasXpsProperties { }

  public interface IXpsPrintCapabilitiesFactory
  {
    [NotNull]
    IXpsPrintCapabilities Create();
  }

  public sealed class XpsPrintCapabilitiesFactory : IXpsPrintCapabilitiesFactory
  {
    private sealed class XpsPrintCapabilities : IXpsPrintCapabilities
    {
      public XpsPrintCapabilities() { }

      [NotNull]
      private IDictionary<XName, IXpsFeature> Features { get; } = new Dictionary<XName, IXpsFeature>();

      [NotNull]
      private IDictionary<XName, IXpsProperty> Properties { get; } = new Dictionary<XName, IXpsProperty>();

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
      public IXpsProperty GetXpsProperty(XName name)
      {
        this.Properties.TryGetValue(name,
                                    out var xpsProperty);

        return xpsProperty;
      }

      /// <inheritdoc />
      public IXpsProperty[] GetXpsProperties()
      {
        return this.Properties.Values.ToArray();
      }

      /// <inheritdoc />
      public void AddXpsFeatures(IEnumerable<IXpsFeature> xpsFeatures)
      {
        foreach (var xpsFeature in xpsFeatures)
        {
          this.Features[xpsFeature.Name] = xpsFeature;
        }
      }

      /// <inheritdoc />
      public void AddXpsProperties(IEnumerable<IXpsProperty> xpsProperties)
      {
        foreach (var xpsProperty in xpsProperties)
        {
          this.Properties[xpsProperty.Name] = xpsProperty;
        }
      }
    }

    /// <inheritdoc />
    public IXpsPrintCapabilities Create()
    {
      var xpsPrintCapabilities = new XpsPrintCapabilities();

      return xpsPrintCapabilities;
    }
  }
}
