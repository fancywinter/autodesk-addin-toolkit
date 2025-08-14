namespace RevitToolkit.Helper;
public static class UnitExtensions
{
  public static double MmToFeet( this double value ) => value / 304.8;

  public static double MeterToFeet( this double value ) => value / 0.3048;

  public static double FeetToMm( this double value ) => value * 304.8;

  public static double FeetToMeter( this double value ) => value * 0.3048;

  public static double RadToDeg( this double value ) => value * 180.0 / Math.PI;

  public static double DegToRad( this double value ) => value * Math.PI / 180.0;

  public static double PercentSlopeToDeg( this double value ) => Math.Atan( value ).RadToDeg();

  public static double DegToPercentSlope( this double value ) => Math.Tan( value.DegToRad() );
}
