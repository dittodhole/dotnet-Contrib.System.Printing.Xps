/** @pp
 * rootnamespace: Contrib.System
 */
namespace Contrib.System.Printing.Xps
{
  using global::System.Xml.Linq;
  using global::JetBrains.Annotations;

  /// <summary>
  ///   Factory for <typeparamref name="TXpsPrinterDefinition"/>.
  /// </summary>
  /// <typeparam name="TXpsPrinterDefinition"/>
  /// <seealso cref="T:Contrib.System.Printing.Xps.XpsPrinterDefinitionFactory"/>
  /// <example>
  ///   This sample shows how to implement your own <see cref="T:Contrib.System.Printing.Xps.IXpsPrinterDefinition"/>.
  ///   <code>
  ///   using global::Contrib.System.Printing.Xps;
  ///   using global::System.Xml.Linq;
  ///
  ///   public interface ICustomXpsPrinterDefinition : IXpsPrinterDefinition { }
  ///
  ///   public class CustomXpsPrinterDefinitionFactory : IXpsPrinterDefinitionFactoryEx&lt;ICustomXpsPrinterDefinition&gt;
  ///   {
  ///     private class CustomXpsPrinterDefinition : ICustomXpsPrinterDefinition
  ///     {
  ///       public string DisplayName { get; set; }
  ///       public string FullName { get; set; }
  ///     }
  ///
  ///     private IXpsPrinterDefinitionFactory XpsPrinterDefinitionFactory { get; } = new XpsPrinterDefinitionFactory();
  ///
  ///     public ICustomXpsPrinterDefinition Create(string displayName,
  ///                                               string fullName,
  ///                                               XElement printCapabilities)
  ///     {
  ///       var xpsPrinterDefinition = this.XpsPrinterDefinitionFactory.Create(displayName,
  ///                                                                          fullName,
  ///                                                                          printCapabilities);
  ///       var customXpsPrinterDefinition = new CustomXpsPrinterDefinition
  ///                                        {
  ///                                          DisplayName = xpsPrinterDefinition.DisplayName,
  ///                                          FullName = xpsPrinterDefinition.FullName
  ///                                        };
  ///
  ///       // TODO use printCapabilities with Contrib.System.Printing.Xps.ExtensionMethods.XElementExtensions to extract needed values
  ///
  ///       return customXpsPrinterDefinition;
  ///     }
  ///   }
  ///   </code>
  /// </example>
  [PublicAPI]
#if CONTRIB_SYSTEM_PRINTING_XPS
  public
#else
  internal
#endif
  partial interface IXpsPrinterDefinitionFactory<out TXpsPrinterDefinition>
    where TXpsPrinterDefinition : IXpsPrinterDefinition
  {
    /// <summary>
    ///   Factory method for <typeparamref name="TXpsPrinterDefinition"/>.
    /// </summary>
    /// <param name="displayName">The display name of the printer.</param>
    /// <param name="fullName">The full name of the printer.</param>
    /// <param name="printCapabilities"/>
    [NotNull]
    TXpsPrinterDefinition Create([NotNull] string displayName,
                                 [NotNull] string fullName,
                                 [NotNull] XElement printCapabilities);
  }

  /// <inheritdoc/>
  [PublicAPI]
#if CONTRIB_SYSTEM_PRINTING_XPS
  public sealed
#else
  internal
#endif
  partial class XpsPrinterDefinitionFactory : IXpsPrinterDefinitionFactory<IXpsPrinterDefinition>
  {
    /// <summary>
    ///   Initializes a new instance of the <see cref="T:Contrib.System.Printing.Xps.XpsPrinterDefinitionFactory"/> class.
    /// </summary>
    [PublicAPI]
    public XpsPrinterDefinitionFactory() { }

    /// <inheritdoc/>
    public IXpsPrinterDefinition Create(string displayName,
                                        string fullName,
                                        XElement printCapabilities)
    {
      var xpsPrinterDefinition = new XpsPrinterDefinition
                                 {
                                   DisplayName = displayName,
                                   FullName = fullName
                                 };

      return xpsPrinterDefinition;
    }
  }
}
