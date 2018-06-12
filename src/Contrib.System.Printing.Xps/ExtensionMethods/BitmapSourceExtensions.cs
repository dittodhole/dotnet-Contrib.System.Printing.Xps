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
    ///   Creates a <see cref="T:System.Windows.Media.Imaging.BitmapEncoder"/> object.
    /// </summary>
    [CanBeNull]
    public delegate BitmapEncoder BitmapEncoderFactory();

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
      var result = bitmapSource.ToBitmap(() => new GifBitmapEncoder());

      return result;
    }

    /// <summary>
    ///   Converts the <paramref name="bitmapSource"/> to <see cref="T:System.Drawing.Bitmap"/>.
    /// </summary>
    /// <param name="bitmapSource"/>
    /// <param name="bitmapEncoderFactory"/>
    /// <exception cref="ArgumentNullException"><paramref name="bitmapSource"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="bitmapEncoderFactory"/> is <see langword="null"/>.</exception>
    /// <exception cref="T:System.Exception"/>
    /// <seealso cref="T:System.Windows.Media.Imaging.BmpBitmapEncoder"/>
    /// <seealso cref="T:System.Windows.Media.Imaging.GifBitmapEncoder"/>
    /// <seealso cref="T:System.Windows.Media.Imaging.JpegBitmapEncoder"/>
    /// <seealso cref="T:System.Windows.Media.Imaging.PngBitmapEncoder"/>
    /// <seealso cref="T:System.Windows.Media.Imaging.TiffBitmapEncoder"/>
    /// <seealso cref="T:System.Windows.Media.Imaging.WmpBitmapEncoder"/>
    [NotNull]
    public static Bitmap ToBitmap([NotNull] this BitmapSource bitmapSource,
                                  [NotNull] BitmapEncoderFactory bitmapEncoderFactory)
    {
      if (bitmapSource == null)
      {
        throw new ArgumentNullException(nameof(bitmapSource));
      }
      if (bitmapEncoderFactory == null)
      {
        throw new ArgumentNullException(nameof(bitmapEncoderFactory));
      }

      Bitmap result;
      using (var memoryStream = new MemoryStream())
      {
        var bitmapFrame = BitmapFrame.Create(bitmapSource);

        var bitmapEncoder = bitmapEncoderFactory.Invoke();
        bitmapEncoder.Frames.Add(bitmapFrame);
        bitmapEncoder.Save(memoryStream);

        result = new Bitmap(memoryStream);
      }

      return result;
    }
  }
}
