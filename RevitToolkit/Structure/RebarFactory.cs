using Autodesk.Revit.DB.Structure;
using RevitToolkit.Helper;

namespace RevitToolkit.Structure;
public class RebarFactory
{
  private RebarHookType? _startHookType;
  private RebarHookOrientation _startHookOrientation;
  private double? _startHookLength;
  private double _startHookRotation;
  private RebarHookType? _endHookType;
  private RebarHookOrientation _endHookOrientation;
  private double? _endHookLength;
  private double _endHookRotation;
  private RebarBarType _barType;
  public XYZ _rebarSetNormal;
  public IList<Curve> _rebarCurves;
  public bool _useExistingShapeIfPossible;
  public bool _createNewShape;
  public RebarStyle _rebarStyle;
  public string? _scheduleMark;
  private DistributionType _distributionType;
  private bool _includeFirstBar;
  private bool _includeLastBar;
  private int? _excludeBarsStartIndex;
  private int _excludeBarsCount;
  private List<int> _excludeBarsAtIndices;

  private RebarFactory( RebarBarType barType, IList<Curve> rebarCurves, XYZ normal )
  {
    _rebarSetNormal = normal;
    _rebarCurves = rebarCurves;
    _barType = barType;
    _startHookOrientation = RebarHookOrientation.Left;
    _endHookOrientation = RebarHookOrientation.Left;
    _useExistingShapeIfPossible = true;
    _createNewShape = true;
    _distributionType = DistributionType.Uniform;
    _includeFirstBar = true;
    _includeLastBar = true;
    _excludeBarsAtIndices = new List<int>();
  }

  public static RebarFactory New( RebarBarType barType, IList<Curve> rebarCurves, XYZ normal )
  {
    return new RebarFactory( barType, rebarCurves, normal );
  }

  public static RebarFactory New( RebarBarType barType, Curve rebarCurve, XYZ normal )
  {
    return new RebarFactory( barType, [ rebarCurve ], normal );
  }

  public RebarFactory SetAsStirrupTie()
  {
    _rebarStyle = RebarStyle.StirrupTie;
    return this;
  }

  public RebarFactory SetScheduleMark( string mark )
  {
    _scheduleMark = mark;
    return this;
  }

  public RebarFactory VaryingLength()
  {
    _distributionType = DistributionType.VaryingLength;
    return this;
  }

  public RebarFactory ExcludeFirstBar()
  {
    _includeFirstBar = false;
    return this;
  }

  public RebarFactory ExcludeLastBar()
  {
    _includeLastBar = false;
    return this;
  }

  public RebarFactory SetStartHook( RebarHookType hookType, bool orientRight = false, double hookRotation = 0, double? overrideLength = null )
  {
    _startHookType = hookType;
    _startHookOrientation = orientRight ? RebarHookOrientation.Right : RebarHookOrientation.Left;
    _startHookLength = overrideLength;
    _startHookRotation = hookRotation;
    return this;
  }

  public RebarFactory SetEndHook( RebarHookType hookType, bool orientRight = false, double hookRotation = 0, double? overrideLength = null )
  {
    _endHookType = hookType;
    _endHookOrientation = orientRight ? RebarHookOrientation.Right : RebarHookOrientation.Left;
    _endHookLength = overrideLength;
    _endHookRotation = hookRotation;
    return this;
  }

  public RebarFactory ExcludeBars( int startIndex, int count )
  {
    _excludeBarsStartIndex = startIndex;
    _excludeBarsCount = count;
    return this;
  }

  public RebarFactory ExcludeBar( int index )
  {
    _excludeBarsAtIndices.Add( index );
    return this;
  }

  public RebarFactory FlipHooks()
  {
    _startHookOrientation = _startHookOrientation == RebarHookOrientation.Left
       ? RebarHookOrientation.Right
       : RebarHookOrientation.Left;
    _endHookOrientation = _endHookOrientation == RebarHookOrientation.Left
       ? RebarHookOrientation.Right
       : RebarHookOrientation.Left;
    return this;
  }

