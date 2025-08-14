namespace RevitToolkit.Storage.Attributes;

public class DocumentationAttribute : Attribute
{
  public string Text { get; set; }

  public DocumentationAttribute( string text )
  {
    Text = text;
  }
}
