namespace RevitToolkit.Helper;

internal static class NumericExtensions
{
  /// <summary>
  /// Compare to double-precision floating-point number.
  /// </summary>
  /// <param name="left"></param>
  /// <param name="right"></param>
  /// <param name="tolerance"></param>
  /// <returns></returns>
  internal static bool IsAlmostEqualsTo( this double left, double right, double tolerance = 1e-8 )
    => Math.Abs( left - right ) < tolerance;

  internal static bool IsAlmostEqualsToZero( this double value, double tolerance = 1e-8 )
    => Math.Abs( value ) < tolerance;
}