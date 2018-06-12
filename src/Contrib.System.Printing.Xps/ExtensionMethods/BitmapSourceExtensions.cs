/** @pp
 * rootnamespace: Contrib.System
 */
namespace Contrib.System.Printing.Xps.ExtensionMethods
{
  using global::System;
  using global::System.Drawing;
  using global::System.IO;
  using global::System.Windows.Media.Imaging;
  using global::JetBrains.Annotations;

  /// <summary>
  ///   Provides extensions for <see cref="T:System.Windows.Media.ImageSource"/> objects.
  /// </summary>
  [PublicAPI]
#if CONTRIB_SYSTEM_PRINTING_XPS
  public
#else
  internal
#endif
  static partial class BitmapSourceExtensions
  {
    /// <summary>
    ///   Converts the <paramref name="bitmapSource"/> to <see cref="T:System.Drawing.Bitmap"/>.
    /// </summary>
    /// <param name="bitmapSource"/>
    /// <exception cref="ArgumentNullException"><paramref name="bitmapSource"/> is <see langword="null"/>.</exception>
    /// <exception cref="T:System.Exception"/>
    [NotNull]
    public static Bitmap ToBitmap([NotNull] this BitmapSource bitmapSource)
    {
      if (bitmapSource == null)
      {
        throw new ArgumentNullException(nameof(bitmapSource));
      }

      var bitmapEncoder = new GifBitmapEncoder();
      var result = bitmapSource.ToBitmap(bitmapEncoder);

      return result;
    }

    /// <summary>
    ///   Converts the <paramref name="bitmapSource"/> to <see cref="T:System.Drawing.Bitmap"/>.
    /// </summary>
    /// <param name="bitmapSource"/>
    /// <param name="bitmapEncoder"/>
    /// <exception cref="ArgumentNullException"><paramref name="bitmapSource"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="bitmapEncoder"/> is <see langword="null"/>.</exception>
    /// <exception cref="T:System.Exception"/>
    /// <seealso cref="T:System.Windows.Media.Imaging.BmpBitmapEncoder"/>
    /// <seealso cref="T:System.Windows.Media.Imaging.GifBitmapEncoder"/>
    /// <seealso cref="T:System.Windows.Media.Imaging.JpegBitmapEncoder"/>
    /// <seealso cref="T:System.Windows.Media.Imaging.PngBitmapEncoder"/>
    /// <seealso cref="T:System.Windows.Media.Imaging.TiffBitmapEncoder"/>
    /// <seealso cref="T:System.Windows.Media.Imaging.WmpBitmapEncoder"/>
    [NotNull]
    public static Bitmap ToBitmap([NotNull] this BitmapSource bitmapSource,
                                  [NotNull] BitmapEncoder bitmapEncoder)
    {
      if (bitmapSource == null)
      {
        throw new ArgumentNullException(nameof(bitmapSource));
      }
      if (bitmapEncoder == null)
      {
        throw new ArgumentNullException(nameof(bitmapEncoder));
      }

      var bitmapFrame = BitmapFrame.Create(bitmapSource);
      bitmapEncoder.Frames.Add(bitmapFrame);

      Bitmap result;
      using (var memoryStream = new MemoryStream())
      {
        bitmapEncoder.Save(memoryStream);

        memoryStream.Seek(0L,
                          SeekOrigin.Begin);

        result = new Bitmap(memoryStream);
      }

      return result;
    }
  }
}
