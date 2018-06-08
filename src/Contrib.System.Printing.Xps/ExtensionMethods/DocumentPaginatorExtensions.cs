/** @pp
 * rootnamespace: Contrib.System
 */
namespace Contrib.System.Printing.Xps.ExtensionMethods
{
  using global::System;
  using global::System.IO;
  using global::System.Windows;
  using global::System.Windows.Documents;
  using global::System.Windows.Media;
  using global::System.Windows.Media.Imaging;
  using global::JetBrains.Annotations;

  [PublicAPI]
#if CONTRIB_SYSTEM_PRINTING_XPS
  public
#else
  internal
#endif
  static partial class DocumentPaginatorExtensions
  {
    /// <summary>
    ///   Creates a <see cref="BitmapEncoder"/> object.
    /// </summary>
    [NotNull]
    public delegate BitmapEncoder BitmapEncoderFactory();

    /// <summary>
    ///   Renders the <paramref name="documentPaginator"/> with the supplied <see cref="BitmapEncoder"/>.
    /// </summary>
    /// <param name="documentPaginator"/>
    /// <param name="bitmapEncoderFactory"/>
    /// <exception cref="ArgumentNullException"><paramref name="documentPaginator"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="bitmapEncoderFactory"/> is <see langword="null"/>.</exception>
    /// <exception cref="Exception"/>
    /// <seealso cref="T:System.Windows.Media.Imaging.RenderTargetBitmap"/>
    /// <seealso cref="T:System.Windows.Media.Imaging.BitmapFrame"/>
    /// <seealso cref="T:System.Windows.Media.Imaging.BitmapEncoder"/>
    [NotNull]
    [ItemNotNull]
    public static MemoryStream[] Render([NotNull] this DocumentPaginator documentPaginator,
                                        [NotNull] [InstantHandle] BitmapEncoderFactory bitmapEncoderFactory)
    {
      var pageCount = documentPaginator.PageCount;

      var result = new MemoryStream[pageCount];

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

        var bitmapEncoder = bitmapEncoderFactory.Invoke();

        {
          var renderTargetBitmap = new RenderTargetBitmap((int) documentPage.Size.Width,
                                                          (int) documentPage.Size.Height,
                                                          96d,
                                                          96d,
                                                          PixelFormats.Default);
          renderTargetBitmap.Render(visual);

          var bitmapFrame = BitmapFrame.Create(renderTargetBitmap);

          bitmapEncoder.Frames.Add(bitmapFrame);
        }

        var memoryStream = result[i] = new MemoryStream();
        bitmapEncoder.Save(memoryStream);
      }

      return result;
    }
  }
}
