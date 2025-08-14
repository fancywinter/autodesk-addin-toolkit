namespace RevitToolkit.Family.Creation;
public class DegToRadConverter : IParameterValueConverter
{
  public object Convert( object value )
  {
    return ( double ) value * Math.PI / 180;
  }

  public object ConvertBack( object value )
  {
    return ( double ) value / Math.PI * 180;
  }
}
