namespace AcadToolkit.Interops;

public abstract class NamedFactoryBase<T> : AbstractNamedContainerBase<T> where T : DBObject
{
  protected NamedFactoryBase( Transaction transaction, ObjectId containerId )
    : base( transaction, containerId )
  {
  }

  protected abstract T CreateItem();

  protected abstract void AddItem( string name, T item );

  public T? Create( string name )
  {
    if ( !IsValidName( name ) || Contains( name ) )
      return null;

    T newItem = CreateItem();
    AddItem( name, newItem );
    Transaction.AddNewlyCreatedDBObject( newItem, true );
    return newItem;
  }
}
