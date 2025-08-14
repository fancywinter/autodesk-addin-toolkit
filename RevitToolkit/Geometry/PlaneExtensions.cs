namespace RevitToolkit.Geometry;
public static class PlaneExtensions
{
  public static XYZ Project( this Plane plane, XYZ point, out double distance )
  {
    plane.Project( point, out UV uv1, out distance );
    return plane.Origin + ( uv1.U * plane.XVec ) + ( uv1.V * plane.YVec );
  }

  public static bool Intersect( Line line, Plane plane, [NotNullWhen( true )] out XYZ? point )
  {
    XYZ p1 = plane.Project( line.Origin, out _ );
    XYZ p2 = plane.Project( line.Origin + line.Direction, out _ );

    if ( p1.IsAlmostEqualTo( p2 ) ) {
      point = p1;
      return true;
    }

    Line projectedLine = Line.CreateUnbound( p1, ( p2 - p1 ).Normalize() );

    line.Intersect( projectedLine, out XYZ? p3 );
    point = p3;
    return p3 != null;
  }
}
