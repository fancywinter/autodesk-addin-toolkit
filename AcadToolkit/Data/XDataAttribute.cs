namespace AcadToolkit.Data;
public sealed class XDataAttribute : Attribute
{
  public string RegAppName { get; }

  public XDataAttribute( string regAppName )
  {
    RegAppName = regAppName;
  }
}
