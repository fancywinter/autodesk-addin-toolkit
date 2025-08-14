namespace AcadToolkit.UI;
public class AcadButtonCommand : System.Windows.Input.ICommand
{
  private readonly string _command;
  private readonly bool _activate;
  private readonly bool _wrapUpInactiveDoc;
  private readonly bool _echoCommand;

  public event EventHandler? CanExecuteChanged;

  public AcadButtonCommand( string command, bool activate = true, bool wrapUpInactiveDoc = false, bool echoCommand = true )
  {
    _command = command;
    _activate = activate;
    _wrapUpInactiveDoc = wrapUpInactiveDoc;
    _echoCommand = echoCommand;
  }

  public bool CanExecute( object? parameter )
  {
    return true;
  }


  public void Execute( object? parameter )
  {
    Autodesk.AutoCAD.ApplicationServices.Core.Application.DocumentManager.MdiActiveDocument
      .SendStringToExecute( $"{_command}\n", _activate, _wrapUpInactiveDoc, _echoCommand );
  }
}
