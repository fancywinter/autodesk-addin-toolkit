namespace AcadToolkit.Creation;

public static class DatabaseFactory
{
  public static Database FromFile( string filePath ) => FromFile( filePath, FileOpenMode.OpenForReadAndAllShare, false, null );

  public static Database FromFile( string filePath, FileOpenMode mode, bool allowCPConversion, string? password )
  {
    Database db = new( false, true );
    try {
      db.ReadDwgFile( filePath, mode, allowCPConversion, password );
      db.CloseInput( true );
      return db;
    }
    catch {
      db.Dispose();
      throw;
    }
  }
}
