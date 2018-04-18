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
  public partial interface IXpsInputBinDefinitionFactoryEx<out TXpsInputBinDefinition>
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

  /// <inheritdoc cref="IXpsInputBinDefinitionFactoryEx{TXpsInputBinDefinition}"/>
  public sealed partial class XpsInputBinDefinitionFactory : IXpsInputBinDefinitionFactoryEx<IXpsInputBinDefinition>
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
