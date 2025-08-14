using Autodesk.Revit.DB.ExtensibleStorage;

namespace RevitToolkit.Storage.Attributes;

/// <summary>
/// Read access level of the schema.
/// </summary>
public class ReadAccessAttribute : Attribute
{
  public AccessLevel Level { get; set; } = AccessLevel.Public;

  public ReadAccessAttribute( AccessLevel level )
  {
    Level = level;
  }
}
