﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Anotar.LibLog;
using JetBrains.Annotations;

namespace Contrib.System.Printing.Xps
{
  public interface IPrintCapabilitiesReader
  {
    /// <exception cref="ArgumentNullException"><paramref name="printCapabilitiesXElement" /> is <see langword="null" />.</exception>
    [NotNull]
    IXpsPrintCapabilities ReadXpsPrintCapabilities([NotNull] XElement printCapabilitiesXElement);

    /// <exception cref="ArgumentNullException"><paramref name="printTicketXElement" /> is <see langword="null" />.</exception>
    [NotNull]
    IXpsPrintTicket ReadXpsPrintTicket([NotNull] XElement printTicketXElement);
  }

  [CanBeNull]
  public delegate XNamespace GetNamespaceOfPrefix([NotNull] string prefix);

  public class PrintCapabilitiesReader : IPrintCapabilitiesReader
  {
    /// <returns>psf:</returns>
    [NotNull]
    public static XNamespace PrinterSchemaFrameworkXNamespace { get; } = XNamespace.Get("http://schemas.microsoft.com/windows/2003/08/printing/printschemaframework");

    /// <returns>psk:</returns>
    [NotNull]
    public static XNamespace PrinterSchemaKeywordsXNamespace { get; } = XNamespace.Get("http://schemas.microsoft.com/windows/2003/08/printing/printschemakeywords");

    /// <returns>psf:Feature</returns>
    [NotNull]
    public static XName FeatureElementXName { get; } = PrintCapabilitiesReader.PrinterSchemaFrameworkXNamespace + "Feature";

    /// <returns>name</returns>
    [NotNull]
    public static XName NameAttributeXName { get; } = XNamespace.None + "name";

    /// <returns>psf:Property</returns>
    [NotNull]
    public static XName PropertyElementXName { get; } = PrintCapabilitiesReader.PrinterSchemaFrameworkXNamespace + "Property";

    /// <returns>psf:ScoredProperty</returns>
    [NotNull]
    public static XName ScoredPropertyElementXName { get; } = PrintCapabilitiesReader.PrinterSchemaFrameworkXNamespace + "ScoredProperty";

    /// <returns>psf:Option</returns>
    [NotNull]
    public static XName OptionElementXName { get; } = PrintCapabilitiesReader.PrinterSchemaFrameworkXNamespace + "Option";

    /// <returns>psf:Value</returns>
    [NotNull]
    public static XName ValueElementXName { get; } = PrintCapabilitiesReader.PrinterSchemaFrameworkXNamespace + "Value";

    /// <returns>psk:DisplayName</returns>
    [NotNull]
    public static XName DisplayNameXName { get; } = PrintCapabilitiesReader.PrinterSchemaKeywordsXNamespace + "DisplayName";

    /// <returns>psk:PageInputBin</returns>
    [NotNull]
    public static XName PageInputBinXName { get; } = PrintCapabilitiesReader.PrinterSchemaKeywordsXNamespace + "PageInputBin";

    /// <returns>psk:DocumentInputBin</returns>
    [NotNull]
    public static XName DocumentInputBinXName { get; } = PrintCapabilitiesReader.PrinterSchemaKeywordsXNamespace + "DocumentInputBin";

    /// <returns>psk:JobInputBin</returns>
    [NotNull]
    public static XName JobInputBinXName { get; } = PrintCapabilitiesReader.PrinterSchemaKeywordsXNamespace + "JobInputBin";

    /// <returns>psk:PageImageableSize</returns>
    [NotNull]
    public static XName PageImageableSizeXName { get; } = PrintCapabilitiesReader.PrinterSchemaKeywordsXNamespace + "PageImageableSize";

    /// <returns>psk:ImageableSizeWidth</returns>
    [NotNull]
    public static XName ImageableSizeWidthXName { get; } = PrintCapabilitiesReader.PrinterSchemaKeywordsXNamespace + "ImageableSizeWidth";

    /// <returns>psk:ImageableSizeHeight</returns>
    [NotNull]
    public static XName ImageableSizeHeightXName { get; } = PrintCapabilitiesReader.PrinterSchemaKeywordsXNamespace + "ImageableSizeHeight";

    /// <returns>psk:PageMediaSize</returns>
    [NotNull]
    public static XName PageMediaSizeXName { get; } = PrintCapabilitiesReader.PrinterSchemaKeywordsXNamespace + "PageMediaSize";

    /// <returns>psk:MediaSizeWidth</returns>
    [NotNull]
    public static XName MediaSizeWidthXName { get; } = PrintCapabilitiesReader.PrinterSchemaKeywordsXNamespace + "MediaSizeWidth";

    /// <returns>psk:MediaSizeHeight</returns>
    [NotNull]
    public static XName MediaSizeHeightXName { get; } = PrintCapabilitiesReader.PrinterSchemaKeywordsXNamespace + "MediaSizeHeight";

