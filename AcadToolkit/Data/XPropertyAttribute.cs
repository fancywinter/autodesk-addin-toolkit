namespace AcadToolkit.Data;
public sealed class XPropertyAttribute : Attribute
{
  public string Name { get; }

  public DxfCode Code { get; set; }

  public bool Serialize { get; }

  public XPropertyAttribute( string name )
  {
    Code = DxfCode.Invalid;
    Name = name;
  }
}
