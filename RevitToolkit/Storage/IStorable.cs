namespace RevitToolkit.Storage;
/// <summary>
/// This interface is used to mark a class as storable in Revit's extensible storage.<br/>
/// Note: Decorates property <see cref="IStorable.Id"/> with <see cref="Attributes.FieldAttribute"/> after implementing this interface.
/// </summary>
public interface IStorable
{
  public int Id { get; set; }
}
