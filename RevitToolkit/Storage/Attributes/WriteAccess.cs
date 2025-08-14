using Autodesk.Revit.DB.ExtensibleStorage;

namespace RevitToolkit.Storage.Attributes;

/// <summary>
/// Write access level of the schema.
/// </summary>
public class WriteAccessAttribute : Attribute
{
  public AccessLevel Level { get; set; } = AccessLevel.Public;

  public WriteAccessAttribute( AccessLevel level )
  {
    Level = level;
  }
}
