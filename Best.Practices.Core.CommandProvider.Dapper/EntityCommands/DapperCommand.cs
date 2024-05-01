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
        const char DAPPER_PARAMETER_INDICATOR = '@';

        protected readonly IDbConnection _connection;
        public IList<CommandDefinition> CommandDefinitions { get; protected set; }

        protected readonly Dictionary<string, DapperTableDefinition> _entityTableTypeMappings;
        public IBaseEntity AffectedEntity { get; }
        public DapperCommand(IDbConnection connection, Entity affectedEntity)
        {
            _connection = connection;
            AffectedEntity = affectedEntity;
            CommandDefinitions = new List<CommandDefinition>();
            _entityTableTypeMappings = new Dictionary<string, DapperTableDefinition>();
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

        public CommandDefinition CreateAnUpdateCommandFromEntityUpdatedPropertiesAndIdCriteria(
           IBaseEntity baseEntity,
           IDictionary<string, IBaseEntity> parentEntities = null)
        {

            return CreateAnUpdateCommandByEntityUpdatedPropertiesWithCriteria(
                baseEntity,
                new Dictionary<string, object>() { { nameof(baseEntity.Id), baseEntity.Id } },
                parentEntities);
        }

        public CommandDefinition CreateAnInsertCommandFromEntityProperties(
            IBaseEntity baseEntity,
            IDictionary<string, IBaseEntity> parentEntities = null)
        {
            var insertableProperties = baseEntity.GetPropertiesToPersist();

            if (parentEntities is not null)
                AddParentEntitiesAsEntityPropertiesToPersist(insertableProperties, parentEntities);

            var tableDefinition = GetTableDefinitionByMappedType(baseEntity.GetType());

            return CreateAnInsertCommandFromParameters(insertableProperties, tableDefinition);
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
            Dictionary<string, object> filterCriteria,
            IDictionary<string, IBaseEntity> parentEntities = null)
        {
            var updatedProperties = baseEntity.GetPropertiesToPersist();

            if (parentEntities is not null)
                AddParentEntitiesAsEntityPropertiesToPersist(updatedProperties, parentEntities);

            var tableDefinition = GetTableDefinitionByMappedType(baseEntity.GetType());

            return CreateAnUpdateCommandFromParameters(updatedProperties, tableDefinition, filterCriteria);
        }

        public CommandDefinition CreateADeleteCommandWithEntityAndIdCriteria(
          IBaseEntity baseEntity)
        {
            var tableDefinition = GetTableDefinitionByMappedType(baseEntity.GetType());

            return CreateADeleteCommandWithCriteria(
                new Dictionary<string, object>() { { nameof(baseEntity.Id), baseEntity.Id } },
                tableDefinition);
        }

        private DapperTableDefinition GetTableDefinitionByMappedType(Type type)
        {
            var typeName = type.Name;

            if (typeof(IProxyTargetAccessor).IsAssignableFrom(type))
                typeName = typeName.Substring(0, typeName.Length - 5);// removes "Proxy" sufix

            _entityTableTypeMappings.TryGetValue(typeName, out var tableDefinition);

            return tableDefinition;
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

        public CommandDefinition? CreateCommandDefinitionByState<TEntity>(TEntity entity, IDictionary<string, IBaseEntity> parentEntities = null)
             where TEntity : IBaseEntity
        {
            CommandDefinition? commandDefinition = null;

            switch (entity.State)
            {
                case EntityState.New:
                    commandDefinition = CreateAnInsertCommandFromEntityProperties(entity, parentEntities);
                    break;
                case EntityState.Updated:
                    commandDefinition = CreateAnUpdateCommandFromEntityUpdatedPropertiesAndIdCriteria(entity, parentEntities);
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