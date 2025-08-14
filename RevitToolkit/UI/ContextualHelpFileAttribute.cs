namespace RevitToolkit.UI;

public abstract class ContextualHelpFileAttribute : ExternalResourceAttribute
{
  protected string FileName { get; }

  public ContextualHelp ContextualHelp => new( ContextualHelpType.ChmFile, GetFullPath( FileName ) );

  public ContextualHelpFileAttribute( string fileName ) : base()
  {
    FileName = fileName;
  }
}
