using System.Xml.Linq;
using Contrib.System.Printing.Xps.ExtensionMethods;
using JetBrains.Annotations;

namespace Contrib.System.Printing.Xps
{
  /// <summary>
  ///   Holds information of an input bin.
  /// </summary>
  /// <seealso cref="XpsInputBinDefinitionFactory.XpsInputBinDefinition"/>
  [PublicAPI]
  public partial interface IXpsInputBinDefinition
  {
    /// <summary>
    ///   The <see cref="XName"/> of the feature for the input bin.
    /// </summary>
    /// <example>{http://schemas.microsoft.com/windows/2003/08/printing/printschemakeywords}JobInputBin</example>
    [NotNull]
    XName FeatureName { get; }

    /// <summary>
    ///   The <see cref="XName"/> of the input bin.
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
    ///   The <see cref="XName"/> of the feed of the input bin.
    /// </summary>
    /// <example>{http://schemas.microsoft.com/windows/2003/08/printing/printschemakeywords}Automatic</example>
    [CanBeNull]
    XName FeedType { get; }
  }

  /// <summary>
  ///   Factory class for <typeparamref name="TXpsInputBinDefinition"/>.
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
  ///     public XName FeatureName { get; set; }
  ///     public string DisplayName { get; set; }
  ///     public XName Name { get; set; }
  ///     public XName FeedType { get; set; }
  ///   }
  ///
  ///   private IXpsInputBinDefinitionFactory XpsInputBinDefinitionFactory { get; } = new XpsInputBinDefinitionFactory();
  ///
  ///   public ICustomXpsInputBinDefinition Create(XElement optionXElement,
  ///                                              XElement printCapabilitiesXElement)
  ///   {
  ///     var xpsInputBinDefinition = this.XpsInputBinDefinitionFactory.Create(optionXElement,
  ///                                                                          printCapabilitiesXElement);
  ///     var customXpsInputBinDefinition = new CustomXpsInputBinDefinition
  ///                                      {
  ///                                        FeatureName = xpsInputBinDefinition.FeatureName,
  ///                                        DisplayName = xpsInputBinDefinition.DisplayName,
  ///                                        Name = xpsInputBinDefinition.Name,
  ///                                        FeedType = xpsInputBinDefinition.FeedType
  ///                                      };
  ///
  ///     // TODO use printCapabilitiesXElement with Contrib.System.Printing.Xps.ExtensionMethods.XElementExtensions to extract needed values
  ///
  ///     return customXpsInputBinDefinition;
  ///   }
  /// }
  /// </code></example>
  public partial interface IXpsInputBinDefinitionFactory<out TXpsInputBinDefinition>
    where TXpsInputBinDefinition : IXpsInputBinDefinition
  {
    /// <summary>
    ///   Factory method for <typeparamref name="TXpsInputBinDefinition"/>.
    /// </summary>
    /// <param name="optionXElement"/>
    /// <param name="printCapabilitiesXElement"/>
    [NotNull]
    TXpsInputBinDefinition Create([NotNull] XElement optionXElement,
                                  [NotNull] XElement printCapabilitiesXElement);
  }

  /// <inheritdoc cref="IXpsInputBinDefinitionFactory{TXpsInputBinDefinition}"/>
  public sealed partial class XpsInputBinDefinitionFactory : IXpsInputBinDefinitionFactory<IXpsInputBinDefinition>
  {
    /// <inheritdoc cref="IXpsInputBinDefinition"/>
    private partial struct XpsInputBinDefinition : IXpsInputBinDefinition
    {
      /// <inheritdoc/>
      public XName FeatureName { get; set; }

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
    public IXpsInputBinDefinition Create(XElement optionXElement,
                                         XElement printCapabilitiesXElement)
    {
      XName featureName;

      var featureXElement = optionXElement.Parent;
      if (featureXElement == null)
      {
        featureName = null;
      }
      else
      {
        featureName = featureXElement.GetNameFromNameAttribute();
      }

      var name = optionXElement.GetNameFromNameAttribute();

      var displayName = optionXElement.FindElementByNameAttribute(XpsServer.DisplayNameXName)
                                      ?.GetValueFromValueElement() as string;

      var feedType = optionXElement.FindElementByNameAttribute(XpsServer.FeedTypeXName)
                                   ?.GetValueFromValueElement() as XName;

      var xpsInputBinDefinition = new XpsInputBinDefinition
                                  {
                                    FeatureName = featureName,
                                    Name = name,
                                    DisplayName = displayName,
                                    FeedType = feedType
                                  };

      return xpsInputBinDefinition;
    }
  }
}