    /// <returns>psk:FeedType</returns>
    [NotNull]
    public static XName FeedTypeXName { get; } = PrintCapabilitiesReader.PrinterSchemaKeywordsXNamespace + "FeedType";

    public PrintCapabilitiesReader()
    {
      this.XpsFeatureFactory = new XpsFeatureFactory();
      this.XpsOptionFactory = new XpsOptionFactory();
      this.XpsPrintCapabilitiesFactory = new XpsPrintCapabilitiesFactory();
      this.XpsPrintTicketFactory = new XpsPrintTicketFactory();
      this.XpsPropertyFactory = new XpsPropertyFactory();
    }

    /// <exception cref="ArgumentNullException"><paramref name="xpsFeatureFactory" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="xpsOptionFactory" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="xpsPrintCapabilitiesFactory" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="xpsPrintTicketFactory" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="xpsPropertyFactory" /> is <see langword="null" />.</exception>
    public PrintCapabilitiesReader([NotNull] IXpsPrintCapabilitiesFactory xpsPrintCapabilitiesFactory,
                                   [NotNull] IXpsPrintTicketFactory xpsPrintTicketFactory,
                                   [NotNull] IXpsFeatureFactory xpsFeatureFactory,
                                   [NotNull] IXpsOptionFactory xpsOptionFactory,
                                   [NotNull] IXpsPropertyFactory xpsPropertyFactory)
    {
      this.XpsFeatureFactory = xpsFeatureFactory ?? throw new ArgumentNullException(nameof(xpsFeatureFactory));
      this.XpsOptionFactory = xpsOptionFactory ?? throw new ArgumentNullException(nameof(xpsOptionFactory));
      this.XpsPrintCapabilitiesFactory = xpsPrintCapabilitiesFactory ?? throw new ArgumentNullException(nameof(xpsPrintCapabilitiesFactory));
      this.XpsPrintTicketFactory = xpsPrintTicketFactory ?? throw new ArgumentNullException(nameof(xpsPrintTicketFactory));
      this.XpsPropertyFactory = xpsPropertyFactory ?? throw new ArgumentNullException(nameof(xpsPropertyFactory));
    }

    [NotNull]
    private IXpsFeatureFactory XpsFeatureFactory { get; }

    [NotNull]
    private IXpsOptionFactory XpsOptionFactory { get; }

    [NotNull]
    private IXpsPrintCapabilitiesFactory XpsPrintCapabilitiesFactory { get; }

