namespace RevitToolkit.UI;

[AttributeUsage( AttributeTargets.Class, AllowMultiple = false )]
public sealed class ContextualHelpUrlAttribute : Attribute
{
  public ContextualHelp ContextualHelp { get; }

  public ContextualHelpUrlAttribute( string url )
  {
    ContextualHelp = new ContextualHelp( ContextualHelpType.Url, url );
  }
}
