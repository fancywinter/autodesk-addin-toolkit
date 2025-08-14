using AcadToolkit.Interops;

namespace AcadToolkit.Creation;

public sealed class LineFactory : EntityFactory<Line>
{
  private Point3d _startPoint;
  private Point3d _endPoint;

  public LineFactory()
  {
    _startPoint = new Point3d();
    _endPoint = new Point3d();
  }

  public static LineFactory New() => new LineFactory();

  public LineFactory StartPoint( double x, double y )
  {
    _startPoint = new Point3d( x, y, 0 );
    return this;
  }

  public LineFactory EndPoint( double x, double y )
  {
    _endPoint = new Point3d( x, y, 0 );
    return this;
  }

  public LineFactory StartPoint( Point3d point )
  {
    _startPoint = point;
    return this;
  }

  public LineFactory EndPoint( Point3d point )
  {
    _endPoint = point;
    return this;
  }

  protected override Line InitializeEntity( TransactionScope scope ) => new Line() { StartPoint = _startPoint, EndPoint = _endPoint };
}
