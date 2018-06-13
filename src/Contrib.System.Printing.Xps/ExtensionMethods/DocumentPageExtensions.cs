/** @pp
 * rootnamespace: Contrib.System
 */
namespace Contrib.System.Printing.Xps.ExtensionMethods
{
  using global::System;
  using global::System.Windows;
  using global::System.Windows.Documents;
  using global::System.Windows.Media;
  using global::System.Windows.Media.Imaging;
  using global::JetBrains.Annotations;

  /// <summary>
  ///   Provides extensions for <see cref="T:System.Windows.Documents.DocumentPage"/> objects.
  /// </summary>
  [PublicAPI]
#if CONTRIB_SYSTEM_PRINTING_XPS
  public
#else
  internal
#endif
  static partial class DocumentPageExtensions
  {
    /// <summary>
    ///   Renders the <paramref name="documentPage"/>.
    /// </summary>
    /// <param name="documentPage"/>
    /// <param name="dpiX"/>
    /// <param name="dpiY"/>
    /// <exception cref="T:System.ArgumentNullException"><paramref name="documentPage"/> is <see langword="null"/>.</exception>
    /// <exception cref="T:System.Exception"/>
    /// <seealso cref="T:System.Windows.Media.Imaging.RenderTargetBitmap"/>
    [NotNull]
    public static BitmapSource Render([NotNull] this DocumentPage documentPage,
                                      double dpiX,
                                      double dpiY)
    {
      if (documentPage == null)
      {
        throw new ArgumentNullException(nameof(documentPage));
      }

      var size = documentPage.Size;
      var width = size.Width / 96D * dpiX;
      var height = size.Height / 96D * dpiY;

      var result = new RenderTargetBitmap((int) width,
                                          (int) height,
                                          dpiX,
                                          dpiY,
                                          PixelFormats.Default);

      {
        var visual = documentPage.Visual;

        if (visual is UIElement uiElement)
        {
          uiElement.UpdateLayout();
        }

        result.Render(visual);
      }

      return result;
    }
  }
}
