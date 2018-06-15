/** @pp
 * rootnamespace: Contrib.System
 */
namespace Contrib.System.Printing.Xps
{
  using global::JetBrains.Annotations;

  /// <summary>
  ///   Holds information of a printer.
  /// </summary>
  /// <seealso cref="T:Contrib.System.Printing.Xps.XpsPrinterDefinitionFactory.XpsPrinterDefinition"/>
  [PublicAPI]
#if CONTRIB_SYSTEM_PRINTING_XPS
  public
#else
  internal
#endif
  partial interface IXpsPrinterDefinition
  {
    /// <summary>
    ///   Gets the display name of the printer.
    /// </summary>
    /// <example>"Microsoft XPS Document Writer"</example>
    [NotNull]
    string DisplayName { get; }

    /// <summary>
    ///   Gets the full name of the printer.
    /// </summary>
    /// <example>"Microsoft XPS Document Writer"</example>
    [NotNull]
    string FullName { get; }

    /// <summary>
    ///   Gets the port name of the printer.
    /// </summary>
    /// <example>"PORTPROMPT:"</example>
    [CanBeNull]
    string PortName { get; }

    /// <summary>
    ///   Gets the driver name of the printer.
    /// </summary>
    /// <example>"Microsoft XPS Document Writer v4"</example>
    [CanBeNull]
    string DriverName { get; }
  }

  partial class XpsPrinterDefinitionFactory
  {
    /// <inheritdoc/>
#if CONTRIB_SYSTEM_PRINTING_XPS
    private sealed
#else
    internal
#endif
    partial class XpsPrinterDefinition : IXpsPrinterDefinition
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
  }
}
