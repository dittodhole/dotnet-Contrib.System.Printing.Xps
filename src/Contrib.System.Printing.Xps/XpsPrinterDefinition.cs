using System.Xml.Linq;
using JetBrains.Annotations;

namespace Contrib.System.Printing.Xps
{
  [PublicAPI]
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

  public partial interface IXpsPrinterDefinitionFactoryEx<out TXpsPrinterDefinition>
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
    private partial struct XpsPrinterDefinition : IXpsPrinterDefinition
    {
      /// <inheritdoc />
      public string DisplayName { get; set; }

      /// <inheritdoc />
      public string FullName { get; set; }

      /// <inheritdoc />
      public string PortName { get; set; }

      /// <inheritdoc />
      public string DriverName { get; set; }
    }

    /// <inheritdoc />
    public IXpsPrinterDefinition Create(string displayName,
                                        string fullName,
                                        string portName,
                                        string driverName,
                                        XElement printCapabilitiesXElement)
    {
      var xpsPrinterDefinition = new XpsPrinterDefinition
                                 {
                                   DisplayName = displayName,
                                   FullName = fullName,
                                   PortName = portName,
                                   DriverName = driverName
                                 };

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
