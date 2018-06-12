namespace Contrib.System.Printing.Xps.Tests
{
  using global::NUnit.Framework;

  [TestFixture]
  public class UnitConverterTests
  {
    [Test]
    public void LengthValueFromMicronToDIP_Should_Succeed()
    {
      var value = UnitConverter.LengthValueFromMicronToDIP(100);

      Assert.IsTrue(value > double.Epsilon);
    }

    [Test]
    public void LengthValueFromDIPToMicron_Should_Succeed()
    {
      var value = UnitConverter.LengthValueFromDIPToMicron(100d);

      Assert.IsTrue(value > 0);
    }
  }
}
