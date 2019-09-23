﻿/** @pp
 * rootnamespace: Contrib.System
 */
namespace Contrib.System.Printing.Xps
{
  using global::System;
  using global::System.Printing;
  using global::System.Xml.Linq;
  using global::JetBrains.Annotations;

  /// <summary>
  ///   Factory for <typeparamref name="TXpsPrinterDefinition"/>.
  /// </summary>
  /// <typeparam name="TXpsPrinterDefinition"/>
  /// <seealso cref="T:Contrib.System.Printing.Xps.XpsPrinterDefinitionFactory"/>
#if CONTRIB_SYSTEM_PRINTING_XPS
  public
#else
  internal
#endif
  partial interface IXpsPrinterDefinitionFactory<out TXpsPrinterDefinition>
    where TXpsPrinterDefinition : IXpsPrinterDefinition
  {
    /// <summary>
    ///   Factory method for <typeparamref name="TXpsPrinterDefinition"/>.
    /// </summary>
    /// <param name="printQueue"/>
    /// <param name="printCapabilities"/>
    /// <exception cref="T:System.ArgumentNullException"><paramref name="printQueue"/> is <see langword="null"/>.</exception>
    /// <exception cref="T:System.ArgumentNullException"><paramref name="printCapabilities"/> is <see langword="null"/>.</exception>
    /// <exception cref="T:System.Exception"/>
    [Pure]
    [NotNull]
    TXpsPrinterDefinition Create([NotNull] PrintQueue printQueue,
                                 [NotNull] XElement printCapabilities);
  }

  /// <seealso cref="T:Contrib.System.Printing.Xps.XpsPrinterDefinitionFactory.XpsPrinterDefinition"/>
#if CONTRIB_SYSTEM_PRINTING_XPS
  public
#else
  internal
#endif
  partial interface IXpsPrinterDefinition
  {
    /// <summary>
    ///   Gets the name of the printer.
    /// </summary>
    [NotNull]
    string Name { get; }

    /// <summary>
    ///   Gets the hosting server name of the printer.
    /// </summary>
    [NotNull]
    string ServerName { get; }
  }

  /// <inheritdoc/>
#if CONTRIB_SYSTEM_PRINTING_XPS
  public sealed
#else
  internal
#endif
  partial class XpsPrinterDefinitionFactory : IXpsPrinterDefinitionFactory<IXpsPrinterDefinition>
  {
    /// <summary>
    ///   Initializes a new instance of the <see cref="T:Contrib.System.Printing.Xps.XpsPrinterDefinitionFactory"/> class.
    /// </summary>
    public XpsPrinterDefinitionFactory() { }

    /// <inheritdoc/>
    public IXpsPrinterDefinition Create(PrintQueue printQueue,
                                        XElement printCapabilities)
    {
      if (printQueue == null)
      {
        throw new ArgumentNullException(nameof(printQueue));
      }
      if (printCapabilities == null)
      {
        throw new ArgumentNullException(nameof(printCapabilities));
      }

      var result = new XpsPrinterDefinition
                   {
                     Name = printQueue.Name,
                     ServerName = printQueue.HostingPrintServer.Name
                   };

      return result;
    }

    /// <inheritdoc/>
#if CONTRIB_SYSTEM_PRINTING_XPS
    private sealed
#else
    internal
#endif
    partial class XpsPrinterDefinition : IXpsPrinterDefinition
    {
      /// <summary>
      ///   Initializes a new instance of the <see cref="T:Contrib.System.Printing.Xps.XpsPrinterDefinitionFactory.XpsPrinterDefinition"/> class.
      /// </summary>
      public XpsPrinterDefinition() { }

      /// <inheritdoc/>
      public string Name { get; set; }

      /// <inheritdoc/>
      public string ServerName { get; set; }
    }
  }
}
