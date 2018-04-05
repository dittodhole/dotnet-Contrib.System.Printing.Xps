using System.Xml.Linq;
using JetBrains.Annotations;

namespace Contrib.System.Printing.Xps
{
  internal interface IXpsGenericProperty
  {
    [NotNull]
    XName Type { get; }

    [NotNull]
    XName Name { get; }

    [NotNull]
    string Value { get; }
  }
}
