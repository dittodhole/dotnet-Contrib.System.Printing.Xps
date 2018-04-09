using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using JetBrains.Annotations;

namespace Contrib.System.Printing.Xps
{
  public interface IXpsFeature
  {
    /// <seealso cref="PrintCapabilitiesReader.JobInputBinXName"/>
    /// <seealso cref="PrintCapabilitiesReader.PageInputBinXName"/>
    /// <seealso cref="PrintCapabilitiesReader.DocumentInputBinXName"/>
    /// <seealso cref="PrintCapabilitiesReader.PageMediaSizeXName"/>
    [NotNull]
    XName Name { get; }

    /// <exception cref="ArgumentNullException"><paramref name="name" /> is <see langword="null" />.</exception>
    [CanBeNull]
    IXpsProperty GetXpsProperty([NotNull] XName name);

    [NotNull]
    [ItemNotNull]
    IXpsProperty[] GetXpsProperties();

    /// <exception cref="ArgumentNullException"><paramref name="name" /> is <see langword="null" />.</exception>
    [CanBeNull]
    IXpsOption GetXpsOption([NotNull] XName name);

    [NotNull]
    [ItemNotNull]
    IXpsOption[] GetXpsOptions();

    /// <exception cref="ArgumentNullException"><paramref name="xpsProperties" /> is <see langword="null" />.</exception>
    void AddXpsProperties([NotNull] [ItemNotNull] [InstantHandle] IEnumerable<IXpsProperty> xpsProperties);

    /// <exception cref="ArgumentNullException"><paramref name="xpsOptions" /> is <see langword="null" />.</exception>
    void AddXpsOptions([NotNull] [ItemNotNull] [InstantHandle] IEnumerable<IXpsOption> xpsOptions);
  }

  public interface IXpsFeatureFactory
  {
    [NotNull]
    IXpsFeature Create([NotNull] XName name);
  }

  public sealed class XpsFeatureFactory : IXpsFeatureFactory
  {
    private sealed class XpsFeature : IXpsFeature
    {
      public XpsFeature([NotNull] XName name)
      {
        this.Name = name;
      }

      /// <inheritdoc />
      public XName Name { get; }

      [NotNull]
      private IDictionary<XName, IXpsProperty> Properties { get; } = new Dictionary<XName, IXpsProperty>();

      [NotNull]
      private IDictionary<XName, IXpsOption> Options { get; } = new Dictionary<XName, IXpsOption>();

      [NotNull]
      private ICollection<IXpsOption> UnnamedOptions { get; } = new List<IXpsOption>();

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
      public IXpsOption GetXpsOption(XName name)
      {
        this.Options.TryGetValue(name,
                                 out var xpsOption);

        return xpsOption;
      }

      /// <inheritdoc />
      public IXpsOption[] GetXpsOptions()
      {
        return this.UnnamedOptions.Concat(this.Options.Values)
                   .ToArray();
      }

      /// <inheritdoc />
      public void AddXpsProperties(IEnumerable<IXpsProperty> xpsProperties)
      {
        foreach (var xpsProperty in xpsProperties)
        {
          this.Properties[xpsProperty.Name] = xpsProperty;
        }
      }

      /// <inheritdoc />
      public void AddXpsOptions(IEnumerable<IXpsOption> xpsOptions)
      {
        foreach (var xpsOption in xpsOptions)
        {
          var key = xpsOption.Name;
          if (key == null)
          {
            this.UnnamedOptions.Add(xpsOption);
          }
          else
          {
            this.Options[key] = xpsOption;
          }
        }
      }
    }

    /// <inheritdoc />
    public IXpsFeature Create(XName name)
    {
      var xpsFeature = new XpsFeature(name);

      return xpsFeature;
    }
  }
}
