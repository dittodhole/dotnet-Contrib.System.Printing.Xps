namespace Contrib.System.Printing.Xps
{
  /// <remarks>Copied from MS.Internal.Printing.Configuration.UnitConverter, ReachFramework</remarks>
  public class UnitConverter
  {
    private UnitConverter()
    {
    }

    public static double LengthValueFromMicronToDIP(int micronValue)
    {
      if (micronValue == int.MinValue)
        return double.MinValue;
      return (double) micronValue / 25400.0 * 96.0;
    }

    public static int LengthValueFromDIPToMicron(double dipValue)
    {
      return (int) (dipValue / 96.0 * 25400.0 + 0.5);
    }
  }
}
