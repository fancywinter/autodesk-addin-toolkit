namespace RevitToolkit.Family.Creation;
public abstract class BaseFamilyModel : IFamilyModel
{
  private List<IModelTranform> _transforms;

  public XYZ LocationPoint { get; set; }

  public IReadOnlyCollection<IModelTranform> Transforms => _transforms;

  public string FamilyName { get; private set; }

  public string SymbolName { get; private set; }

  public BaseFamilyModel( string familyName, string symbolName )
  {
    FamilyName = familyName;
    SymbolName = symbolName;
    LocationPoint = XYZ.Zero;
    _transforms = new List<IModelTranform>();
  }

  public BaseFamilyModel Translate( XYZ displacement )
  {
    _transforms.Add( new ModelTranslation( displacement ) );
    return this;
  }

  public BaseFamilyModel Rotate( Line axis, double angle )
  {
    _transforms.Add( new ModelRotation( axis, angle ) );
    return this;
  }

  public BaseFamilyModel Mirror( Plane plane )
  {
    _transforms.Add( new ModelMirror( plane ) );
    return this;
  }
}
