using System.Xml.Linq;
using JetBrains.Annotations;

namespace Contrib.System.Printing.Xps
{
  /// <summary>
  ///   Holds information of a printer.
  /// </summary>
  /// <seealso cref="XpsPrinterDefinitionFactory.XpsPrinterDefinition"/>
  [PublicAPI]
  public partial interface IXpsPrinterDefinition
  {
    /// <summary>
    ///   The display name of the printer.
    /// </summary>
    /// <example>Microsoft XPS Document Writer</example>
    [NotNull]
    string DisplayName { get; }

    /// <summary>
    ///   The full name of the printer.
    /// </summary>
    /// <example>Microsoft XPS Document Writer</example>
    [NotNull]
    string FullName { get; }

    /// <summary>
    ///   The port name of the printer.
    /// </summary>
    /// <example>PORTPROMPT:</example>
    [CanBeNull]
    string PortName { get; }

    /// <summary>
    ///   The driver name of the printer.
    /// </summary>
    /// <example>Microsoft XPS Document Writer v4</example>
    [CanBeNull]
    string DriverName { get; }
  }

  /// <summary>
  ///   Factory class for <typeparamref name="TXpsPrinterDefinition"/>.
  /// </summary>
  /// <typeparam name="TXpsPrinterDefinition"/>
  /// <seealso cref="XpsPrinterDefinitionFactory"/>
  /// <example><code>
  /// using Contrib.System.Printing.Xps;
  /// using System.Xml.Linq;
  ///
  /// public interface ICustomXpsPrinterDefinition : IXpsPrinterDefinition { }
  ///
  /// public class CustomXpsPrinterDefinitionFactory : IXpsPrinterDefinitionFactoryEx&lt;ICustomXpsPrinterDefinition&gt;
  /// {
  ///   private class CustomXpsPrinterDefinition : ICustomXpsPrinterDefinition
  ///   {
  ///     public string DisplayName { get; set; }
  ///     public string FullName { get; set; }
  ///     public string PortName { get; set; }
  ///     public string DriverName { get; set; }
  ///   }
  ///
  ///   private IXpsPrinterDefinitionFactory XpsPrinterDefinitionFactory { get; } = new XpsPrinterDefinitionFactory();
  ///
  ///   public ICustomXpsPrinterDefinition Create(string displayName,
  ///                                             string fullName,
  ///                                             string portName,
  ///                                             string driverName,
  ///                                             XElement printCapabilitiesXElement)
  ///   {
  ///     var xpsPrinterDefinition = this.XpsPrinterDefinitionFactory.Create(displayName,
  ///                                                                        fullName,
  ///                                                                        portName,
  ///                                                                        driverName,
  ///                                                                        printCapabilitiesXElement);
  ///     var customXpsPrinterDefinition = new CustomXpsPrinterDefinition
  ///                                      {
  ///                                        DisplayName = xpsPrinterDefinition.DisplayName,
  ///                                        FullName = xpsPrinterDefinition.FullName,
  ///                                        PortName = xpsPrinterDefinition.PortName,
  ///                                        DriverName = xpsPrinterDefinition.DriverName
  ///                                      };
  ///
  ///     // TODO use printCapabilitiesXElement with Contrib.System.Printing.Xps.ExtensionMethods.XElementExtensions to extract needed values
  ///
  ///     return customXpsPrinterDefinition;
  ///   }
  /// }
  /// </code></example>
  [PublicAPI]
  public partial interface IXpsPrinterDefinitionFactory<out TXpsPrinterDefinition>
    where TXpsPrinterDefinition : IXpsPrinterDefinition
  {
    /// <summary>
    ///   Factory method for <typeparamref name="TXpsPrinterDefinition"/>.
    /// </summary>
    /// <param name="displayName"/>
    /// <param name="fullName"/>
    /// <param name="portName"/>
    /// <param name="driverName"/>
    /// <param name="printCapabilitiesXElement"/>
    [NotNull]
    TXpsPrinterDefinition Create([NotNull] string displayName,
                                 [NotNull] string fullName,
                                 [CanBeNull] string portName,
                                 [CanBeNull] string driverName,
                                 [NotNull] XElement printCapabilitiesXElement);
  }

  /// <inheritdoc cref="IXpsInputBinDefinitionFactory{TXpsInputBinDefinition}"/>
  [PublicAPI]
  public sealed partial class XpsPrinterDefinitionFactory : IXpsPrinterDefinitionFactory<IXpsPrinterDefinition>
  {
    /// <inheritdoc cref="IXpsPrinterDefinition"/>
    private partial struct XpsPrinterDefinition : IXpsPrinterDefinition
    {
      /// <inheritdoc/>
      public string DisplayName { get; set; }

      /// <inheritdoc/>
      public string FullName { get; set; }

      /// <inheritdoc/>
      public string PortName { get; set; }

      /// <inheritdoc/>
      public string DriverName { get; set; }
    }

    /// <summary>
    ///   Initializes a new instance of the <see cref="XpsPrinterDefinitionFactory"/> class.
    /// </summary>
    [PublicAPI]
    public XpsPrinterDefinitionFactory() { }

    /// <inheritdoc/>
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
  }
}
