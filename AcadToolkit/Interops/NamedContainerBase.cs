namespace AcadToolkit.Interops;

public abstract class NamedContainerBase<T> : AbstractNamedContainerBase<T> where T : DBObject
{
  protected NamedContainerBase( Transaction transaction, ObjectId containerId )
    : base( transaction, containerId )
  {
  }

  public abstract void Register( string name, T item );
}
