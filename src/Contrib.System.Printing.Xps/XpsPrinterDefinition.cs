using System;
using System.Linq;
using System.Xml.Linq;
using Contrib.System.Printing.Xps.ExtensionMethods;
using JetBrains.Annotations;

namespace Contrib.System.Printing.Xps
{
  public interface IHasValues
  {
    [CanBeNull]
    object GetValue([NotNull] [ItemNotNull] params XName[] names);
  }

  public interface IXpsPrinterDefinition : IHasValues
  {
    [NotNull]
    string DisplayName { get; }

    [NotNull]
    string FullName { get; }

    [CanBeNull]
    string PortName { get; }

    [CanBeNull]
    string DriverName { get; }
  }

  public interface IXpsPrinterDefinitionFactory
  {
    [NotNull]
    IXpsPrinterDefinition Create([NotNull] string displayName,
                                 [NotNull] string fullName,
                                 [CanBeNull] string portName,
                                 [CanBeNull] string driverName,
                                 [NotNull] XElement printCapabilitiesXElement);
  }

  public sealed class XpsPrinterDefinitionFactory : IXpsPrinterDefinitionFactory
  {
    private sealed class XpsPrinterDefinition : IXpsPrinterDefinition
    {
      public XpsPrinterDefinition([NotNull] string displayName,
                                  [NotNull] string fullName,
                                  [CanBeNull] string portName,
                                  [CanBeNull] string driverName,
                                  [NotNull] XElement printCapabilitiesXElement)
      {
        this.DisplayName = displayName;
        this.FullName = fullName;
        this.PortName = portName;
        this.DriverName = driverName;
        this.PrintCapabilitiesXElement = printCapabilitiesXElement;
      }

      [NotNull]
      private XElement PrintCapabilitiesXElement  { get; }

      /// <inheritdoc />
      public string DisplayName { get; }

      /// <inheritdoc />
      public string FullName { get; }

      /// <inheritdoc />
      public string PortName { get; }

      /// <inheritdoc />
      public string DriverName { get; }

      /// <inheritdoc />
      public object GetValue(params XName[] names)
      {
        object value;

        var xelement = names.Aggregate(this.PrintCapabilitiesXElement,
                                       (current,
                                        name) => current?.FindElementByNameAttribute(name));
        if (xelement == null)
        {
          value = null;
        }
        else
        {
          value = xelement.GetValueFromValueElement();
        }

        return value;
      }
    }

    /// <inheritdoc />
    public IXpsPrinterDefinition Create(string displayName,
                                        string fullName,
                                        string portName,
                                        string driverName,
                                        XElement printCapabilitiesXElement)
    {
      var xpsPrinterDefinition = new XpsPrinterDefinition(displayName,
                                                          fullName,
                                                          portName,
                                                          driverName,
                                                          printCapabilitiesXElement);

      return xpsPrinterDefinition;
    }
  }
}
