/** @pp
 * rootnamespace: Contrib.System
 */
namespace Contrib.System.Printing.Xps
{
  using global::System;
  using global::System.Xml.Linq;
  using global::Contrib.System.Printing.Xps.ExtensionMethods;
  using global::JetBrains.Annotations;

  /// <summary>
  ///   Holds information of an input bin.
  /// </summary>
  /// <seealso cref="XpsInputBinDefinitionFactory.XpsInputBinDefinition"/>
  [PublicAPI]
#if CONTRIB_SYSTEM_PRINTING_XPS
  public
#else
  internal
#endif
  partial interface IXpsInputBinDefinition
  {
    /// <summary>
    ///   The name of the feature.
    /// </summary>
    /// <example>{http://schemas.microsoft.com/windows/2003/08/printing/printschemakeywords}JobInputBin</example>
    [NotNull]
    XpsName Feature { get; }

    /// <summary>
    ///   The name of the input bin.
    /// </summary>
    /// <example>{http://schemas.microsoft.com/windows/2003/08/printing/printschemakeywords}AutoSelect</example>
    [NotNull]
    XpsName Name { get; }

    /// <summary>
    ///   The display name of the input bin.
    /// </summary>
    /// <example>"Automatically Select"</example>
    [CanBeNull]
    string DisplayName { get; }

    /// <summary>
    ///   The feed type of the input bin.
    /// </summary>
    /// <example>{http://schemas.microsoft.com/windows/2003/08/printing/printschemakeywords}Automatic</example>
    [CanBeNull]
    XpsName FeedType { get; }
  }

  /// <summary>
  ///   Factory for <typeparamref name="TXpsInputBinDefinition"/>.
  /// </summary>
  /// <typeparam name="TXpsInputBinDefinition"/>
  /// <seealso cref="XpsInputBinDefinitionFactory"/>
  /// <example>
  ///   This sample shows how to implement your own <see cref="IXpsInputBinDefinition"/>.
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
    /// <exception cref="ArgumentNullException"><paramref name="feature"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="name"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="option"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="printCapabilities"/> is <see langword="null"/>.</exception>
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
    /// <inheritdoc/>
#if CONTRIB_SYSTEM_PRINTING_XPS
    private sealed
#else
    internal
#endif
    partial class XpsInputBinDefinition : IXpsInputBinDefinition
    {
      /// <inheritdoc/>
      public XpsName Feature { get; set; }

      /// <inheritdoc/>
      public XpsName Name { get; set; }

      /// <inheritdoc/>
      public string DisplayName { get; set; }

      /// <inheritdoc/>
      public XpsName FeedType { get; set; }
    }

    /// <summary>
    ///   Initializes a new instance of the <see cref="XpsInputBinDefinitionFactory"/> class.
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

      var xpsInputBinDefinition = new XpsInputBinDefinition
                                  {
                                    Feature = feature,
                                    Name = name,
                                    DisplayName = displayName,
                                    FeedType = feedType
                                  };

      return xpsInputBinDefinition;
    }
  }
}
