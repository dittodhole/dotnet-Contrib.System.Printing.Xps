/** @pp
 * rootnamespace: Contrib.System
 */
namespace Contrib.System.Printing.Xps
{
  using global::JetBrains.Annotations;

  /// <summary>
  ///   Holds information of an input bin.
  /// </summary>
  /// <seealso cref="T:Contrib.System.Printing.Xps.XpsInputBinDefinitionFactory.XpsInputBinDefinition"/>
  [PublicAPI]
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

  partial class XpsInputBinDefinitionFactory
  {
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
      [PublicAPI]
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
