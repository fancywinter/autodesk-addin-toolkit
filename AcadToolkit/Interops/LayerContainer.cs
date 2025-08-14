namespace AcadToolkit.Interops;

public sealed class LayerContainer : TableContainerBase<LayerTableRecord>
{
  internal LayerContainer( Transaction transaction, ObjectId containerId )
    : base( transaction, containerId )
  {
  }

  protected override LayerTableRecord CreateItem() => new();
}
