using Autodesk.Revit.DB.ExtensibleStorage;

namespace RevitToolkit.Storage;
public static class EntityLinqExtensions
{
  /// <summary>
  /// Filters a sequence of elements that have an entity which satisfies the given predicate.
  /// </summary>
  /// <typeparam name="TElement">The type of the elements of source.</typeparam>
  /// <param name="elements">Sequence of elements to filter.</param>
  /// <param name="schema">Schema of entity.</param>
  /// <param name="predicate">A function to test each element for a condition.</param>
  /// <returns></returns>
  public static IEnumerable<TElement> GetElementsHasEntityWhich<TElement>( this IEnumerable<TElement> elements, Schema schema, Func<Entity, bool> predicate )
    where TElement : Element
  {
    return elements.Where( element => predicate( element.GetEntity( schema ) ) );
  }

  public static IEnumerable<TElement> GetElementsHasValidEntity<TElement>( this IEnumerable<TElement> elements, Schema schema )
    where TElement : Element
  {
    return elements.GetElementsHasEntityWhich( schema, entity => entity.IsValid() );
  }

  public static IEnumerable<Entity> GetEntitiesWhich( this IEnumerable<Element> elements, Schema schema, Func<Entity, bool> predicate )
  {
    return elements.GetElementsHasEntityWhich( schema, predicate ).Select( element => element.GetEntity( schema ) );
  }
}
