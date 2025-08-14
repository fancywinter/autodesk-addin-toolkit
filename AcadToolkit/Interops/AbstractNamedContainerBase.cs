using System.Collections;

namespace AcadToolkit.Interops;

public abstract class AbstractNamedContainerBase<T> : ContainerBase<T> where T : DBObject
{
  protected AbstractNamedContainerBase( Transaction transaction, ObjectId containerId )
    : base( transaction, containerId )
  {
  }

  protected abstract ObjectId GetObjectId( object iteratorItem );

  public override IEnumerable<ObjectId> ObjectIds
  {
    get
    {
      foreach ( object entry in GetObjects() ) {
        yield return GetObjectId( entry );
      }
    }
  }

  public virtual int Count() => GetObjects().Count();

  private IEnumerable<object> GetObjects()
    => ( ( IEnumerable ) GetContainer() ).Cast<object>();

  public bool IsValidName( string name )
  {
    try {
      SymbolUtilityServices.ValidateSymbolName( name, false );
      return true;
    }
    catch {
      return false;
    }
  }

  public abstract bool Contains( string name );

  public abstract ObjectId this[ string name ] { get; }

  public bool TryGetId( string name, out ObjectId id )
  {
    if ( Contains( name ) ) {
      id = this[ name ];
      return true;
    }
    id = ObjectId.Null;
    return false;
  }

  public bool TryGetObject( string name, OpenMode openMode, [NotNullWhen( true )] out T? obj )
  {
    obj = default;
    if ( TryGetId( name, out ObjectId id ) ) {
      obj = GetObject( id, openMode );
      return true;
    }
    return false;
  }
}
