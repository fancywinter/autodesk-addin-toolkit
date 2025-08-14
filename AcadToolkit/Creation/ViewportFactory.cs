using AcadToolkit.Interops;

namespace AcadToolkit.Creation;

public sealed class ViewportFactory : EntityFactory<Viewport>
{
  private Point3d _centerPoint;
  private Extents3d _bounds;
  private double? _extendBounds;
  private bool _useCustomScale;
  private double _customScale;
  private StandardScaleType _standardScaleType;

  public ViewportFactory( Extents3d bounds )
  {
    _bounds = bounds;
    _useCustomScale = false;
    _customScale = 1.0;
    _standardScaleType = StandardScaleType.Scale1To1;
  }

  public static ViewportFactory New( Extents3d bounds ) => new ViewportFactory( bounds );

  public ViewportFactory CustomScale( double scale )
  {
    _useCustomScale = true;
    _customScale = scale;
    return this;
  }

  public ViewportFactory TopLeftLocation( Point3d point )
  {
    _centerPoint = point;
    return this;
  }

  public ViewportFactory CenterPoint( Point3d point )
  {
    _centerPoint = point;
    return this;
  }

  public ViewportFactory ExtendBounds( double value )
  {
    _extendBounds = value;
    return this;
  }

  public ViewportFactory StandardScale( StandardScaleType standardScale )
  {
    _useCustomScale = false;
    _standardScaleType = standardScale;
    return this;
  }

  private double GetScaleFactor()
  {
    if ( _useCustomScale )
      return _customScale;
    else
      switch ( _standardScaleType ) {
        case StandardScaleType.Scale1To1:
          return 1;
        case StandardScaleType.Scale1To2:
          return 1d / 2;
        case StandardScaleType.Scale1To4:
          return 1d / 4;
        case StandardScaleType.Scale1To5:
          return 1d / 5;
        case StandardScaleType.Scale1To8:
          return 1d / 8;
        case StandardScaleType.Scale1To10:
          return 1d / 10;
        case StandardScaleType.Scale1To16:
          return 1d / 16;
        case StandardScaleType.Scale1To20:
          return 1d / 20;
        case StandardScaleType.Scale1To30:
          return 1d / 30;
        case StandardScaleType.Scale1To40:
          return 1d / 40;
        case StandardScaleType.Scale1To50:
          return 1d / 50;
        case StandardScaleType.Scale1To100:
          return 1d / 100;
        case StandardScaleType.Scale2To1:
          return 2;
        case StandardScaleType.Scale4To1:
          return 4;
        case StandardScaleType.Scale8To1:
          return 8;
        case StandardScaleType.Scale10To1:
          return 10;
        case StandardScaleType.Scale100To1:
          return 100;
      }
    return 1;
  }

  protected override Viewport InitializeEntity( TransactionScope scope )
  {
    Viewport viewport = new Viewport();
    viewport.CenterPoint = _centerPoint;
    viewport.Width = ( _bounds.MaxPoint.X - _bounds.MinPoint.X ) * GetScaleFactor();
    viewport.Height = ( _bounds.MaxPoint.Y - _bounds.MinPoint.Y ) * GetScaleFactor();
    if ( _extendBounds != null ) {
      viewport.Width += _extendBounds.Value;
      viewport.Height += _extendBounds.Value;
    }
    viewport.ViewTarget = new Point3d( ( _bounds.MinPoint.X + _bounds.MaxPoint.X ) / 2, ( _bounds.MinPoint.Y + _bounds.MaxPoint.Y ) / 2, 0 );
    if ( _useCustomScale )
      viewport.CustomScale = _customScale;
    else
      viewport.StandardScale = StandardScaleType.Scale1To50;
    return viewport;
  }

  protected override void OnEntityCreated( Viewport entity ) => entity.On = true;
}
