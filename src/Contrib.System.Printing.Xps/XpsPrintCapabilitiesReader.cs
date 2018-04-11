using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Anotar.LibLog;
using JetBrains.Annotations;

namespace Contrib.System.Printing.Xps
{
  public interface IXpsPrintCapabilitiesReader
  {
    /// <exception cref="ArgumentNullException"><paramref name="printCapabilitiesXElement" /> is <see langword="null" />.</exception>
    [NotNull]
    IXpsPrintCapabilities ReadXpsPrintCapabilities([NotNull] XElement printCapabilitiesXElement);
  }

  [CanBeNull]
  public delegate XNamespace GetNamespaceOfPrefix([NotNull] string prefix);

  public class XpsPrintCapabilitiesReader : IXpsPrintCapabilitiesReader
  {
    /// <returns>xsi:</returns>
    [NotNull]
    public static XNamespace XmlSchemaInstanceXNamespace { get; } = XNamespace.Get("http://www.w3.org/2001/XMLSchema-instance");

    /// <returns>xsd:</returns>
    [NotNull]
    public static XNamespace XmlSchemaXNamespace { get; } = XNamespace.Get("http://www.w3.org/2001/XMLSchema");

    /// <returns>psf:</returns>
    [NotNull]
    public static XNamespace PrinterSchemaFrameworkXNamespace { get; } = XNamespace.Get("http://schemas.microsoft.com/windows/2003/08/printing/printschemaframework");

    /// <returns>psk:</returns>
    [NotNull]
    public static XNamespace PrinterSchemaKeywordsXNamespace { get; } = XNamespace.Get("http://schemas.microsoft.com/windows/2003/08/printing/printschemakeywords");

    /// <returns>psf:Feature</returns>
    [NotNull]
    public static XName FeatureElementXName { get; } = XpsPrintCapabilitiesReader.PrinterSchemaFrameworkXNamespace + "Feature";

    /// <returns>name</returns>
    [NotNull]
    public static XName NameAttributeXName { get; } = XNamespace.None + "name";

    /// <returns>psf:Property</returns>
    [NotNull]
    public static XName PropertyElementXName { get; } = XpsPrintCapabilitiesReader.PrinterSchemaFrameworkXNamespace + "Property";

    /// <returns>psf:ScoredProperty</returns>
    [NotNull]
    public static XName ScoredPropertyElementXName { get; } = XpsPrintCapabilitiesReader.PrinterSchemaFrameworkXNamespace + "ScoredProperty";

    /// <returns>psf:Option</returns>
    [NotNull]
    public static XName OptionElementXName { get; } = XpsPrintCapabilitiesReader.PrinterSchemaFrameworkXNamespace + "Option";

    /// <returns>psf:Value</returns>
    [NotNull]
    public static XName ValueElementXName { get; } = XpsPrintCapabilitiesReader.PrinterSchemaFrameworkXNamespace + "Value";

    /// <returns>psk:DisplayName</returns>
    [NotNull]
    public static XName DisplayNameXName { get; } = XpsPrintCapabilitiesReader.PrinterSchemaKeywordsXNamespace + "DisplayName";

    /// <returns>psk:PageInputBin</returns>
    [NotNull]
    public static XName PageInputBinXName { get; } = XpsPrintCapabilitiesReader.PrinterSchemaKeywordsXNamespace + "PageInputBin";

    /// <returns>psk:DocumentInputBin</returns>
    [NotNull]
    public static XName DocumentInputBinXName { get; } = XpsPrintCapabilitiesReader.PrinterSchemaKeywordsXNamespace + "DocumentInputBin";

    /// <returns>psk:JobInputBin</returns>
    [NotNull]
    public static XName JobInputBinXName { get; } = XpsPrintCapabilitiesReader.PrinterSchemaKeywordsXNamespace + "JobInputBin";

    /// <returns>psk:PageImageableSize</returns>
    [NotNull]
    public static XName PageImageableSizeXName { get; } = XpsPrintCapabilitiesReader.PrinterSchemaKeywordsXNamespace + "PageImageableSize";

    /// <returns>psk:ImageableSizeWidth</returns>
    [NotNull]
    public static XName ImageableSizeWidthXName { get; } = XpsPrintCapabilitiesReader.PrinterSchemaKeywordsXNamespace + "ImageableSizeWidth";

    /// <returns>psk:ImageableSizeHeight</returns>
    [NotNull]
    public static XName ImageableSizeHeightXName { get; } = XpsPrintCapabilitiesReader.PrinterSchemaKeywordsXNamespace + "ImageableSizeHeight";

    /// <returns>psk:PageMediaSize</returns>
    [NotNull]
    public static XName PageMediaSizeXName { get; } = XpsPrintCapabilitiesReader.PrinterSchemaKeywordsXNamespace + "PageMediaSize";

