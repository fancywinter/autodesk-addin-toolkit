using Autodesk.Windows;
using System.Windows.Media.Imaging;

namespace RevitToolkit.UI;

public abstract class ToolTipAttribute : ExternalResourceAttribute
{
  protected string Title { get; init; }

  protected string Content { get; init; }

  public string? VideoFileName { get; set; }

  public string? ImageFileName { get; set; }

  public RibbonToolTip Tooltip => new()
  {
    Title = Title,
    ExpandedContent = Content,
    ExpandedVideo = VideoFileName != null ? new Uri( GetFullPath( VideoFileName ) ) : null,
    ExpandedImage = ImageFileName != null ? new BitmapImage( new Uri( GetFullPath( ImageFileName ), UriKind.Absolute ) ) : null
  };

  protected ToolTipAttribute( string title, string content )
  {
    Title = title;
    Content = content;
  }
}
