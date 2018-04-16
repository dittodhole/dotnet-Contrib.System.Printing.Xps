using System.Xml.Linq;
using JetBrains.Annotations;

namespace Contrib.System.Printing.Xps
{
  public partial interface IXpsPrinterDefinition
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

  public partial interface IXpsPrinterDefinitionFactory
  {
    [NotNull]
    IXpsPrinterDefinition Create([NotNull] string displayName,
                                 [NotNull] string fullName,
                                 [CanBeNull] string portName,
                                 [CanBeNull] string driverName,
                                 [NotNull] XElement printCapabilitiesXElement);
  }

  public partial interface IXpsPrinterDefinitionFactoryEx<TXpsPrinterDefinition>
    where TXpsPrinterDefinition : IXpsPrinterDefinition
  {
    [NotNull]
    TXpsPrinterDefinition Create([NotNull] string displayName,
                                 [NotNull] string fullName,
                                 [CanBeNull] string portName,
                                 [CanBeNull] string driverName,
                                 [NotNull] XElement printCapabilitiesXElement);
  }

  public sealed partial class XpsPrinterDefinitionFactory : IXpsPrinterDefinitionFactoryEx<IXpsPrinterDefinition>,
                                                            IXpsPrinterDefinitionFactory
  {
    private sealed partial class XpsPrinterDefinition : IXpsPrinterDefinition
    {
      public XpsPrinterDefinition([NotNull] string displayName,
                                  [NotNull] string fullName,
                                  [CanBeNull] string portName,
                                  [CanBeNull] string driverName)
      {
        this.DisplayName = displayName;
        this.FullName = fullName;
        this.PortName = portName;
        this.DriverName = driverName;
      }

      /// <inheritdoc />
      public string DisplayName { get; }

      /// <inheritdoc />
      public string FullName { get; }

      /// <inheritdoc />
      public string PortName { get; }

      /// <inheritdoc />
      public string DriverName { get; }
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
                                                          driverName);

      return xpsPrinterDefinition;
    }

    /// <inheritdoc />
    IXpsPrinterDefinition IXpsPrinterDefinitionFactory.Create(string displayName,
                                                              string fullName,
                                                              string portName,
                                                              string driverName,
                                                              XElement printCapabilitiesXElement)
    {
      return this.Create(displayName,
                         fullName,
                         portName,
                         driverName,
                         printCapabilitiesXElement);
    }
  }
}
