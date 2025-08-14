using AcadToolkit.Interops;

namespace AcadToolkit.Helpers;

public static class EntityHelper
{
  public static ObjectIdCollection ArrayPath( TransactionScope scope, ObjectId entityId, Point3d startPoint, Point3d endPoint, int number )
  {
    double spacing = startPoint.DistanceTo( endPoint ) / ( number - 1 );
    Vector3d direction = ( endPoint - startPoint ).GetNormal();
    return ArrayPath( scope, entityId, direction, number, spacing );
  }

  public static ObjectIdCollection ArrayPath( TransactionScope scope, ObjectId entityId, Vector3d direction, int number, double spacing )
  {
    Vector3d dispVector = direction * spacing;

    ObjectIdCollection objectIds = new() { entityId };
    number--;
    for ( int i = 0; i < number; i++ ) {
      ObjectId _ = Copy( scope, objectIds[ ^1 ], dispVector );
      objectIds.Add( _ );
    }

    return objectIds;
  }

  public static void Erase( TransactionScope scope, ObjectId entityId ) => scope.ModelSpace.GetObject( entityId, OpenMode.ForWrite ).Erase();

  public static ObjectId Copy( TransactionScope scope, ObjectId entityId, Vector3d displacement )
  {
    using Entity clone = ( Entity ) scope.ModelSpace.GetObject( entityId ).Clone();
    clone.TransformBy( Matrix3d.Displacement( displacement ) );
    return scope.ModelSpace.Add( clone );
  }

  public static ObjectId Copy( TransactionScope scope, ObjectId entityId, double dx, double dy, double dz )
  {
    using Entity clone = ( Entity ) scope.ModelSpace.GetObject( entityId ).Clone();
    clone.TransformBy( Matrix3d.Displacement( new Vector3d( dx, dy, dz ) ) );
    return scope.ModelSpace.Add( clone );
  }

  public static void Transform( TransactionScope scope, ObjectId entityId, Matrix3d transform )
  {
    using Entity entity = scope.ModelSpace.GetObject( entityId, OpenMode.ForWrite );
    entity.TransformBy( transform );
  }

  public static void Move( TransactionScope scope, ObjectId entityId, Vector3d displacement )
  {
    Transform( scope, entityId, Matrix3d.Displacement( displacement ) );
  }

  public static void Rotate( TransactionScope scope, ObjectId entityId, double angle, Vector3d axis, Point3d center )
  {
    Transform( scope, entityId, Matrix3d.Rotation( angle, axis, center ) );
  }

  public static void Mirror( TransactionScope scope, ObjectId entityId, Line3d axis )
  {
    Transform( scope, entityId, Matrix3d.Mirroring( axis ) );
  }

  public static ObjectId CopyMirror( TransactionScope scope, ObjectId entityId, Line3d axis )
  {
    using Entity clone = ( Entity ) scope.ModelSpace.GetObject( entityId ).Clone();
    clone.TransformBy( Matrix3d.Mirroring( axis ) );
    return scope.ModelSpace.Add( clone );
  }
}
