using Best.Practices.Core.Domain.Enumerators;
using Best.Practices.Core.Domain.Models.Interfaces;
using Best.Practices.Core.Extensions;
using System.Collections;

namespace Best.Practices.Core.Domain.Models
{
    public abstract class BaseEntity : IBaseEntity
    {
        public virtual Guid Id { get; protected set; }
        public virtual EntityState State { get; protected set; }
        public virtual DateTime CreationDate { get; protected set; }
        public virtual Dictionary<string, object> PersistedValues { get; protected set; }

        public static T InstantiateANewEntity<T>() where T : BaseEntity, new()
        {
            var entity = new T();

            entity.Id = Guid.NewGuid();
            entity.CreationDate = DateTime.UtcNow;
            entity.State = EntityState.New;

            return entity;
        }

        public static T InstantiateAnExistentEntity<T>(Guid id, DateTime creationDate) where T : BaseEntity, new()
        {
            var entity = new T();

            entity.Id = id;
            entity.CreationDate = creationDate;
            entity.State = EntityState.Unchanged;

            return entity;
        }

        protected BaseEntity()
        {
            Id = Guid.NewGuid();
            CreationDate = DateTime.UtcNow;
            State = EntityState.New;
            PersistedValues = [];
        }

        protected void InitializePersistedValues(IBaseEntity entity)
        {
            var entityType = entity.GetType();

            var properties = entityType.GetProperties()
                .Where(x => !x.Name.In(nameof(PersistedValues), nameof(State)));

            foreach (var property in properties)
            {
                object propertyValue = property.GetValue(entity, null);

                if (propertyValue is not null)
                {
                    var propertyType = propertyValue.GetType();

                    if (propertyType.IsSubclassOf(typeof(BaseEntity)))
                    {
                        var entityProperty = (BaseEntity)propertyValue;

                        entityProperty.State = EntityState.Unchanged;

                        InitializePersistedValues(entityProperty);
                    }
                    else if (propertyType.Name.Contains("IEntityList") || propertyType.Name.Contains("EntityList"))
                    {
                        var entityList = propertyValue as IList;

                        foreach (var entityListItem in entityList)
                        {
                            var entityItem = (BaseEntity)entityListItem;

                            entityItem.State = EntityState.Unchanged;

                            InitializePersistedValues(entityItem);
                        }
                    }
                    else
                    {
                        PersistedValues[property.Name] = propertyValue;
                    }
                }
            }
        }

        public bool PropertyIsUpdated(object propertyOldValue, object propertyNewValue)
        {
            var entityProperty = propertyNewValue as IBaseEntity;

            if (entityProperty is not null)
                return entityProperty.State == EntityState.Updated;
            else
                return ((((propertyOldValue == null) && (propertyNewValue != null)) ||
                            ((propertyOldValue != null) && (propertyNewValue == null))) ||
                            ((propertyOldValue != null) && (propertyNewValue != null) &&
                            (!propertyOldValue.Equals(propertyNewValue))));
        }

        public virtual Dictionary<string, object> GetPropertiesToPersist()
        {
            var properties = new Dictionary<string, object>();

            if (State == EntityState.New)
                properties = GetInsertableProperties();
            else if (State == EntityState.Updated)
                properties = GetUpdatedProperties();

            return properties;
        }

        protected virtual Dictionary<string, object> GetUpdatedProperties()
        {
            var objectType = GetType();
            var properties = objectType.GetProperties()
                .Where(p => !p.Name.In(nameof(State), nameof(PersistedValues), nameof(PersistedValues)));

            var updatedProperties = new Dictionary<string, object>();

            foreach (var property in properties)
            {
                var oldPropertyValue = PersistedValues[property.Name];
                var currentValue = property.GetValue(this, null);

                if (this.PropertyIsUpdated(oldPropertyValue, currentValue))
                    updatedProperties[property.Name] = currentValue;
            }

            return updatedProperties;
        }

        protected virtual Dictionary<string, object> GetInsertableProperties()
        {
            var objectType = GetType();
            var properties = objectType.GetProperties()
                .Where(p => !p.Name.In(nameof(State), nameof(PersistedValues), nameof(PersistedValues)));

            var insertableProperties = new Dictionary<string, object>();

            foreach (var property in properties)
            {
                var propertyValue = property.GetValue(this, null);

                insertableProperties[property.Name] = propertyValue;
            }

            return insertableProperties;
        }

        public void SetStateAsUpdated()
        {
            if (State.In(EntityState.Unchanged, EntityState.Persisted))
            {
                State = EntityState.Updated;
            }
        }

        public void SetStateAsDeleted()
        {
            if (State != EntityState.New)
            {
                State = EntityState.Deleted;
            }
        }

        public void SetStateAsPersisted()
        {
            switch (State)
            {
                case EntityState.New:
                case EntityState.Updated:
                    {
                        State = EntityState.Persisted;
                        break;
                    };
                case EntityState.Deleted:
                    {
                        State = EntityState.PersistedDeleted;
                        break;
                    };
            };
        }

        public void SetStateAsUnchanged()
        {
            State = EntityState.Unchanged;

            InitializePersistedValues(this);
        }
        public virtual IBaseEntity EntityClone()
        {
            var cloneEntity = (BaseEntity)this.DeepClone();

            cloneEntity.Id = Guid.NewGuid();
            cloneEntity.CreationDate = DateTime.UtcNow;
            cloneEntity.State = EntityState.New;

            return cloneEntity;
        }

        public virtual object Clone()
        {
            return EntityClone();
        }
    }
}