    /// <returns>psk:MediaSizeWidth</returns>
    [NotNull]
    public static XName MediaSizeWidthXName { get; } = XpsPrintCapabilitiesReader.PrinterSchemaKeywordsXNamespace + "MediaSizeWidth";

    /// <returns>psk:MediaSizeHeight</returns>
    [NotNull]
    public static XName MediaSizeHeightXName { get; } = XpsPrintCapabilitiesReader.PrinterSchemaKeywordsXNamespace + "MediaSizeHeight";

    /// <returns>psk:FeedType</returns>
    [NotNull]
    public static XName FeedTypeXName { get; } = XpsPrintCapabilitiesReader.PrinterSchemaKeywordsXNamespace + "FeedType";

    /// <returns>xsi:type</returns>
    [NotNull]
    public static XName TypeXName { get; } = XpsPrintCapabilitiesReader.XmlSchemaInstanceXNamespace + "type";

    /// <returns>xsd:integer</returns>
    [NotNull]
    public static XName IntegerTypeXName { get; } = XpsPrintCapabilitiesReader.XmlSchemaXNamespace + "integer";

    /// <returns>xsd:string</returns>
    [NotNull]
    public static XName StringTypeXName { get; } = XpsPrintCapabilitiesReader.XmlSchemaXNamespace + "string";

    /// <returns>xsd:QName</returns>
    [NotNull]
    public static XName QNameTypeXName { get; } = XpsPrintCapabilitiesReader.XmlSchemaXNamespace + "QName";

    public XpsPrintCapabilitiesReader()
      : this(new XpsPrintCapabilitiesFactory(),
             new XpsFeatureFactory(),
             new XpsOptionFactory(),
             new XpsPropertyFactory()) { }

    /// <exception cref="ArgumentNullException"><paramref name="xpsPrintCapabilitiesFactory" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="xpsFeatureFactory" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="xpsOptionFactory" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="xpsPropertyFactory" /> is <see langword="null" />.</exception>
    public XpsPrintCapabilitiesReader([NotNull] IXpsPrintCapabilitiesFactory xpsPrintCapabilitiesFactory,
                                      [NotNull] IXpsFeatureFactory xpsFeatureFactory,
                                      [NotNull] IXpsOptionFactory xpsOptionFactory,
                                      [NotNull] IXpsPropertyFactory xpsPropertyFactory)
    {
      this.XpsPrintCapabilitiesFactory = xpsPrintCapabilitiesFactory ?? throw new ArgumentNullException(nameof(xpsPrintCapabilitiesFactory));
      this.XpsFeatureFactory = xpsFeatureFactory ?? throw new ArgumentNullException(nameof(xpsFeatureFactory));
      this.XpsOptionFactory = xpsOptionFactory ?? throw new ArgumentNullException(nameof(xpsOptionFactory));
      this.XpsPropertyFactory = xpsPropertyFactory ?? throw new ArgumentNullException(nameof(xpsPropertyFactory));
    }

    [NotNull]
    private IXpsFeatureFactory XpsFeatureFactory { get; }

    [NotNull]
    private IXpsOptionFactory XpsOptionFactory { get; }

    [NotNull]
    private IXpsPrintCapabilitiesFactory XpsPrintCapabilitiesFactory { get; }

    [NotNull]
    private IXpsPropertyFactory XpsPropertyFactory { get; }

    /// <inheritdoc />
    public virtual IXpsPrintCapabilities ReadXpsPrintCapabilities(XElement printCapabilitiesXElement)
    {
      if (printCapabilitiesXElement == null)
      {
        throw new ArgumentNullException(nameof(printCapabilitiesXElement));
      }

      var xpsPrintCapabilities = this.ReadXpsPrintCapabilitiesImpl(printCapabilitiesXElement);

      return xpsPrintCapabilities;
    }

    [NotNull]
    protected virtual IXpsPrintCapabilities ReadXpsPrintCapabilitiesImpl([NotNull] XElement printCapabilitiesXElement)
    {
      var xpsPrintCapabilities = this.XpsPrintCapabilitiesFactory.Create();

      {
        var xpsFeatures = this.ReadXpsFeaturesImpl(printCapabilitiesXElement);
        xpsPrintCapabilities.AddXpsFeatures(xpsFeatures);
      }
      {
        var xpsProperties = this.ReadXpsPropertiesImpl(printCapabilitiesXElement,
                                                       XpsPrintCapabilitiesReader.PropertyElementXName);
        xpsPrintCapabilities.AddXpsProperties(xpsProperties);
      }

      return xpsPrintCapabilities;
    }