    [NotNull]
    private IXpsPrintTicketFactory XpsPrintTicketFactory { get; }

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
                                                       PrintCapabilitiesReader.PropertyElementXName);
        xpsPrintCapabilities.AddXpsProperties(xpsProperties);
      }

      return xpsPrintCapabilities;
    }

    /// <inheritdoc />
    public virtual IXpsPrintTicket ReadXpsPrintTicket(XElement printTicketXElement)
    {
      if (printTicketXElement == null)
      {
        throw new ArgumentNullException(nameof(printTicketXElement));
      }

      var xpsPrintTicket = this.ReadXpsPrintTicketImpl(printTicketXElement);

      return xpsPrintTicket;
    }

    [NotNull]
    protected virtual IXpsPrintTicket ReadXpsPrintTicketImpl([NotNull] XElement printTicketXElement)
    {
      var xpsPrintTicket = this.XpsPrintTicketFactory.Create();

      {
        var xpsFeatures = this.ReadXpsFeaturesImpl(printTicketXElement);
        xpsPrintTicket.AddXpsFeatures(xpsFeatures);
      }

      return xpsPrintTicket;
    }

    [NotNull]
    [ItemNotNull]
    protected virtual IEnumerable<IXpsFeature> ReadXpsFeaturesImpl([NotNull] XElement printCapabilitiesXElement)
    {
      var featureXElements = printCapabilitiesXElement.Elements(PrintCapabilitiesReader.FeatureElementXName);
      foreach (var featureXElement in featureXElements)
      {
        var nameXAttribute = featureXElement.Attribute(PrintCapabilitiesReader.NameAttributeXName);
        if (nameXAttribute == null)
        {
          LogTo.Warn($"Could not get {nameof(nameXAttribute)} from {nameof(featureXElement)}: {featureXElement}");
          continue;
        }

        var name = nameXAttribute.Value;
        var nameXName = PrintCapabilitiesReader.GetXName(name,
                                                         featureXElement.GetNamespaceOfPrefix);
        if (nameXName == null)
        {
          LogTo.Warn($"Could not get {nameof(nameXName)} from {nameof(nameXAttribute)}: {nameXAttribute}");
          continue;
        }

        var xpsFeature = this.XpsFeatureFactory.Create(nameXName);
        {
          var xpsProperties = this.ReadXpsPropertiesImpl(featureXElement,
                                                         PrintCapabilitiesReader.PropertyElementXName);
          xpsFeature.AddXpsProperties(xpsProperties);
        }
        {
          var xpsOptions = this.ReadXpsOptionsImpl(featureXElement);
          xpsFeature.AddXpsOptions(xpsOptions);
        }

        yield return xpsFeature;
      }
    }

    [NotNull]
    [ItemNotNull]
    protected virtual IEnumerable<IXpsOption> ReadXpsOptionsImpl([NotNull] XElement featureXElement)
    {
      var optionXElements = featureXElement.Elements(PrintCapabilitiesReader.OptionElementXName);
      foreach (var optionXElement in optionXElements)
      {
        IXpsOption xpsOption;
        var nameXAttribute = optionXElement.Attribute(PrintCapabilitiesReader.NameAttributeXName);
        if (nameXAttribute == null)
        {
          xpsOption = this.XpsOptionFactory.Create();
        }
        else
        {
          var name = nameXAttribute.Value;
          var nameXName = PrintCapabilitiesReader.GetXName(name,
                                                           optionXElement.GetNamespaceOfPrefix);
          if (nameXName == null)
          {
            LogTo.Warn($"Could not get {nameof(nameXName)} from {nameof(nameXAttribute)}: {nameXAttribute}");
            continue;
          }

          xpsOption = this.XpsOptionFactory.Create(nameXName);
        }

        {
          var xpsProperties = this.ReadXpsPropertiesImpl(optionXElement,
                                                         PrintCapabilitiesReader.PropertyElementXName);
          xpsOption.AddXpsProperties(xpsProperties);
        }
        {
          var xpsProperties = this.ReadXpsPropertiesImpl(optionXElement,
                                                         PrintCapabilitiesReader.ScoredPropertyElementXName);
          xpsOption.AddXpsProperties(xpsProperties);
        }

        yield return xpsOption;
      }
    }

    [NotNull]
    [ItemNotNull]
    protected virtual IEnumerable<IXpsProperty> ReadXpsPropertiesImpl([NotNull] XElement xelement,
                                                                      [NotNull] XName propertyElementXName)
    {
      var propertyXElements = xelement.Elements(propertyElementXName);
      foreach (var propertyXElement in propertyXElements)
      {
        var nameXAttribute = propertyXElement.Attribute(PrintCapabilitiesReader.NameAttributeXName);
        if (nameXAttribute == null)
        {
          LogTo.Warn($"Could not get {nameof(nameXAttribute)} from {nameof(propertyXElement)}: {propertyXElement}");
          continue;
        }

        var name = nameXAttribute.Value;
        var nameXName = PrintCapabilitiesReader.GetXName(name,
                                                         propertyXElement.GetNamespaceOfPrefix);
        if (nameXName == null)
        {
          LogTo.Warn($"Could not get {nameof(nameXName)} from {nameof(nameXAttribute)}: {nameXAttribute}");
          continue;
        }

        IXpsProperty xpsProperty;

        var valueXElement = propertyXElement.Element(PrintCapabilitiesReader.ValueElementXName);
        if (valueXElement == null)
        {
          xpsProperty = this.XpsPropertyFactory.Create(nameXName,
                                                       propertyElementXName);
        }
        else
        {
          var value = valueXElement.Value;
          var valueXName = PrintCapabilitiesReader.GetXName(value,
                                                            propertyXElement.GetNamespaceOfPrefix);
          xpsProperty = this.XpsPropertyFactory.Create(nameXName,
                                                       propertyElementXName,
                                                       value,
                                                       valueXName);
        }

        // TODO verify behaviour: is it either VALUE _or_ child PROPERTIES?

        {
          var xpsProperties = this.ReadXpsPropertiesImpl(propertyXElement,
                                                         PrintCapabilitiesReader.PropertyElementXName);
          xpsProperty.AddXpsProperties(xpsProperties);
        }

        yield return xpsProperty;
      }
    }

    /// <code>
    ///   using Contrib.System.Printing.Xps;
    ///
    ///   var xname = XmlHelper.GetXName(<paramref name="str"/>: "psk:JobInputBin",
    ///                                  <paramref name="getNamespaceOfPrefix"/>: xelement.GetNamespaceOfPrefix);
    ///   if (xname.NamespaceName == "http://schemas.microsoft.com/windows/2003/08/printing/printschemakeywords") ...
    ///   if (xname.LocalName == "JobInputBin") ...
    /// </code>
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
          xname = XName.Get(str);
        }
        else if (localName == null)
        {
          xname = XName.Get(str);
        }
        else
        {
          var xnamespace = getNamespaceOfPrefix.Invoke(prefix);
          if (xnamespace == null)
          {
            xname = null;
          }
          else
          {
            xname = xnamespace + localName;
          }
        }
      }

      return xname;
    }
  }
}