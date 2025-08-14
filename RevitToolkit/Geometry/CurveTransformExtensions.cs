namespace RevitToolkit.Geometry;
public static class CurveTransformExtensions
{
  public static TCurve CreateTransformed<TCurve>( this TCurve curve, Transform transform ) where TCurve : Curve
  {
    return ( TCurve ) curve.CreateTransformed( transform );
  }

  public static TCurve CreateOffset<TCurve>( this TCurve curve, double offsetDist, XYZ referenceVector ) where TCurve : Curve
  {
    return ( TCurve ) curve.CreateOffset( offsetDist, referenceVector );
  }

  public static TCurve Translate<TCurve>( this TCurve curve, double dx, double dy, double dz ) where TCurve : Curve
  {
    return ( TCurve ) curve.CreateTransformed( Transform.CreateTranslation( dx * XYZ.BasisX + dy * XYZ.BasisY + dz * XYZ.BasisZ ) );
  }

  public static TCurve Translate<TCurve>( this TCurve curve, XYZ vector ) where TCurve : Curve
  {
    return ( TCurve ) curve.CreateTransformed( Transform.CreateTranslation( vector ) );
  }

  public static TCurve TranslateAlongXAxis<TCurve>( this TCurve curve, double dx ) where TCurve : Curve
  {
    return ( TCurve ) curve.CreateTransformed( Transform.CreateTranslation( dx * XYZ.BasisX ) );
  }

  public static TCurve TranslateAlongYAxis<TCurve>( this TCurve curve, double dy ) where TCurve : Curve
  {
    return ( TCurve ) curve.CreateTransformed( Transform.CreateTranslation( dy * XYZ.BasisY ) );
  }

  public static TCurve TranslateAlongZAxis<TCurve>( this TCurve curve, double dz ) where TCurve : Curve
  {
    return ( TCurve ) curve.CreateTransformed( Transform.CreateTranslation( dz * XYZ.BasisZ ) );
  }

  public static TCurve CreateRotation<TCurve>( this TCurve curve, XYZ axis, double angle ) where TCurve : Curve
  {
    return ( TCurve ) curve.CreateTransformed( Transform.CreateRotation( axis, angle ) );
  }

  public static TCurve CreateMirror<TCurve>( this TCurve curve, Plane plane ) where TCurve : Curve
  {
    return ( TCurve ) curve.CreateTransformed( Transform.CreateReflection( plane ) );
  }
}
