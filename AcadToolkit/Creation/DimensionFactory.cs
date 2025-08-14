using AcadToolkit.Interops;

namespace AcadToolkit.Creation;

public sealed class DimensionFactory : EntityFactory<RotatedDimension>
{
  private string _style;
  private Point3d _xLine1Point;
  private Point3d _xLine2Point;
  private double _rotation;
  private Point3d _dimLinePoint;
  private Point3d? _textPosition;
  private string? _textOverride;

  public DimensionFactory( string style )
  {
    _style = style;
  }

  public static DimensionFactory New( string style ) => new DimensionFactory( style );

  public DimensionFactory XLine1Point( Point3d point )
  {
    _xLine1Point = point;
    return this;
  }

  public DimensionFactory XLine2Point( Point3d point )
  {
    _xLine2Point = point;
    return this;
  }

  public DimensionFactory DimLinePoint( Point3d point )
  {
    _dimLinePoint = point;
    return this;
  }

  public DimensionFactory Rotate( double angle )
  {
    _rotation = angle;
    return this;
  }

  public DimensionFactory MoveTextPosition( Point3d point )
  {
    _textPosition = point;
    return this;
  }

  public DimensionFactory OverrideText( string text )
  {
    _textOverride = text;
    return this;
  }

  protected override RotatedDimension InitializeEntity( TransactionScope scope )
  {
    RotatedDimension dim = new();
    dim.Rotation = _rotation;
    dim.XLine1Point = _xLine1Point;
    dim.XLine2Point = _xLine2Point;
    dim.DimLinePoint = _dimLinePoint;

    if ( scope.DimStyles.TryGetId( _style, out ObjectId styleId ) )
      dim.DimensionStyle = styleId;

    if ( _textPosition != null )
      dim.TextPosition = _textPosition.Value;

    if ( _textOverride != null )
      dim.DimensionText = _textOverride;

    return dim;
  }
}
