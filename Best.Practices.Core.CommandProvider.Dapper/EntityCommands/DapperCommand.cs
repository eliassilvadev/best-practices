using Best.Practices.Core.CommandProvider.Dapper.CommandProviders;
using Best.Practices.Core.CommandProvider.Dapper.Extensions;
using Best.Practices.Core.Common;
using Best.Practices.Core.Domain.Cqrs;
using Best.Practices.Core.Domain.Enumerators;
using Best.Practices.Core.Domain.Models.Interfaces;
using Best.Practices.Core.Exceptions;
using Best.Practices.Core.Extensions;
using Castle.DynamicProxy;
using Dapper;
using System.Data;
using static Dapper.SqlMapper;

namespace Best.Practices.Core.CommandProvider.Dapper.EntityCommands
{
    public class DapperCommand<Entity> : IEntityCommand where Entity : IBaseEntity
    {
        protected class EntityTableMapping
        {
            public DapperTableDefinition TableDefinition { get; set; }
            public Dictionary<string, IBaseEntity> ParentEntities { get; set; }

            public EntityTableMapping()
            {
                ParentEntities = new Dictionary<string, IBaseEntity>();
            }

            public EntityTableMapping WithParentEntity(string entityFieldName, IBaseEntity baseEntity)
            {
                ParentEntities[entityFieldName] = baseEntity;

                return this;
            }
        };

        const char DAPPER_PARAMETER_INDICATOR = '@';

        protected readonly IDbConnection _connection;
        public IList<CommandDefinition> CommandDefinitions { get; protected set; }

        protected readonly Dictionary<string, EntityTableMapping> _entityTableTypeMappings;
        public IBaseEntity AffectedEntity { get; }
        public DapperCommand(IDbConnection connection, Entity affectedEntity)
        {
            _connection = connection;
            AffectedEntity = affectedEntity;
            CommandDefinitions = new List<CommandDefinition>();
            _entityTableTypeMappings = new Dictionary<string, EntityTableMapping>();
        }

        public virtual async Task<bool> ExecuteAsync()
        {
            bool sucess;
            try
            {
                CreateCommandDefinitions((Entity)AffectedEntity);

                foreach (var commandDefinition in CommandDefinitions)
                {
                    await _connection.ExecuteAsync(commandDefinition);
                }

                sucess = true;
            }
            catch (Exception ex)
            {
                throw new CommandExecutionException(CommonConstants.ErrorMessages.DefaultErrorMessage + CommonConstants.StringEnter + ex.Message);
            }

            return sucess;
        }

        protected EntityTableMapping AddTypeMapping(string typeName, DapperTableDefinition tableDefinition)
        {
            var entityTypeMapping = new EntityTableMapping() { TableDefinition = tableDefinition };

            _entityTableTypeMappings.Add(typeName, entityTypeMapping);

            return entityTypeMapping;
        }

        protected void AddCommandDefinition(CommandDefinition? commandDefinition)
        {
            if (commandDefinition.HasValue)
                CommandDefinitions.Add(commandDefinition.Value);
        }

        public CommandDefinition CreateAnInsertCommandFromParameters(
            Dictionary<string, object> entityParameters,
            DapperTableDefinition tableDefinition)
        {
            var insertScript = "Insert Into " + tableDefinition.TableName + "(" + CommonConstants.StringEnter;

            var fieldsToInsert = string.Empty;
            var parameterNames = string.Empty;

            var dapperParameters = new DynamicParameters();

            foreach (var entityParameter in entityParameters)
            {
                var tableColumnDefinition = GetTableColumnDefinition(tableDefinition.ColumnDefinitions, entityParameter.Key);

                if (tableColumnDefinition is not null)
                {
                    if (!fieldsToInsert.IsEmpty())
                    {
                        fieldsToInsert += CommonConstants.StringComma + CommonConstants.StringEnter;
                        parameterNames += CommonConstants.StringComma + CommonConstants.StringEnter;
                    }

                    fieldsToInsert += tableColumnDefinition.DbFieldName;
                    parameterNames += DAPPER_PARAMETER_INDICATOR + tableColumnDefinition.DbFieldName;

                    dapperParameters.AddNullable(tableColumnDefinition.DbFieldName, GetParameterValue(entityParameter.Key, entityParameter.Value), size: tableColumnDefinition.Size);
                }
            }

            insertScript += fieldsToInsert + ")" + CommonConstants.StringEnter + "Values(" + CommonConstants.StringEnter;

            insertScript += parameterNames + ");";

            var commandDefinition = new CommandDefinition(insertScript, dapperParameters);

            AddCommandDefinition(commandDefinition);

            return commandDefinition;
        }

        private static DapperTableColumnDefinition GetTableColumnDefinition(List<DapperTableColumnDefinition> columnDefinitions, string key)
        {
            return columnDefinitions.FirstOrDefault(c => c.EntityFieldName == key);
        }

