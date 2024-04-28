using Best.Practices.Core.CommandProvider.Dapper.CommandProviders;
using Best.Practices.Core.CommandProvider.Dapper.Extensions;
using Best.Practices.Core.Common;
using Best.Practices.Core.Domain.Cqrs;
using Best.Practices.Core.Domain.Enumerators;
using Best.Practices.Core.Domain.Models.Interfaces;
using Best.Practices.Core.Exceptions;
using Best.Practices.Core.Extensions;
using Dapper;
using System.Data;

namespace Best.Practices.Core.CommandProvider.Dapper.EntityCommands
{
    public class DapperCommand<Entity> : IEntityCommand where Entity : IBaseEntity
    {
        const char DAPPER_PARAMETER_INDICATOR = '@';

        protected readonly IDbConnection _connection;

        public IBaseEntity AffectedEntity { get; }
        public DapperCommand(IDbConnection connection, Entity affectedEntity)
        {
            _connection = connection;
            AffectedEntity = affectedEntity;
        }

        public virtual bool Execute()
        {
            bool sucess;
            try
            {
                var commandDefinitions = GetCommandDefinitions((Entity)AffectedEntity);

                foreach (var commandDefinition in commandDefinitions)
                {
                    _connection.Execute(commandDefinition);
                }

                sucess = true;
            }
            catch (Exception ex)
            {
                throw new CommandExecutionException(CommonConstants.ErrorMessages.DefaultErrorMessage + CommonConstants.StringEnter + ex.Message);
            }

            return sucess;
        }

        public CommandDefinition InsertCommandFromParameters(
            Dictionary<string, object> entityParameters,
            DapperTableDefinition tableDefinition)
        {
            var insertScript = "Insert Into " + tableDefinition.TableName + "(" + CommonConstants.StringEnter;

            var fieldsToInsert = string.Empty;
            var parameterNames = string.Empty;

            var dapperParameters = new DynamicParameters();

            foreach (var entityParameter in entityParameters)
            {
                var tableColumnDefinition = tableDefinition.ColumnDefinitions.FirstOrDefault(e => e.EntityFieldName == entityParameter.Key);

                if (tableColumnDefinition is not null)
                {
                    if (!fieldsToInsert.IsEmpty())
                    {
                        fieldsToInsert += CommonConstants.StringComma + CommonConstants.StringEnter;
                        parameterNames += CommonConstants.StringComma + CommonConstants.StringEnter;
                    }

                    fieldsToInsert += entityParameter.Key;
                    parameterNames += DAPPER_PARAMETER_INDICATOR + entityParameter.Key;

                    dapperParameters.AddNullable(tableColumnDefinition.DbFieldName, entityParameter.Value, size: tableColumnDefinition.Size);
                }
            }

            insertScript += fieldsToInsert + ")" + CommonConstants.StringEnter + "Values(" + CommonConstants.StringEnter;

            insertScript += parameterNames + ");";

            return new CommandDefinition(insertScript, dapperParameters);
        }

        public CommandDefinition UpdateCommandFromParameters(
            Dictionary<string, object> entityParameters,
            DapperTableDefinition tableDefinition,
            Dictionary<string, object> filterCriteria)
        {
            var updateScript = "Update " + tableDefinition.TableName + " Set" + CommonConstants.StringEnter;

            var fieldsToUpdate = string.Empty;
            var dapperParameters = new DynamicParameters();

            foreach (var entityParameter in entityParameters)
            {
                var tableColumnDefinition = tableDefinition.ColumnDefinitions.FirstOrDefault(d => d.EntityFieldName == entityParameter.Key);

                if (tableColumnDefinition is not null)
                {
                    if (!fieldsToUpdate.IsEmpty())
                        fieldsToUpdate += CommonConstants.StringComma + CommonConstants.StringEnter;

                    fieldsToUpdate += tableColumnDefinition.DbFieldName + " = " + DAPPER_PARAMETER_INDICATOR + tableColumnDefinition.DbFieldName;

                    dapperParameters.AddNullable(tableColumnDefinition.DbFieldName, entityParameter.Value, size: tableColumnDefinition.Size);
                }
            }

            updateScript += fieldsToUpdate + CommonConstants.StringEnter + "Where" + CommonConstants.StringEnter;

            var filterFieldNamesScript = string.Empty;

            foreach (var filterCriteriaPart in filterCriteria)
            {
                var tableColumnDefinition = tableDefinition.ColumnDefinitions.FirstOrDefault(d => d.EntityFieldName == filterCriteriaPart.Key);

                if (tableColumnDefinition is not null)
                {
                    if (!filterFieldNamesScript.IsEmpty())
                        filterFieldNamesScript += CommonConstants.StringEnter + "And ";

                    if (filterCriteriaPart.Value is not null)
                        filterFieldNamesScript += tableColumnDefinition.DbFieldName + " = " + DAPPER_PARAMETER_INDICATOR + tableColumnDefinition.DbFieldName;
                    else
                        filterFieldNamesScript += tableColumnDefinition.DbFieldName + " is null";

                    dapperParameters.AddNullable(tableColumnDefinition.DbFieldName, filterCriteriaPart.Value, size: tableColumnDefinition.Size);
                }
            }

            updateScript += filterFieldNamesScript + ";";

            return new CommandDefinition(updateScript, dapperParameters);
        }

