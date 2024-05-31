using Best.Practices.Core.Domain.Models.Interfaces;

namespace Best.Practices.Core.Domain.Cqrs.CommandProviders
{
    public class InMemoryConnection<Entity> where Entity : IBaseEntity
    {
        private static Dictionary<string, IList<Entity>> _persistedEntities;

        public Dictionary<string, IList<Entity>> PersistedEntities
        {
            get
            {
                if (!_persistedEntities.ContainsKey(typeof(Entity).Name))
                {
                    _persistedEntities[typeof(Entity).Name] = new List<Entity>();
                }

                return _persistedEntities;
            }
            set
            {
                _persistedEntities = value;
            }
        }
    }
}
