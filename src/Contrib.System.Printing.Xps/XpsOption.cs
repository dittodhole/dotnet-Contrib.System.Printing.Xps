using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Contrib.System.Printing.Xps.ExtensionMethods;
using JetBrains.Annotations;

namespace Contrib.System.Printing.Xps
{
  public interface IXpsOption : IHasXpsProperties
  {
    [CanBeNull]
    XName Name { get; }

    [CanBeNull]
    object GetValue([NotNull] XName name);
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

      [NotNull]
      private IDictionary<XName, IXpsProperty> Properties { get; } = new Dictionary<XName, IXpsProperty>();

      /// <inheritdoc />
      public XName Name { get; }

      /// <inheritdoc />
      public object GetValue(XName name)
      {
        object value;

        var xpsProperty = this.FindXpsProperty(name);
        if (xpsProperty == null)
        {
          value = null;
        }
        else
        {
          value = xpsProperty.Value;
        }

        return value;
      }

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
