using Autodesk.Revit.DB.ExtensibleStorage;

namespace RevitToolkit.Storage;
public class DataStorageContext
{
  private Document _document;
  public DataStorageContext( Document document )
  {
    _document = document;
  }

  public void BatchSave<T>( IEnumerable<T> records ) where T : IStorable
  {
    foreach ( T record in records )
      Save( record );
  }

  public void Save<T>( T record ) where T : IStorable
  {
    Schema? schema = typeof( T ).GetSchema()
      ?? throw new InvalidOperationException( $"Schema not found." );
    RemoveStorageIfExists<T>( record.Id );
    SaveAsStorage( record );
  }

  public IEnumerable<T> GetAll<T>() where T : IStorable
  {
    return GetDataStorages().Select( storage => storage.GetObject<T>() ).OfType<T>();
  }

  private void RemoveStorageIfExists<T>( int id ) where T : IStorable
  {
    Schema? schema = typeof( T ).GetSchema()
      ?? throw new InvalidOperationException( $"Schema not found." );
    Field fieldId = schema.GetField( nameof( IStorable.Id ) );
    RemoveStoragesWhich<T>( e => e.HasFieldValue( fieldId, id ) );
  }

  public void RemoveStoragesWhich<T>( Func<Entity, bool> predicate ) where T : IStorable
  {
    Schema? schema = typeof( T ).GetSchema()
      ?? throw new InvalidOperationException( $"Schema not found." );
    IList<ElementId> existingStorageIds = GetDataStorages()
      .GetElementsHasEntityWhich( schema, predicate )
      .Select( x => x.Id ).ToList();
    _document.Delete( existingStorageIds );
  }

  public void PurgeDataStorages<T>()
  {
    Schema? schema = typeof( T ).GetSchema();
    if ( schema == null )
      return;
    IList<ElementId> dataStorageIds = GetDataStorages()
      .GetElementsHasValidEntity( schema )
      .Select( e => e.Id ).ToList();
    _document.Delete( dataStorageIds );
  }


  /// <summary>
  /// This method ensures data will be stored in a Data Storage object with given name.
  /// </summary>
  /// <typeparam name="T"></typeparam>
  /// <param name="value">Storing data.</param>
  /// <param name="document">Target Revit's document context.</param>
  /// <param name="storageName">Data storage's name.</param>
  /// <returns>A data storage if success, or null if false.</returns>
  public DataStorage? SaveAsStorage<T>( T value, string? storageName = null )
  {
    DataStorage storage = DataStorage.Create( _document );
    storage.Name = storageName;
    storage.SaveObject( value );
    return storage;
  }

  private IEnumerable<Element> GetDataStorages()
  {
    return new FilteredElementCollector( _document ).OfClass( typeof( DataStorage ) );
  }
}
