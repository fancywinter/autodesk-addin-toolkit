namespace RevitToolkit.Family.Creation;
public class ParameterAttribute : Attribute
{
  public string Name { get; }
  public Type? Converter { get; }

  public ParameterAttribute( string name, Type? converter = null )
  {
    Name = name;
    Converter = converter;
  }
}
