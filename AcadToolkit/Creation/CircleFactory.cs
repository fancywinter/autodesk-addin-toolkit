using AcadToolkit.Interops;

namespace AcadToolkit.Creation;

public sealed class CircleFactory : EntityFactory<Circle>
{
  private Point3d _center;
  private double _radius;

  public CircleFactory( Point3d center, double radius )
  {
    _center = center;
    _radius = radius;
  }

  public static CircleFactory New( Point3d center, double radius ) => new CircleFactory( center, radius );

  protected override Circle InitializeEntity( TransactionScope scope ) => new Circle() { Center = _center, Radius = _radius };
}
