namespace RevitToolkit.UI;

[AttributeUsage( AttributeTargets.Class, AllowMultiple = false )]
public sealed class RevitCommandAttribute : Attribute
{
  public string Name { get; init; }

  public string Text { get; init; }

  public string Description { get; set; }

  public RevitCommandAttribute( string name, string text )
  {
    Name = name;
    Text = text;
    Description = string.Empty;
  }
}
