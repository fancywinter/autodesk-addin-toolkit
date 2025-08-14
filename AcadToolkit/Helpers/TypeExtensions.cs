namespace AcadToolkit.Helpers;

internal static class TypeExtensions
{
  public static bool IsNullable( this Type type, [NotNullWhen( true )] out Type? underlyingType )
  {
    underlyingType = Nullable.GetUnderlyingType( type );
    return underlyingType != null;
  }

  public static bool IsNullable( this Type type ) => type.IsNullable( out _ );
}
