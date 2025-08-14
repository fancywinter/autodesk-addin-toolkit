using Autodesk.Revit.DB.ExtensibleStorage;

namespace RevitToolkit.Storage;

/// <summary>
/// A helper class support creating Revit's extensible data object from CLR class instances.
/// </summary>
public static class ExtensibleStorageHelper
{
  /// <summary>
  /// Save data to a Revit element.
  /// </summary>
  /// <typeparam name="T"></typeparam>
  /// <param name="value"></param>
  /// <param name="owner">Element that stored extended data.</param>
  public static void SaveObject<T>( this Element owner, T value )
  {
    Entity? entity = value.SerializeToEntity();
    owner.SetEntity( entity );
  }

  /// <summary>
  /// Get data from Revit target element.
  /// </summary>
  /// <typeparam name="T"></typeparam>
  /// <param name="owner">Element stores data.</param>
  /// <returns></returns>
  public static T? GetObject<T>( this Element owner )
  {
    Schema? schema = typeof( T ).GetSchema();
    if ( schema == null )
      return default;

    Entity entity = owner.GetEntity( schema );
    if ( !entity.IsValid() )
      return default;

    return entity.DeserializeToObject<T>();
  }
}
