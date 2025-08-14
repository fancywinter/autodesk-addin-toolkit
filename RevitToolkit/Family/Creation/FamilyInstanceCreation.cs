using Autodesk.Revit.DB.Structure;
using System.ComponentModel;
using System.Reflection;

namespace RevitToolkit.Family.Creation;

public class FamilyInstanceCreation
{
  private Document _document;
  private readonly List<IFamilyModel> _models;

  private FamilyInstanceCreation( Document document )
  {
    _document = document;
    _models = new List<IFamilyModel>();
  }

  public static FamilyInstanceCreation Initialize( Document document )
  {
    return new FamilyInstanceCreation( document );
  }

  public FamilyInstanceCreation AppendModel( IFamilyModel model )
  {
    _models.Add( model );
    return this;
  }

  public FamilyInstanceCreation AppendModels( IEnumerable<IFamilyModel> models )
  {
    _models.AddRange( models );
    return this;
  }

  public void Create()
  {
    _models.ForEach( CreateSingleInstance );
  }

  private void CreateSingleInstance( IFamilyModel familyModel )
  {
    // Get family symbol
    FamilySymbol familySymbol = new FilteredElementCollector( _document )
      .OfClass( typeof( FamilySymbol ) )
      .WhereElementIsElementType()
      .Cast<FamilySymbol>()
      .First( x => x.FamilyName == familyModel.FamilyName && x.Name == familyModel.SymbolName );

    // Place instance
    FamilyInstance? instance = familyModel is BaseAdaptiveFamilyModel adaptiveFamilyModel
      ? CreateInstance( familySymbol, adaptiveFamilyModel.PlacementPoints )
      : CreateInstance( familySymbol );

    if ( instance == null )
      return;

    foreach ( IModelTranform tranform in familyModel.Transforms )
      tranform.ApplyTransform( instance );

    // Assign parameters
    Dictionary<string,ParameterValue> paramValueDict = GetDbParameters( familyModel );

    foreach ( Parameter param in instance.Parameters ) {
      if ( param.IsReadOnly ) {
        continue;
      }

      if ( !paramValueDict.TryGetValue( param.Definition.Name, out ParameterValue paramValue ) ) {
        continue;
      }

      ForgeTypeId unitType = param.Definition.GetDataType();

      switch ( param.StorageType ) {
        case StorageType.Integer:
          param.Set( Convert.ToInt32( paramValue.Value ) );
          break;

        case StorageType.Double:
          double value = ( double ) paramValue.Value;
          param.Set( value );
          break;

        case StorageType.String:
          param.Set( ( string ) paramValue.Value );
          break;

        case StorageType.ElementId:
          param.Set( ( ElementId ) paramValue.Value );
          break;
      }
    }
  }

  #region Placing model

  private FamilyInstance? CreateInstance( FamilySymbol symbol, IList<XYZ> adaptPointCoords )
  {
    FamilyInstance instances = AdaptiveComponentInstanceUtils.CreateAdaptiveComponentInstance( _document, symbol );
    List<ReferencePoint> adaptPoints = AdaptiveComponentInstanceUtils.GetInstancePlacementPointElementRefIds( instances )
      .Select( id => ( ReferencePoint ) _document.GetElement( id ) ).ToList();

    if ( adaptPoints.Count != adaptPointCoords.Count )
      return instances;
    for ( int i = 0; i < adaptPoints.Count; i++ ) {
      XYZ displacement = adaptPointCoords[ i ].Subtract( adaptPoints[ i ].Position );
      ElementTransformUtils.MoveElement( _document, adaptPoints[ i ].Id, displacement );
    }

    return instances;
  }

  private FamilyInstance? CreateInstance( FamilySymbol symbol )
  {
    FamilyInstance familyInstance = _document.Create.NewFamilyInstance( XYZ.Zero, symbol, StructuralType.UnknownFraming );
    _document.Regenerate();
    return familyInstance;
  }

  #endregion

  #region Get property value function

  private static Dictionary<string, ParameterValue> GetDbParameters( IFamilyModel instanceDb )
  {
    Dictionary<string, ParameterValue> result = new Dictionary<string, ParameterValue>();
    foreach ( PropertyInfo prop in instanceDb.GetType().GetProperties() ) {
      DescriptionAttribute attribute = ( DescriptionAttribute ) Attribute.GetCustomAttribute( prop, typeof( DescriptionAttribute ) )!;
      if ( attribute == null )
        continue;

      object? value = prop.GetValue( instanceDb );
      if ( value is null )
        continue;

      string name = attribute.Description;

      StorageType dataType = StorageType.None;
      Type propType = prop.PropertyType;
      if ( propType == typeof( double ) )
        dataType = StorageType.Double;

      if ( propType == typeof( int ) )
        dataType = StorageType.Integer;

      if ( propType == typeof( string ) )
        dataType = StorageType.String;

      if ( propType == typeof( bool ) ) {
        dataType = StorageType.Integer;
        result.Add( name, new ParameterValue( name, dataType, ( bool ) value ? 1 : 0 ) );
        continue;
      }
      if ( dataType == StorageType.None )
        continue;

      result.Add( name, new ParameterValue( name, dataType, value ) );
    }

    return result;
  }

  private class ParameterValue
  {
    public string Name { get; private set; }
    public StorageType DataType { get; private set; }
    public object Value { get; }

    public ParameterValue( string name, StorageType dataType, object value )
    {
      Name = name;
      DataType = dataType;
      Value = value;
    }
  }

  #endregion
}
