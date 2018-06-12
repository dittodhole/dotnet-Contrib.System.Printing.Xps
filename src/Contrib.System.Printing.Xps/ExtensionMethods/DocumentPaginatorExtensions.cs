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
  ///   Provides extensions for <see cref="T:System.Windows.Documents.DocumentPaginator"/> objects.
  /// </summary>
  [PublicAPI]
#if CONTRIB_SYSTEM_PRINTING_XPS
  public
#else
  internal
#endif
  static partial class DocumentPaginatorExtensions
  {
    /// <summary>
    ///   Renders the <paramref name="documentPaginator"/> with the supplied <see cref="T:System.Windows.Media.Imaging.BitmapEncoder"/>.
    /// </summary>
    /// <param name="documentPaginator"/>
    /// <param name="dpiX"/>
    /// <param name="dpiY"/>
    /// <exception cref="T:System.ArgumentNullException"><paramref name="documentPaginator"/> is <see langword="null"/>.</exception>
    /// <exception cref="T:System.Exception"/>
    /// <seealso cref="T:System.Windows.Media.Imaging.RenderTargetBitmap"/>
    [NotNull]
    [ItemNotNull]
    public static BitmapSource[] Render([NotNull] this DocumentPaginator documentPaginator,
                                        double dpiX,
                                        double dpiY)
    {
      if (documentPaginator == null)
      {
        throw new ArgumentNullException(nameof(documentPaginator));
      }

      var pageCount = documentPaginator.PageCount;

      var result = new BitmapSource[pageCount];

      for (var i = 0;
           i < pageCount;
           i++)
      {
        var documentPage = documentPaginator.GetPage(i);
        var visual = documentPage.Visual;

        if (visual is UIElement uiElement)
        {
          uiElement.UpdateLayout();
        }

        var size = documentPage.Size;
        var width = size.Width / 96D * dpiX;
        var height = size.Height / 96D * dpiY;
        var renderTargetBitmap = new RenderTargetBitmap((int) width,
                                                        (int) height,
                                                        dpiX,
                                                        dpiY,
                                                        PixelFormats.Default);
        renderTargetBitmap.Render(visual);

        result[i] = (BitmapSource) renderTargetBitmap.GetAsFrozen();
      }

      return result;
    }
  }
}
