namespace Library.Core.Infrastructure
{
    public interface IEntityWithTypedId<TId>
    {
        TId Id { get; }
    }
}
