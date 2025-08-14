using AcadToolkit.Creation;
using AcadToolkit.Helpers;
using AcadToolkit.Interops;
using Newtonsoft.Json;
using System.Reflection;

namespace AcadToolkit.Data;

public static class XDataHelper
{
  #region Assign XData

  /// <summary>
  /// Assigns XData extracting from class object to given AutoCAD objects.
  /// </summary>
  /// <typeparam name="T">Type of class object contains XData information, specifies by <see cref="XDataAttribute"/>.</typeparam>
  /// <param name="scope">Represents to <see cref="Transaction"/>.</param>
  /// <param name="targetIds">Collection of AutoCAD object IDs.</param>
  /// <param name="data">Object contains information to contructs XData.</param>
  /// <exception cref="InvalidOperationException"></exception>
  public static void AssignXData<T>( TransactionScope scope, IEnumerable<ObjectId> targetIds, T data )
    where T : class, new()
  {
    if ( !TryExtractResultBuffer( data, out ResultBuffer? resultBuffer ) )
      throw new InvalidOperationException( "Failed to extract data." );

    EnsureRegAppExists( scope, ( string ) resultBuffer.AsArray().First().Value );

    foreach ( ObjectId id in targetIds ) {
      DBObject? dbObj = scope.GetObject<DBObject>( id, OpenMode.ForWrite );
      if ( dbObj != null )
        dbObj.XData = resultBuffer;
    }
  }

  /// <summary>
  /// Assigns XData extracting from class object to given AutoCAD objects.
  /// </summary>
  /// <typeparam name="T">Type of class object contains XData information, specifies by <see cref="XDataAttribute"/>.</typeparam>
  /// <param name="scope">Represents to <see cref="Transaction"/>.</param>
  /// <param name="targetId">ID of an AutoCAD object.</param>
  /// <param name="data">Object contains information to contructs XData.</param>
  public static void AssignXData<T>( TransactionScope scope, ObjectId targetId, T data )
    where T : class, new()
  {
    AssignXData( scope, [ targetId ], data );
  }

  /// <summary>
  /// Assigns XData extracting from class object to given AutoCAD objects.
  /// </summary>
  /// <typeparam name="T">Type of class object contains XData information, specifies by <see cref="XDataAttribute"/>.</typeparam>
  /// <param name="scope">Represents to <see cref="Transaction"/>.</param>
  /// <param name="targetIdCollection">Collection of AutoCAD object IDs.</param>
  /// <param name="data">Object contains information to contructs XData.</param>
  /// <exception cref="InvalidOperationException"></exception>
  public static void AssignXData<T>( TransactionScope scope, ObjectIdCollection targetIdCollection, T data )
    where T : class, new()
  {
    AssignXData( scope, targetIdCollection.Cast<ObjectId>(), data );
  }

  /// <summary>
  /// Add new <see cref="RegAppTableRecord"/> with given name to AutoCAD database if not exists.
  /// </summary>
  /// <param name="scope">Represents to <see cref="Transaction"/>.</param>
  /// <param name="name">Name of reg app.</param>
  public static void EnsureRegAppExists( TransactionScope scope, string name )
  {
    scope.RegApps.Create( name );
  }

  /// <summary>
  /// Retrieves reg app name declared in <see cref="XDataAttribute"/> from a type.
  /// </summary>
  /// <param name="type">Class type decorated with <see cref="XDataAttribute"/>.</param>
  /// <param name="result">Reg app name.</param>
  /// <returns>true if operation successfully, otherwise false.</returns>
  private static bool TryGetRegAppName( Type type, [NotNullWhen( true )] out string? result )
  {
    result = type.GetCustomAttribute<XDataAttribute>()!.RegAppName;
    return result != null;
  }

  /// <summary>
  /// Extracts result buffer from class object. 
  /// A property of an object corresponding to a <see cref="TypedValue"/> 
  /// if it has decorated with <see cref="XPropertyAttribute"/> .
  /// </summary>
  /// <typeparam name="T">Type of class object contains XData information, specifies by <see cref="XDataAttribute"/>.</typeparam>
  /// <param name="data">Object contains information to contructs XData.</param>
  /// <param name="result">An result buffer object as XData.</param>
  /// <returns>true if <paramref name="data"/> successfully extracted to <see cref="ResultBuffer"/>, otherwise false</returns>
  /// <exception cref="InvalidOperationException"></exception>
  private static bool TryExtractResultBuffer<T>( T data, [NotNullWhen( true )] out ResultBuffer? result )
  {
    result = default;
    Type classType = typeof( T );
    if ( !TryGetRegAppName( classType, out string? regAppName ) )
      throw new InvalidOperationException( "Failed to get reg app name from class definition." );

    XDataBuilder factory = XDataBuilder.New().AddDataRegAppName( regAppName );

    classType.GetProperties().ToList().ForEach( p =>
    {
      XPropertyAttribute propAttr = p.GetCustomAttribute<XPropertyAttribute>()!;
      string name = propAttr.Name;
      DxfCode code = propAttr.Code;
      bool serialize = propAttr.Serialize;
      object? value = p.GetValue( data, null );

      if ( value == null )  // Skip null value
        return;

      factory.AddDataAsciiString( name );
      if ( serialize ) {
        string valueString = JsonConvert.SerializeObject( value );
        factory.AddDataAsciiString( valueString );
        return;   // Continue
      }
      if ( IsValidDxfCode( code ) )
        factory.AddData( value, code );
      else
        factory.AddData( value );
    } );

    if ( !factory.HasData )
      return false;

    result = factory.ToResultBuffer();

    return true;
  }

