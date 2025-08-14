namespace RevitToolkit.Family.Creation;
public abstract class BaseAdaptiveFamilyModel : BaseFamilyModel
{
  public IList<XYZ> PlacementPoints { get; }

  protected BaseAdaptiveFamilyModel( string familyName, string symbolName ) 
    : base( familyName, symbolName )
  {
    PlacementPoints = new List<XYZ>();
  }
}
