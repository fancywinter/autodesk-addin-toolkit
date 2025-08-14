using System.Reflection;

namespace RevitToolkit.UI;

internal static class ReflectionExtensions
{
  internal static bool TryGetAttribute<TAttribute>( this Type type, [NotNullWhen( true )] out TAttribute? result ) where TAttribute : Attribute
  {
    result = type.GetCustomAttribute<TAttribute>();
    return result != null;
  }

  internal static bool TryExecuteNonPublicMethod<TResult>( this object instance, string methodName, [NotNullWhen( true )] out TResult? result, params object[]? parameters )
  {
    result = default;
    try {
      MethodInfo? mi = instance.GetType().GetMethod( methodName, BindingFlags.NonPublic | BindingFlags.Instance );
      if ( mi?.Invoke( instance, parameters ) is not TResult ret )
        return false;
      result = ret;
    }
    catch {
      return false;
    }
    return true;
  }
}
