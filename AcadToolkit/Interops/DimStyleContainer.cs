namespace AcadToolkit.Interops;

public sealed class DimStyleContainer : TableContainerBase<DimStyleTableRecord>
{
  internal DimStyleContainer( Transaction transaction, ObjectId containerId )
    : base( transaction, containerId )
  {
  }

  protected override DimStyleTableRecord CreateItem() => new();
}
