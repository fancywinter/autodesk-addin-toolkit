namespace RevitToolkit.Storage.Attributes;

/// <summary>
/// Indicate that a type stores information of Revit Schema object and able to constructs schema from that type.
/// </summary>
[AttributeUsage( AttributeTargets.Class | AttributeTargets.Struct )]
public class SchemaAttribute : Attribute
{
  /// <summary>
  /// The identifier of the Schema.
  /// </summary>
  public string Guid { get; private set; }

  /// <summary>
  /// The user-friendly name of the Schema.
  /// </summary>
  public string? Name { get; set; }

  /// <summary>
  /// The id of the third-party vendor that may access entities of this Schema under the Vendor access level.
  /// </summary>
  public string? VendorId { get; set; }

  /// <summary>
  /// The GUID of the application or add-in that may access entities of this Schema under the Application access level.
  /// </summary>
  public string? AppGuid { get; set; }

  public SchemaAttribute( string guid )
  {
    Guid = guid;
  }
}
