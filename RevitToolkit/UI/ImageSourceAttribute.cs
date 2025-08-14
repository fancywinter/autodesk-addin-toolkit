using System.Windows.Media.Imaging;

namespace RevitToolkit.UI;

public abstract class ImageSourceAttribute : ExternalResourceAttribute
{
  protected string ImageFileName { get; }

  public bool LargeSize { get; }

  public BitmapImage ImageSource => new( new Uri( GetFullPath( ImageFileName ), UriKind.Absolute ) );

  protected ImageSourceAttribute( string imageFileName, bool largeSize ) : base()
  {
    ImageFileName = imageFileName;
    LargeSize = largeSize;
  }
}
