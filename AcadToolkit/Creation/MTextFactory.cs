using AcadToolkit.Interops;

namespace AcadToolkit.Creation;

public sealed class MTextFactory : EntityFactory<MText>
{
  private readonly string _contents;
  private Point3d _location;
  private double _textHeight;
  private string? _textStyleName;
  private AttachmentPoint? _attachment;

  public MTextFactory( string contents )
  {
    _contents = contents;
    _textHeight = 1.0;
  }

  public static MTextFactory New( string contents ) => new MTextFactory( contents );

  public MTextFactory Location( Point3d point )
  {
    _location = point;
    return this;
  }

  public MTextFactory TextStyle( string name )
  {
    _textStyleName = name;
    return this;
  }

  public MTextFactory TextHeight( double height )
  {
    _textHeight = height;
    return this;
  }

  public MTextFactory Justify( AttachmentPoint attachment )
  {
    _attachment = attachment;
    return this;
  }

  protected override MText InitializeEntity( TransactionScope scope )
  {
    MText mtext = new();
    mtext.Location = _location;
    mtext.Contents = _contents;
    mtext.TextHeight = _textHeight;
    if ( _textStyleName != null ) {
      if ( scope.TextStyles.TryGetId( _textStyleName, out ObjectId textStyleId ) )
        mtext.TextStyleId = textStyleId;
    }
    if ( _attachment != null )
      mtext.Attachment = _attachment.Value;
    return mtext;
  }
}
