namespace AcadToolkit.Interops;

public sealed class LineTypeContainer : TableContainerBase<LinetypeTableRecord>
{
  internal LineTypeContainer( Transaction transaction, ObjectId containerId )
    : base( transaction, containerId )
  {
  }

  protected override LinetypeTableRecord CreateItem() => new();
}
