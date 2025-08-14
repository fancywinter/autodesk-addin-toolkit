namespace AcadToolkit.Interops;

public sealed class BlockContainer : TableContainerBase<BlockTableRecord>
{
  internal BlockContainer( Transaction transaction, ObjectId containerId )
    : base( transaction, containerId )
  {
  }

  protected override BlockTableRecord CreateItem() => new BlockTableRecord();
}
