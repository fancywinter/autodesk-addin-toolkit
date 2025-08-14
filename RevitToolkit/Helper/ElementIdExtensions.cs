namespace RevitToolkit.Helper;
public static class ElementIdExtensions
{
  public static bool IsValid( this ElementId id )
  {
    return id != ElementId.InvalidElementId;
  }
}
