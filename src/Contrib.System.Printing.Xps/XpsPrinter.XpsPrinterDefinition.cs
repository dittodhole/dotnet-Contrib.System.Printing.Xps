using System;
using System.Printing;
using System.Xml.Linq;
using JetBrains.Annotations;

namespace Contrib.System.Printing.Xps
{
  public interface IXpsPrinterDefinition
  {
    [NotNull]
    string Name { get; }

    [NotNull]
    string FullName { get; }

    [CanBeNull]
    string PortName { get; }

    [CanBeNull]
    string DriverName { get; }

    [CanBeNull]
    double? ImageableSizeWidth { get; }

    [CanBeNull]
    double? ImageableSizeHeight { get; }
  }

  public partial class XpsPrinter
  {
    internal sealed class XpsPrinterDefinition : IXpsPrinterDefinition,
                                                 IEquatable<XpsPrinterDefinition>
    {
      private XpsPrinterDefinition([NotNull] string name,
                                   [NotNull] string fullName,
                                   [CanBeNull] string portName,
                                   [CanBeNull] string driverName,
                                   [NotNull] IXpsPrintCapabilities xpsPrintCapabilities)
      {
        this.Name = name;
        this.FullName = fullName;
        this.PortName = portName;
        this.DriverName = driverName;
        this.XpsPrintCapabilities = xpsPrintCapabilities;
      }

      /// <inheritdoc />
      public string Name { get; }

      /// <inheritdoc />
      public string FullName { get; }

      /// <inheritdoc />
      public double? ImageableSizeWidth
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
      public double? ImageableSizeHeight
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
      private double? GetImageableSize([NotNull] XName imageableSizeXName)
      {
        var xpsProperty = this.XpsPrintCapabilities.GetXpsProperty(Xps.PrintCapabilitiesReader.PageImageableSizeXName);
        if (xpsProperty == null)
        {
          return null;
        }

        xpsProperty = xpsProperty.GetXpsProperty(imageableSizeXName);
        if (xpsProperty == null)
        {
          return null;
        }

        if (double.TryParse(xpsProperty.Value,
                            out var value))
        {
          return value;
        }

        return null;
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
