namespace RevitToolkit.Geometry;

public abstract class BaseGeometryElement
{
   /// <summary>
   /// Revit's document.
   /// </summary>
   protected Document Document { get; }

   /// <summary>
   /// Host contains rebars.
   /// </summary>
   public FamilyInstance Instance { get; }

   /// <summary>
   /// Geometry data of host.
   /// </summary>
   public GeometryElement InstanceGeometry { get; }

   /// <summary>
   /// Geometry data of symbol of host.
   /// </summary>
   public GeometryElement SymbolGeometry { get; }

   /// <summary>
   /// Indicates host has been joined or cut by an other family instance.
   /// </summary>
   public bool HasCutOrJoined { get; }

   /// <summary>
   /// Up direction of host.
   /// </summary>
   public XYZ UpDirection { get; }

   protected BaseGeometryElement( FamilyInstance instance )
   {
      Instance = instance;
      Document = Instance.Document;
      Options option = new Options()
      {
         ComputeReferences = true,
         IncludeNonVisibleObjects = true
      };
      InstanceGeometry = Instance.get_Geometry( option );
      GeometryElement? transformedGeo = InstanceGeometry.FirstOrDefault<GeometryInstance>()?.GetInstanceGeometry();
      if ( transformedGeo != null && transformedGeo.Any() )
         InstanceGeometry = transformedGeo;
      else
         HasCutOrJoined = true;
      SymbolGeometry = Instance.get_Geometry( option ).First<GeometryInstance>().GetSymbolGeometry();
      UpDirection = Instance.FacingOrientation.CrossProduct( Instance.HandOrientation );
   }
}
