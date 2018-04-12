﻿using System;
using System.Linq;
using System.Xml.Linq;
using Contrib.System.Printing.Xps.ExtensionMethods;
using JetBrains.Annotations;

namespace Contrib.System.Printing.Xps
{
  public interface IXpsInputBinDefinition : IHasValues
  {
    [NotNull]
    XName FeatureName { get; }

    [NotNull]
    XName Name { get; }

    [CanBeNull]
    string DisplayName { get; }
  }

  public interface IXpsInputBinDefinitionFactory
  {
    [NotNull]
    IXpsInputBinDefinition Create([NotNull] XElement optionXElement,
                                  [NotNull] XElement printCapabilitiesXElement);
  }

  public sealed class XpsInputBinDefinitionFactory : IXpsInputBinDefinitionFactory
  {
    // TODO check need for IEquatable
    private sealed class XpsInputBinDefinition : IXpsInputBinDefinition,
                                                 IEquatable<XpsInputBinDefinition>
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
      public string DisplayName
      {
        get
        {
          var displayName = this.GetValue(XpsServer.DisplayNameXName) as string;

          return displayName;
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
      public object GetValue(params XName[] names)
      {
        object value;

        var xelement = names.Aggregate(this.OptionXElement,
                                       (current,
                                        name) => current?.FindElementByNameAttribute(name));
        if (xelement == null)
        {
          xelement = names.Aggregate(this.PrintCapabilitiesXElement,
                                     (current,
                                      name) => current?.FindElementByNameAttribute(name));
        }

        if (xelement == null)
        {
          value = null;
        }
        else
        {
          value = xelement.GetValueFromValueElement();
        }

        return value;
      }

      /// <inheritdoc />
      public bool Equals(XpsInputBinDefinition other)
      {
        if (object.ReferenceEquals(null,
                                   other))
        {
          return false;
        }

        if (object.ReferenceEquals(this,
                                   other))
        {
          return true;
        }

        return string.Equals(this.Name,
                             other.Name);
      }

      /// <inheritdoc />
      public override bool Equals(object obj)
      {
        if (object.ReferenceEquals(null,
                                   obj))
        {
          return false;
        }

        if (object.ReferenceEquals(this,
                                   obj))
        {
          return true;
        }

        return obj is XpsInputBinDefinition && this.Equals((XpsInputBinDefinition) obj);
      }

      /// <inheritdoc />
      public override int GetHashCode()
      {
        return this.Name.GetHashCode();
      }

      public static bool operator ==(XpsInputBinDefinition left,
                                     XpsInputBinDefinition right)
      {
        return object.Equals(left,
                             right);
      }

      public static bool operator !=(XpsInputBinDefinition left,
                                     XpsInputBinDefinition right)
      {
        return !object.Equals(left,
                              right);
      }

      /// <inheritdoc />
      public override string ToString()
      {
        return this.Name.ToString();
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
  }
}