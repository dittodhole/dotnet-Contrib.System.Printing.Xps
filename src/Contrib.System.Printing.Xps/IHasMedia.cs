using JetBrains.Annotations;

namespace Contrib.System.Printing.Xps
{
  public interface IHasMedia
  {
    [CanBeNull]
    double? MediaWidth { get; }

    [CanBeNull]
    double? MediaHeight { get; }
  }
}
