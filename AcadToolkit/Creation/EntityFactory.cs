using AcadToolkit.Interops;

namespace AcadToolkit.Creation;

public abstract class EntityFactory<TEntity> : IEntityFactory where TEntity : Entity
{
  protected EntityFactory()
  {
  }

  public ObjectId AddToModelSpace( TransactionScope scope, string layerName = "0" ) => CreateEntity( scope.ModelSpace, scope, layerName );

  public ObjectId AddToPaperSpace( TransactionScope scope, string layerName = "0" ) => CreateEntity( scope.PaperSpace, scope, layerName );

  protected abstract TEntity InitializeEntity( TransactionScope scope );

  protected virtual void OnEntityCreated( TEntity entity )
  {
  }

  private ObjectId CreateEntity( EntityContainer container, TransactionScope scope, string layerName )
  {
    using TEntity entity = InitializeEntity( scope );
    entity.Layer = layerName;
    ObjectId id = container.Add( entity );
    OnEntityCreated( entity );
    return id;
  }
}
