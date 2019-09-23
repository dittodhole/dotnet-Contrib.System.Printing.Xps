/** @pp
 * rootnamespace: Contrib.System
 */
namespace Contrib.System.Printing.Xps
{
  using global::System.Xml.Linq;
  using global::JetBrains.Annotations;

  partial class XpsServer
  {
    /// <summary>
    ///   psf:PrintCapabilities
    /// </summary>
    [NotNull]
    public static XName PrintCapabilitiesName => XpsServer.PrinterSchemaFrameworkNamespace.GetName("PrintCapabilities");

    /// <summary>
    ///   psf:PrintTicket
    /// </summary>
    [NotNull]
    public static XName PrintTicketName => XpsServer.PrinterSchemaFrameworkNamespace.GetName("PrintTicket");

    /// <summary>
    ///   psf:Feature
    /// </summary>
    [NotNull]
    public static XName FeatureName => XpsServer.PrinterSchemaFrameworkNamespace.GetName("Feature");

    /// <summary>
    ///   name
    /// </summary>
    [NotNull]
    public static XName NameName => XNamespace.None.GetName("name");

    /// <summary>
    ///   psf:Property
    /// </summary>
    [NotNull]
    public static XName PropertyName => XpsServer.PrinterSchemaFrameworkNamespace.GetName("Property");

    /// <summary>
    ///   psf:ScoredProperty
    /// </summary>
    [NotNull]
    public static XName ScoredPropertyName => XpsServer.PrinterSchemaFrameworkNamespace.GetName("ScoredProperty");

    /// <summary>
    ///   psf:Option
    /// </summary>
    [NotNull]
    public static XName OptionName => XpsServer.PrinterSchemaFrameworkNamespace.GetName("Option");

    /// <summary>
    ///   psf:Value
    /// </summary>
    [NotNull]
    public static XName ValueName => XpsServer.PrinterSchemaFrameworkNamespace.GetName("Value");

    /// <summary>
    ///   xsi:type
    /// </summary>
    [NotNull]
    public static XName TypeName => XpsServer.XmlSchemaInstanceNamespace.GetName("type");

    /// <summary>
    ///   xsd:integer
    /// </summary>
    [NotNull]
    public static XName IntegerName => XpsServer.XmlSchemaNamespace.GetName("integer");

    /// <summary>
    ///   xsd:string
    /// </summary>
    [NotNull]
    public static XName StringName => XpsServer.XmlSchemaNamespace.GetName("string");

    /// <summary>
    ///   xsd:QName
    /// </summary>
    [NotNull]
    public static XName QNameName => XpsServer.XmlSchemaNamespace.GetName("QName");

    /// <summary>
    ///   constrained
    /// </summary>
    [NotNull]
    public static XName ConstrainedName => XNamespace.None.GetName("constrained");

    /// <summary>
    ///   psk:none
    /// </summary>
    [NotNull]
    public static XName NoneName => XpsServer.PrinterSchemaKeywordsNamespace.GetName("None");

    /// <summary>
    ///   psk:DeviceSettings
    /// </summary>
    [NotNull]
    public static XName DeviceSettingsName => XpsServer.PrinterSchemaKeywordsNamespace.GetName("DeviceSettings");
  }
}
