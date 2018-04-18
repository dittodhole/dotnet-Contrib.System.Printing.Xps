using System.Xml.Linq;
using Contrib.System.Printing.Xps.ExtensionMethods;
using JetBrains.Annotations;

namespace Contrib.System.Printing.Xps
{
  /// <summary>
  /// 
  /// </summary>
  [PublicAPI]
  public partial interface IXpsInputBinDefinition
  {
    /// <summary>
    /// 
    /// </summary>
    [NotNull]
    XName FeatureName { get; }

    /// <summary>
    /// 
    /// </summary>
    [NotNull]
    XName Name { get; }

    /// <summary>
    /// 
    /// </summary>
    [CanBeNull]
    string DisplayName { get; }

    /// <summary>
    /// 
    /// </summary>
    [CanBeNull]
    XName FeedType { get; }
  }

  /// <summary>
  /// 
  /// </summary>
  public partial interface IXpsInputBinDefinitionFactory
  {
    /// <summary>
    /// 
    /// </summary>
    /// <param name="optionXElement"></param>
    /// <param name="printCapabilitiesXElement"></param>
    [NotNull]
    IXpsInputBinDefinition Create([NotNull] XElement optionXElement,
                                  [NotNull] XElement printCapabilitiesXElement);
  }

  /// <summary>
  /// 
  /// </summary>
  /// <typeparam name="TXpsInputBinDefinition" />
  public partial interface IXpsInputBinDefinitionFactoryEx<out TXpsInputBinDefinition>
    where TXpsInputBinDefinition : IXpsInputBinDefinition
  {
    /// <summary>
    /// 
    /// </summary>
    /// <param name="optionXElement" />
    /// <param name="printCapabilitiesXElement" />
    [NotNull]
    TXpsInputBinDefinition Create([NotNull] XElement optionXElement,
                                  [NotNull] XElement printCapabilitiesXElement);
  }

  /// <inheritdoc cref="IXpsInputBinDefinitionFactoryEx{TXpsInputBinDefinition}" />
  public sealed partial class XpsInputBinDefinitionFactory : IXpsInputBinDefinitionFactoryEx<IXpsInputBinDefinition>,
                                                             IXpsInputBinDefinitionFactory
  {
    /// <inheritdoc cref="IXpsInputBinDefinition"/>
    private partial struct XpsInputBinDefinition : IXpsInputBinDefinition
    {
      /// <inheritdoc />
      public XName FeatureName { get; set; }

      /// <inheritdoc />
      public XName Name { get; set; }

      /// <inheritdoc />
      public string DisplayName { get; set; }

      /// <inheritdoc />
      public XName FeedType { get; set; }
    }

    /// <inheritdoc />
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

    /// <inheritdoc />
    IXpsInputBinDefinition IXpsInputBinDefinitionFactory.Create(XElement optionXElement,
                                                                XElement printCapabilitiesXElement)
    {
      return this.Create(optionXElement,
                         printCapabilitiesXElement);
    }
  }
}
