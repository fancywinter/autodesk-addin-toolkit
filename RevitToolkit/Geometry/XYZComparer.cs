namespace RevitToolkit.Geometry;
internal class XYZComparer : IEqualityComparer<XYZ>
{
  public bool Equals( XYZ? left, XYZ? right )
  {
    if ( left is null || right is null )
      return true;
    return left.IsAlmostEqualTo( right );
  }

  public int GetHashCode( XYZ obj )
  {
    return GetHashCode();
  }
}
