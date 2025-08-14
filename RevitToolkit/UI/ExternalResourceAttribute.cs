using System.IO;

namespace RevitToolkit.UI;

[AttributeUsage( AttributeTargets.Class, AllowMultiple = false, Inherited = true )]
public abstract class ExternalResourceAttribute : Attribute
{
  protected ExternalResourceAttribute()
  {
  }

  protected abstract string GetResourceDirectory();

  protected string GetFullPath( string fileName ) => Path.Combine( GetResourceDirectory(), fileName );

  protected bool ValidateFilePath( string fileName ) => File.Exists( GetFullPath( fileName ) );
}