        public CommandDefinition CreateAnUpdateCommandFromParameters(
            Dictionary<string, object> entityParameters,
            DapperTableDefinition tableDefinition,
            Dictionary<string, object> filterCriteria)
        {
            var updateScript = "Update " + tableDefinition.TableName + " Set" + CommonConstants.StringEnter;

            var fieldsToUpdate = string.Empty;
            var dapperParameters = new DynamicParameters();

            foreach (var entityParameter in entityParameters)
            {
                var tableColumnDefinition = GetTableColumnDefinition(tableDefinition.ColumnDefinitions, entityParameter.Key);

                if (tableColumnDefinition is not null)
                {
                    if (!fieldsToUpdate.IsEmpty())
                        fieldsToUpdate += CommonConstants.StringComma + CommonConstants.StringEnter;

                    fieldsToUpdate += tableColumnDefinition.DbFieldName + " = " + DAPPER_PARAMETER_INDICATOR + tableColumnDefinition.DbFieldName;

                    dapperParameters.AddNullable(tableColumnDefinition.DbFieldName, GetParameterValue(entityParameter.Key, entityParameter.Value), size: tableColumnDefinition.Size);
                }
            }

            updateScript += fieldsToUpdate + CommonConstants.StringEnter + "Where" + CommonConstants.StringEnter;

            var filterFieldNamesScript = string.Empty;

            foreach (var filterCriteriaPart in filterCriteria)
            {
                var tableColumnDefinition = GetTableColumnDefinition(tableDefinition.ColumnDefinitions, filterCriteriaPart.Key);

                if (tableColumnDefinition is not null)
                {
                    if (!filterFieldNamesScript.IsEmpty())
                        filterFieldNamesScript += CommonConstants.StringEnter + "And ";

                    if (filterCriteriaPart.Value is not null)
                        filterFieldNamesScript += tableColumnDefinition.DbFieldName + " = " + DAPPER_PARAMETER_INDICATOR + tableColumnDefinition.DbFieldName;
                    else
                        filterFieldNamesScript += tableColumnDefinition.DbFieldName + " is null";

                    dapperParameters.AddNullable(tableColumnDefinition.DbFieldName, GetParameterValue(filterCriteriaPart.Key, filterCriteriaPart.Value), size: tableColumnDefinition.Size);
                }
            }

            updateScript += filterFieldNamesScript + ";";

            var commandDefinition = new CommandDefinition(updateScript, dapperParameters);

            AddCommandDefinition(commandDefinition);

            return commandDefinition;
        }

        private static object GetParameterValue(string key, object value)
        {
            var entity = (value as IBaseEntity);

            if (entity is not null)
            {
                if (!key.Contains(CommonConstants.CharFullStop)) // by default gets Id field 
                    return entity.Id;
                else
                {
                    string[] subProperties = key.Split(CommonConstants.CharFullStop);

                    var entityType = entity.GetType();

                    var propertyName = subProperties[1];

                    var property = entityType.GetProperties()
                        .Where(p => p.Name == propertyName)
                        .FirstOrDefault();

                    if (property is not null)
                        return property.GetValue(entity, null);
                    else
                        return null;
                }
            }

            return value;
        }

        public CommandDefinition CreateAnUpdateCommandFromEntityUpdatedPropertiesAndIdCriteria(IBaseEntity baseEntity)
        {

            return CreateAnUpdateCommandByEntityUpdatedPropertiesWithCriteria(
                baseEntity,
                new Dictionary<string, object>() { { nameof(baseEntity.Id), baseEntity.Id } });
        }

        public CommandDefinition CreateAnInsertCommandFromEntityProperties(IBaseEntity baseEntity)
        {
            var insertableProperties = baseEntity.GetPropertiesToPersist();

            var parentEntities = GetParentEntities(baseEntity.GetType());

            if (!parentEntities.IsNullOrEmpty())
                AddParentEntitiesAsEntityPropertiesToPersist(insertableProperties, parentEntities);

            var entityTableMapping = GetTableDefinitionByMappedType(baseEntity.GetType());

            if (entityTableMapping is null)
                throw new CommandExecutionException(CommonConstants.ErrorMessages.EntityMappingError.Format(baseEntity.GetType().Name));

            return CreateAnInsertCommandFromParameters(insertableProperties, entityTableMapping.TableDefinition);
        }

        private void AddParentEntitiesAsEntityPropertiesToPersist(IDictionary<string, object> propertiesToPersist, IDictionary<string, IBaseEntity> parentEntities)
        {
            foreach (var parentEntity in parentEntities)
            {
                propertiesToPersist[parentEntity.Key] = parentEntity.Value;
            }
        }

