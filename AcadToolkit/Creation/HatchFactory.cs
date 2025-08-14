using AcadToolkit.Interops;

namespace AcadToolkit.Creation;

public sealed class HatchFactory : EntityFactory<Hatch>
{
  private readonly string _patternName;
  private readonly HatchPatternType _patternType;
  private readonly ObjectIdCollection _boundaries;
  private double _patternAngle;
  private double _patternScale;

  public HatchFactory( HatchPatternType patternType, string patternName )
  {
    _patternName = patternName;
    _patternType = patternType;
    _boundaries = new ObjectIdCollection();
    _patternScale = 1.0;
  }

  public static HatchFactory NewPreDefined( string patternName ) => new HatchFactory( HatchPatternType.PreDefined, patternName );

  public static HatchFactory NewUserDefined( string patternName ) => new HatchFactory( HatchPatternType.PreDefined, patternName );

  public static HatchFactory NewCustomDefined( string patternName ) => new HatchFactory( HatchPatternType.PreDefined, patternName );

  public HatchFactory SetPatternAngle( double angle )
  {
    _patternAngle = angle;
    return this;
  }

  public HatchFactory SetPatternScale( double scale )
  {
    _patternScale = scale;
    return this;
  }

  protected override Hatch InitializeEntity( TransactionScope scope )
  {
    // TODO: hatch factory, add more case
    Hatch hatch = new();
    hatch.SetHatchPattern( _patternType, _patternName );
    hatch.Associative = true;
    hatch.PatternAngle = _patternAngle;
    hatch.PatternScale = _patternScale;
    hatch.AppendLoop( HatchLoopTypes.Outermost, _boundaries );
    hatch.EvaluateHatch( true );
    return hatch;
  }
}
