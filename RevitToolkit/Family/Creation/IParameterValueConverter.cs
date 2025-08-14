namespace RevitToolkit.Family.Creation;
public interface IParameterValueConverter
{
  object Convert( object value );

  object ConvertBack( object value );
}