        public CommandDefinition CreateAnUpdateCommandByEntityUpdatedPropertiesWithCriteria(
            IBaseEntity baseEntity,
            Dictionary<string, object> filterCriteria)
        {
            var updatedProperties = baseEntity.GetPropertiesToPersist();

            var parentEntities = GetParentEntities(baseEntity.GetType());

            if (!parentEntities.IsNullOrEmpty())
                AddParentEntitiesAsEntityPropertiesToPersist(updatedProperties, parentEntities);

            var entityTableMapping = GetTableDefinitionByMappedType(baseEntity.GetType());

            if (entityTableMapping is null)
                throw new CommandExecutionException(CommonConstants.ErrorMessages.EntityMappingError.Format(baseEntity.GetType().Name));

            return CreateAnUpdateCommandFromParameters(updatedProperties, entityTableMapping.TableDefinition, filterCriteria);
        }

        private IDictionary<string, IBaseEntity> GetParentEntities(Type type)
        {
            var entityTableMapping = GetTableDefinitionByMappedType(type);

            if (entityTableMapping is null)
                return new Dictionary<string, IBaseEntity>();

            return entityTableMapping.ParentEntities;
        }

        public CommandDefinition CreateADeleteCommandWithEntityAndIdCriteria(
          IBaseEntity baseEntity)
        {
            var entityTableMapping = GetTableDefinitionByMappedType(baseEntity.GetType());

            if (entityTableMapping is null)
                throw new CommandExecutionException(CommonConstants.ErrorMessages.EntityMappingError.Format(baseEntity.GetType().Name));

            return CreateADeleteCommandWithCriteria(
                new Dictionary<string, object>() { { nameof(baseEntity.Id), baseEntity.Id } },
                entityTableMapping.TableDefinition);
        }

        private EntityTableMapping GetTableDefinitionByMappedType(Type type)
        {
            var typeName = type.Name;

            if (typeof(IProxyTargetAccessor).IsAssignableFrom(type))
                typeName = typeName.Substring(0, typeName.Length - 5);// removes "Proxy" sufix

            _entityTableTypeMappings.TryGetValue(typeName, out var entityTableMapping);

            if (entityTableMapping is null)
                return null;

            return entityTableMapping;
        }

        public CommandDefinition CreateADeleteCommandWithCriteria(
        Dictionary<string, object> filterCriteria,
        DapperTableDefinition tableDefinition)
        {
            var deleteScript = "Delete From" + CommonConstants.StringEnter + tableDefinition.TableName + CommonConstants.StringEnter + "Where" + CommonConstants.StringEnter;

            var filterFieldNamesScript = string.Empty;

            var parameters = new DynamicParameters();

            foreach (var filterCriteriaPart in filterCriteria)
            {
                var tableColumnDefinition = GetTableColumnDefinition(tableDefinition.ColumnDefinitions, filterCriteriaPart.Key);

                if (tableColumnDefinition is not null)
                {
                    if (!filterFieldNamesScript.IsEmpty())
                        filterFieldNamesScript += CommonConstants.StringEnter + "And ";

                    if (filterCriteriaPart.Value is not null)
                        filterFieldNamesScript += tableColumnDefinition.DbFieldName + " = " + DAPPER_PARAMETER_INDICATOR + tableColumnDefinition.DbFieldName;
                    else
                        filterFieldNamesScript += tableColumnDefinition.DbFieldName + " is null";

                    parameters.AddNullable(tableColumnDefinition.DbFieldName, GetParameterValue(filterCriteriaPart.Key, filterCriteriaPart.Value), size: tableColumnDefinition.Size);
                }
            }

            deleteScript += filterFieldNamesScript + ";";

            var commandDefinition = new CommandDefinition(deleteScript, parameters);

            AddCommandDefinition(commandDefinition);

            return commandDefinition;
        }

        public IList<CommandDefinition> CreateCommandDefinitionByState<TEntity>(IEnumerable<TEntity> entities)
            where TEntity : IBaseEntity
        {
            var commandDefinitions = new List<CommandDefinition>();

            foreach (var entitty in entities)
            {
                var commandDefinition = CreateCommandDefinitionByState(entitty);

                if (commandDefinition.HasValue)
                    commandDefinitions.Add(commandDefinition.Value);
            }

            return commandDefinitions;
        }

        public CommandDefinition? CreateCommandDefinitionByState<TEntity>(TEntity entity)
             where TEntity : IBaseEntity
        {
            CommandDefinition? commandDefinition = null;

            switch (entity.State)
            {
                case EntityState.New:
                    commandDefinition = CreateAnInsertCommandFromEntityProperties(entity);
                    break;
                case EntityState.Updated:
                    commandDefinition = CreateAnUpdateCommandFromEntityUpdatedPropertiesAndIdCriteria(entity);
                    break;
                case EntityState.Deleted:
                    commandDefinition = CreateADeleteCommandWithEntityAndIdCriteria(entity);
                    break;
                default:
                    break;
            }

            return commandDefinition;
        }

        public virtual IList<CommandDefinition> CreateCommandDefinitions(Entity entity)
        {
            return [];
        }
    }
}