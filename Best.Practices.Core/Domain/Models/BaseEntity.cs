using Best.Practices.Core.Domain.Enumerators;
using Best.Practices.Core.Domain.Models.Interfaces;
using Best.Practices.Core.Domain.Observer;
using Best.Practices.Core.Extensions;
using LinFu.DynamicProxy;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;

namespace Best.Practices.Core.Domain.Models
{
    public abstract class BaseEntity : IBaseEntity
    {
        public virtual Guid Id { get; protected set; }
        public virtual EntityState State { get; protected set; }
        public virtual DateTime CreationDate { get; protected set; }
        public virtual Dictionary<string, object> PersistedValues { get; protected set; }
        public virtual IList<IEntityObserver> Observers { get; protected set; }

        public string GetTypeName(Type type)
        {
            var typeName = type.Name;

            if (typeof(IProxy).IsAssignableFrom(type))
                typeName = typeName.Substring(0, typeName.Length - 5);// removes "Proxy" sufix

            return typeName;
        }
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (GetTypeName(GetType()) != GetTypeName(obj.GetType()))
            {
                return false;
            }

            if (Id != Guid.Empty && ((BaseEntity)obj).Id != Guid.Empty && Id == ((BaseEntity)obj).Id)
            {
                return true;
            }

            return this == obj;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        protected BaseEntity()
        {
            Id = Guid.NewGuid();
            CreationDate = DateTime.UtcNow;
            State = EntityState.New;
            PersistedValues = [];
            Observers = new List<IEntityObserver>();
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

                        entity.PersistedValues[property.Name] = propertyValue;

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
                        entity.PersistedValues[property.Name] = propertyValue;
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
                .Where(p => !p.Name.In(nameof(State), nameof(PersistedValues), nameof(PersistedValues), nameof(Observers)));

            var updatedProperties = new Dictionary<string, object>();

            foreach (var property in properties)
            {
                if (PersistedValues.ContainsKey(property.Name))
                {
                    var oldPropertyValue = PersistedValues[property.Name];
                    var currentValue = property.GetValue(this, null);

                    if (this.PropertyIsUpdated(oldPropertyValue, currentValue))
                        updatedProperties[property.Name] = currentValue;
                }
            }

            return updatedProperties;
        }

        protected virtual Dictionary<string, object> GetInsertableProperties()
        {
            var objectType = GetType();
            var properties = objectType.GetProperties()
                .Where(p => !p.Name.In(nameof(State), nameof(PersistedValues), nameof(PersistedValues), nameof(Observers)));

            var insertableProperties = new Dictionary<string, object>();

            foreach (var property in properties)
            {
                var propertyValue = property.GetValue(this, null);

                insertableProperties[property.Name] = propertyValue;
            }

            return insertableProperties;
        }

        public virtual void SetStateAsUpdated()
        {
            if (State.In(EntityState.Unchanged, EntityState.Persisted))
            {
                State = EntityState.Updated;
            }
        }

        public virtual void SetStateAsDeleted()
        {
            if (State != EntityState.New)
            {
                State = EntityState.Deleted;
            }
        }

        public virtual void SetStateAsPersisted()
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

        public virtual void SetStateAsUnchanged()
        {
            State = EntityState.Unchanged;

            InitializePersistedValues(this);
        }
        public virtual IBaseEntity EntityClone()
        {
            var cloneEntity = (BaseEntity)this.DeepClone(nameof(Id), nameof(CreationDate), nameof(State));

            cloneEntity.Id = Guid.NewGuid();
            cloneEntity.CreationDate = DateTime.UtcNow;
            cloneEntity.State = EntityState.New;

            return cloneEntity;
        }

        public virtual void Copy(IBaseEntity entity)
        {
            var serialized = JsonConvert.SerializeObject(
                entity,
                new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Objects,
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });

            var json = JToken.Parse(serialized);

            if (json[nameof(Id)] != null)
                json[nameof(Id)].Parent.Remove();

            if (json[nameof(CreationDate)] != null)
                json[nameof(CreationDate)].Parent.Remove();

            if (json[nameof(State)] != null)
                json[nameof(State)].Parent.Remove();

            JsonConvert.PopulateObject(json.ToString(), this);
        }

        public virtual object Clone()
        {
            return EntityClone();
        }

        public void AddObserver(IEntityObserver observer)
        {
            Observers.Add(observer);
        }

        public void RemoveObserver(IEntityObserver observer)
        {
            Observers.Remove(observer);
        }

        public void NotifyEntityObserversPropertyUpdate(string propertyName, object propertyValue)
        {
            bool propertyUpdated = true;

            if (PersistedValues.TryGetValue(propertyName, out object oldPropertyValue))
            {
                propertyUpdated = PropertyIsUpdated(oldPropertyValue, propertyValue);
            }

            if (propertyUpdated)
                Observers.ForEach(o => o.NotifyEntityPropertyUpdate(this, propertyName, propertyValue));
        }
    }
}