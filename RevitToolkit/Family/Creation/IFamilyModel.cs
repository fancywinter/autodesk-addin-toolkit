namespace RevitToolkit.Family.Creation;
public interface IFamilyModel
{
  IReadOnlyCollection<IModelTranform> Transforms { get; }

  string FamilyName { get; }

  string SymbolName { get; }
}