  #endregion

  #region Get XData

  public static ResultBuffer? GetResultBuffer( TransactionScope scope, ObjectId dbObjectId, string regAppName )
  {
    return scope.GetObject<DBObject>( dbObjectId )?.GetXDataForApplication( regAppName );
  }

  public static ResultBuffer? GetResultBuffer<T>( TransactionScope scope, ObjectId dbObjectId )
    where T : class, new()
  {
    Type classType = typeof( T );
    if ( !TryGetRegAppName( classType, out string? regAppName ) )
      throw new InvalidOperationException( "Failed to get reg app name from class definition." );

    return GetResultBuffer( scope, dbObjectId, regAppName );
  }

  private static Dictionary<string, object> GetValueDictionary( ResultBuffer resultBuffer )
  {
    TypedValue[] typedValues = resultBuffer.AsArray();
    Dictionary<string, object> result = new Dictionary<string, object>();
    for ( int i = 1; i < typedValues.Length; i += 2 ) {
      if ( i + 1 == typedValues.Length )
        break;

      if ( typedValues[ i ].TypeCode != ( int ) DxfCode.ExtendedDataAsciiString )
        continue;

      if ( !IsValidDxfCode( ( DxfCode ) typedValues[ i + 1 ].TypeCode ) )
        continue;

      result.Add( ( string ) typedValues[ i ].Value, typedValues[ i + 1 ].Value );
    }
    return result;
  }

  private static Dictionary<string, PropertyInfo> GetPropertyInfoDictionary( Type type )
  {
    return type.GetProperties().ToDictionary( p => p.GetCustomAttribute<XPropertyAttribute>()!.Name, p => p );
  }

  private static bool TryMapValues<T>( Dictionary<string, PropertyInfo> propertyDict, Dictionary<string, object> valueDict, [NotNullWhen( true )] out T? dataObj )
  {
    dataObj = ( T? ) Activator.CreateInstance( typeof( T ) );
    if ( dataObj == null )
      return false;
    foreach ( KeyValuePair<string, PropertyInfo> propertyEntry in propertyDict ) {
      if ( !valueDict.TryGetValue( propertyEntry.Key, out object? value ) ) {
        if ( !propertyEntry.Value.PropertyType.IsNullable() )
          return false;

        continue;
      }
      propertyEntry.Value.SetValue( dataObj, value );
    }
    return true;
  }

  public static T? GetXPropertyValue<T>( TransactionScope scope, ObjectId dbObjectId, string regAppName, string propertyName ) where T : notnull
  {
    ResultBuffer? resultBuffer = GetResultBuffer( scope, dbObjectId, regAppName );
    if ( resultBuffer == null )
      return default;

    if ( !GetValueDictionary( resultBuffer ).TryGetValue( propertyName, out object? result ) )
      return default;

    return ( T ) result;
  }

  public static T? GetXData<T>( TransactionScope scope, ObjectId dbObjectId )
    where T : class, new()
  {
    ResultBuffer? resultBuffer = GetResultBuffer<T>( scope, dbObjectId );
    if ( resultBuffer == null )
      return default;

    TryMapValues( GetPropertyInfoDictionary( typeof( T ) ), GetValueDictionary( resultBuffer ), out T? result );

    return result;
  }

  #endregion

  #region Validation

  private static readonly DxfCode[] validExtendedDxfCodes = [
    DxfCode.ExtendedDataAsciiString,
    DxfCode.ExtendedDataRegAppName,
    DxfCode.ExtendedDataControlString,
    DxfCode.ExtendedDataLayerName,
    DxfCode.ExtendedDataBinaryChunk,
    DxfCode.ExtendedDataHandle,
    DxfCode.ExtendedDataXCoordinate,
    DxfCode.ExtendedDataYCoordinate,
    DxfCode.ExtendedDataZCoordinate,
    DxfCode.ExtendedDataWorldXCoordinate,
    //DxfCode.ExtendedDataWorldYCoordinate,
    //DxfCode.ExtendedDataWorldZCoordinate,
    DxfCode.ExtendedDataWorldXDisp,
    //DxfCode.ExtendedDataWorldYDisp,
    //DxfCode.ExtendedDataWorldZDisp,
    DxfCode.ExtendedDataWorldXDir,
    //DxfCode.ExtendedDataWorldYDir,
    //DxfCode.ExtendedDataWorldZDir,
    DxfCode.ExtendedDataReal,
    DxfCode.ExtendedDataDist,
    DxfCode.ExtendedDataScale,
    DxfCode.ExtendedDataInteger16,
    DxfCode.ExtendedDataInteger32
  ];

  private static bool IsValidDxfCode( DxfCode code ) => validExtendedDxfCodes.Contains( code );

  #endregion
}