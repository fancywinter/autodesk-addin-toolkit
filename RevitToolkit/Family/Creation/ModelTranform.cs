namespace RevitToolkit.Family.Creation;

public interface IModelTranform
{
  void ApplyTransform( FamilyInstance instance );
}

public class ModelTranslation : IModelTranform
{
  private XYZ _displacement;

  public ModelTranslation( XYZ displacement )
  {
    _displacement = displacement;
  }
  public void ApplyTransform( FamilyInstance instance )
  {
    ElementTransformUtils.MoveElement( instance.Document, instance.Id, _displacement );
  }
}

public class ModelRotation : IModelTranform
{
  private Line _axis;
  private double _angle;
  public ModelRotation( Line axis, double angle )
  {
    _axis = axis;
    _angle = angle;
  }
  public void ApplyTransform( FamilyInstance instance )
  {
    ElementTransformUtils.RotateElement( instance.Document, instance.Id, _axis, _angle );
  }
}

public class ModelMirror : IModelTranform
{
  private Plane _plane;
  public ModelMirror( Plane plane )
  {
    _plane = plane;
  }
  public void ApplyTransform( FamilyInstance instance )
  {
    if ( ElementTransformUtils.CanMirrorElement( instance.Document, instance.Id ) )
      ElementTransformUtils.MirrorElement( instance.Document, instance.Id, _plane );
  }
}
