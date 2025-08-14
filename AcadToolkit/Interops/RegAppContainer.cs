namespace AcadToolkit.Interops;

public sealed class RegAppContainer : TableContainerBase<RegAppTableRecord>
{
  internal RegAppContainer( Transaction transaction, ObjectId containerId )
    : base( transaction, containerId )
  {
  }

  protected override RegAppTableRecord CreateItem() => new();
}