  public Rebar CreateSingleRebar( Element host )
  {

    Rebar rebar = Rebar.CreateFromCurves(
       host.Document, _rebarStyle, _barType,
       _startHookType, _endHookType,
       host, _rebarSetNormal, _rebarCurves,
       _startHookOrientation, _endHookOrientation,
       _useExistingShapeIfPossible, _createNewShape );
    rebar.DistributionType = _distributionType;
    if ( _startHookType != null ) {
      if ( _startHookLength != null ) {
        rebar.EnableHookLengthOverride( true );
        rebar.GetBendData().HookLength0 = _startHookLength.Value;
      }
      if ( !_startHookRotation.IsAlmostEqualsToZero() )
        rebar.SetHookRotationAngle( _startHookRotation, 0 );
    }
    if ( _endHookType != null ) {
      if ( _endHookLength != null ) {
        rebar.EnableHookLengthOverride( true );
        rebar.GetBendData().HookLength1 = _endHookLength.Value;
      }
      if ( !_endHookRotation.IsAlmostEqualsToZero() )
        rebar.SetHookRotationAngle( _endHookRotation, 1 );
    }
    if ( !string.IsNullOrEmpty( _scheduleMark ) )
      rebar.ScheduleMark = _scheduleMark;
    return rebar;
  }

  private void ExcludeBarsInSet( Rebar rebar )
  {
    rebar.IncludeFirstBar = _includeFirstBar;
    rebar.IncludeLastBar = _includeLastBar;

    if ( _excludeBarsAtIndices.Count > 0 )
      _excludeBarsAtIndices.ForEach( i => rebar.SetBarIncluded( false, i ) );

    if ( _excludeBarsStartIndex != null && _excludeBarsCount > 0 ) {
      int maxIndex = _excludeBarsStartIndex.Value + _excludeBarsCount;
      for ( int i = _excludeBarsStartIndex.Value; i < maxIndex; i++ ) {
        rebar.SetBarIncluded( false, i );
      }
    }
  }

  public Rebar CreateLayoutAsFixedNumber(
     Element host,
     int numberOfBarPositions,
     double arrayLength,
     bool barsOnNormalSide = true )
  {
    Rebar rebar = CreateSingleRebar( host );
    rebar.GetShapeDrivenAccessor()
      .SetLayoutAsFixedNumber( numberOfBarPositions, arrayLength, barsOnNormalSide, _includeFirstBar, _includeLastBar );
    ExcludeBarsInSet( rebar );
    return rebar;
  }

  public Rebar CreateLayoutAsMaximumSpacing(
     Element host,
     double spacing,
     double arrayLength,
     bool barsOnNormalSide = true )
  {
    Rebar rebar = CreateSingleRebar( host );
    rebar.GetShapeDrivenAccessor()
      .SetLayoutAsMaximumSpacing( spacing, arrayLength, barsOnNormalSide, true, true );
    ExcludeBarsInSet( rebar );
    return rebar;
  }

  public Rebar CreateLayoutMinimumClearSpacing(
     Element host,
     double spacing,
     double arrayLength,
     bool barsOnNormalSide = true )
  {
    Rebar rebar = CreateSingleRebar( host );
    rebar.GetShapeDrivenAccessor()
      .SetLayoutAsMinimumClearSpacing( spacing, arrayLength, barsOnNormalSide, true, true );
    ExcludeBarsInSet( rebar );
    return rebar;
  }

  public Rebar CreateLayoutAsNumberWithSpacing(
     Element host,
     int numberOfBarPositions,
     double spacing,
     bool barsOnNormalSide = true )
  {
    Rebar rebar = CreateSingleRebar( host );
    rebar.GetShapeDrivenAccessor()
      .SetLayoutAsNumberWithSpacing( numberOfBarPositions, spacing, barsOnNormalSide, true, true );
    ExcludeBarsInSet( rebar );
    return rebar;
  }
}
