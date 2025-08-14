using System.Reflection;

namespace RevitToolkit.UI;

public sealed class RibbonBuilder
{
  private readonly UIControlledApplication _application;
  private readonly string _tabName;
  private RibbonPanel? _currentPanel;

  [MemberNotNullWhen( true, nameof( _currentPanel ) )]
  public bool HasAnyPanels { get; private set; }

  public RibbonBuilder( UIControlledApplication application, string tabName )
  {
    _tabName = tabName;
    _application = application;
    _application.CreateRibbonTab( _tabName );
  }

  public static RibbonBuilder New( UIControlledApplication application, string tabName ) => new( application, tabName );

  public RibbonBuilder CreatePanel( string panelName )
  {
    _currentPanel = _application.CreateRibbonPanel( _tabName, panelName );
    HasAnyPanels = _currentPanel != null;
    return this;
  }

  public RibbonBuilder AddSeparator()
  {
    if ( !HasAnyPanels )
      throw new InvalidOperationException( "Ribbon does not have any panel" );

    _currentPanel.AddSeparator();

    return this;
  }

  #region Stack butons

  public RibbonBuilder AddStackedButtons( Type commandType1, Type commandType2 )
  {
    if ( !HasAnyPanels )
      throw new InvalidOperationException( "Ribbon does not have any panel" );
    PushButtonData buttonData1 = CreatePushButtonData( commandType1 );
    PushButtonData buttonData2 = CreatePushButtonData( commandType2 );
    IList<RibbonItem> buttons = _currentPanel.AddStackedItems( buttonData1, buttonData2 );
    AssignToolTip( buttons[ 0 ], commandType1 );
    AssignToolTip( buttons[ 1 ], commandType2 );

    return this;
  }

  public RibbonBuilder AddStackedButtons( Type commandType1, Type commandType2, Type commandType3 )
  {
    if ( !HasAnyPanels )
      throw new InvalidOperationException( "Ribbon does not have any panel" );
    PushButtonData buttonData1 = CreatePushButtonData( commandType1 );
    PushButtonData buttonData2 = CreatePushButtonData( commandType2 );
    PushButtonData buttonData3 = CreatePushButtonData( commandType3 );
    IList<RibbonItem> buttons = _currentPanel.AddStackedItems( buttonData1, buttonData2, buttonData3 );
    AssignToolTip( buttons[ 0 ], commandType1 );
    AssignToolTip( buttons[ 1 ], commandType2 );
    AssignToolTip( buttons[ 2 ], commandType3 );

    return this;
  }

  #endregion

  #region Split button

  public RibbonBuilder AddSplitButton( string name, string text, params Type[] commandTypes )
  {
    if ( !HasAnyPanels )
      throw new InvalidOperationException( "Ribbon does not have any panel" );
    SplitButtonData splitButtonData = new( name, text );
    SplitButton splitButton = ( SplitButton ) _currentPanel.AddItem( splitButtonData );
    foreach ( Type commandType in commandTypes )
      CreatePushButton( splitButton, commandType );

    return this;
  }

  public RibbonBuilder AddSplitButton( string name, params Type[] commandTypes )
  {
    AddSplitButton( name, string.Empty, commandTypes );

    return this;
  }

  #endregion

  #region Push button

  public RibbonBuilder AddPushButton<TCommand>() where TCommand : IExternalCommand, new()
  {
    if ( !HasAnyPanels )
      throw new InvalidOperationException( "Ribbon does not have any panel" );
    CreatePushButton( _currentPanel, typeof( TCommand ) );
    return this;
  }

  private void CreatePushButton( object host, Type commandType )
  {
    if ( !commandType.TryGetAttribute( out RevitCommandAttribute? commandAttr ) )
      return;

    var buttonData = CreatePushButtonData( commandType );

    RibbonItem? item = null;
    if ( host is RibbonPanel panel )
      item = panel.AddItem( buttonData );
    else if ( host is SplitButton splitButton )
      item = splitButton.AddPushButton( buttonData );

    if ( item == null )
      return;

    AssignToolTip( item, commandType );
  }

  private void AssignToolTip( RibbonItem item, Type commandType )
  {
    ToolTipAttribute? tooltipAttr = commandType.GetCustomAttribute<ToolTipAttribute>();
    ContextualHelpFileAttribute? chFileAttr = commandType.GetCustomAttribute<ContextualHelpFileAttribute>();
    ContextualHelpUrlAttribute? chUrlAttr = commandType.GetCustomAttribute<ContextualHelpUrlAttribute>();

    ContextualHelp? contextualHelp = chFileAttr?.ContextualHelp ?? chUrlAttr?.ContextualHelp;
    if ( contextualHelp is not null )
      item.SetContextualHelp( contextualHelp );
    if ( RibbonHelper.GetInternalRibbonItem( item ) is Autodesk.Windows.RibbonItem internalItem )
      internalItem.ToolTip = tooltipAttr?.Tooltip;
  }

  private PushButtonData CreatePushButtonData( Type commandType )
  {
    RevitCommandAttribute commandAttr = commandType.GetCustomAttribute<RevitCommandAttribute>()!;
    IEnumerable<ImageSourceAttribute> imgSrcAtts = commandType.GetCustomAttributes<ImageSourceAttribute>();

    ImageSourceAttribute? largeImgSrcAttr = imgSrcAtts.FirstOrDefault( att => att.LargeSize );
    ImageSourceAttribute? imgSrcAttr = imgSrcAtts.FirstOrDefault( att => !att.LargeSize );

    return new PushButtonData( commandAttr.Name, commandAttr.Text, commandType.Assembly.Location, commandType.FullName )
    {
      LongDescription = commandAttr.Description,
      LargeImage = largeImgSrcAttr?.ImageSource,
      Image = imgSrcAttr?.ImageSource
    };
  }

  #endregion
}