using System;
using System.Collections.Generic;
using System.Xml.Linq;
using JetBrains.Annotations;

namespace Contrib.System.Printing.Xps
{
  public interface IXpsProperty
  {
    /// <seealso cref="PrintCapabilitiesReader.DisplayNameXName"/>
    /// <seealso cref="PrintCapabilitiesReader.MediaSizeWidthXName"/>
    /// <seealso cref="PrintCapabilitiesReader.MediaSizeHeightXName"/>
    /// <seealso cref="PrintCapabilitiesReader.ImageableSizeWidthXName"/>
    /// <seealso cref="PrintCapabilitiesReader.ImageableSizeHeightXName"/>
    /// <seealso cref="PrintCapabilitiesReader.PageImageableSizeXName"/>
    [NotNull]
    XName Name { get; }

    [NotNull]
    XName Type { get; }

    [CanBeNull]
    string Value { get; }

    [CanBeNull]
    XName ValueXName { get; }

    /// <exception cref="ArgumentNullException"><paramref name="name" /> is <see langword="null" />.</exception>
    [CanBeNull]
    IXpsProperty GetXpsProperty([NotNull] XName name);

    /// <exception cref="ArgumentNullException"><paramref name="xpsProperties" /> is <see langword="null" />.</exception>
    void AddXpsProperties([NotNull] [ItemNotNull] [InstantHandle] IEnumerable<IXpsProperty> xpsProperties);
  }

  public interface IXpsPropertyFactory
  {
    [NotNull]
    IXpsProperty Create([NotNull] XName name,
                        [NotNull] XName type,
                        [CanBeNull] string value,
                        [CanBeNull] XName valueXName);

    [NotNull]
    IXpsProperty Create([NotNull] XName name,
                        [NotNull] XName type);
  }

  public sealed class XpsPropertyFactory : IXpsPropertyFactory
  {
    private sealed class XpsProperty : IXpsProperty
    {
      public XpsProperty([NotNull] XName name,
                         [NotNull] XName type,
                         [NotNull] string value,
                         [CanBeNull] XName valueXName)
      {
        this.Name = name;
        this.Type = type;
        this.Value = value;
        this.ValueXName = valueXName;
      }

      public XpsProperty([NotNull] XName name,
                         [NotNull] XName type)
      {
        this.Name = name;
        this.Type = type;
      }

      /// <inheritdoc />
      public XName Name { get; }

      /// <inheritdoc />
      public XName Type { get; }

      /// <inheritdoc />
      public string Value { get; }

      /// <inheritdoc />
      public XName ValueXName { get; }

      [NotNull]
      private IDictionary<XName, IXpsProperty> Properties { get; } = new Dictionary<XName, IXpsProperty>();

      /// <inheritdoc />
      public IXpsProperty GetXpsProperty(XName name)
      {
        this.Properties.TryGetValue(name,
                                    out var xpsProperty);

        return xpsProperty;
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
    public IXpsProperty Create(XName name,
                               XName type,
                               string value,
                               XName valueXName)
    {
      var xpsProperty = new XpsProperty(name,
                                        type,
                                        value,
                                        valueXName);

      return xpsProperty;
    }

    /// <inheritdoc />
    public IXpsProperty Create(XName name,
                               XName type)
    {
      var xpsProperty = new XpsProperty(name,
                                        type);

      return xpsProperty;
    }
  }
}
