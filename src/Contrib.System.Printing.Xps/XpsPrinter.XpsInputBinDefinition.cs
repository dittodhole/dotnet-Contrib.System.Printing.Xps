﻿using System;
using System.Xml.Linq;
using JetBrains.Annotations;

namespace Contrib.System.Printing.Xps
{
  public interface IXpsInputBinDefinition
  {
    /// <seealso cref="PrintCapabilitiesReader.JobInputBinXName"/>
    /// <seealso cref="PrintCapabilitiesReader.PageInputBinXName"/>
    /// <seealso cref="PrintCapabilitiesReader.DocumentInputBinXName"/>
    [NotNull]
    XName FeatureName { get; }

    [NotNull]
    XName Name { get; }

    [CanBeNull]
    string DisplayName { get; }

    [CanBeNull]
    double? MediaSizeWidth { get; }

    [CanBeNull]
    double? MediaSizeHeight { get; }

    /// <returns>{http://schemas.microsoft.com/windows/2003/08/printing/printschemakeywords}Manual</returns>
    [CanBeNull]
    XName FeedType { get; }
  }

  public partial class XpsPrinter
  {
    internal sealed class XpsInputBinDefinition : IXpsInputBinDefinition,
                                                  IEquatable<XpsInputBinDefinition>
    {
      private XpsInputBinDefinition([NotNull] XName featureName,
                                    [NotNull] IXpsOption xpsOption,
                                    [NotNull] IXpsPrintTicket xpsPrintTicket)
      {
        this.FeatureName = featureName;
        this.XpsOption = xpsOption;
        this.XpsPrintTicket = xpsPrintTicket;
      }

      [NotNull]
      private IXpsOption XpsOption { get; }

      [NotNull]
      private IXpsPrintTicket XpsPrintTicket { get; }

      /// <inheritdoc />
      public XName FeatureName { get; }

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
            displayName = xpsProperty.Value;
          }

          return displayName;
        }
      }

      /// <inheritdoc />
      public double? MediaSizeWidth
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
      public double? MediaSizeHeight
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
          var xpsProperty = this.XpsOption.GetXpsProperty(Xps.PrintCapabilitiesReader.FeedTypeXName);
          if (xpsProperty == null)
          {
            return null;
          }

          var result = xpsProperty.ValueXName;

          return result;
        }
      }

      [CanBeNull]
      private double? GetPageMediaSize([NotNull] XName mediaSizeXName)
      {
        var xpsFeature = this.XpsPrintTicket.GetXpsFeature(Xps.PrintCapabilitiesReader.PageMediaSizeXName);
        if (xpsFeature == null)
        {
          return null;
        }

        var xpsOptions = xpsFeature.GetXpsOptions();
        foreach (var xpsOption in xpsOptions)
        {
          var xpsProperty = xpsOption.GetXpsProperty(mediaSizeXName);
          if (xpsProperty != null)
          {
            if (double.TryParse(xpsProperty.Value,
                                out var value))
            {
              return value;
            }
          }
        }

        return null;
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
      public static XpsInputBinDefinition Create([NotNull] XName xpsFeatureName,
                                                 [NotNull] IXpsOption xpsOption,
                                                 [NotNull] IXpsPrintTicket xpsPrintTicket)
      {
        var xpsInputBinDefinition = new XpsInputBinDefinition(xpsFeatureName,
                                                              xpsOption,
                                                              xpsPrintTicket);

        return xpsInputBinDefinition;
      }
    }
  }
}
