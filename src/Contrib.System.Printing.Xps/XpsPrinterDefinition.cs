using System;
using System.Xml.Linq;
using Contrib.System.Printing.Xps.ExtensionMethods;
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
    object GetValue([NotNull] XName name);
  }

  public interface IXpsPrinterDefinitionFactory
  {
    [NotNull]
    IXpsPrinterDefinition Create([NotNull] string displayName,
                                 [NotNull] string fullName,
                                 [CanBeNull] string portName,
                                 [CanBeNull] string driverName,
                                 [NotNull] IXpsPrintCapabilities xpsPrintCapabilities);
  }

  public sealed class XpsPrinterDefinitionFactory : IXpsPrinterDefinitionFactory
  {
    private sealed class XpsPrinterDefinition : IXpsPrinterDefinition,
                                                IEquatable<XpsPrinterDefinition>
    {
      public XpsPrinterDefinition([NotNull] string displayName,
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

      [NotNull]
      private IXpsPrintCapabilities XpsPrintCapabilities { get; }

      /// <inheritdoc />
      public string DisplayName { get; }

      /// <inheritdoc />
      public string FullName { get; }

      /// <inheritdoc />
      public string PortName { get; }

      /// <inheritdoc />
      public string DriverName { get; }

      /// <inheritdoc />
      public object GetValue(XName name)
      {
        object value;

        var xpsProperty = this.XpsPrintCapabilities.FindXpsProperty(name);
        if (xpsProperty == null)
        {
          value = null;
        }
        else
        {
          value = xpsProperty.Value;
        }

        return value;
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
    }

    /// <inheritdoc />
    public IXpsPrinterDefinition Create(string displayName,
                                        string fullName,
                                        string portName,
                                        string driverName,
                                        IXpsPrintCapabilities xpsPrintCapabilities)
    {
      var xpsPrinterDefinition = new XpsPrinterDefinition(displayName,
                                                          fullName,
                                                          portName,
                                                          driverName,
                                                          xpsPrintCapabilities);

      return xpsPrinterDefinition;
    }
  }
}
