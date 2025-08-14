using AcadToolkit.Interops;

namespace AcadToolkit.Creation;

public interface IEntityFactory
{
  ObjectId AddToModelSpace( TransactionScope scope, string layerName );

  ObjectId AddToPaperSpace( TransactionScope scope, string layerName );
}
