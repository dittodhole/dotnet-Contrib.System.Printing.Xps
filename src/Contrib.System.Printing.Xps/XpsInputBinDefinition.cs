﻿using System.Xml.Linq;
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

  public partial interface IXpsInputBinDefinitionFactoryEx<TXpsInputBinDefinition> : IXpsInputBinDefinitionFactory
    where TXpsInputBinDefinition : IXpsInputBinDefinition
  {
    [NotNull]
    TXpsInputBinDefinition Create([NotNull] XElement optionXElement,
                                  [NotNull] XElement printCapabilitiesXElement);
  }

  public sealed partial class XpsInputBinDefinitionFactory : IXpsInputBinDefinitionFactoryEx<IXpsInputBinDefinition>
  {
    private sealed partial class XpsInputBinDefinition : IXpsInputBinDefinition
    {
      public XpsInputBinDefinition([NotNull] XElement optionXElement,
                                   [NotNull] XElement printCapabilitiesXElement)
      {
        this.OptionXElement = optionXElement;
        this.PrintCapabilitiesXElement = printCapabilitiesXElement;
      }

      [NotNull]
      private XElement PrintCapabilitiesXElement { get; }

      [NotNull]
      private XElement OptionXElement { get; }

      /// <inheritdoc />
      public XName FeatureName
      {
        get
        {
          XName featureName;

          var featureXElement = this.OptionXElement.Parent;
          if (featureXElement == null)
          {
            featureName = null;
          }
          else
          {
            featureName = featureXElement.GetNameFromNameAttribute();
          }

          return featureName;
        }
      }

      /// <inheritdoc />
      public XName Name
      {
        get
        {
          var name = this.OptionXElement.GetNameFromNameAttribute();

          return name;
        }
      }

      /// <inheritdoc />
      public string DisplayName
      {
        get
        {
          var displayName = this.OptionXElement.FindElementByNameAttribute(XpsServer.DisplayNameXName)
                                ?.GetValueFromValueElement() as string;

          return displayName;
        }
      }

      /// <inheritdoc />
      public XName FeedType
      {
        get
        {
          var feedType = this.OptionXElement.FindElementByNameAttribute(XpsServer.FeedTypeXName)
                             ?.GetValueFromValueElement() as XName;

          return feedType;
        }
      }
    }

    /// <inheritdoc />
    public IXpsInputBinDefinition Create(XElement optionXElement,
                                         XElement printCapabilitiesXElement)
    {
      var xpsInputBinDefinition = new XpsInputBinDefinition(optionXElement,
                                                            printCapabilitiesXElement);

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
