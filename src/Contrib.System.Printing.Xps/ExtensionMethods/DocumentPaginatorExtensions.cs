/** @pp
 * rootnamespace: Contrib.System
 */
namespace Contrib.System.Printing.Xps.ExtensionMethods
{
  using global::System.IO;
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
    ///   Creates a <see cref="T:System.Windows.Media.Imaging.BitmapEncoder"/> object.
    /// </summary>
    [NotNull]
    public delegate BitmapEncoder BitmapEncoderFactory();

    /// <summary>
    ///   Renders the <paramref name="documentPaginator"/> with the supplied <see cref="T:System.Windows.Media.Imaging.BitmapEncoder"/>.
    /// </summary>
    /// <param name="documentPaginator"/>
    /// <param name="resolutionX"/>
    /// <param name="resolutionY"/>
    /// <param name="bitmapEncoderFactory"/>
    /// <exception cref="T:System.ArgumentNullException"><paramref name="documentPaginator"/> is <see langword="null"/>.</exception>
    /// <exception cref="T:System.ArgumentNullException"><paramref name="bitmapEncoderFactory"/> is <see langword="null"/>.</exception>
    /// <exception cref="T:System.Exception"/>
    /// <seealso cref="T:System.Windows.Media.Imaging.RenderTargetBitmap"/>
    /// <seealso cref="T:System.Windows.Media.Imaging.BitmapFrame"/>
    /// <seealso cref="T:System.Windows.Media.Imaging.BitmapEncoder"/>
    [NotNull]
    [ItemNotNull]
    public static MemoryStream[] Render([NotNull] this DocumentPaginator documentPaginator,
                                        long resolutionX,
                                        long resolutionY,
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
          var width = documentPage.Size.Width / 96d * resolutionX;
          var height = documentPage.Size.Height / 96d * resolutionY;
          var renderTargetBitmap = new RenderTargetBitmap((int) width,
                                                          (int) height,
                                                          resolutionX,
                                                          resolutionY,
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
