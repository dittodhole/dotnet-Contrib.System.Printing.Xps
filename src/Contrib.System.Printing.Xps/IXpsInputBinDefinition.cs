using JetBrains.Annotations;

namespace Contrib.System.Printing.Xps
{
  public interface IXpsInputBinDefinition : IHasMedia
  {
    [NotNull]
    string FullName { get; }

    [NotNull]
    string Name { get; }

    [NotNull]
    string DisplayName { get; }

    [NotNull]
    string FeatureName { get; }

    [NotNull]
    string NamespacePrefix { get; }

    [NotNull]
    string NamespaceUri { get; }
  }
}
