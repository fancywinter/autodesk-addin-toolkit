using RevitToolkit.Helper;

namespace RevitToolkit.Creation;
public static class ModelLineFactory
{
  public static void CreateModelLine( this Document doc, XYZ point1, XYZ point2 )
  {
    Line line = Line.CreateBound( point1, point2 );
    XYZ direction = ( point2 - point1 ).Normalize();
    XYZ up = XYZ.BasisZ;
    if ( direction.DotProduct( up ).IsAlmostEqualsTo( 1 ) ) // Line is nearly vertical
      up = XYZ.BasisX;

    XYZ normal = direction.CrossProduct( up ).Normalize();
    Plane plane = Plane.CreateByNormalAndOrigin( normal, point1 );
    SketchPlane sketchPlane = SketchPlane.Create( doc, plane );
    doc.Create.NewModelCurve( line, sketchPlane );
  }
}
