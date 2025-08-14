using Autodesk.Revit.DB.ExtensibleStorage;
using Newtonsoft.Json;
using RevitToolkit.Storage.Attributes;
using System.Reflection;

namespace RevitToolkit.Storage;
public static class EntityConvert
{
  public static T? DeserializeToObject<T>( this Entity entity )
  {
    T instance = Activator.CreateInstance<T>();
    Type manifest = typeof( T );
    PropertyInfo[] props = manifest.GetProperties();
    foreach ( PropertyInfo prop in props ) {
      object? value = entity.GetObjectValue( entity.Schema, prop, prop.PropertyType );
      if ( value != null )
        prop.SetValue( instance, value, null );
    }

    FieldInfo[] fields = manifest.GetFields();
    foreach ( FieldInfo field in fields ) {
      object? value = entity.GetObjectValue( entity.Schema, field, field.FieldType );
      if ( value != null )
        field.SetValue( instance, value );
    }

    return instance;
  }

  public static Entity? SerializeToEntity<T>( this T value )
  {
    Type? objectType = value?.GetType();
    if ( objectType == null )
      return default;

    Schema? schema = objectType.CreateSchemaIfNotExists();

    if ( schema == null )
      return default;

    Entity entity = new Entity( schema );

    PropertyInfo[] props = objectType.GetProperties();
    foreach ( PropertyInfo prop in props ) {
      object? propValue = prop.GetValue( value, null );
      if ( propValue == null )
        continue;
      entity.SetEntityFieldValue( schema, prop, prop.PropertyType, propValue );
    }

    FieldInfo[] fields = objectType.GetFields();
    foreach ( FieldInfo field in fields ) {
      object? fieldValue = field.GetValue( value );
      if ( fieldValue == null )
        continue;
      entity.SetEntityFieldValue( schema, field, field.FieldType, fieldValue );
    }

    return entity;
  }

  private static void SetEntityFieldValue( this Entity entity, Schema schema, MemberInfo manifest, Type dataType, object value )
  {
    if ( manifest.GetCustomAttribute<FieldAttribute>() is not FieldAttribute fieldAttr )
      return;

    Field schemaField = schema.GetField( fieldAttr.Name ?? manifest.Name );

    if ( fieldAttr.CanBeNull ) {
      string json = JsonConvert.SerializeObject( value );
      entity.SetValue( schemaField, json );
    }
    else if ( FieldTypeValidator.IsAllowableDataType( dataType ) )
      entity.SetValue( schemaField, value );

    else if ( FieldTypeValidator.IsArrayDataType( dataType, out Type? itemType ) )
      entity.SetArrayValue( schemaField, value, itemType );

    else if ( FieldTypeValidator.IsArrayDataType( dataType, out Type? dictValueType ) && dataType.GetDictKeyType() == typeof( string ) )
      entity.SetMapValue( schemaField, value, dictValueType );

    else if ( dataType.GetSchema() != null ) {
      Entity? subEntity = value.SerializeToEntity();
      if ( subEntity == null )
        return;
      entity.SetValue( schemaField, subEntity );
    }
    else {
      string json = JsonConvert.SerializeObject( value );
      entity.SetValue( schemaField, json );
    }
  }

  private static object? GetObjectValue( this Entity entity, Schema schema, MemberInfo manifest, Type dataType )
  {
    if ( manifest.GetCustomAttribute<FieldAttribute>() is not FieldAttribute fieldAttr )
      return null;
    Field schemaField = schema.GetField( fieldAttr.Name ?? manifest.Name );
    if ( fieldAttr.CanBeNull ) {
      string? json = entity.GetValue( schemaField, typeof( string ) ) as string;
      return string.IsNullOrEmpty( json ) ? null : JsonConvert.DeserializeObject( json ?? string.Empty, dataType );
    }
    else if ( FieldTypeValidator.IsAllowableDataType( dataType ) )
      return entity.GetValue( schemaField, dataType );

    else if ( dataType.GetSchema() is Schema subSchema ) {
      object? instance = Activator.CreateInstance( dataType );
      Entity subEntity = entity.Get<Entity>( schema.GetField( fieldAttr.Name ?? manifest.Name ) );
      PropertyInfo[] props = dataType.GetProperties();
      foreach ( PropertyInfo prop in props ) {
        object? value = subEntity.GetObjectValue( subSchema, prop, prop.PropertyType );
        if ( value != null )
          prop.SetValue( instance, value, null );
      }

      FieldInfo[] fields = dataType.GetFields();
      foreach ( FieldInfo field in fields ) {
        object? value = subEntity.GetObjectValue( subSchema, field, field.FieldType );
        if ( value != null )
          field.SetValue( instance, value );
      }

      return instance;
    }

    else if ( FieldTypeValidator.IsArrayDataType( dataType, out _ ) )
      return entity.GetArrayValue( schemaField, dataType );

    else if ( FieldTypeValidator.IsMapDataType( dataType, out _ ) )
      return entity.GetArrayValue( schemaField, dataType );

    else {
      string? json = entity.GetValue( schemaField, typeof( string ) ) as string;
      return string.IsNullOrEmpty( json ) ? null : JsonConvert.DeserializeObject( json ?? string.Empty, dataType );
    }
  }
}
