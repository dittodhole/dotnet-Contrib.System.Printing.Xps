using System;
using System.Linq;
using System.Xml.Linq;
using JetBrains.Annotations;

namespace Contrib.System.Printing.Xps
{
  public interface IXpsInputBinDefinition
  {
    /// <remarks>
    ///   The value is one of the following:
    ///   <list type="bullet">
    ///     <item>
    ///       <description><see cref="PrintCapabilitiesReader.PageInputBinXName"/></description>
    ///     </item>
    ///     <item>
    ///       <description><see cref="PrintCapabilitiesReader.DocumentInputBinXName"/></description>
    ///     </item>
    ///     <item>
    ///       <description><see cref="PrintCapabilitiesReader.JobInputBinXName"/></description>
    ///     </item>
    ///   </list>
    /// </remarks>
    [NotNull]
    XName FeatureName { get; }

    [NotNull]
    XName Name { get; }

    [CanBeNull]
    string DisplayName { get; }

    [CanBeNull]
    long? MediaSizeWidth { get; }

    [CanBeNull]
    long? MediaSizeHeight { get; }

    [CanBeNull]
    XName FeedType { get; }
  }

  public partial class XpsServer
  {
    protected sealed class XpsInputBinDefinition : IXpsInputBinDefinition,
                                                   IEquatable<XpsInputBinDefinition>
    {
      private XpsInputBinDefinition([NotNull] IXpsFeature xpsFeature,
                                    [NotNull] IXpsOption xpsOption,
                                    [NotNull] IXpsPrintTicket xpsPrintTicket)
      {
        this.XpsFeature = xpsFeature;
        this.XpsOption = xpsOption;
        this.XpsPrintTicket = xpsPrintTicket;
      }

      [NotNull]
      private IXpsFeature XpsFeature { get; }

      [NotNull]
      private IXpsOption XpsOption { get; }

      [NotNull]
      private IXpsPrintTicket XpsPrintTicket { get; }

      /// <inheritdoc />
      public XName FeatureName => this.XpsFeature.Name;

      /// <inheritdoc />
      public XName Name => this.XpsOption.Name;

      /// <inheritdoc />
      public string DisplayName
      {
        get
        {
          string displayName;

          var xpsProperty = this.XpsOption.GetXpsProperty(Xps.PrintCapabilitiesReader.DisplayNameXName);
          if (xpsProperty == null)
          {
            displayName = null;
          }
          else
          {
            var value = xpsProperty.Value;
            if (value == null)
            {
              displayName = null;
            }
            else
            {
              displayName = value as string;
            }
          }

          return displayName;
        }
      }

      /// <inheritdoc />
      public long? MediaSizeWidth
      {
        get
        {
          var mediaSizeWidth = this.GetPageMediaSize(Xps.PrintCapabilitiesReader.MediaSizeWidthXName);
          var mediaSizeHeight = this.GetPageMediaSize(Xps.PrintCapabilitiesReader.MediaSizeHeightXName);

          var result = NumberHelper.GetDimension(mediaSizeWidth,
                                                 mediaSizeHeight,
                                                 false);

          return result;
        }
      }

      /// <inheritdoc />
      public long? MediaSizeHeight
      {
        get
        {
          var mediaSizeWidth = this.GetPageMediaSize(Xps.PrintCapabilitiesReader.MediaSizeWidthXName);
          var mediaSizeHeight = this.GetPageMediaSize(Xps.PrintCapabilitiesReader.MediaSizeHeightXName);

          var result = NumberHelper.GetDimension(mediaSizeWidth,
                                                 mediaSizeHeight,
                                                 true);

          return result;
        }
      }

      /// <inheritdoc />
      public XName FeedType
      {
        get
        {
          XName feedType;
          var xpsProperty = this.XpsOption.GetXpsProperty(Xps.PrintCapabilitiesReader.FeedTypeXName);
          if (xpsProperty == null)
          {
            feedType = null;
          }
          else
          {
            var value = xpsProperty.Value;
            if (value == null)
            {
              feedType = null;
            }
            else
            {
              feedType = xpsProperty.Value as XName;
            }
          }

          return feedType;
        }
      }

      [CanBeNull]
      private long? GetPageMediaSize([NotNull] XName mediaSizeXName)
      {
        long? pageMediaSize;

        var xpsFeature = this.XpsPrintTicket.GetXpsFeature(Xps.PrintCapabilitiesReader.PageMediaSizeXName);
        if (xpsFeature != null)
        {
          var xpsOptions = xpsFeature.GetXpsOptions();
          var xpsOption = xpsOptions.FirstOrDefault();
          if (xpsOption == null)
          {
            pageMediaSize = null;
          }
          else
          {
            var xpsProperty = xpsOption.GetXpsProperty(mediaSizeXName);
            if (xpsProperty == null)
            {
              pageMediaSize = null;
            }
            else
            {
              var value = xpsProperty.Value;
              if (value is long longValue)
              {
                pageMediaSize = longValue;
              }
              else
              {
                pageMediaSize = null;
              }
            }
          }
        }
        else
        {
          pageMediaSize = null;
        }

        return pageMediaSize;
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

      [NotNull]
      public static XpsInputBinDefinition Create([NotNull] IXpsFeature xpsFeature,
                                                 [NotNull] IXpsOption xpsOption,
                                                 [NotNull] IXpsPrintTicket xpsPrintTicket)
      {
        var xpsInputBinDefinition = new XpsInputBinDefinition(xpsFeature,
                                                              xpsOption,
                                                              xpsPrintTicket);

        return xpsInputBinDefinition;
      }
    }
  }
}
