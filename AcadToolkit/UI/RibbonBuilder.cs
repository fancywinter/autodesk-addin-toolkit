using Autodesk.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace AcadToolkit.UI;
public class RibbonBuilder
{
  private readonly RibbonControl _ribbonControl = ComponentManager.Ribbon;
  private RibbonTab? _currentTab;
  private RibbonPanel? _currentPanel;

  public RibbonBuilder()
  {
  }

  public RibbonBuilder AddTab( string id, string title )
  {
    _currentTab = new RibbonTab()
    {
      Id = id,
      Title = title
    };
    _ribbonControl.Tabs.Add( _currentTab );
    return this;
  }

  public RibbonBuilder AddPanel( string panelName )
  {
    if ( _currentTab == null )
      throw new InvalidOperationException( "No tab has been added. Please add a tab first." );

    _currentPanel = new RibbonPanel
    {
      Source = new RibbonPanelSource { Title = panelName },
      FloatingOrientation = Orientation.Horizontal
    };
    _currentTab.Panels.Add( _currentPanel );

    return this;
  }

  public RibbonBuilder AddLargeButton(
    string name,
    string displayText,
    string command,
    string imagePath,
    string tooltipTitle = "",
    string tooltipContent = "" )
  {
    if ( _currentPanel == null )
      throw new InvalidOperationException( "No panel has been added. Please add a panel first." );

    RibbonButton button = CreateButton( name, displayText, command, RibbonItemSize.Large, Orientation.Vertical, null, imagePath, tooltipTitle, tooltipContent );
    _currentPanel.Source.Items.Add( button );

    return this;
  }

  public RibbonBuilder AddStandardButton(
    string name,
    string displayText,
    string command,
    string imagePath,
    string tooltipTitle = "",
    string tooltipContent = "" )
  {
    if ( _currentPanel == null )
      throw new InvalidOperationException( "No panel has been added. Please add a panel first." );

    RibbonButton button = CreateButton( name, displayText, command, RibbonItemSize.Standard, Orientation.Horizontal, imagePath, null, tooltipTitle, tooltipContent );
    _currentPanel.Source.Items.Add( button );

    return this;
  }

  public RibbonBuilder AddStandardButton(
    string name,
    string command,
    string imagePath,
    string tooltipTitle = "",
    string tooltipContent = "" )
  {
    if ( _currentPanel == null )
      throw new InvalidOperationException( "No panel has been added. Please add a panel first." );

    RibbonButton button = CreateButton( name, null, command, RibbonItemSize.Standard, Orientation.Horizontal, imagePath, null, tooltipTitle, tooltipContent );
    _currentPanel.Source.Items.Add( button );

    return this;
  }

  private RibbonButton CreateButton(
    string name,
    string? displayText,
    string command,
    RibbonItemSize size,
    Orientation orientation,
    string? imagePath,
    string? largeImagePath,
    string tooltipTitle = "",
    string tooltipContent = "" )
  {
    return new RibbonButton
    {
      Name = name,
      Text = displayText,
      ShowText = displayText != null,
      CommandHandler = new AcadButtonCommand( command ),
      ToolTip = new RibbonToolTip { Title = tooltipTitle, Content = tooltipContent, Command = command },
      Orientation = orientation,
      Size = size,
      Image = imagePath != null ? new BitmapImage( new Uri( imagePath ) ) : null,
      LargeImage = largeImagePath != null ? new BitmapImage( new Uri( largeImagePath ) ) : null
    };
  }
}
