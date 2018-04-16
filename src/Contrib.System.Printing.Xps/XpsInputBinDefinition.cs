using System.Xml.Linq;
using Contrib.System.Printing.Xps.ExtensionMethods;
using JetBrains.Annotations;

namespace Contrib.System.Printing.Xps
{
  public partial interface IXpsInputBinDefinition
  {
    [NotNull]
    XName FeatureName { get; }

    [NotNull]
    XName Name { get; }

    [CanBeNull]
    string DisplayName { get; }

    [CanBeNull]
    XName FeedType { get; }
  }

  public partial interface IXpsInputBinDefinitionFactory
  {
    [NotNull]
    IXpsInputBinDefinition Create([NotNull] XElement optionXElement,
                                  [NotNull] XElement printCapabilitiesXElement);
  }

  public partial interface IXpsInputBinDefinitionFactoryEx<out TXpsInputBinDefinition>
    where TXpsInputBinDefinition : IXpsInputBinDefinition
  {
    [NotNull]
    TXpsInputBinDefinition Create([NotNull] XElement optionXElement,
                                  [NotNull] XElement printCapabilitiesXElement);
  }

  public sealed partial class XpsInputBinDefinitionFactory : IXpsInputBinDefinitionFactoryEx<IXpsInputBinDefinition>,
                                                             IXpsInputBinDefinitionFactory
  {
    private sealed partial class XpsInputBinDefinition : IXpsInputBinDefinition
    {
      public XpsInputBinDefinition([NotNull] XName featureName,
                                   [NotNull] XName name,
                                   [CanBeNull] string displayName,
                                   [CanBeNull] XName feedType)
      {
        this.FeatureName = featureName;
        this.Name = name;
        this.DisplayName = displayName;
        this.FeedType = feedType;
      }

      /// <inheritdoc />
      public XName FeatureName { get; }

      /// <inheritdoc />
      public XName Name { get; }

      /// <inheritdoc />
      public string DisplayName { get; }

      /// <inheritdoc />
      public XName FeedType { get; }
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

      var xpsInputBinDefinition = new XpsInputBinDefinition(featureName,
                                                            name,
                                                            displayName,
                                                            feedType);

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
