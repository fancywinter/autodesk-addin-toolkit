namespace RevitToolkit.Helper;
public static class LocationExtensions
{
  public static XYZ GetLocationPoint( this FamilyInstance instance )
  {
    return ( ( LocationPoint ) instance.Location ).Point;
  }

  public static double GetRotation( this FamilyInstance instance )
  {
    return ( ( LocationPoint ) instance.Location ).Rotation;
  }

  public static bool TryGetLocationPoint( this FamilyInstance instance, [NotNullWhen( true )] out XYZ? point )
  {
    point = null;
    if ( instance.Location is LocationPoint loc ) {
      point = loc.Point;
    }
    return point != null;
  }

  public static T GetLocationCurve<T>( this FamilyInstance instance ) where T : Curve
  {
    return ( T ) ( ( LocationCurve ) instance.Location ).Curve;
  }

  public static bool TryGetLocationCurve<T>( this FamilyInstance instance, [NotNullWhen( true )] out T? curve )
     where T : Curve
  {
    curve = default;
    if ( instance.Location is LocationCurve loc ) {
      curve = loc.Curve as T;
    }
    return curve != null;
  }
}
