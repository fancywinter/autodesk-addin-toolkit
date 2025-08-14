using System.IO;

namespace RevitToolkit.Helper;
public sealed class RevitDocumentHandle : IDisposable
{
  private readonly bool _saveModified;

  public Document? Document { get; init; }

  public ModelPath? ModelPath { get; init; }

  public bool FileExistsOnDisk { get; init; }

  [MemberNotNullWhen( true, nameof( Document ), nameof( ModelPath ) )]
  public bool IsValidDocument { get; init; }

  public bool OpenCancelled { get; init; }

  public bool IsCorruptedModel { get; init; }

  public bool IsFamilyDocument => Document?.IsFamilyDocument ?? false;

  public static RevitDocumentHandle FromFile( Autodesk.Revit.ApplicationServices.Application application, string filePath, bool saveModified = false )
  {
    return new RevitDocumentHandle( application, filePath, saveModified );
  }

  public RevitDocumentHandle( Autodesk.Revit.ApplicationServices.Application application, string filePath, bool saveModified = false )
  {
    try {
      if ( !File.Exists( filePath ) )
        return;
      FileExistsOnDisk = true;
      ModelPath = ModelPathUtils.ConvertUserVisiblePathToModelPath( filePath );
      Document = application.OpenDocumentFile( ModelPath, new OpenOptions { Audit = true } );
      IsValidDocument = true;
      _saveModified = saveModified;
    }
    catch ( Exception ex ) {
      if ( ex is Autodesk.Revit.Exceptions.OperationCanceledException )
        OpenCancelled = true;
      if ( ex is Autodesk.Revit.Exceptions.CorruptModelException )
        IsCorruptedModel = true;
    }
  }

  public void Dispose() => Document?.Close( _saveModified );
}
