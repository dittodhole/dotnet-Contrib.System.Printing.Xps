﻿/** @pp
 * rootnamespace: Contrib.System
 */
namespace Contrib.System.Printing.Xps.ExtensionMethods
{
  using global::System;
  using global::System.Drawing.Printing;
  using global::System.Linq;
  using global::JetBrains.Annotations;

  /// <summary>
  ///   Provides extensions for <see cref="T:System.Drawing.Printing.PrinterSettings"/> objects.
  /// </summary>
  [PublicAPI]
#if CONTRIB_SYSTEM_PRINTING_XPS
  public
#else
  internal
#endif
  static partial class PrinterSettingsExtensions
  {
    /// <summary>
    ///   Sets the printer and input bin of <paramref name="printerSettings"/>.
    /// </summary>
    /// <param name="printerSettings"/>
    /// <param name="xpsPrinterDefinition"/>
    /// <param name="xpsInputBinDefinition"/>
    /// <exception cref="T:System.ArgumentNullException"><paramref name="printerSettings"/> is <see langword="null"/>.</exception>
    /// <exception cref="T:System.ArgumentNullException"><paramref name="xpsPrinterDefinition"/> is <see langword="null"/>.</exception>
    /// <exception cref="T:System.Exception"/>
    [MustUseReturnValue]
    public static bool Set([NotNull] this PrinterSettings printerSettings,
                           [NotNull] IXpsPrinterDefinition xpsPrinterDefinition,
                           [CanBeNull] IXpsInputBinDefinition xpsInputBinDefinition = null)
    {
      if (printerSettings == null)
      {
        throw new ArgumentNullException(nameof(printerSettings));
      }
      if (xpsPrinterDefinition == null)
      {
        throw new ArgumentNullException(nameof(xpsPrinterDefinition));
      }

      printerSettings.PrinterName = xpsPrinterDefinition.FullName;

      bool result;
      if (printerSettings.IsValid)
      {
        if (xpsInputBinDefinition == null)
        {
          result = true;
        }
        else
        {
          result = false;

          foreach (var paperSource in printerSettings.PaperSources.Cast<PaperSource>())
          {
            result = string.Equals(paperSource.SourceName,
                                   xpsInputBinDefinition.DisplayName,
                                   StringComparison.Ordinal);
            if (result)
            {
              printerSettings.DefaultPageSettings.PaperSource = paperSource;
              break;
            }
          }
        }
      }
      else
      {
        result = false;
      }

      return result;
    }
  }
}