        public CommandDefinition UpdateCommandFromEntityUpdatedPropertiesAndIdCriteria(
           IBaseEntity baseEntity,
           DapperTableDefinition tableDefinition)
        {

            return UpdateCommandByEntityUpdatedPropertiesWithCriteria(
                baseEntity,
                new Dictionary<string, object>() { { nameof(baseEntity.Id), baseEntity.Id } },
                tableDefinition);
        }

        public CommandDefinition InsertCommandFromEntityProperties(
            IBaseEntity baseEntity,
            DapperTableDefinition tableDefinition)
        {
            var insertableProperties = baseEntity.GetInsertableProperties();

            return InsertCommandFromParameters(insertableProperties, tableDefinition);
        }

        public CommandDefinition UpdateCommandByEntityUpdatedPropertiesWithCriteria(
            IBaseEntity baseEntity,
            Dictionary<string, object> filterCriteria,
            DapperTableDefinition tableDefinitions)
        {
            var updatedProperties = baseEntity.GetUpdatedProperties();

            return UpdateCommandFromParameters(updatedProperties, tableDefinitions, filterCriteria);
        }

        public CommandDefinition DeleteCommandWithEntityAndIdCriteria(
          IBaseEntity baseEntity,
          DapperTableDefinition tableDefinition)
        {
            return DeleteCommandWithCriteria(
                new Dictionary<string, object>() { { nameof(baseEntity.Id), baseEntity.Id } },
                tableDefinition);
        }

        public CommandDefinition DeleteCommandWithCriteria(
        Dictionary<string, object> filterCriteria,
        DapperTableDefinition tableDefinition)
        {
            var deleteScript = "Delete From" + CommonConstants.StringEnter + tableDefinition.TableName + CommonConstants.StringEnter + "Where" + CommonConstants.StringEnter;

            var filterFieldNamesScript = string.Empty;

            var parameters = new DynamicParameters();

            foreach (var filterCriteriaPart in filterCriteria)
            {
                var tableColumnDefinition = tableDefinition.ColumnDefinitions.FirstOrDefault(d => d.EntityFieldName == filterCriteriaPart.Key);

                if (tableColumnDefinition is not null)
                {
                    if (!filterFieldNamesScript.IsEmpty())
                        filterFieldNamesScript += CommonConstants.StringEnter + "And ";

                    if (filterCriteriaPart.Value is not null)
                        filterFieldNamesScript += tableColumnDefinition.DbFieldName + " = " + DAPPER_PARAMETER_INDICATOR + tableColumnDefinition.DbFieldName;
                    else
                        filterFieldNamesScript += tableColumnDefinition.DbFieldName + " is null";

                    parameters.AddNullable(tableColumnDefinition.DbFieldName, filterCriteriaPart.Value, size: tableColumnDefinition.Size);
                }
            }

            deleteScript += filterFieldNamesScript + ";";

            return new CommandDefinition(deleteScript, parameters);
        }

        public CommandDefinition? GetCommandDefinitionByState<TEntity>(TEntity entity, DapperTableDefinition tableDefinition)
             where TEntity : IBaseEntity
        {
            CommandDefinition? commandDefinition = null;

            switch (entity.State)
            {
                case EntityState.New:
                    commandDefinition = InsertCommandFromEntityProperties(entity, tableDefinition);
                    break;
                case EntityState.Updated:
                    commandDefinition = UpdateCommandFromEntityUpdatedPropertiesAndIdCriteria(entity, tableDefinition);
                    break;
                case EntityState.Deleted:
                    commandDefinition = DeleteCommandWithEntityAndIdCriteria(entity, tableDefinition);
                    break;
                default:
                    break;
            }

            return commandDefinition;
        }

        public virtual IList<CommandDefinition> GetCommandDefinitions(Entity entity)
        {
            return [];
        }
    }
}