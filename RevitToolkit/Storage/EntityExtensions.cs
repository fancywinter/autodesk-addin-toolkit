using Autodesk.Revit.DB.ExtensibleStorage;

namespace RevitToolkit.Storage;

public static class EntityExtensions
{
  public static bool HasFieldValue( this Entity entity, Field field, object value )
  {
    return entity.GetValue( field, value.GetType() )?.Equals( value ) ?? false;
  }

  public static Entity SetValue( this Entity entity, Field field, object value )
  {
    Type valueType = value.GetType();

    if ( valueType == typeof( int ) )
      entity.Set( field, ( int ) value );

    else if ( valueType == typeof( short ) )
      entity.Set( field, ( short ) value );

    else if ( valueType == typeof( byte ) )
      entity.Set( field, ( byte ) value );

    else if ( valueType == typeof( double ) )
      entity.Set( field, ( double ) value, UnitTypeId.Custom );

    else if ( valueType == typeof( float ) )
      entity.Set( field, ( float ) value, UnitTypeId.Custom );

    else if ( valueType == typeof( bool ) )
      entity.Set( field, ( bool ) value );

    else if ( valueType == typeof( string ) )
      entity.Set( field, ( string ) value );

    else if ( valueType == typeof( Guid ) )
      entity.Set( field, ( Guid ) value );

    else if ( valueType == typeof( ElementId ) )
      entity.Set( field, ( ElementId ) value );

    else if ( valueType == typeof( XYZ ) )
      entity.Set( field, ( XYZ ) value, UnitTypeId.Custom );

    else if ( valueType == typeof( UV ) )
      entity.Set( field, ( UV ) value, UnitTypeId.Custom );

    else if ( valueType == typeof( Entity ) )
      entity.Set( field, ( Entity ) value );

    return entity;
  }

  public static object? GetValue( this Entity entity, Field field, Type valueType )
  {
    if ( valueType == typeof( int ) )
      return entity.Get<int>( field );

    else if ( valueType == typeof( short ) )
      return entity.Get<short>( field );

    else if ( valueType == typeof( byte ) )
      return entity.Get<byte>( field );

    else if ( valueType == typeof( double ) )
      return entity.Get<double>( field, UnitTypeId.Custom );

    else if ( valueType == typeof( float ) )
      return entity.Get<float>( field, UnitTypeId.Custom );

    else if ( valueType == typeof( bool ) )
      return entity.Get<bool>( field );

    else if ( valueType == typeof( string ) )
      return entity.Get<string>( field );

    else if ( valueType == typeof( Guid ) )
      return entity.Get<Guid>( field );

    else if ( valueType == typeof( ElementId ) )
      return entity.Get<ElementId>( field );

    else if ( valueType == typeof( XYZ ) )
      return entity.Get<XYZ>( field, UnitTypeId.Custom );

    else if ( valueType == typeof( UV ) )
      return entity.Get<UV>( field, UnitTypeId.Custom );

    else if ( valueType == typeof( Entity ) )
      return entity.Get<Entity>( field );

    return null;
  }

  public static Entity SetArrayValue( this Entity entity, Field field, object value, Type itemType )
  {
    if ( itemType == typeof( int ) )
      entity.Set( field, ( IList<int> ) value );

    else if ( itemType == typeof( short ) )
      entity.Set( field, ( IList<short> ) value );

    else if ( itemType == typeof( byte ) )
      entity.Set( field, ( IList<byte> ) value );

    else if ( itemType == typeof( double ) )
      entity.Set( field, ( IList<double> ) value, UnitTypeId.Custom );

    else if ( itemType == typeof( float ) )
      entity.Set( field, ( IList<float> ) value, UnitTypeId.Custom );

    else if ( itemType == typeof( bool ) )
      entity.Set( field, ( IList<bool> ) value );

    else if ( itemType == typeof( string ) )
      entity.Set( field, ( IList<string> ) value );

    else if ( itemType == typeof( Guid ) )
      entity.Set( field, ( IList<Guid> ) value );

    else if ( itemType == typeof( ElementId ) )
      entity.Set( field, ( IList<ElementId> ) value );

    else if ( itemType == typeof( XYZ ) )
      entity.Set( field, ( IList<XYZ> ) value, UnitTypeId.Custom );

    else if ( itemType == typeof( UV ) )
      entity.Set( field, ( IList<UV> ) value, UnitTypeId.Custom );

    return entity;
  }

