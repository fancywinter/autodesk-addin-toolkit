using Autodesk.Revit.DB.ExtensibleStorage;
using RevitToolkit.Storage.Attributes;
using System.Reflection;

namespace RevitToolkit.Storage;

public static class SchemaExtensions
{
  /// <summary>
  /// Create schema from a type.
  /// </summary>
  /// <typeparam name="T">A type which represents Revit Schema. It must decorates by <see cref="SchemaAttribute"/>.</typeparam>
  /// <returns>A schema if success, or null if false.</returns>
  public static Schema? CreateSchema<T>()
  {
    return typeof( T ).CreateSchema();
  }

  /// <summary>
  /// Create schema from a type.
  /// </summary>
  /// <param name="manifest">A type which represents Revit Schema. It must decorates by <see cref="SchemaAttribute"/>.</param>
  /// <returns>A schema if success, or null if false.</returns>
  public static Schema? CreateSchema( this Type manifest )
  {
    if ( manifest.GetCustomAttribute<SchemaAttribute>() is not SchemaAttribute schemaAtt )
      return default;

    AccessLevel readAccess = manifest.GetCustomAttribute<ReadAccessAttribute>()?.Level ?? AccessLevel.Public;
    AccessLevel writeAccess = manifest.GetCustomAttribute<WriteAccessAttribute>()?.Level ?? AccessLevel.Public;
    string? documentation = manifest.GetCustomAttribute<DocumentationAttribute>()?.Text;

    SchemaBuilder builder = new SchemaBuilder( Guid.Parse( schemaAtt.Guid ) )
      .SetReadAccessLevel( readAccess )
      .SetWriteAccessLevel( writeAccess )
      .SetSchemaName( schemaAtt.Name ?? manifest.Name );

    if ( schemaAtt.VendorId != null )
      builder.SetVendorId( schemaAtt.VendorId );

    if ( schemaAtt.AppGuid != null )
      builder.SetApplicationGUID( Guid.Parse( schemaAtt.AppGuid ) );

    if ( documentation != null )
      builder.SetDocumentation( documentation );

    PropertyInfo[] props = manifest.GetProperties();
    foreach ( PropertyInfo prop in props ) {
      if ( prop.PropertyType == prop.DeclaringType )
        continue;

      builder.AddField( prop, prop.PropertyType );
    }

    FieldInfo[] fields = manifest.GetFields();
    foreach ( FieldInfo field in fields ) {
      if ( field.FieldType == field.DeclaringType )
        continue;

      builder.AddField( field, field.FieldType );
    }

    return builder.Finish();
  }

  // TODO: Add exception wrapper with more infomation
  /// <summary>
  /// Create new schema if it not exists from a type. If no schema created, an exception will be thrown.
  /// </summary>
  /// <typeparam name="T">A type which represents Revit Schema. It must decorates by <see cref="SchemaAttribute"/>.</typeparam>
  /// <returns>A schema if success, or null if.</returns>
  public static Schema EnsureSchemaExists<T>()
  {
    return CreateSchemaIfNotExists<T>() ?? throw new Exception( "Can not create schema." );
  }

  // TODO: Add exception wrapper with more infomation
  /// <summary>
  /// Create new schema if it not exists from a type. If no schema created, an exception will be thrown.
  /// </summary>
  /// <param name="manifest">A type which represents Revit Schema. It must decorates by <see cref="SchemaAttribute"/>.</param>
  /// <returns>A schema if success, or null if false.</returns>
  public static Schema EnsureSchemaExists( this Type manifest )
  {
    return manifest.CreateSchemaIfNotExists() ?? throw new Exception( "Can not create schema." );
  }

  /// <summary>
  /// Get schema from a type.
  /// </summary>
  /// <typeparam name="T">A type which represents an Revit Schema. It must decorates by <see cref="SchemaAttribute"/>.</typeparam>
  /// <returns>A schema if success, or null if false.</returns>
  public static Schema? GetSchema<T>()
  {
    return typeof( T ).GetSchema();
  }

  /// <summary>
  /// Get schema from a type.
  /// </summary>
  /// <param name="manifest">A type which represents an Revit Schema. It must decorates by <see cref="SchemaAttribute"/>.</param>
  /// <returns>A schema if success, or null if false.</returns>
  public static Schema? GetSchema( this Type manifest )
  {
    return manifest.GetCustomAttribute<SchemaAttribute>() is SchemaAttribute schemaAtt
      ? Schema.Lookup( Guid.Parse( schemaAtt.Guid ) )
      : default;
  }

