namespace AcadToolkit.Helpers;
public static class Entity2dExtensions
{
  public static T Mirror<T>( this T entity, Line2d axis, bool createNew = false ) where T : Entity2d
    => Transform( entity, Matrix2d.Mirroring( axis ), createNew );

  public static T Transform<T>( this T entity, Matrix2d matrix, bool createNew = false ) where T : Entity2d
  {
    if ( createNew ) {
      T newEntity = ( T ) entity.Clone();
      newEntity.TransformBy( matrix );
      return newEntity;
    }
    else {
      entity.TransformBy( matrix );
      return entity;
    }
  }
}
