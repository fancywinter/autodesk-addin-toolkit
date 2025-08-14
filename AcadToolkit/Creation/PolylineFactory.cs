using AcadToolkit.Interops;

namespace AcadToolkit.Creation;

public sealed class PolylineFactory : EntityFactory<Polyline>
{
  private readonly List<Point2d> _vertices = new();
  private bool _isClosed;

  public static PolylineFactory New() => new PolylineFactory();

  public static PolylineFactory NewSquare( Point2d center, double breadth )
    => new PolylineFactory()
      .AddVertex( center.X - breadth / 2, center.Y - breadth / 2 )
      .AddVertex( center.X - breadth / 2, center.Y + breadth / 2 )
      .AddVertex( center.X + breadth / 2, center.Y + breadth / 2 )
      .AddVertex( center.X + breadth / 2, center.Y - breadth / 2 )
      .Close();

  public static PolylineFactory NewSquare2( Point2d bottomLeft, double breadth )
    => new PolylineFactory()
      .AddVertex( bottomLeft.X, bottomLeft.Y )
      .AddVertex( bottomLeft.X, bottomLeft.Y + breadth )
      .AddVertex( bottomLeft.X + breadth, bottomLeft.Y + breadth )
      .AddVertex( bottomLeft.X + breadth, bottomLeft.Y )
      .Close();

  public static PolylineFactory NewRectangle( Point2d center, double width, double length )
    => new PolylineFactory()
    .AddVertex( center.X - length / 2, center.Y - width / 2 )
    .AddVertex( center.X - length / 2, center.Y + width / 2 )
    .AddVertex( center.X + length / 2, center.Y + width / 2 )
    .AddVertex( center.X + length / 2, center.Y - width / 2 )
    .Close();

  public PolylineFactory MoveTo( double dx, double dy )
  {
    _vertices.Add( _vertices[ ^1 ].Add( Vector2d.XAxis * dx ).Add( Vector2d.YAxis * dy ) );
    return this;
  }

  public PolylineFactory MoveUp( double distance )
  {
    MoveTo( 0, distance );
    return this;
  }

  public PolylineFactory MoveDown( double distance )
  {
    MoveTo( 0, -distance );
    return this;
  }

  public PolylineFactory MoveLeft( double distance )
  {
    MoveTo( -distance, 0 );
    return this;
  }

  public PolylineFactory MoveRight( double distance )
  {
    MoveTo( distance, 0 );
    return this;
  }

  public PolylineFactory AddVertex( double x, double y )
  {
    _vertices.Add( new Point2d( x, y ) );
    return this;
  }

  public PolylineFactory AddVertex( Point2d point )
  {
    _vertices.Add( point );
    return this;
  }

  public PolylineFactory AddVerties( params Point2d[] points )
  {
    _vertices.AddRange( points );
    return this;
  }

  public PolylineFactory AddVerties( IEnumerable<Point2d> points )
  {
    _vertices.AddRange( points );
    return this;
  }

  public PolylineFactory Close()
  {
    _isClosed = true;
    return this;
  }

  protected override Polyline InitializeEntity( TransactionScope scope )
  {
    Polyline polyline = new();
    for ( int i = 0; i < _vertices.Count; i++ ) {
      polyline.AddVertexAt( i, _vertices[ i ], 0, 0, 0 );
    }
    polyline.Closed = _isClosed;
    return polyline;
  }
}
