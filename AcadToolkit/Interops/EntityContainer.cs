namespace AcadToolkit.Interops;

public sealed class EntityContainer : ContainerBase<Entity>
{
  public EntityContainer( Transaction transaction, ObjectId containerId )
    : base( transaction, containerId )
  {
  }

  public ObjectId Add( Entity entity )
  {
    BlockTableRecord btr = GetBlockTableRecord( OpenMode.ForWrite );
    ObjectId id = btr.AppendEntity( entity );
    Transaction.AddNewlyCreatedDBObject( entity, true );
    return id;
  }

  public override IEnumerable<ObjectId> ObjectIds
  {
    get
    {
      BlockTableRecord btr = GetBlockTableRecord( OpenMode.ForRead );
      return btr.Cast<ObjectId>();
    }
  }

  private BlockTableRecord GetBlockTableRecord( OpenMode openMode ) => ( BlockTableRecord ) GetContainer( openMode );
}
