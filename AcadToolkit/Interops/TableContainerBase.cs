namespace AcadToolkit.Interops;

public abstract class TableContainerBase<T> : NamedFactoryBase<T> where T : SymbolTableRecord
{
  protected TableContainerBase( Transaction transaction, ObjectId containerId )
    : base( transaction, containerId )
  {
  }

  private SymbolTable GetSymbolTable( OpenMode openMode = OpenMode.ForRead ) => ( SymbolTable ) GetContainer( openMode );

  protected sealed override ObjectId GetObjectId( object iteratorItem ) => ( ObjectId ) iteratorItem;

  public sealed override bool Contains( ObjectId id ) => GetSymbolTable().Has( id );

  public sealed override bool Contains( string name ) => GetSymbolTable().Has( name );

  public sealed override ObjectId this[ string name ] => GetSymbolTable()[ name ];

  protected sealed override void AddItem( string name, T item )
  {
    item.Name = name;
    GetSymbolTable( OpenMode.ForWrite ).Add( item );
  }
}
