/** @pp
 * rootnamespace: Contrib.System.Printing.Xps
 */
namespace Contrib.System.Printing.Xps
{
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
    XName Feature { get; }

    /// <summary>
    ///   The name of the input bin.
    /// </summary>
    /// <example>{http://schemas.microsoft.com/windows/2003/08/printing/printschemakeywords}AutoSelect</example>
    [NotNull]
    XName Name { get; }

    /// <summary>
    ///   The display name of the input bin.
    /// </summary>
    /// <example>Automatically Select</example>
    [CanBeNull]
    string DisplayName { get; }

    /// <summary>
    ///   The feed type of the input bin.
    /// </summary>
    /// <example>{http://schemas.microsoft.com/windows/2003/08/printing/printschemakeywords}Automatic</example>
    [CanBeNull]
    XName FeedType { get; }
  }

  /// <summary>
  ///   Factory for <typeparamref name="TXpsInputBinDefinition"/>.
  /// </summary>
  /// <typeparam name="TXpsInputBinDefinition"/>
  /// <seealso cref="XpsInputBinDefinitionFactory"/>
  /// <example><code>
  /// using Contrib.System.Printing.Xps;
  /// using System.Xml.Linq;
  ///
  /// public interface ICustomXpsInputBinDefinition : IXpsInputBinDefinition { }
  ///
  /// public class CustomXpsInputBinDefinitionFactory : IXpsInputBinDefinitionFactoryEx&lt;ICustomXpsInputBinDefinition&gt;
  /// {
  ///   private class CustomXpsInputBinDefinition : ICustomXpsInputBinDefinition
  ///   {
  ///     public XName Feature { get; set; }
  ///     public string DisplayName { get; set; }
  ///     public XName Name { get; set; }
  ///     public XName FeedType { get; set; }
  ///   }
  ///
  ///   private IXpsInputBinDefinitionFactory XpsInputBinDefinitionFactory { get; } = new XpsInputBinDefinitionFactory();
  ///
  ///   public ICustomXpsInputBinDefinition Create(XElement option,
  ///                                              XElement printCapabilities)
  ///   {
  ///     var xpsInputBinDefinition = this.XpsInputBinDefinitionFactory.Create(option,
  ///                                                                          printCapabilities);
  ///     var customXpsInputBinDefinition = new CustomXpsInputBinDefinition
  ///                                      {
  ///                                        Feature = xpsInputBinDefinition.Feature,
  ///                                        DisplayName = xpsInputBinDefinition.DisplayName,
  ///                                        Name = xpsInputBinDefinition.Name,
  ///                                        FeedType = xpsInputBinDefinition.FeedType
  ///                                      };
  ///
  ///     // TODO use printCapabilities with Contrib.System.Printing.Xps.ExtensionMethods.XElementExtensions to extract needed values
  ///
  ///     return customXpsInputBinDefinition;
  ///   }
  /// }
  /// </code></example>
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
    /// <param name="option"/>
    /// <param name="printCapabilities"/>
    [NotNull]
    TXpsInputBinDefinition Create([NotNull] XElement option,
                                  [NotNull] XElement printCapabilities);
  }

  /// <inheritdoc cref="IXpsInputBinDefinitionFactory{TXpsInputBinDefinition}"/>
  [PublicAPI]
#if CONTRIB_SYSTEM_PRINTING_XPS
  public sealed
#else
  internal
#endif
  partial class XpsInputBinDefinitionFactory : IXpsInputBinDefinitionFactory<IXpsInputBinDefinition>
  {
    /// <inheritdoc cref="IXpsInputBinDefinition"/>
#if CONTRIB_SYSTEM_PRINTING_XPS
    private sealed
#else
    internal
#endif
    partial class XpsInputBinDefinition : IXpsInputBinDefinition
    {
      /// <inheritdoc/>
      public XName Feature { get; set; }

      /// <inheritdoc/>
      public XName Name { get; set; }

      /// <inheritdoc/>
      public string DisplayName { get; set; }

      /// <inheritdoc/>
      public XName FeedType { get; set; }
    }

    /// <summary>
    ///   Initializes a new instance of the <see cref="XpsInputBinDefinitionFactory"/> class.
    /// </summary>
    [PublicAPI]
    public XpsInputBinDefinitionFactory() { }

    /// <inheritdoc/>
    public IXpsInputBinDefinition Create(XElement option,
                                         XElement printCapabilities)
    {
      XName featureName;

      var feature = option.Parent;
      if (feature == null)
      {
        featureName = null;
      }
      else
      {
        featureName = feature.GetNameFromNameAttribute();
      }

      var name = option.GetNameFromNameAttribute();

      var displayName = option.FindElementByNameAttribute(XpsServer.DisplayNameName)
                              ?.GetValueFromValueElement() as string;

      var feedType = option.FindElementByNameAttribute(XpsServer.FeedTypeName)
                           ?.GetValueFromValueElement() as XName;

      var xpsInputBinDefinition = new XpsInputBinDefinition
                                  {
                                    Feature = featureName,
                                    Name = name,
                                    DisplayName = displayName,
                                    FeedType = feedType
                                  };

      return xpsInputBinDefinition;
    }
  }
}
