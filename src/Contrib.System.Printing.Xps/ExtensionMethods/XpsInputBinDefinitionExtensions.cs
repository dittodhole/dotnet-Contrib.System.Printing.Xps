﻿using System;
using System.Printing;
using JetBrains.Annotations;

namespace Contrib.System.Printing.Xps.ExtensionMethods
{
  public static class XpsInputBinDefinitionExtensions
  {
    /// <exception cref="Exception" />
    [Pure]
    [NotNull]
    public static PrintTicket GetPrintTicket([NotNull] this IXpsInputBinDefinition xpsInputBinDefinition)
    {
      var featureName = xpsInputBinDefinition.FeatureName;
      var inputBinName = xpsInputBinDefinition.Name;
      var namespacePrefix = xpsInputBinDefinition.NamespacePrefix;
      var namespaceUri = xpsInputBinDefinition.NamespaceUri;

      var result = PrintTicketExtensions.GetPrintTicket(featureName,
                                                        inputBinName,
                                                        namespacePrefix,
                                                        namespaceUri);

      return result;
    }

    /// <exception cref="ArgumentNullException"><paramref name="xpsInputBinDefinition" /> is <see langword="null" />.</exception>
    [Pure]
    public static InputBin GetInputBin([NotNull] this IXpsInputBinDefinition xpsInputBinDefinition)
    {
      if (xpsInputBinDefinition == null)
      {
        throw new ArgumentNullException(nameof(xpsInputBinDefinition));
      }

      var name = xpsInputBinDefinition.Name;

      const string namespacePrefix = "psk:";
      if (name.StartsWith(namespacePrefix,
                          StringComparison.Ordinal))
      {
        name = name.Substring(namespacePrefix.Length);
      }

      if (!Enum.TryParse(name,
                         out InputBin result))
      {
        result = InputBin.Unknown;
      }

      return result;
    }
  }
}
