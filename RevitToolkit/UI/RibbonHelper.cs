namespace RevitToolkit.UI;

public static class RibbonHelper
{
  public static Autodesk.Windows.RibbonItem? GetInternalRibbonItem( RibbonItem item )
  {
    return item.TryExecuteNonPublicMethod( "getRibbonItem", out Autodesk.Windows.RibbonItem? result ) ? result : null;
  }
}
