using RevitToolkit.Helper;

namespace RevitToolkit.Geometry;
public static class XYZExtensions
{
  public static XYZ GetPointDirectTo( this XYZ left, XYZ right, double distance )
  {
    return left.Add( distance * ( right - left ).Normalize() );
  }

  public static bool IsPerpendicularTo( this XYZ left, XYZ right )
  {
    return left.DotProduct( right ).IsAlmostEqualsToZero( 1e-8 );
  }

  public static bool IsParallelTo( this XYZ left, XYZ right )
  {
    return left.IsSameDirection( right ) || left.IsOppositeDirection( right );
  }

  public static bool IsSameDirection( this XYZ left, XYZ right )
  {
    return left.Normalize().IsAlmostEqualTo( right.Normalize(), 0.01 );
  }

  public static bool IsOppositeDirection( this XYZ left, XYZ right )
  {
    return left.Normalize().Negate().IsAlmostEqualTo( right.Normalize(), 0.01 );
  }

  public static XYZ Translate( this XYZ source, double dx, double dy, double dz )
  {
    return source.TranslateAlongXAxis( dx ).TranslateAlongYAxis( dy ).TranslateAlongZAxis( dz );
  }

  public static XYZ TranslateAlongXAxis( this XYZ source, double dx )
  {
    return source + dx * XYZ.BasisX;
  }

  public static XYZ TranslateAlongYAxis( this XYZ source, double dy )
  {
    return source + dy * XYZ.BasisY;
  }

  public static XYZ TranslateAlongZAxis( this XYZ source, double dz )
  {
    return source + dz * XYZ.BasisZ;
  }

  public static bool IsLowerThan( this XYZ left, XYZ right )
  {
    return left.Z < right.Z;
  }
}
