namespace AcadToolkit;

public static class AcGroupFactory
{
  public static ObjectId CreateGroup( Transaction transaction, string name, ObjectIdCollection entityIdCollection, string description = "" )
  {
    Database db = Autodesk.AutoCAD.ApplicationServices.Core.Application.DocumentManager.MdiActiveDocument.Database;
    using DBDictionary groupDict = ( DBDictionary ) transaction.GetObject( db.GroupDictionaryId, OpenMode.ForWrite );
    using Group group = new Group( description, true );
    ObjectId id = groupDict.SetAt( name, group );
    transaction.AddNewlyCreatedDBObject( group, true );
    group.InsertAt( 0, entityIdCollection );
    return id;
  }
}
