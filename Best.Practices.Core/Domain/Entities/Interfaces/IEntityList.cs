namespace Best.Practices.Core.Domain.Entities.Interfaces
{
    public interface IEntityList<Entity> : IList<Entity>
    {
        IBaseEntity Parent { get; }

        IList<Entity> AllItems { get; }

        IList<Entity> Items { get; }

        IList<Entity> DeletedItems { get; }

        void AddRange(IEnumerable<Entity> items);

        IEntityList<Entity> Clone();

        int RemoveAll(Predicate<Entity> match);
    }
}