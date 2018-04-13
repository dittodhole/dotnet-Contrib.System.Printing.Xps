using System;
using System.Linq;
using System.Printing;
using System.Reflection;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace Contrib.System.Printing.Xps
{
  public static class UnitConverter
  {
    [NotNull]
    private static Type FindInternalType()
    {
      var printTicket = new PrintTicket(); // to force loading of ReachFramework
      var unitConverterType = AppDomain.CurrentDomain.GetAssemblies()
                                       .Select(assembly => assembly.GetType("MS.Internal.Printing.Configuration.UnitConverter"))
                                       .FirstOrDefault(type => type != null);

      return unitConverterType;
    }

    [NotNull]
    private static MethodInfo FindInternalMethod([CallerMemberName] string callerMemberName = "")
    {
      var unitConverterType = UnitConverter.FindInternalType();
      var methodInfo = unitConverterType.GetMethod(callerMemberName);

      return methodInfo;
    }

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
