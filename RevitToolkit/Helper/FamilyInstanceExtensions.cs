namespace RevitToolkit.Helper;
public static class FamilyInstanceExtensions
{
  public static FamilyInstance? GetNestedInstance( this FamilyInstance instance, string familyName )
  {
    return instance.GetDependentElements( new ElementClassFilter( typeof( FamilyInstance ) ) )
      .Select( id => instance.Document.GetElement( id ) ).OfType<FamilyInstance>()
      .FirstOrDefault( instance => instance.Symbol.FamilyName == familyName );
  }

  public static List<FamilyInstance> GetNestedInstances( this FamilyInstance instance, string familyName )
  {
    return instance.GetDependentElements( new ElementClassFilter( typeof( FamilyInstance ) ) )
      .Select( id => instance.Document.GetElement( id ) as FamilyInstance ).OfType<FamilyInstance>()
      .Where( instance => instance.Symbol.FamilyName == familyName ).ToList();
  }
}
