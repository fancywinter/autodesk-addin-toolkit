namespace AcadToolkit.Creation;

public sealed class XDataBuilder
{
  private readonly IList<TypedValue> _typedValues;

  public XDataBuilder()
  {
    _typedValues = new List<TypedValue>();
  }

  public static XDataBuilder New() => new XDataBuilder();

  public XDataBuilder AddData( object value )
  {
    if ( value is string )
      AddDataAsciiString( ( string ) value );

    else if ( value is byte[] )
      AddDataBinaryChunk( ( byte[] ) value );

    else if ( value is Handle )
      AddDataHandle( ( Handle ) value );

    else if ( value is Point3d )
      AddDataXCoordinate( ( Point3d ) value );

    else if ( value is double )
      AddDataReal( ( double ) value );

    else if ( value is short )
      AddDataInt16( ( short ) value );

    else if ( value is int )
      AddDataInt32( ( int ) value );

    return this;
  }

  public XDataBuilder AddData( object value, DxfCode code )
  {
    _typedValues.Add( new TypedValue( ( int ) code, value ) );
    return this;
  }

  public XDataBuilder AddDataRegAppName( string value )
  {
    _typedValues.Add( new TypedValue( ( int ) DxfCode.ExtendedDataRegAppName, value ) );
    return this;
  }

  public XDataBuilder AddDataAsciiString( string value )
  {
    _typedValues.Add( new TypedValue( ( int ) DxfCode.ExtendedDataAsciiString, value ) );
    return this;
  }

  public XDataBuilder AddDataLayerName( string value )
  {
    _typedValues.Add( new TypedValue( ( int ) DxfCode.ExtendedDataLayerName, value ) );
    return this;
  }

  public XDataBuilder AddDataBinaryChunk( byte[] value )
  {
    _typedValues.Add( new TypedValue( ( int ) DxfCode.ExtendedDataBinaryChunk, value ) );
    return this;
  }

  public XDataBuilder AddDataHandle( Handle value )
  {
    _typedValues.Add( new TypedValue( ( int ) DxfCode.ExtendedDataHandle, value ) );
    return this;
  }

  public XDataBuilder AddDataXCoordinate( Point3d value )
  {
    _typedValues.Add( new TypedValue( ( int ) DxfCode.ExtendedDataXCoordinate, value ) );
    return this;
  }

  public XDataBuilder AddDataWorldXCoordinate( Point3d value )
  {
    _typedValues.Add( new TypedValue( ( int ) DxfCode.ExtendedDataWorldXCoordinate, value ) );
    return this;
  }

  public XDataBuilder AddDataWorldXDisplacement( Point3d value )
  {
    _typedValues.Add( new TypedValue( ( int ) DxfCode.ExtendedDataWorldXDisp, value ) );
    return this;
  }

  public XDataBuilder AddDataWorldXDirection( Point3d value )
  {
    _typedValues.Add( new TypedValue( ( int ) DxfCode.ExtendedDataWorldXDir, value ) );
    return this;
  }

  public XDataBuilder AddDataControlString( string value )
  {
    _typedValues.Add( new TypedValue( ( int ) DxfCode.ExtendedDataControlString, value ) );
    return this;
  }

  public XDataBuilder AddDataScale( double value )
  {
    _typedValues.Add( new TypedValue( ( int ) DxfCode.ExtendedDataScale, value ) );
    return this;
  }

  public XDataBuilder AddDataInt16( short value )
  {
    _typedValues.Add( new TypedValue( ( int ) DxfCode.ExtendedDataInteger16, value ) );
    return this;
  }

  public XDataBuilder AddDataInt32( int value )
  {
    _typedValues.Add( new TypedValue( ( int ) DxfCode.ExtendedDataInteger32, value ) );
    return this;
  }

  public XDataBuilder AddDataReal( double value )
  {
    _typedValues.Add( new TypedValue( ( int ) DxfCode.ExtendedDataReal, value ) );
    return this;
  }

  public XDataBuilder AddDataDistance( double value )
  {
    _typedValues.Add( new TypedValue( ( int ) DxfCode.ExtendedDataDist, value ) );
    return this;
  }

  public bool HasData => _typedValues.Any( tv => tv.TypeCode != ( int ) DxfCode.ExtendedDataRegAppName );

  public bool HasAppName => _typedValues.Any( tv => tv.TypeCode == ( int ) DxfCode.ExtendedDataRegAppName );

  public ResultBuffer ToResultBuffer() => new ResultBuffer( _typedValues.ToArray() );
}
