namespace RevitToolkit.Family.Creation;
public class MmToFeetConverter : IParameterValueConverter
{
  public object Convert( object value )
  {
    return ( double ) value / 304.8;
  }

  public object ConvertBack( object value )
  {
    return ( double ) value * 304.8;
  }
}
