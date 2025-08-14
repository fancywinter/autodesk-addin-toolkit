namespace RevitToolkit.Storage;
internal static class FieldTypeValidator
{
  /// <summary>
  /// The following simple data types are allowed:<br/>
  /// int, short, byte, double, float, bool, string, GUID, XYZ, UV.
  /// </summary>
  /// <param name="type">Data type to check.</param>
  /// <returns></returns>
  public static bool IsAllowableDataType( Type type )
  {
    return type == typeof( int )
        || type == typeof( short )
        || type == typeof( byte )
        || type == typeof( double )
        || type == typeof( float )
        || type == typeof( bool )
        || type == typeof( string )
        || type == typeof( Guid )
        || type == typeof( ElementId )
        || type == typeof( XYZ )
        || type == typeof( UV );
  }

  public static bool IsArrayDataType( Type type, [NotNullWhen( true )] out Type? itemType )
  {
    itemType = null;
    if ( type.IsArray ||  type.IsGenericType && type.GetGenericTypeDefinition() == typeof( IList<> )  )
      if ( type.TryGetListItemType( out itemType ) && IsAllowableDataType( itemType ) )
        return itemType != null;
    return false;
  }

  public static bool IsMapDataType( Type type, [NotNullWhen( true )] out Type? itemType )
  {
    itemType = null;
    if ( type.IsGenericType && type.GetGenericTypeDefinition() == typeof( IDictionary<,> ) )
      if ( type.TryGetListItemType( out itemType ) && IsAllowableDataType( itemType ) )
        return itemType != null;
    return false;
  }

  public static bool IsValid( Type type )
  {
    return IsAllowableDataType( type ) || IsArrayDataType( type, out _ ) || IsMapDataType( type, out _ );
  }
}
