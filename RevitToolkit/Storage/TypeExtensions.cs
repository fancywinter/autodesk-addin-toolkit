namespace RevitToolkit.Storage;

internal static class TypeExtensions
{
  internal static bool TryGetListItemType( this Type listType, [NotNullWhen( true )] out Type? result )
  {
    Type[] arguments = listType.GetGenericArguments();
    result = arguments.Length == 1 ? arguments[ 0 ] : default;
    return arguments.Length == 1;
  }

  internal static Type? GetDictKeyType( this Type dictType )
  {
    Type[] arguments = dictType.GetGenericArguments();
    return arguments.Length == 2 ? arguments[ 0 ] : default;
  }
}
