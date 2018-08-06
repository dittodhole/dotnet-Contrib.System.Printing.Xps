/** @pp
 * rootnamespace: Contrib.System
 */
namespace Contrib.System.Printing.Xps
{
  using global::System.Xml.Linq;
  using global::Contrib.System.Printing.Xps.ExtensionMethods;
  using global::Anotar.LibLog;
  using global::JetBrains.Annotations;

  /// <summary>
  ///   Factory for <typeparamref name="TXpsInputBinDefinition"/>.
  /// </summary>
  /// <typeparam name="TXpsInputBinDefinition"/>
  /// <seealso cref="T:Contrib.System.Printing.Xps.XpsInputBinDefinitionFactory"/>
  /// <example>
  ///   This sample shows how to implement your own <see cref="T:Contrib.System.Printing.Xps.IXpsInputBinDefinition"/>.
  ///   <code>
  ///   using global::Contrib.System.Printing.Xps;
  ///   using global::System.Xml.Linq;
  ///
  ///   public interface ICustomXpsInputBinDefinition : IXpsInputBinDefinition { }
  ///
  ///   public class CustomXpsInputBinDefinitionFactory : IXpsInputBinDefinitionFactoryEx&lt;ICustomXpsInputBinDefinition&gt;
  ///   {
  ///     private class CustomXpsInputBinDefinition : ICustomXpsInputBinDefinition
  ///     {
  ///       public XName Feature { get; set; }
  ///       public string DisplayName { get; set; }
  ///       public XName Name { get; set; }
  ///       public XName FeedType { get; set; }
  ///     }
  ///
  ///     private IXpsInputBinDefinitionFactory XpsInputBinDefinitionFactory { get; } = new XpsInputBinDefinitionFactory();
  ///
  ///     public ICustomXpsInputBinDefinition Create(XElement option,
  ///                                                XElement printCapabilities)
  ///     {
  ///       var xpsInputBinDefinition = this.XpsInputBinDefinitionFactory.Create(option,
  ///                                                                            printCapabilities);
  ///       var customXpsInputBinDefinition = new CustomXpsInputBinDefinition
  ///                                        {
  ///                                          Feature = xpsInputBinDefinition.Feature,
  ///                                          DisplayName = xpsInputBinDefinition.DisplayName,
  ///                                          Name = xpsInputBinDefinition.Name,
  ///                                          FeedType = xpsInputBinDefinition.FeedType
  ///                                        };
  ///
  ///       // TODO use printCapabilities with Contrib.System.Printing.Xps.ExtensionMethods.XElementExtensions to extract needed values
  ///
  ///       return customXpsInputBinDefinition;
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
  partial interface IXpsInputBinDefinitionFactory<out TXpsInputBinDefinition>
    where TXpsInputBinDefinition : IXpsInputBinDefinition
  {
    /// <summary>
    ///   Factory method for <typeparamref name="TXpsInputBinDefinition"/>.
    /// </summary>
    /// <param name="feature"/>
    /// <param name="name"/>
    /// <param name="option"/>
    /// <param name="printCapabilities"/>
    /// <exception cref="T:System.ArgumentNullException"><paramref name="feature"/> is <see langword="null"/>.</exception>
    /// <exception cref="T:System.ArgumentNullException"><paramref name="name"/> is <see langword="null"/>.</exception>
    /// <exception cref="T:System.ArgumentNullException"><paramref name="option"/> is <see langword="null"/>.</exception>
    /// <exception cref="T:System.ArgumentNullException"><paramref name="printCapabilities"/> is <see langword="null"/>.</exception>
    [NotNull]
    TXpsInputBinDefinition Create([NotNull] XpsName feature,
                                  [NotNull] XpsName name,
                                  [NotNull] XElement option,
                                  [NotNull] XElement printCapabilities);
  }

  /// <inheritdoc/>
  [PublicAPI]
#if CONTRIB_SYSTEM_PRINTING_XPS
  public sealed
#else
  internal
#endif
  partial class XpsInputBinDefinitionFactory : IXpsInputBinDefinitionFactory<IXpsInputBinDefinition>
  {
    /// <summary>
    ///   Initializes a new instance of the <see cref="T:Contrib.System.Printing.Xps.XpsInputBinDefinitionFactory"/> class.
    /// </summary>
    [PublicAPI]
    public XpsInputBinDefinitionFactory() { }

    /// <inheritdoc/>
    public IXpsInputBinDefinition Create(XpsName feature,
                                         XpsName name,
                                         XElement option,
                                         XElement printCapabilities)
    {
      var displayName = option.FindElementByNameAttribute(XpsServer.DisplayNameName)
                              ?.Element(XpsServer.ValueName)
                              ?.GetValue() as string;

      var feedType = option.FindElementByNameAttribute(XpsServer.FeedTypeName)
                           ?.Element(XpsServer.ValueName)
                           ?.GetValue() as XpsName;

      bool isAvailable;
      var constrainedXName = option.GetXName(option.Attribute(XpsServer.ConstrainedName)
                                                   ?.Value);
      if (constrainedXName == null)
      {
        LogTo.Warn($"Could not get {nameof(XName)} from {nameof(XAttribute)} '{XpsServer.ConstrainedName}': {option}");
        isAvailable = true;
      }
      else if (constrainedXName == XpsServer.DeviceSettingsName)
      {
        isAvailable = false;
      }
      else if (constrainedXName == XpsServer.NoneName)
      {
        isAvailable = true;
      }
      else
      {
        LogTo.Warn($"Could not get {nameof(IXpsInputBinDefinition.IsAvailable)} from '{constrainedXName}', falling back to '{true}': {option}");
        isAvailable = true;
      }

      var xpsInputBinDefinition = new XpsInputBinDefinition
                                  {
                                    Feature = feature,
                                    Name = name,
                                    DisplayName = displayName,
                                    FeedType = feedType,
                                    IsAvailable = isAvailable
                                  };

      return xpsInputBinDefinition;
    }
  }
}
