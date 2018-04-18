using System;
using System.Printing;
using System.Reflection;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace Contrib.System.Printing.Xps
{
  [PublicAPI]
  public static partial class UnitConverter
  {
    /// <exception cref="Exception" />
    [NotNull]
    private static Type FindInternalType()
    {
      var unitConverterType = new PrintTicket().GetType()
                                               .Assembly.GetType("MS.Internal.Printing.Configuration.UnitConverter");

      return unitConverterType;
    }

    /// <exception cref="Exception" />
    [NotNull]
    private static MethodInfo FindInternalMethod([CallerMemberName] string callerMemberName = "")
    {
      var unitConverterType = UnitConverter.FindInternalType();
      var methodInfo = unitConverterType.GetMethod(callerMemberName);

      return methodInfo;
    }

    /// <exception cref="Exception" />
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

    /// <exception cref="Exception" />
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