  public static object? GetArrayValue( this Entity entity, Field field, Type itemType )
  {
    if ( itemType == typeof( int ) )
      return entity.Get<IList<int>>( field );

    else if ( itemType == typeof( short ) )
      return entity.Get<IList<short>>( field );

    else if ( itemType == typeof( byte ) )
      return entity.Get<IList<byte>>( field );

    else if ( itemType == typeof( double ) )
      return entity.Get<IList<double>>( field, UnitTypeId.Custom );

    else if ( itemType == typeof( float ) )
      return entity.Get<IList<float>>( field, UnitTypeId.Custom );

    else if ( itemType == typeof( bool ) )
      return entity.Get<IList<bool>>( field );

    else if ( itemType == typeof( string ) )
      return entity.Get<IList<string>>( field );

    else if ( itemType == typeof( Guid ) )
      return entity.Get<IList<Guid>>( field );

    else if ( itemType == typeof( ElementId ) )
      return entity.Get<IList<ElementId>>( field );

    else if ( itemType == typeof( XYZ ) )
      return entity.Get<IList<XYZ>>( field, UnitTypeId.Custom );

    else if ( itemType == typeof( UV ) )
      return entity.Get<IList<UV>>( field, UnitTypeId.Custom );

    return null;
  }

  public static Entity SetMapValue( this Entity entity, Field field, object value, Type dictValueType )
  {
    if ( dictValueType == typeof( int ) )
      entity.Set( field, ( IDictionary<string, int> ) value );

    else if ( dictValueType == typeof( short ) )
      entity.Set( field, ( IDictionary<string, short> ) value );

    else if ( dictValueType == typeof( byte ) )
      entity.Set( field, ( IDictionary<string, byte> ) value );

    else if ( dictValueType == typeof( double ) )
      entity.Set( field, ( IDictionary<string, double> ) value, UnitTypeId.Custom );

    else if ( dictValueType == typeof( float ) )
      entity.Set( field, ( IDictionary<string, float> ) value, UnitTypeId.Custom );

    else if ( dictValueType == typeof( bool ) )
      entity.Set( field, ( IDictionary<string, bool> ) value );

    else if ( dictValueType == typeof( string ) )
      entity.Set( field, ( IDictionary<string, string> ) value );

    else if ( dictValueType == typeof( Guid ) )
      entity.Set( field, ( IDictionary<string, Guid> ) value );

    else if ( dictValueType == typeof( ElementId ) )
      entity.Set( field, ( IDictionary<string, ElementId> ) value );

    else if ( dictValueType == typeof( XYZ ) )
      entity.Set( field, ( IDictionary<string, XYZ> ) value, UnitTypeId.Custom );

    else if ( dictValueType == typeof( UV ) )
      entity.Set( field, ( IDictionary<string, UV> ) value, UnitTypeId.Custom );

    return entity;
  }

  public static object? GetMapValue( this Entity entity, Field field, Type dictValueType )
  {
    if ( dictValueType == typeof( int ) )
      return entity.Get<IDictionary<string, int>>( field );

    else if ( dictValueType == typeof( short ) )
      return entity.Get<IDictionary<string, short>>( field );

    else if ( dictValueType == typeof( byte ) )
      return entity.Get<IDictionary<string, byte>>( field );

    else if ( dictValueType == typeof( double ) )
      entity.Get<IDictionary<string, double>>( field, UnitTypeId.Custom );

    else if ( dictValueType == typeof( float ) )
      entity.Get<IDictionary<string, float>>( field, UnitTypeId.Custom );

    else if ( dictValueType == typeof( bool ) )
      return entity.Get<IDictionary<string, bool>>( field );

    else if ( dictValueType == typeof( string ) )
      return entity.Get<IDictionary<string, string>>( field );

    else if ( dictValueType == typeof( Guid ) )
      return entity.Get<IDictionary<string, Guid>>( field );

    else if ( dictValueType == typeof( ElementId ) )
      return entity.Get<IDictionary<string, ElementId>>( field );

    else if ( dictValueType == typeof( XYZ ) )
      return entity.Get<IDictionary<string, XYZ>>( field, UnitTypeId.Custom );

    else if ( dictValueType == typeof( UV ) )
      return entity.Get<IDictionary<string, UV>>( field, UnitTypeId.Custom );

    return entity;
  }
}
