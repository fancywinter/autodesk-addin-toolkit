namespace RevitToolkit.Helper;
public static class DocumentExtensions
{
  public static ElementId? GetDefaultElementId<T>( this Document document ) where T : Element
  {
    return new FilteredElementCollector( document ).OfClass( typeof( T ) ).FirstElementId();
  }

  public static T? GetElement<T>( this Document document, string uniqueId ) where T : Element
  {
    return document.GetElement( uniqueId ) as T;
  }

  public static T? GetElement<T>( this Document doc, ElementId id ) where T : Element
  {
    return doc.GetElement( id ) as T;
  }

  public static IEnumerable<T> GetElements<T>( this Document doc, IEnumerable<ElementId> ids ) where T : Element
  {
    return ids.Where( id => id.IsValid() ).Select( id => doc.GetElement( id ) ).OfType<T>();
  }
}
