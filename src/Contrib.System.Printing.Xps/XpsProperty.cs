using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using JetBrains.Annotations;

namespace Contrib.System.Printing.Xps
{
  public interface IXpsProperty
  {
    /// <remarks>
    ///   The value is one of the following (but not limited to):
    ///   <list type="bullet">
    ///     <item>
    ///       <description><see cref="XpsPrintCapabilitiesReader.DisplayNameXName"/></description>
    ///     </item>
    ///     <item>
    ///       <description><see cref="XpsPrintCapabilitiesReader.MediaSizeWidthXName"/></description>
    ///     </item>
    ///     <item>
    ///       <description><see cref="XpsPrintCapabilitiesReader.MediaSizeHeightXName"/></description>
    ///     </item>
    ///     <item>
    ///       <description><see cref="XpsPrintCapabilitiesReader.ImageableSizeWidthXName"/></description>
    ///     </item>
    ///     <item>
    ///       <description><see cref="XpsPrintCapabilitiesReader.ImageableSizeHeightXName"/></description>
    ///     </item>
    ///     <item>
    ///       <description><see cref="XpsPrintCapabilitiesReader.PageImageableSizeXName"/></description>
    ///     </item>
    ///   </list>
    /// </remarks>
    [NotNull]
    XName Name { get; }

    /// <remarks>
    ///   The value is one of the following:
    ///   <list type="bullet">
    ///     <item>
    ///       <description><see cref="XpsPrintCapabilitiesReader.PropertyElementXName"/></description>
    ///     </item>
    ///     <item>
    ///       <description><see cref="XpsPrintCapabilitiesReader.ScoredPropertyElementXName"/></description>
    ///     </item>
    ///   </list>
    /// </remarks>
    [NotNull]
    XName Type { get; }

    [CanBeNull]
    object Value { get; }

    /// <exception cref="ArgumentNullException"><paramref name="name" /> is <see langword="null" />.</exception>
    [CanBeNull]
    IXpsProperty GetXpsProperty([NotNull] XName name);

    [NotNull]
    [ItemNotNull]
    IXpsProperty[] GetXpsProperties();

    /// <exception cref="ArgumentNullException"><paramref name="xpsProperties" /> is <see langword="null" />.</exception>
    void AddXpsProperties([NotNull] [ItemNotNull] [InstantHandle] IEnumerable<IXpsProperty> xpsProperties);
  }

  public interface IXpsPropertyFactory
  {
    [NotNull]
    IXpsProperty Create([NotNull] XName name,
                        [NotNull] XName type,
                        [NotNull] object value);

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
                         [NotNull] object value)
      {
        this.Name = name;
        this.Type = type;
        this.Value = value;
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
      public object Value { get; }

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
      public IXpsProperty[] GetXpsProperties()
      {
        return this.Properties.Values.ToArray();
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
                               object value)
    {
      var xpsProperty = new XpsProperty(name,
                                        type,
                                        value);

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