  /// <summary>
  /// Create new schema if it not exists from a type.
  /// </summary>
  /// <typeparam name="T">A type which represents Revit Schema. It must decorates by <see cref="SchemaAttribute"/>.</typeparam>
  /// <returns>A schema if success, or null if false.</returns>
  public static Schema? CreateSchemaIfNotExists<T>()
  {
    return GetSchema<T>() ?? CreateSchema<T>();
  }

  /// <summary>
  /// Create new schema if it not exists from a type.
  /// </summary>
  /// <param name="manifest">A type which represents Revit Schema. It must decorates by <see cref="SchemaAttribute"/>.</param>
  /// <returns>A schema if success, or null if false.</returns>
  public static Schema? CreateSchemaIfNotExists( this Type manifest )
  {
    return manifest.GetSchema() ?? manifest.CreateSchema();
  }

  /// <summary>
  /// Erase all public schemas and related entities from the document. 
  /// </summary>
  /// <param name="document">Target Revit's document context.</param>
  public static void PurgeSchemas( this Document document )
  {
    foreach ( Schema schema in Schema.ListSchemas() )
      if ( schema.WriteAccessLevel == AccessLevel.Public )
        document.EraseSchemaAndAllEntities( schema );
  }

  private static void AddField( this SchemaBuilder builder, MemberInfo manifest, Type dataType )
  {
    string? docs = manifest.GetCustomAttribute<DocumentationAttribute>()?.Text;

    if ( manifest.GetCustomAttribute<FieldAttribute>() is not FieldAttribute fieldAttr )
      return;

    if ( fieldAttr.CanBeNull )
      builder.AppendSimpleField( fieldAttr.Name ?? manifest.Name, typeof( string ), docs );

    else if ( FieldTypeValidator.IsAllowableDataType( dataType ) )
      builder.AppendSimpleField( fieldAttr.Name ?? manifest.Name, dataType, docs );

    else if ( dataType.CreateSchemaIfNotExists() is Schema subSchema )
      builder.AppendSimpleField( fieldAttr.Name ?? manifest.Name, typeof( Entity ), docs, subSchema.GUID );

    else if ( FieldTypeValidator.IsArrayDataType( dataType, out Type? itemType ) )
      builder.AppendArrayField( fieldAttr.Name ?? manifest.Name, itemType, docs );

    else if ( FieldTypeValidator.IsMapDataType( dataType, out Type? dictValueType ) )
      builder.AppendMapField( fieldAttr.Name ?? manifest.Name, dictValueType, docs );

    else
      builder.AppendSimpleField( fieldAttr.Name ?? manifest.Name, typeof( string ), docs );
  }

  private static SchemaBuilder AppendSimpleField( this SchemaBuilder builder, string fieldName, Type type, string? documentation = null, Guid? subSchemaGuid = null )
  {
    FieldBuilder fieldBuilder = builder.AddSimpleField( fieldName, type );

    if ( fieldBuilder.NeedsUnits() )
      fieldBuilder.SetSpec( SpecTypeId.Custom );

    if ( subSchemaGuid != null )
      fieldBuilder.SetSubSchemaGUID( subSchemaGuid.Value );

    if ( documentation != null )
      fieldBuilder.SetDocumentation( documentation );

    return builder;
  }

  private static SchemaBuilder AppendArrayField( this SchemaBuilder builder, string fieldName, Type type, string? documentation = null )
  {
    FieldBuilder fieldBuilder = builder.AddArrayField( fieldName, type );

    if ( fieldBuilder.NeedsUnits() )
      fieldBuilder.SetSpec( SpecTypeId.Custom );

    if ( documentation != null )
      fieldBuilder.SetDocumentation( documentation );
    return builder;
  }

  private static SchemaBuilder AppendMapField( this SchemaBuilder builder, string fieldName, Type valueType, string? documentation = null )
  {
    FieldBuilder fieldBuilder = builder.AddMapField( fieldName, typeof( string ), valueType );

    if ( fieldBuilder.NeedsUnits() )
      fieldBuilder.SetSpec( SpecTypeId.Custom );

    if ( documentation != null )
      fieldBuilder.SetDocumentation( documentation );
    return builder;
  }
}
