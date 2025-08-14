using Autodesk.Revit.DB.Structure;

namespace RevitToolkit.Helper;
public static class RebarHostExtensions
{
  public static void RemoveExistingRebars( this Element host )
  {
    Document document = host.Document;
    IList<ElementId> rebarIds = host.GetDependentElements( new ElementClassFilter( typeof( Rebar ) ) );
    foreach ( ElementId rebarId in rebarIds )
      document.Delete( rebarId );
  }
}
