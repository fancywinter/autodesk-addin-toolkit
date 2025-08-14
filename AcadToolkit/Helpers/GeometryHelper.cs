namespace AcadToolkit.Helpers;

public static class GeometryHelper
{
  public static Line2d NewVerticalLine2d( double px, double py ) => new Line2d( new Point2d( px, py ), Vector2d.YAxis );

  public static Line2d NewHorizontalLine2d( double px, double py ) => new Line2d( new Point2d( px, py ), Vector2d.XAxis );

  public static Vector2d NewUnitVector2d( double slope ) => new Vector2d( 1, slope ).GetNormal();

  public static LineSegment3d NewLineSegment3d( Point3d startPoint, Vector3d direction, double length )
    => new LineSegment3d( startPoint, startPoint + direction * length );

  public static LineSegment3d NewLineSegment3dAlongXAxis( Point3d startPoint, double length )
    => NewLineSegment3d( startPoint, Vector3d.XAxis, length );

  public static LineSegment3d NewLineSegment3dAlongYAxis( Point3d startPoint, double length )
    => NewLineSegment3d( startPoint, Vector3d.YAxis, length );
}