    [NotNull]
    [ItemNotNull]
    protected virtual IEnumerable<IXpsFeature> ReadXpsFeaturesImpl([NotNull] XElement printCapabilitiesXElement)
    {
      var featureXElements = printCapabilitiesXElement.Elements(XpsPrintCapabilitiesReader.FeatureElementXName);
      foreach (var featureXElement in featureXElements)
      {
        var xpsFeature = this.ReadXpsFeatureImpl(featureXElement);
        if (xpsFeature == null)
        {
          continue;
        }

        yield return xpsFeature;
      }
    }

    [CanBeNull]
    protected virtual IXpsFeature ReadXpsFeatureImpl([NotNull] XElement featureXElement)
    {
      var nameXAttribute = featureXElement.Attribute(XpsPrintCapabilitiesReader.NameAttributeXName);
      if (nameXAttribute == null)
      {
        LogTo.Warn($"Could not get {nameof(XAttribute)} '{XpsPrintCapabilitiesReader.NameAttributeXName}' from {nameof(XElement)}: {featureXElement}");
        return null;
      }

      var name = nameXAttribute.Value;
      var nameXName = XpsPrintCapabilitiesReader.GetXName(name,
                                                       featureXElement.GetNamespaceOfPrefix);
      if (nameXName == null)
      {
        LogTo.Warn($"Could not get {nameof(XName)} from {nameof(XAttribute)} '{XpsPrintCapabilitiesReader.NameAttributeXName}': {featureXElement}");
        return null;
      }

      var xpsFeature = this.XpsFeatureFactory.Create(nameXName);
      {
        var xpsProperties = this.ReadXpsPropertiesImpl(featureXElement,
                                                       XpsPrintCapabilitiesReader.PropertyElementXName);
        xpsFeature.AddXpsProperties(xpsProperties);
      }
      {
        var xpsOptions = this.ReadXpsOptionsImpl(featureXElement);
        xpsFeature.AddXpsOptions(xpsOptions);
      }

      return xpsFeature;
    }

    [NotNull]
    [ItemNotNull]
    protected virtual IEnumerable<IXpsOption> ReadXpsOptionsImpl([NotNull] XElement featureXElement)
    {
      var optionXElements = featureXElement.Elements(XpsPrintCapabilitiesReader.OptionElementXName);
      foreach (var optionXElement in optionXElements)
      {
        var xpsOption = this.ReadXpsOptionImpl(optionXElement);
        if (xpsOption == null)
        {
          continue;
        }

        yield return xpsOption;
      }
    }

    [CanBeNull]
    protected virtual IXpsOption ReadXpsOptionImpl([NotNull] XElement optionXElement)
    {
      IXpsOption xpsOption;
      var nameXAttribute = optionXElement.Attribute(XpsPrintCapabilitiesReader.NameAttributeXName);
      if (nameXAttribute == null)
      {
        xpsOption = this.XpsOptionFactory.Create();
      }
      else
      {
        var name = nameXAttribute.Value;
        var nameXName = XpsPrintCapabilitiesReader.GetXName(name,
                                                         optionXElement.GetNamespaceOfPrefix);
        if (nameXName == null)
        {
          LogTo.Warn($"Could not get {nameof(XName)} from {nameof(XAttribute)} '{XpsPrintCapabilitiesReader.NameAttributeXName}': {optionXElement}");
          return null;
        }

        xpsOption = this.XpsOptionFactory.Create(nameXName);
      }

      {
        var xpsProperties = this.ReadXpsPropertiesImpl(optionXElement,
                                                       XpsPrintCapabilitiesReader.PropertyElementXName);
        xpsOption.AddXpsProperties(xpsProperties);
      }
      {
        var xpsProperties = this.ReadXpsPropertiesImpl(optionXElement,
                                                       XpsPrintCapabilitiesReader.ScoredPropertyElementXName);
        xpsOption.AddXpsProperties(xpsProperties);
      }

      return xpsOption;
    }

    [NotNull]
    [ItemNotNull]
    protected virtual IEnumerable<IXpsProperty> ReadXpsPropertiesImpl([NotNull] XElement xelement,
                                                                      [NotNull] XName propertyElementXName)
    {
      var propertyXElements = xelement.Elements(propertyElementXName);
      foreach (var propertyXElement in propertyXElements)
      {
        var xpsProperty = this.ReadXpsPropertyImpl(propertyElementXName,
                                                   propertyXElement);
        if (xpsProperty == null)
        {
          continue;
        }

        yield return xpsProperty;
      }
    }

