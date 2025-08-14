namespace RevitToolkit.Geometry;
public static class LineFactory
{
  public static Line CreateLineAlongXAxis( XYZ startPoint, double length )
  {
    return Line.CreateBound( startPoint, startPoint.TranslateAlongXAxis( length ) );
  }

  public static Line CreateLineAlongYAxis( XYZ startPoint, double length )
  {
    return Line.CreateBound( startPoint, startPoint.TranslateAlongYAxis( length ) );
  }

  public static Line CreateLineAlongZAxis( XYZ startPoint, double length )
  {
    return Line.CreateBound( startPoint, startPoint.TranslateAlongZAxis( length ) );
  }

  public static Line CreateUnboundLineAlongZAxis( XYZ startPoint )
  {
    return Line.CreateUnbound( startPoint, XYZ.BasisZ );
  }
}
