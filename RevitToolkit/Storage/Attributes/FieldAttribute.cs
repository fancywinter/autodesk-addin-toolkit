namespace RevitToolkit.Storage.Attributes;

/// <summary>
/// Indicate that a member or a property stores information of Revit Schema Field.<br/>
/// Use for following data types:<br/>
/// - <see cref="int"/> , default value is '0'<br/>
/// - <see cref="short"/> , default value is '0'<br/>
/// - <see cref="byte"/> , default value is '0'<br/>
/// - <see cref="double"/> , default value is '0.0'<br/>
/// - <see cref="float"/> , default value is '0.0'<br/>
/// - <see cref="bool"/> , default value is 'false'<br/>
/// - <see cref="string"/> , default value is an string empty<br/>
/// - <see cref="Guid"/> , default value is a empty GUID {00000000-0000-0000-0000-000000000000}<br/>
/// - <see cref="Autodesk.Revit.DB.ElementId"/> , default value is an invalid element ID<br/>
/// - <see cref="Autodesk.Revit.DB.XYZ"/> , default value is (0.0,0.0,0.0)<br/>
/// - <see cref="Autodesk.Revit.DB.UV"/> , default value is (0.0,0.0)<br/>
/// - Data type inherits from <see cref="IList{T}"/>, such as <see cref="List{T}"/> or an array.
/// - Data type inherits from <see cref="IDictionary{TKey, TValue}"/>.
/// </summary>
[AttributeUsage( AttributeTargets.Property | AttributeTargets.Field )]
public class FieldAttribute : Attribute
{
  public string? Name { get; init; }

  public bool CanBeNull { get; set; }

  public FieldAttribute( string? name = null )
  {
    Name = name;
  }
}