    [CanBeNull]
    protected virtual IXpsProperty ReadXpsPropertyImpl([NotNull] XName propertyElementXName,
                                                       [NotNull] XElement propertyXElement)
    {
      var nameXAttribute = propertyXElement.Attribute(XpsPrintCapabilitiesReader.NameAttributeXName);
      if (nameXAttribute == null)
      {
        LogTo.Warn($"Could not get {nameof(XAttribute)} '{XpsPrintCapabilitiesReader.NameAttributeXName}' from {nameof(XElement)}: {propertyXElement}");
        return null;
      }

      var name = nameXAttribute.Value;
      var nameXName = XpsPrintCapabilitiesReader.GetXName(name,
                                                       propertyXElement.GetNamespaceOfPrefix);
      if (nameXName == null)
      {
        LogTo.Warn($"Could not get {nameof(XName)} from {nameof(XAttribute)} '{XpsPrintCapabilitiesReader.NameAttributeXName}': {propertyXElement}");
        return null;
      }

      object value;
      {
        var valueXElement = propertyXElement.Element(XpsPrintCapabilitiesReader.ValueElementXName);
        if (valueXElement == null)
        {
          value = null;
        }
        else
        {
          value = this.ReadValueImpl(valueXElement);
        }
      }

      IXpsProperty xpsProperty;
      if (value == null)
      {
        xpsProperty = this.XpsPropertyFactory.Create(nameXName,
                                                     propertyElementXName);
      }
      else
      {
        xpsProperty = this.XpsPropertyFactory.Create(nameXName,
                                                     propertyElementXName,
                                                     value);
      }

      // TODO verify behaviour: is it either VALUE _or_ child PROPERTIES?

      {
        var xpsProperties = this.ReadXpsPropertiesImpl(propertyXElement,
                                                       XpsPrintCapabilitiesReader.PropertyElementXName);
        xpsProperty.AddXpsProperties(xpsProperties);
      }

      return xpsProperty;
    }

    [CanBeNull]
    protected virtual object ReadValueImpl([NotNull] XElement valueXElement)
    {
      object value;
      // TODO there *MUST* be a better way for handling xsi:type

      var rawValue = valueXElement.Value;

      var typeXAttribute = valueXElement.Attribute(XpsPrintCapabilitiesReader.TypeXName);
      if (typeXAttribute == null)
      {
        value = rawValue;
      }
      else
      {
        var type = typeXAttribute.Value;
        var typeXName = XpsPrintCapabilitiesReader.GetXName(type,
                                                         valueXElement.GetNamespaceOfPrefix);
        if (typeXName == null)
        {
          LogTo.Warn($"Could not get {nameof(XName)} from {nameof(XAttribute)} '{XpsPrintCapabilitiesReader.TypeXName}': {valueXElement}");
          value = null;
        }
        else if (typeXName == XpsPrintCapabilitiesReader.StringTypeXName)
        {
          value = rawValue;
        }
        else if (typeXName == XpsPrintCapabilitiesReader.IntegerTypeXName)
        {
          if (long.TryParse(rawValue,
                            out var longValue))
          {
            value = longValue;
          }
          else
          {
            value = null;
          }
        }
        else if (typeXName == XpsPrintCapabilitiesReader.QNameTypeXName)
        {
          var xnameValue = XpsPrintCapabilitiesReader.GetXName(rawValue,
                                                            valueXElement.GetNamespaceOfPrefix);
          if (xnameValue == null)
          {
            LogTo.Warn($"Could not get {nameof(XName)} from {nameof(XElement)}: {valueXElement}");
            value = null;
          }
          else
          {
            value = xnameValue;
          }
        }
        else
        {
          // TODO support more types :beers:
          value = rawValue;
        }
      }

      return value;
    }

    [Pure]
    [CanBeNull]
    public static XName GetXName([CanBeNull] string str,
                                 [NotNull] GetNamespaceOfPrefix getNamespaceOfPrefix)
    {
      XName xname;
      if (str == null)
      {
        xname = null;
      }
      else
      {
        string prefix;
        string localName;
        if (str.Contains(':'))
        {
          var parts = str.Split(':');
          prefix = parts.ElementAtOrDefault(0);
          localName = parts.ElementAtOrDefault(1);
        }
        else
        {
          prefix = null;
          localName = null;
        }

        if (prefix == null)
        {
          try
          {
            xname = XName.Get(str);
          }
          catch (Exception exception)
          {
            LogTo.WarnException($"Could not get {nameof(XName)} from '{str}'.",
                                exception);
            xname = null;
          }
        }
        else if (localName == null)
        {
          try
          {
            xname = XName.Get(str);
          }
          catch (Exception exception)
          {
            LogTo.WarnException($"Could not get {nameof(XName)} from '{str}'.",
                                exception);
            xname = null;
          }
        }
        else
        {
          var xnamespace = getNamespaceOfPrefix.Invoke(prefix);
          if (xnamespace == null)
          {
            LogTo.Warn($"Could not get {nameof(XNamespace)} for {nameof(prefix)} '{prefix}'.");
            xname = null;
          }
          else
          {
            try
            {
              xname = xnamespace.GetName(localName);
            }
            catch (Exception exception)
            {
              LogTo.WarnException($"Could not get {nameof(XName)} from '{localName}'.",
                                  exception);
              xname = null;
            }
          }
        }
      }

      return xname;
    }
  }
}
