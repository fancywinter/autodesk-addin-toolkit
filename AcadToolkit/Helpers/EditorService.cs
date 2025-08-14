using Application = Autodesk.AutoCAD.ApplicationServices.Core.Application;

namespace AcadToolkit.Helpers;

public static class EditorService
{
  public static void Regen() => Application.DocumentManager.MdiActiveDocument.Editor.Regen();

  public static Point3d? PickPoint( string promptMsg )
  {
    Editor editor = Application.DocumentManager.MdiActiveDocument.Editor;
    PromptPointOptions option = new PromptPointOptions( promptMsg );
    PromptPointResult result = editor.GetPoint( option );
    return result.Status == PromptStatus.OK ? result.Value : null;
  }

  public static ObjectId? PickEntity( string promptMsg )
  {
    Editor editor = Application.DocumentManager.MdiActiveDocument.Editor;
    PromptEntityOptions option = new PromptEntityOptions( promptMsg );
    PromptEntityResult result = editor.GetEntity( option );
    return result.Status == PromptStatus.OK ? result.ObjectId : null;
  }
}
