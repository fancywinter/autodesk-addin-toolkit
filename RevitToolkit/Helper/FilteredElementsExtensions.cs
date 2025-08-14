namespace RevitToolkit.Helper;
public static class FilteredElementsExtensions
{
  public static IEnumerable<T> GetElementTypes<T>( this Document document ) where T : ElementType
  {
    return new FilteredElementCollector( document ).OfClass( typeof( T ) ).Cast<T>();
  }

  public static IEnumerable<T> GetElementTypes<T>( this Document document, string name ) where T : ElementType
  {
    return new FilteredElementCollector( document ).OfClass( typeof( T ) ).Cast<T>().Where( t => t.Name == name );
  }

  public static T? GetElementType<T>( this Document document, string name ) where T : ElementType
  {
    return new FilteredElementCollector( document ).OfClass( typeof( T ) ).Cast<T>().Where( t => t.Name == name ).FirstOrDefault();
  }
}
