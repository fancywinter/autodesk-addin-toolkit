namespace RevitToolkit.Helper;
public static class ReferenceHelper
{
  public static string GetSymbolReferenceId( Document doc, Reference reference )
  {
    string sr = reference.ConvertToStableRepresentation( doc );
    string[] tokens = sr.Split( [ ":" ], StringSplitOptions.RemoveEmptyEntries );
    return $"{tokens[ tokens.Count() - 2 ]}:{tokens[ tokens.Count() - 1 ]}";
  }

  public static Reference? GetReference( Element instance, Reference sourceReference )
  {
    string referenceId = GetSymbolReferenceId( instance.Document, sourceReference );
    Reference? referenceFound = null;
    string? id = null;
    switch ( sourceReference.ElementReferenceType ) {
      case ElementReferenceType.REFERENCE_TYPE_NONE:
        id = instance.UniqueId.ToString() + ":0:INSTANCE:" + ( ( FamilyInstance ) instance ).Symbol.UniqueId.ToString() + ":" + referenceId;
        break;

      case ElementReferenceType.REFERENCE_TYPE_LINEAR:
      case ElementReferenceType.REFERENCE_TYPE_SURFACE:
        id = instance.UniqueId.ToString() + ":" + referenceId;
        break;

    }
    if ( id != null )
      referenceFound = Reference.ParseFromStableRepresentation( instance.Document, id );
    return referenceFound;
  }
}
