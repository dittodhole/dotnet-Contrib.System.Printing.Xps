/** @pp
 * rootnamespace: Contrib.System
 */
namespace Contrib.System.Printing.Xps
{
  using global::System;
  using global::System.Printing;
  using global::System.Reflection;
  using global::System.Runtime.CompilerServices;
  using global::JetBrains.Annotations;

  // ReSharper disable InconsistentNaming

  /// <summary>
  ///   A transparent proxy for <see cref="T:MS.Internal.Printing.Configuration.UnitConverter"/>.
  /// </summary>
  [PublicAPI]
#if CONTRIB_SYSTEM_PRINTING_XPS
  public
#else
  internal
#endif
  static partial class UnitConverter
  {
    /// <exception cref="T:System.Exception"/>
    [NotNull]
    private static Type FindInternalType()
    {
      var unitConverterType = typeof(PrintTicket).Assembly.GetType("MS.Internal.Printing.Configuration.UnitConverter");

      return unitConverterType;
    }

    /// <exception cref="T:System.Exception"/>
    [NotNull]
    private static MethodInfo FindInternalMethod([CallerMemberName] string callerMemberName = "")
    {
      var unitConverterType = UnitConverter.FindInternalType();
      var methodInfo = unitConverterType.GetMethod(callerMemberName);

      return methodInfo;
    }

    /// <summary>
    ///   Converts micron to device-independent pixels.
    /// </summary>
    /// <param name="micronValue"/>
    /// <seealso cref="M:MS.Internal.Printing.Configuration.UnitConverter.LengthValueFromMicronToDIP(int)"/>
    /// <exception cref="T:System.Exception"/>
    public static double LengthValueFromMicronToDIP(int micronValue)
    {
      var methodInfo = UnitConverter.FindInternalMethod();
      var result = methodInfo.Invoke(null,
                                     new object[]
                                     {
                                       micronValue
                                     });

      return (double) result;
    }

    /// <summary>
    ///   Converts device-independent pixels to micron.
    /// </summary>
    /// <param name="dipValue"/>
    /// <seealso cref="M:MS.Internal.Printing.Configuration.UnitConverter.LengthValueFromDIPToMicron(double)"/>
    /// <exception cref="T:System.Exception"/>
    public static int LengthValueFromDIPToMicron(double dipValue)
    {
      var methodInfo = UnitConverter.FindInternalMethod();
      var result = methodInfo.Invoke(null,
                                     new object[]
                                     {
                                       dipValue
                                     });

      return (int) result;
    }
  }
}
