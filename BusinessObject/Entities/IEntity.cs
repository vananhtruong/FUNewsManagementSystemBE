namespace BusinessObject.Entities
{
    public interface IEntity<TKey>
    {
        TKey Id { get; }
    }
}
