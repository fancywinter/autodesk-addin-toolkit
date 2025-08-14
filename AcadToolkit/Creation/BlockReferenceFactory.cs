using AcadToolkit.Interops;

namespace AcadToolkit.Creation;

public sealed class BlockReferenceFactory
{
  private readonly string _blockName;
  private readonly Point3d _position;
  private Scale3d _scale;
  private double _rotation;
  private readonly Dictionary<string, string> _attributeDict = new();
  private readonly Dictionary<string, object> _dynamicPropDict = new();

  public BlockReferenceFactory( string blockName, double posX, double posY )
  {
    _blockName = blockName;
    _position = new Point3d( posX, posY, 0 );
    _scale = new Scale3d( 1.0 );
  }

  public static BlockReferenceFactory New( string blockName, double posX, double posY )
    => new BlockReferenceFactory( blockName, posX, posY );

  public BlockReferenceFactory SetAttribute( string tag, string text )
  {
    if ( _attributeDict.ContainsKey( tag ) )
      _attributeDict[ tag ] = text;
    else
      _attributeDict.Add( tag, text );
    return this;
  }

  public BlockReferenceFactory SetDynamicProperty( string name, object value )
  {
    if ( _dynamicPropDict.ContainsKey( name ) )
      _dynamicPropDict[ name ] = value;
    else
      _dynamicPropDict.Add( name, value );
    return this;
  }

  public BlockReferenceFactory Rotate( double angle )
  {
    _rotation = angle;
    return this;
  }

  public BlockReferenceFactory ScaleUniform( double factor )
  {
    _scale = new Scale3d( factor );
    return this;
  }

  public BlockReferenceFactory Scale( double x, double y )
  {
    _scale = new Scale3d( x, y, 1 );
    return this;
  }

  public ObjectId Insert( TransactionScope scope, string layer )
  {
    if ( !scope.Blocks.TryGetObject( _blockName, OpenMode.ForRead, out BlockTableRecord? block ) )
      return ObjectId.Null;

    using BlockReference bref = new( _position, block.Id );
    bref.Layer = layer;
    bref.Rotation = _rotation;
    bref.ScaleFactors = _scale;

    ObjectId brefId = scope.ModelSpace.Add( bref );

    if ( block.HasAttributeDefinitions && _attributeDict.Any() ) {
      foreach ( ObjectId id in block ) {
        AttributeDefinition? attributeDef = scope.GetObject<AttributeDefinition>( id, OpenMode.ForRead );

        if ( attributeDef == null )
          continue;

        if ( attributeDef.Constant )
          continue;

        using AttributeReference attributeRef = new();
        attributeRef.SetAttributeFromBlock( attributeDef, bref.BlockTransform );
        attributeRef.Position = attributeDef.Position.TransformBy( bref.BlockTransform );
        attributeRef.TextString = _attributeDict.TryGetValue( attributeDef.Tag, out string? value ) ? value : string.Empty;
        bref.AttributeCollection.AppendAttribute( attributeRef );
      }
    }

    if ( bref.IsDynamicBlock && _dynamicPropDict.Any() ) {
      foreach ( DynamicBlockReferenceProperty prop in bref.DynamicBlockReferencePropertyCollection ) {

        if ( prop.ReadOnly )
          continue;

        if ( !_dynamicPropDict.TryGetValue( prop.PropertyName, out object? value ) )
          continue;

        prop.Value = value;
      }
    }

    return brefId;
  }
}
