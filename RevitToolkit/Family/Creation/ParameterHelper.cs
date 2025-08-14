using System.Reflection;

namespace RevitToolkit.Family.Creation;
public static class ParameterHelper
{
  public static T GetParameterValues<T>( Element element ) where T : class, new()
  {
    T parameterValueObject = ( T ) Activator.CreateInstance( typeof( T ) )!;
    PropertyInfo[] props = typeof( T ).GetProperties();
    foreach ( PropertyInfo prop in props ) {
      if ( !prop.CanWrite )
        continue;

      ParameterAttribute paramAttr = prop.GetCustomAttribute<ParameterAttribute>();
      if ( paramAttr == null )
        continue;

      string paramName = paramAttr.Name;
      Parameter matchParam = element.LookupParameter( paramName );

      if ( matchParam == null )
        continue;

      object? paramValue = GetParameterValue( matchParam );
      if ( paramValue == null )
        continue;

      if ( paramAttr.Converter is not null && Activator.CreateInstance( paramAttr.Converter ) is IParameterValueConverter converterInstance ) {
        object convertedValue = converterInstance.ConvertBack( paramValue );
        prop.SetValue( parameterValueObject, convertedValue );
        continue;
      }

      if ( MatchDataType( matchParam, prop.PropertyType ) )
        prop.SetValue( parameterValueObject, paramValue );
    }
    return parameterValueObject;
  }

  public static void SetParameterValues<T>( Element element, T parameterValues ) where T : class, new()
  {
    PropertyInfo[] props = typeof( T ).GetProperties();
    foreach ( PropertyInfo prop in props ) {
      ParameterAttribute paramAttr = prop.GetCustomAttribute<ParameterAttribute>();
      if ( paramAttr == null )
        continue;

      string paramName = paramAttr.Name;
      Parameter matchParam = element.LookupParameter( paramName );

      if ( matchParam == null )
        continue;

      if ( matchParam.IsReadOnly )
        continue;

      object? paramValue = GetParameterValue( matchParam );
      if ( paramValue == null )
        continue;

      object propValue = prop.GetValue( parameterValues );
      if ( propValue == null )
        continue;

      if ( paramAttr.Converter is not null && Activator.CreateInstance( paramAttr.Converter ) is IParameterValueConverter converterInstance ) {
        object convertedValue = converterInstance.Convert( propValue );
        SetParameterValue( matchParam, convertedValue );
        continue;
      }

      if ( MatchDataType( matchParam, prop.PropertyType ) )
        SetParameterValue( matchParam, propValue );
    }
  }

  private static bool MatchDataType( Parameter param, Type propertyType )
  {
    switch ( param.StorageType ) {
      case StorageType.Double:
        return propertyType == typeof( double );

      case StorageType.ElementId:
        return propertyType == typeof( ElementId );

      case StorageType.Integer:
        return SpecTypeId.Boolean.YesNo == param.Definition.GetDataType()
           ? propertyType == typeof( bool )
           : propertyType == typeof( int );

      case StorageType.String:
        return propertyType == typeof( string );

      default:
        return false;
    }
  }

  private static object? GetParameterValue( Parameter param )
  {
    switch ( param.StorageType ) {
      case StorageType.Double:
        return param.AsDouble();

      case StorageType.ElementId:
        return param.AsElementId();

      case StorageType.Integer:
        return SpecTypeId.Boolean.YesNo == param.Definition.GetDataType()
           ? param.AsInteger() == 1
           : param.AsInteger();

      case StorageType.String:
        return param.AsString();

      default:
        return null;
    }
  }

  private static void SetParameterValue( Parameter param, object value )
  {
    switch ( param.StorageType ) {
      case StorageType.Double:
        if ( value is double doubleValue )
          param.Set( doubleValue );
        break;

      case StorageType.ElementId:
        if ( value is ElementId idValue )
          param.Set( idValue );
        break;

      case StorageType.Integer:
        if ( value is bool boolValue )
          param.Set( boolValue ? 1 : 0 );
        else if ( value is int intValue )
          param.Set( intValue );
        break;

      case StorageType.String:
        if ( value is string stringValue )
          param.Set( stringValue );
        break;

      default:
        break;
    }
  }
}
