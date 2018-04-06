﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using JetBrains.Annotations;

namespace Contrib.System.Printing.Xps
{
  public interface IXpsOption
  {
    [CanBeNull]
    XName Name { get; }

    [NotNull]
    [ItemNotNull]
    IXpsProperty[] GetXpsProperties();

    /// <exception cref="ArgumentNullException"><paramref name="name" /> is <see langword="null" />.</exception>
    [CanBeNull]
    IXpsProperty GetXpsProperty([NotNull] XName name);

    /// <exception cref="ArgumentNullException"><paramref name="xpsProperties" /> is <see langword="null" />.</exception>
    void AddXpsProperties([NotNull] [ItemNotNull] [InstantHandle] IEnumerable<IXpsProperty> xpsProperties);
  }

  public interface IXpsOptionFactory
  {
    [NotNull]
    IXpsOption Create();

    [NotNull]
    IXpsOption Create([NotNull] XName name);
  }

  public sealed class XpsOptionFactory : IXpsOptionFactory
  {
    private sealed class XpsOption : IXpsOption
    {
      public XpsOption() { }

      public XpsOption([NotNull] XName name)
      {
        this.Name = name;
      }

      /// <inheritdoc />
      public XName Name { get; }

      [NotNull]
      private IDictionary<XName, IXpsProperty> Properties { get; } = new Dictionary<XName, IXpsProperty>();

      /// <inheritdoc />
      public IXpsProperty[] GetXpsProperties()
      {
        return this.Properties.Values.ToArray();
      }

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
    public IXpsOption Create()
    {
      var xpsOption = new XpsOption();

      return xpsOption;
    }

    /// <inheritdoc />
    public IXpsOption Create(XName name)
    {
      var xpsOption = new XpsOption(name);

      return xpsOption;
    }
  }
}
