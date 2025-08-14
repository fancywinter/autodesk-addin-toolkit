namespace AcadToolkit.Helpers;

public static class LineExtensions
{
  public static LineSegment3d GetGeLineSegment3d( this Line line )
  {
    return ( LineSegment3d ) line.GetGeCurve();
  }

  public static Line3d GetGeLine3d( this Line line )
  {
    return new( line.StartPoint, ( line.EndPoint - line.StartPoint ).GetNormal() );
  }
}
