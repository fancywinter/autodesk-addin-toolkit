namespace AcadToolkit.Interops;

public sealed class TextStyleContainer : TableContainerBase<TextStyleTableRecord>
{
  internal TextStyleContainer( Transaction transaction, ObjectId containerId )
    : base( transaction, containerId )
  {
  }

  protected override TextStyleTableRecord CreateItem() => new();
}
