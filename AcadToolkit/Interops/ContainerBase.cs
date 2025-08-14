using System.Collections;

namespace AcadToolkit.Interops;

public abstract class ContainerBase<T> : IEnumerable<T> where T : DBObject
{
  protected Transaction Transaction { get; init; }
  public ObjectId ContainerId { get; init; }

  protected ContainerBase( Transaction transaction, ObjectId containerId )
  {
    Transaction = transaction;
    ContainerId = containerId;
  }

  public IEnumerator<T> GetEnumerator()
  {
    foreach ( ObjectId id in ObjectIds ) {
      yield return GetObject( id );
    }
  }

  public T GetObject( ObjectId id, OpenMode openMode = OpenMode.ForRead ) => ( T ) Transaction.GetObject( id, openMode );

  IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

  public abstract IEnumerable<ObjectId> ObjectIds { get; }

  public bool Contains( T value ) => Contains( value.ObjectId );

  public virtual bool Contains( ObjectId id ) => ObjectIds.Contains( id );

  protected DBObject GetContainer( OpenMode openMode = OpenMode.ForRead ) => Transaction.GetObject( ContainerId, openMode );
}
