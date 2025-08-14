using AcadToolkit.Interops;

namespace AcadToolkit.Helpers;

public static class AutoCadService
{
  public static void NewDrawing( string dwtPath )
  {
    DocumentCollection docMgr = Autodesk.AutoCAD.ApplicationServices.Core.Application.DocumentManager;
    Document doc = docMgr.Add( dwtPath );
    docMgr.MdiActiveDocument = doc;
  }

  public static void CopyBlockDef( Database srcDb, Database targetDb, string blockName )
  {
    using TransactionScope srcScope = new( srcDb );
    if ( !srcScope.Blocks.TryGetId( blockName, out ObjectId blockId ) ) {
      return;
    }
    using IdMapping mapping = new();
    targetDb.WblockCloneObjects(
      new() { blockId },
      targetDb.BlockTableId, mapping,
      DuplicateRecordCloning.Replace, false );
  }

  public static void CopyLayer( Database srcDb, Database targetDb, string layerName )
  {
    using TransactionScope srcScope = new( srcDb );
    if ( !srcScope.Layers.TryGetId( layerName, out ObjectId id ) ) {
      return;
    }
    using IdMapping mapping = new();
    targetDb.WblockCloneObjects(
      new() { id },
      targetDb.LayerTableId, mapping,
      DuplicateRecordCloning.Replace, false );
  }

  public static void CopyDimStyle( Database srcDb, Database targetDb, string dimStyleName )
  {
    using TransactionScope srcScope = new( srcDb );
    if ( !srcScope.DimStyles.TryGetId( dimStyleName, out ObjectId id ) ) {
      return;
    }
    using IdMapping mapping = new();
    targetDb.WblockCloneObjects(
      new() { id },
      targetDb.DimStyleTableId, mapping,
      DuplicateRecordCloning.Replace, false );
  }

  public static Document GetDocument( Database db ) => Autodesk.AutoCAD.ApplicationServices.Core.Application.DocumentManager.GetDocument( db );

  public static Editor GetEditor( Database db ) => GetDocument( db ).Editor;

  public static T GetSystemVariable<T>( string name ) => ( T ) Application.GetSystemVariable( name );

  public static void SwitchToModelSpace()
  {
    Document doc = Application.DocumentManager.MdiActiveDocument;
    if ( !doc.Database.TileMode )
      doc.Editor.SwitchToModelSpace();
  }

  public static void ToggleSpace()
  {
    // Get the current document
    Document doc = Application.DocumentManager.MdiActiveDocument;

    // Get the current values of CVPORT and TILEMODE
    short cvports = GetSystemVariable<short>( "CVPORT" );
    short tilemode = GetSystemVariable<short>( "TILEMODE" );

    // Check to see if the Model layout is active, TILEMODE is 1 when
    // the Model layout is active
    if ( tilemode == 0 ) {
      // Check to see if Model space is active in a viewport,
      // CVPORT is 2 if Model space is active 
      if ( cvports == 2 )
        doc.Editor.SwitchToPaperSpace();
      else
        doc.Editor.SwitchToModelSpace();
    }
    else {
      // Switch to the previous Paper space layout
      Application.SetSystemVariable( "TILEMODE", 0 );
    }
  }
}
