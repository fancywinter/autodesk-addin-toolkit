using AcadToolkit.Creation;

namespace AcadToolkit.Interops;

public sealed class TransactionScope : IDisposable
{
  public Database Database { get; private set; }
  private readonly Transaction _transaction;
  private readonly bool _suppressDisposeDatabase;

  public bool IsAborted { get; private set; }

  public TransactionScope( Database database ) : this( database, suppressDisposeDatabase: true )
  {
  }

  private TransactionScope( Database database, bool suppressDisposeDatabase )
  {
    Database = database;
    _transaction = database.TransactionManager.StartTransaction();
    _suppressDisposeDatabase = suppressDisposeDatabase;
  }

  public static TransactionScope? FromActiveDocument()
  {
    if ( Autodesk.AutoCAD.ApplicationServices.Core.Application.DocumentManager.MdiActiveDocument is not { } doc )
      return null;
    return new TransactionScope( doc.Database );
  }

  public static TransactionScope FromFile( string filePath )
  {
    return FromFile( filePath, FileOpenMode.OpenForReadAndAllShare, false, null );
  }

  public static TransactionScope FromFile( string filePath, FileOpenMode mode, bool allowCPConversion, string? password )
  {
    return new TransactionScope( DatabaseFactory.FromFile( filePath, mode, allowCPConversion, password ), suppressDisposeDatabase: false );
  }

  public void Dispose()
  {
    if ( _transaction is not { IsDisposed: false } )
      return;
    if ( !IsAborted )
      _transaction.Commit();
    _transaction.Dispose();
    if ( !_suppressDisposeDatabase )
      Database.Dispose();
  }

  public T? GetObject<T>( ObjectId id ) where T : DBObject { return GetObject<T>( id, OpenMode.ForRead ); }

  public T? GetObject<T>( ObjectId id, OpenMode mode ) where T : DBObject
  {
    return id.IsNull || id.IsEffectivelyErased ? default : _transaction.GetObject( id, mode ) as T;
  }

  public T? GetObject<T>( ObjectId id, OpenMode mode, bool openErased ) where T : DBObject
  { return id.IsNull || id.IsEffectivelyErased ? default : _transaction.GetObject( id, mode, openErased ) as T; }

  public T? GetObject<T>( ObjectId id, OpenMode mode, bool openErased, bool forceOpenOnLockedLayer ) where T : DBObject
  {
    return id.IsNull || id.IsEffectivelyErased ? default : _transaction.GetObject( id, mode, openErased, forceOpenOnLockedLayer ) as T;
  }
  public bool TryGetObjectId( in Handle handle, out ObjectId id ) { return Database.TryGetObjectId( handle, out id ); }

  public void Abort()
  {
    IsAborted = true;
    _transaction.Abort();
  }

  public EntityContainer? GetEntityContainer( ObjectId blockTableRecordId )
    => Blocks.Contains( blockTableRecordId ) ? new EntityContainer( _transaction, blockTableRecordId ) : null;

  public EntityContainer? GetEntityContainer( string blockTableRecordName )
    => Blocks.Contains( blockTableRecordName ) ? new EntityContainer( _transaction, Blocks[ blockTableRecordName ] ) : null;

  private EntityContainer? _modelSpace;

  public EntityContainer ModelSpace
  {
    get
    {
      _modelSpace ??= new EntityContainer( _transaction, Blocks[ BlockTableRecord.ModelSpace ] );
      return _modelSpace;
    }
  }

  private EntityContainer? _paperSpace;

  public EntityContainer PaperSpace
  {
    get
    {
      _paperSpace ??= new EntityContainer( _transaction, Blocks[ BlockTableRecord.PaperSpace ] );
      return _paperSpace;
    }
  }

  private EntityContainer? _currentSpace;

  public EntityContainer CurrentSpace
  {
    get
    {
      _currentSpace ??= new EntityContainer( _transaction, Database.CurrentSpaceId );
      return _currentSpace;
    }
  }

  private BlockContainer? _blocks;

  public BlockContainer Blocks
  {
    get
    {
      _blocks ??= new BlockContainer( _transaction, Database.BlockTableId );
      return _blocks;
    }
  }

  private LayerContainer? _layers;

  public LayerContainer Layers
  {
    get
    {
      _layers ??= new LayerContainer( _transaction, Database.LayerTableId );
      return _layers;
    }
  }

  private LineTypeContainer? _linetypes;

  public LineTypeContainer Linetypes
  {
    get
    {
      _linetypes ??= new LineTypeContainer( _transaction, Database.LinetypeTableId );
      return _linetypes;
    }
  }

  private DimStyleContainer? _dimStyles;

  public DimStyleContainer DimStyles
  {
    get
    {
      _dimStyles ??= new DimStyleContainer( _transaction, Database.DimStyleTableId );
      return _dimStyles;
    }

  }

  private RegAppContainer? _regApps;

  public RegAppContainer RegApps
  {
    get
    {
      _regApps ??= new RegAppContainer( _transaction, Database.RegAppTableId );
      return _regApps;
    }

  }

  private TextStyleContainer? _textStyles;

  public TextStyleContainer TextStyles
  {
    get
    {
      _textStyles ??= new TextStyleContainer( _transaction, Database.TextStyleTableId );
      return _textStyles;
    }
  }

  private DBDictionaryContainer? _dbDictionary;

  public DBDictionaryContainer DBDictionary
  {
    get
    {
      _dbDictionary ??= new DBDictionaryContainer( _transaction, Database.NamedObjectsDictionaryId );
      return _dbDictionary;
    }
  }
}
