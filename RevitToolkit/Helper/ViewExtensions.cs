namespace RevitToolkit.Helper;
public static class ViewExtensions
{
  public static ElementId GetViewElementId( this View view )
  {
#if REVIT2024_OR_GREATER
    return new ElementId( view.Id.Value - 1 );
#else
    return new ElementId( view.Id.IntegerValue - 1 );
#endif
  }
}
