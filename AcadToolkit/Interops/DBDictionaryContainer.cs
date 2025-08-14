namespace AcadToolkit.Interops;

public sealed class DBDictionaryContainer : NamedContainerBase<DBObject>
{
  public DBDictionaryContainer( Transaction transaction, ObjectId containerId )
    : base( transaction, containerId )
  {
  }

  private DBDictionary GetStorage( OpenMode openMode = OpenMode.ForRead ) => ( DBDictionary ) GetContainer( openMode );

  public override ObjectId this[ string name ] => GetStorage().GetAt( name );

  public override bool Contains( string name )
  {
    DBDictionary dic = GetStorage();
    return dic.Contains( name );
  }

  protected override ObjectId GetObjectId( object iteratorItem )
  {
    return ( ( DBDictionaryEntry ) iteratorItem ).Value;
  }

  public override void Register( string name, DBObject item )
  {
    if ( !IsValidName( name ) || Contains( name ) )
      return;

    DBDictionary dic = GetStorage( OpenMode.ForWrite );
    dic.SetAt( name, item );
    Transaction.AddNewlyCreatedDBObject( item, true );
  }

  public ObjectId? Remove( string name )
  {
    DBDictionary dic = GetStorage( OpenMode.ForWrite );
    if ( !dic.Contains( name ) )
      return null;
    return dic.Remove( name );
  }
}
