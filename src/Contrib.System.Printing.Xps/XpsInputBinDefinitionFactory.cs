/** @pp
 * rootnamespace: Contrib.System
 */
namespace Contrib.System.Printing.Xps
{
  using global::System.Xml.Linq;
  using global::Anotar.LibLog;
  using global::Contrib.System.Printing.Xps.ExtensionMethods;
  using global::JetBrains.Annotations;

  /// <summary>
  ///   Factory for <typeparamref name="TXpsInputBinDefinition"/>.
  /// </summary>
  /// <typeparam name="TXpsInputBinDefinition"/>
  /// <seealso cref="T:Contrib.System.Printing.Xps.XpsInputBinDefinitionFactory"/>
#if CONTRIB_SYSTEM_PRINTING_XPS
  public
#else
  internal
#endif
  partial interface IXpsInputBinDefinitionFactory<out TXpsInputBinDefinition>
    where TXpsInputBinDefinition : IXpsInputBinDefinition
  {
    /// <summary>
    ///   Factory method for <typeparamref name="TXpsInputBinDefinition"/>.
    /// </summary>
    /// <param name="feature"/>
    /// <param name="name"/>
    /// <param name="option"/>
    /// <param name="printCapabilities"/>
    /// <exception cref="T:System.ArgumentNullException"><paramref name="feature"/> is <see langword="null"/>.</exception>
    /// <exception cref="T:System.ArgumentNullException"><paramref name="name"/> is <see langword="null"/>.</exception>
    /// <exception cref="T:System.ArgumentNullException"><paramref name="option"/> is <see langword="null"/>.</exception>
    /// <exception cref="T:System.ArgumentNullException"><paramref name="printCapabilities"/> is <see langword="null"/>.</exception>
    /// <exception cref="T:System.Exception"/>
    [Pure]
    [NotNull]
    TXpsInputBinDefinition Create([NotNull] XpsName feature,
                                  [NotNull] XpsName name,
                                  [NotNull] XElement option,
                                  [NotNull] XElement printCapabilities);
  }

  /// <summary>
  ///   Holds information of an input bin.
  /// </summary>
  /// <seealso cref="T:Contrib.System.Printing.Xps.XpsInputBinDefinitionFactory.XpsInputBinDefinition"/>
#if CONTRIB_SYSTEM_PRINTING_XPS
  public
#else
  internal
#endif
  partial interface IXpsInputBinDefinition
  {
    /// <summary>
    ///   Gets the name of the feature.
    /// </summary>
    /// <example>{http://schemas.microsoft.com/windows/2003/08/printing/printschemakeywords}JobInputBin</example>
    [NotNull]
    XpsName Feature { get; }

    /// <summary>
    ///   Gets the name of the input bin.
    /// </summary>
    /// <example>{http://schemas.microsoft.com/windows/2003/08/printing/printschemakeywords}AutoSelect</example>
    [NotNull]
    XpsName Name { get; }

    /// <summary>
    ///   Gets the display name of the input bin.
    /// </summary>
    /// <example>"Automatically Select"</example>
    [CanBeNull]
    string DisplayName { get; }

    /// <summary>
    ///   Gets the feed type of the input bin.
    /// </summary>
    /// <example>{http://schemas.microsoft.com/windows/2003/08/printing/printschemakeywords}Automatic</example>
    [CanBeNull]
    XpsName FeedType { get; }

    /// <summary>
    ///   Gets the active state of the input bin.
    /// </summary>
    bool IsAvailable { get; }
  }

  /// <inheritdoc/>
#if CONTRIB_SYSTEM_PRINTING_XPS
  public sealed
#else
  internal
#endif
  partial class XpsInputBinDefinitionFactory : IXpsInputBinDefinitionFactory<IXpsInputBinDefinition>
  {
    /// <summary>
    ///   Initializes a new instance of the <see cref="T:Contrib.System.Printing.Xps.XpsInputBinDefinitionFactory"/> class.
    /// </summary>
    public XpsInputBinDefinitionFactory() { }

    /// <inheritdoc/>
    public IXpsInputBinDefinition Create(XpsName feature,
                                         XpsName name,
                                         XElement option,
                                         XElement printCapabilities)
    {
      var displayName = option.FindElementByNameAttribute(XpsServer.DisplayNameName)
                              ?.Element(XpsServer.ValueName)
                              ?.GetValue() as string;

      var feedType = option.FindElementByNameAttribute(XpsServer.FeedTypeName)
                           ?.Element(XpsServer.ValueName)
                           ?.GetValue() as XpsName;

      bool isAvailable;
      var constrainedXName = option.GetXName(option.Attribute(XpsServer.ConstrainedName)
                                                   ?.Value);
      if (constrainedXName == null)
      {
        LogTo.Warn($"Could not get {nameof(XName)} from {nameof(XAttribute)} '{XpsServer.ConstrainedName}': {option}");
        isAvailable = true;
      }
      else if (constrainedXName == XpsServer.DeviceSettingsName)
      {
        isAvailable = false;
      }
      else if (constrainedXName == XpsServer.NoneName)
      {
        isAvailable = true;
      }
      else
      {
        LogTo.Warn($"Could not get {nameof(IXpsInputBinDefinition.IsAvailable)} from '{constrainedXName}', falling back to '{true}': {option}");
        isAvailable = true;
      }

      var xpsInputBinDefinition = new XpsInputBinDefinition
                                  {
                                    Feature = feature,
                                    Name = name,
                                    DisplayName = displayName,
                                    FeedType = feedType,
                                    IsAvailable = isAvailable
                                  };

      return xpsInputBinDefinition;
    }

    /// <inheritdoc/>
#if CONTRIB_SYSTEM_PRINTING_XPS
    private sealed
#else
    internal
#endif
    partial class XpsInputBinDefinition : IXpsInputBinDefinition
    {
      /// <summary>
      ///   Initializes a new instance of the <see cref="T:Contrib.System.Printing.Xps.XpsInputBinDefinitionFactory.XpsInputBinDefinition"/> class.
      /// </summary>
      public XpsInputBinDefinition() { }

      /// <inheritdoc/>
      public XpsName Feature { get; set; }

      /// <inheritdoc/>
      public XpsName Name { get; set; }

      /// <inheritdoc/>
      public string DisplayName { get; set; }

      /// <inheritdoc/>
      public XpsName FeedType { get; set; }

      /// <inheritdoc/>
      public bool IsAvailable { get; set; }
    }
  }
}
