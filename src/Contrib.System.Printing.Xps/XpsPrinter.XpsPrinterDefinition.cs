using System;
using System.Printing;
using System.Xml.Linq;
using JetBrains.Annotations;

namespace Contrib.System.Printing.Xps
{
  public interface IXpsPrinterDefinition
  {
    [NotNull]
    string DisplayName { get; }

    [NotNull]
    string FullName { get; }

    [CanBeNull]
    string PortName { get; }

    [CanBeNull]
    string DriverName { get; }

    [CanBeNull]
    long? ImageableSizeWidth { get; }

    [CanBeNull]
    long? ImageableSizeHeight { get; }
  }

  public partial class XpsPrinter
  {
    internal sealed class XpsPrinterDefinition : IXpsPrinterDefinition,
                                                 IEquatable<XpsPrinterDefinition>
    {
      private XpsPrinterDefinition([NotNull] string displayName,
                                   [NotNull] string fullName,
                                   [CanBeNull] string portName,
                                   [CanBeNull] string driverName,
                                   [NotNull] IXpsPrintCapabilities xpsPrintCapabilities)
      {
        this.DisplayName = displayName;
        this.FullName = fullName;
        this.PortName = portName;
        this.DriverName = driverName;
        this.XpsPrintCapabilities = xpsPrintCapabilities;
      }

      /// <inheritdoc />
      public string DisplayName { get; }

      /// <inheritdoc />
      public string FullName { get; }

      /// <inheritdoc />
      public long? ImageableSizeWidth
      {
        get
        {
          var imageableSizeWidth = this.GetImageableSize(Xps.PrintCapabilitiesReader.ImageableSizeWidthXName);
          var imageableSizeHeight = this.GetImageableSize(Xps.PrintCapabilitiesReader.ImageableSizeHeightXName);

          var result = NumberHelper.GetDimension(imageableSizeWidth,
                                                 imageableSizeHeight,
                                                 false);

          return result;
        }
      }

      /// <inheritdoc />
      public long? ImageableSizeHeight
      {
        get
        {
          var imageableSizeWidth = this.GetImageableSize(Xps.PrintCapabilitiesReader.ImageableSizeWidthXName);
          var imageableSizeHeight = this.GetImageableSize(Xps.PrintCapabilitiesReader.ImageableSizeHeightXName);

          var result = NumberHelper.GetDimension(imageableSizeWidth,
                                                 imageableSizeHeight,
                                                 true);

          return result;
        }
      }

      /// <inheritdoc />
      public string PortName { get; }

      /// <inheritdoc />
      public string DriverName { get; }

      [NotNull]
      private IXpsPrintCapabilities XpsPrintCapabilities { get; }

      [CanBeNull]
      private long? GetImageableSize([NotNull] XName imageableSizeXName)
      {
        long? imageableSize;

        var xpsProperty = this.XpsPrintCapabilities.GetXpsProperty(Xps.PrintCapabilitiesReader.PageImageableSizeXName);
        if (xpsProperty == null)
        {
          imageableSize = null;
        }
        else
        {
          xpsProperty = xpsProperty.GetXpsProperty(imageableSizeXName);
          if (xpsProperty == null)
          {
            imageableSize = null;
          }
          else
          {
            var value = xpsProperty.Value;
            if (value is long longValue)
            {
              imageableSize = longValue;
            }
            else
            {
              imageableSize = null;
            }
          }
        }

        return imageableSize;
      }

      /// <inheritdoc />
      public bool Equals(XpsPrinterDefinition other)
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

        return string.Equals(this.FullName,
                             other.FullName);
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

        return obj is XpsPrinterDefinition && this.Equals((XpsPrinterDefinition) obj);
      }

      /// <inheritdoc />
      public override int GetHashCode()
      {
        return this.FullName.GetHashCode();
      }

      public static bool operator ==(XpsPrinterDefinition left,
                                     XpsPrinterDefinition right)
      {
        return object.Equals(left,
                             right);
      }

      public static bool operator !=(XpsPrinterDefinition left,
                                     XpsPrinterDefinition right)
      {
        return !object.Equals(left,
                              right);
      }

      /// <inheritdoc />
      public override string ToString()
      {
        return this.FullName;
      }

      [NotNull]
      public static XpsPrinterDefinition Create([NotNull] PrintQueue printQueue,
                                                [NotNull] IXpsPrintCapabilities xpsPrintCapabilities)
      {
        var xpsPrinterDefinition = new XpsPrinterDefinition(printQueue.Name,
                                                            printQueue.FullName,
                                                            printQueue.QueuePort?.Name,
                                                            printQueue.QueueDriver?.Name,
                                                            xpsPrintCapabilities);

        return xpsPrinterDefinition;
      }
    }
  }
}
