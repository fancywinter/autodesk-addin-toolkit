namespace RevitToolkit.Helper;
public static class ViewSheetExtensions
{
  public static ElementId GetTitleBlockId( this ViewSheet sheet )
  {
    ElementId titleBlockId = new FilteredElementCollector( sheet.Document, sheet.Id ).OfCategory( BuiltInCategory.OST_TitleBlocks ).FirstElementId();
    return sheet.Document.GetElement( titleBlockId ).GetTypeId();
  }

  public static ICollection<TView> GetDependentViews<TView>( this ViewSheet sheet ) where TView : View
  {
    return sheet.Document.GetElements<TView>( sheet.GetDependentViewIds() ).ToList();
  }

  public static ICollection<TView> GetPlacedViews<TView>( this ViewSheet sheet ) where TView : View
  {
    return sheet.Document.GetElements<TView>( sheet.GetAllPlacedViews() ).ToList();
  }

  public static ICollection<Viewport> GetViewports( this ViewSheet sheet )
  {
    return sheet.Document.GetElements<Viewport>( sheet.GetAllViewports() ).ToList();
  }
}
