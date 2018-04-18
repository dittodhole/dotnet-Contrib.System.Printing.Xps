using System.Xml.Linq;
using JetBrains.Annotations;

namespace Contrib.System.Printing.Xps
{
  /// <summary>
  /// 
  /// </summary>
  [PublicAPI]
  public partial interface IXpsPrinterDefinition
  {
    /// <summary>
    /// 
    /// </summary>
    [NotNull]
    string DisplayName { get; }

    /// <summary>
    /// 
    /// </summary>
    [NotNull]
    string FullName { get; }

    /// <summary>
    /// 
    /// </summary>
    [CanBeNull]
    string PortName { get; }

    /// <summary>
    /// 
    /// </summary>
    [CanBeNull]
    string DriverName { get; }
  }

  /// <summary>
  /// 
  /// </summary>
  public partial interface IXpsPrinterDefinitionFactory
  {
    /// <summary>
    /// 
    /// </summary>
    /// <param name="displayName" />
    /// <param name="fullName" />
    /// <param name="portName" />
    /// <param name="driverName" />
    /// <param name="printCapabilitiesXElement" />
    [NotNull]
    IXpsPrinterDefinition Create([NotNull] string displayName,
                                 [NotNull] string fullName,
                                 [CanBeNull] string portName,
                                 [CanBeNull] string driverName,
                                 [NotNull] XElement printCapabilitiesXElement);
  }

  /// <summary>
  /// 
  /// </summary>
  /// <typeparam name="TXpsPrinterDefinition" />
  public partial interface IXpsPrinterDefinitionFactoryEx<out TXpsPrinterDefinition>
    where TXpsPrinterDefinition : IXpsPrinterDefinition
  {
    /// <summary>
    /// 
    /// </summary>
    /// <param name="displayName" />
    /// <param name="fullName" />
    /// <param name="portName" />
    /// <param name="driverName" />
    /// <param name="printCapabilitiesXElement" />
    [NotNull]
    TXpsPrinterDefinition Create([NotNull] string displayName,
                                 [NotNull] string fullName,
                                 [CanBeNull] string portName,
                                 [CanBeNull] string driverName,
                                 [NotNull] XElement printCapabilitiesXElement);
  }

  /// <inheritdoc cref="IXpsInputBinDefinitionFactoryEx{TXpsInputBinDefinition}"/>
  public sealed partial class XpsPrinterDefinitionFactory : IXpsPrinterDefinitionFactoryEx<IXpsPrinterDefinition>,
                                                            IXpsPrinterDefinitionFactory
  {
    /// <inheritdoc cref="IXpsPrinterDefinition"/>
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
