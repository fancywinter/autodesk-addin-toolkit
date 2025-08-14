namespace AcadToolkit.Helpers;

public static class ObjectIdCollectionExtensions
{
  public static void AddRange( this ObjectIdCollection source, params ObjectIdCollection[] collections )
  {
    foreach ( ObjectIdCollection collection in collections )
      foreach ( ObjectId id in collection )
        source.Add( id );
  }

  public static void AddRange( this ObjectIdCollection source, params IEnumerable<ObjectId>[] collections )
  {
    foreach ( IEnumerable<ObjectId> collection in collections )
      foreach ( ObjectId id in collection )
        source.Add( id );
  }
}
