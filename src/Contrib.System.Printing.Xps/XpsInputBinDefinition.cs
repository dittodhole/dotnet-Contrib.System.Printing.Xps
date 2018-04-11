using System;
using System.Xml.Linq;
using Contrib.System.Printing.Xps.ExtensionMethods;
using JetBrains.Annotations;

namespace Contrib.System.Printing.Xps
{
  public interface IXpsInputBinDefinition
  {
    /// <remarks>
    ///   The value is one of the following:
    ///   <list type="bullet">
    ///     <item>
    ///       <description><see cref="XpsPrintCapabilitiesReader.PageInputBinXName"/></description>
    ///     </item>
    ///     <item>
    ///       <description><see cref="XpsPrintCapabilitiesReader.DocumentInputBinXName"/></description>
    ///     </item>
    ///     <item>
    ///       <description><see cref="XpsPrintCapabilitiesReader.JobInputBinXName"/></description>
    ///     </item>
    ///   </list>
    /// </remarks>
    [NotNull]
    XName FeatureName { get; }

    [NotNull]
    XName Name { get; }

    [CanBeNull]
    object GetValue([NotNull] XName name);
  }

  public interface IXpsInputBinDefinitionFactory
  {
    [NotNull]
    IXpsInputBinDefinition Create([NotNull] IXpsFeature xpsFeature,
                                  [NotNull] IXpsOption xpsOption,
                                  [NotNull] IXpsPrintCapabilities xpsPrintCapabilities);
  }

  public sealed class XpsInputBinDefinitionFactory : IXpsInputBinDefinitionFactory
  {
    private sealed class XpsInputBinDefinition : IXpsInputBinDefinition,
                                                 IEquatable<XpsInputBinDefinition>
    {
      public XpsInputBinDefinition([NotNull] IXpsFeature xpsFeature,
                                   [NotNull] IXpsOption xpsOption,
                                   [NotNull] IXpsPrintCapabilities xpsPrintCapabilities)
      {
        this.XpsFeature = xpsFeature;
        this.XpsOption = xpsOption;
        this.XpsPrintCapabilities = xpsPrintCapabilities;
      }

      [NotNull]
      private IXpsFeature XpsFeature { get; }

      [NotNull]
      private IXpsOption XpsOption { get; }

      [NotNull]
      private IXpsPrintCapabilities XpsPrintCapabilities { get; }

      /// <inheritdoc />
      public XName FeatureName => this.XpsFeature.Name;

      /// <inheritdoc />
      public XName Name => this.XpsOption.Name;

      /// <inheritdoc />
      public object GetValue(XName name)
      {
        object value;

        var xpsProperty = this.XpsOption.FindXpsProperty(name);
        if (xpsProperty == null)
        {
          xpsProperty = this.XpsPrintCapabilities.FindXpsProperty(name);
          if (xpsProperty == null)
          {
            value = null;
          }
          else
          {
            value = xpsProperty.Value;
          }
        }
        else
        {
          value = xpsProperty.Value;
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
    public IXpsInputBinDefinition Create(IXpsFeature xpsFeature,
                                         IXpsOption xpsOption,
                                         IXpsPrintCapabilities xpsPrintCapabilities)
    {
      var xpsInputBinDefinition = new XpsInputBinDefinition(xpsFeature,
                                                            xpsOption,
                                                            xpsPrintCapabilities);

      return xpsInputBinDefinition;
    }
  }
}
