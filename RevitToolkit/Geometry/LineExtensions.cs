namespace RevitToolkit.Geometry;
public static class LineExtensions
{
  public static Line StretchEnds( this Line line, double length1, double length2 )
  {
    return Line.CreateBound(
      line.GetEndPoint( 0 ) - line.Direction * length1,
      line.GetEndPoint( 1 ) + line.Direction * length2 );
  }

  public static XYZ GetLowerPoint( this Line line )
  {
    XYZ point0 = line.GetEndPoint( 0 );
    XYZ point1 = line.GetEndPoint( 1 );
    return point0.Z < point1.Z ? point0 : point1;
  }

  public static XYZ GetHigherPoint( this Line line )
  {
    XYZ point0 = line.GetEndPoint( 0 );
    XYZ point1 = line.GetEndPoint( 1 );
    return point0.Z > point1.Z ? point0 : point1;
  }

  public static XYZ GetStartPoint( this Line line )
  {
    return line.GetEndPoint( 0 );
  }

  public static XYZ GetEndPoint( this Line line )
  {
    return line.GetEndPoint( 1 );
  }

  public static bool HasEndPoint( this Curve curve, XYZ point )
  {
    return curve.GetEndPoint( 0 ).IsAlmostEqualTo( point )
      || curve.GetEndPoint( 1 ).IsAlmostEqualTo( point );
  }

  public static XYZ GetCenterPoint( this Line line )
  {
    return line.Evaluate( 0.5, true );
  }

  public static bool Intersect( this Line left, Line right, [NotNullWhen( true )] out XYZ? point )
  {
    point = null;
    SetComparisonResult setComparison = left.Intersect( right, out IntersectionResultArray result );
    if ( setComparison != SetComparisonResult.Disjoint ) {
      point = result.get_Item( 0 ).XYZPoint;
      return true;
    }
    return false;
  }

  public static Line GetUnboundLine( this Line line )
  {
    return line.IsBound ? Line.CreateUnbound( line.Origin, line.Direction ) : line;
  }
}
