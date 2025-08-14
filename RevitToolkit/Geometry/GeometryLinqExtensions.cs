using RevitToolkit.Geometry;
using RevitToolkit.Helper;

namespace RevitToolkit.Geometry;
public static class GeometryLinqExtensions
{
  public static IEnumerable<T> GeometryObjects<T>( this GeometryElement source ) where T : GeometryObject
  {
    return source.OfType<T>();
  }

  public static T? SingleOrDefault<T>( this GeometryElement source ) where T : GeometryObject
  {
    return source.OfType<T>().SingleOrDefault();
  }

  public static T? SingleOrDefaultWhich<T>( this GeometryElement source, Func<T, bool> predicate ) where T : GeometryObject
  {
    return source.OfType<T>().SingleOrDefault( predicate );
  }

  public static T? FirstOrDefault<T>( this GeometryElement source ) where T : GeometryObject
  {
    return source.OfType<T>().FirstOrDefault();
  }

  public static T? FirstOrDefaultWhich<T>( this GeometryElement source, Func<T, bool> predicate ) where T : GeometryObject
  {
    return source.OfType<T>().FirstOrDefault( predicate );
  }

  public static T First<T>( this GeometryElement source ) where T : GeometryObject
  {
    return source.OfType<T>().First();
  }

  public static T FirstWhich<T>( this GeometryElement source, Func<T, bool> predicate ) where T : GeometryObject
  {
    return source.OfType<T>().First( predicate );
  }

  public static IEnumerable<Solid> Solids( this GeometryElement source )
  {
    return source.GeometryObjects<Solid>();
  }

  public static IEnumerable<Solid> SolidsExcludeEmpty( this GeometryElement source )
  {
    return source.GeometryObjects<Solid>().Where( s => !s.Volume.IsAlmostEqualsToZero() );
  }

  public static IEnumerable<TFace> Faces<TFace>( this Solid source ) where TFace : Face
  {
    return source.Faces.OfType<TFace>();
  }

  public static IEnumerable<PlanarFace> VerticalPlanarFaces( this Solid source )
  {
    return source.Faces<PlanarFace>().Where( p => p.FaceNormal.CrossProduct( XYZ.BasisX ).IsParallelTo( XYZ.BasisZ ) || p.FaceNormal.CrossProduct( XYZ.BasisX ).IsZeroLength() );
  }

  public static IEnumerable<PlanarFace> HorizontalPlanarFaces( this Solid source )
  {
    return source.Faces<PlanarFace>().Where( p => p.FaceNormal.IsParallelTo( XYZ.BasisZ ) );
  }

  public static PlanarFace? BottommostHorizontalPlanarFace( this Solid source )
  {
    return source.HorizontalPlanarFaces().OrderBy( p => p.Origin.Z ).FirstOrDefault();
  }

  public static PlanarFace? TopmostHorizontalPlanarFace( this Solid source )
  {
    return source.HorizontalPlanarFaces().OrderBy( p => p.Origin.Z ).LastOrDefault();
  }

  public static IEnumerable<EdgeArray> EdgeLoops( this Face source )
  {
    return source.EdgeLoops.OfType<EdgeArray>();
  }

  public static IEnumerable<EdgeArray> EdgeLoopsWhich( this Face source, Func<EdgeArray, bool> predicate )
  {
    return source.EdgeLoops.OfType<EdgeArray>().Where( predicate );
  }

  public static EdgeArray SingleEdgeLoop( this Face source )
  {
    return source.EdgeLoops.OfType<EdgeArray>().First();
  }

  public static IEnumerable<TCurve> SingleCurveLoop<TCurve>( this Face source ) where TCurve : Curve
  {
    return source.GetEdgesAsCurveLoops().First().OfType<TCurve>();
  }

  public static EdgeArray? EdgeLoopsAt( this Face source, int index )
  {
    var loops = source.EdgeLoops();
    return index < loops.Count() ? loops.ElementAt( index ) : null;
  }

  public static IEnumerable<TCurve> AsCurves<TCurve>( this EdgeArray source ) where TCurve : Curve
  {
    return source.OfType<Edge>().Select( e => e.AsCurve() ).OfType<TCurve>();
  }

  public static IEnumerable<TCurve> AsCurvesWhich<TCurve>( this EdgeArray source, Func<TCurve, bool> predicate ) where TCurve : Curve
  {
    return source.AsCurves<TCurve>().Where( predicate );
  }

  public static TCurve FirstCurveWhich<TCurve>( this EdgeArray source, Func<TCurve, bool> predicate ) where TCurve : Curve
  {
    return source.AsCurves<TCurve>().Where( predicate ).First();
  }

  public static IEnumerable<XYZ> GetEndPoints( this Curve source )
  {
    return [ source.GetEndPoint( 0 ), source.GetEndPoint( 1 ) ];
  }

  public static IEnumerable<XYZ> Vertices( this EdgeArray source )
  {
    return source.OfType<Edge>().Select( e => e.AsCurve() )
      .SelectMany( c => c.GetEndPoints() )
      .Distinct( new XYZComparer() );
  }

  public static IEnumerable<XYZ> VerticesWhich( this EdgeArray source, Func<XYZ, bool> predicate )
  {
    return source.Vertices().Where( predicate );
  }

  public static IEnumerable<Curve> GetCurves( this PlanarFace planarFace )
  {
    return planarFace.GetEdgesAsCurveLoops().SelectMany( curveLoop => curveLoop );
  }
}
