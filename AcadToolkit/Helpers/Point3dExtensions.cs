namespace AcadToolkit.Helpers;

public static class Point3dExtensions
{
  public static Point3d Translate( this Point3d point, double dx, double dy, double dz )
    => point.TransformBy( Matrix3d.Displacement( new Vector3d( dx, dy, dz ) ) );

  public static Point3d TranslateAlongXAxis( this Point3d point, double dx )
    => point.TransformBy( Matrix3d.Displacement( new Vector3d( dx, 0, 0 ) ) );

  public static Point3d TranslateAlongYAxis( this Point3d point, double dy )
    => point.TransformBy( Matrix3d.Displacement( new Vector3d( 0, dy, 0 ) ) );

  public static Point3d Mirror( this Point3d point, Line3d axis )
    => point.TransformBy( Matrix3d.Mirroring( axis ) );
}