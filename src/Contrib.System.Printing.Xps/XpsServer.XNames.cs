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
    [PublicAPI]
    [NotNull]
    public static XName PrintCapabilitiesName => XpsServer.PrinterSchemaFrameworkNamespace.GetName("PrintCapabilities");

    /// <summary>
    ///   psf:PrintTicket
    /// </summary>
    [PublicAPI]
    [NotNull]
    public static XName PrintTicketName => XpsServer.PrinterSchemaFrameworkNamespace.GetName("PrintTicket");

    /// <summary>
    ///   psf:Feature
    /// </summary>
    [PublicAPI]
    [NotNull]
    public static XName FeatureName => XpsServer.PrinterSchemaFrameworkNamespace.GetName("Feature");

    /// <summary>
    ///   name
    /// </summary>
    [PublicAPI]
    [NotNull]
    public static XName NameName => XNamespace.None.GetName("name");

    /// <summary>
    ///   psf:Property
    /// </summary>
    [PublicAPI]
    [NotNull]
    public static XName PropertyName => XpsServer.PrinterSchemaFrameworkNamespace.GetName("Property");

    /// <summary>
    ///   psf:ScoredProperty
    /// </summary>
    [PublicAPI]
    [NotNull]
    public static XName ScoredPropertyName => XpsServer.PrinterSchemaFrameworkNamespace.GetName("ScoredProperty");

    /// <summary>
    ///   psf:Option
    /// </summary>
    [PublicAPI]
    [NotNull]
    public static XName OptionName => XpsServer.PrinterSchemaFrameworkNamespace.GetName("Option");

    /// <summary>
    ///   psf:Value
    /// </summary>
    [PublicAPI]
    [NotNull]
    public static XName ValueName => XpsServer.PrinterSchemaFrameworkNamespace.GetName("Value");

    /// <summary>
    ///   xsi:type
    /// </summary>
    [PublicAPI]
    [NotNull]
    public static XName TypeName => XpsServer.XmlSchemaInstanceNamespace.GetName("type");

    /// <summary>
    ///   xsd:integer
    /// </summary>
    [PublicAPI]
    [NotNull]
    public static XName IntegerName => XpsServer.XmlSchemaNamespace.GetName("integer");

    /// <summary>
    ///   xsd:string
    /// </summary>
    [PublicAPI]
    [NotNull]
    public static XName StringName => XpsServer.XmlSchemaNamespace.GetName("string");

    /// <summary>
    ///   xsd:QName
    /// </summary>
    [PublicAPI]
    [NotNull]
    public static XName QNameName => XpsServer.XmlSchemaNamespace.GetName("QName");
  }
}
