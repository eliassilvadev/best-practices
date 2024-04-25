using Best.Practices.Core.CommandProvider.Dapper.CommandProviders;
using Best.Practices.Core.Common;
using Best.Practices.Core.Domain.Cqrs;
using Best.Practices.Core.Domain.Models.Interfaces;
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
                Console.WriteLine(ex.Message);

                sucess = false;
            }

            return sucess;
        }

        public CommandDefinition InsertCommandFromParameters(
            Dictionary<string, object> entityParameters, IList<DapperTableColumnDefinitions> tableColumnDefinitions, string tableName)
        {
            var insertScript = "Insert Into " + tableName + CommonConstants.StringEnter + "(" + tableName;

            var fieldsToInsert = string.Empty;
            var parameterNames = string.Empty;

            var dapperParameters = new DynamicParameters();

            foreach (var entityParameter in entityParameters)
            {
                var tableColumnDefinition = tableColumnDefinitions.FirstOrDefault(d => d.EntityFieldName == entityParameter.Key);

                if (tableColumnDefinition is not null)
                {
                    if (!fieldsToInsert.IsEmpty())
                    {
                        fieldsToInsert += CommonConstants.StringComma;
                        parameterNames += CommonConstants.StringComma + CommonConstants.StringEnter;
                    }

                    fieldsToInsert += entityParameter.Key + CommonConstants.StringEnter;
                    parameterNames += DAPPER_PARAMETER_INDICATOR + entityParameter.Key + CommonConstants.StringEnter;

                    if (entityParameter.Value is not null)
                        dapperParameters.Add(tableColumnDefinition.DbFieldName, entityParameter.Value, size: tableColumnDefinition.Size);
                    else
                        dapperParameters.Add(tableColumnDefinition.DbFieldName, DBNull.Value);
                }
            }

            insertScript += fieldsToInsert + " Values (" + CommonConstants.StringEnter;

            insertScript += parameterNames + ");";

            return new CommandDefinition(insertScript, dapperParameters);
        }

        public CommandDefinition UpdateCommandFromParameters(IList<string> fieldNamesToUpdate, IList<string> filterCriteria, string tableName)
        {
            var updateScript = "Update " + tableName + " Set " + CommonConstants.StringEnter;

            var fieldsToUpdate = string.Empty;

            foreach (var parameterName in fieldNamesToUpdate)
            {
                if (!fieldsToUpdate.IsEmpty())
                    fieldsToUpdate += CommonConstants.StringEnter;

                fieldsToUpdate += parameterName + " = " + DAPPER_PARAMETER_INDICATOR + parameterName;
            }

            updateScript += fieldsToUpdate + " Where " + CommonConstants.StringEnter;

            var filterFieldNamesScript = string.Empty;

            foreach (var filterFieldName in filterCriteria)
            {
                if (!filterFieldNamesScript.IsEmpty())
                    filterFieldNamesScript += CommonConstants.StringEnter;

                filterFieldNamesScript += filterFieldName + " = " + DAPPER_PARAMETER_INDICATOR + filterFieldName;
            }

            updateScript += filterFieldNamesScript + CommonConstants.StringEnter + ";";

            return new CommandDefinition(updateScript);
        }

        public CommandDefinition UpdateCommandFromEntityUpdatedPropertiesAndIdCriteria(
           IBaseEntity baseEntity,
           IList<DapperTableColumnDefinitions> tableColumnDefinitions,
           string tableName,
           string idFieldName)
        {
            var filterCriteriaById = new Dictionary<string, object>()
            {
                { idFieldName, baseEntity.Id }
            };

            return UpdateCommandByEntityUpdatedPropertiesWithCriteria(baseEntity, filterCriteriaById, tableColumnDefinitions, tableName);
        }

        public CommandDefinition InsertCommandFromEntityProperties(
            IBaseEntity baseEntity,
            IList<DapperTableColumnDefinitions> tableColumnDefinitions,
            string tableName)
        {
            var insertScript = "Insert Into " + tableName + "(" + CommonConstants.StringEnter;

            var fieldsToInsert = string.Empty;
            var parameterNames = string.Empty;

            var dapperParameters = new DynamicParameters();

            var insertableProperties = baseEntity.GetInsertableProperties();

            foreach (var insertableProperty in insertableProperties)
            {
                var tableColumnDefinition = tableColumnDefinitions.FirstOrDefault(e => e.EntityFieldName == insertableProperty.Key);

                if (tableColumnDefinition is not null)
                {
                    if (!fieldsToInsert.IsEmpty())
                    {
                        fieldsToInsert += CommonConstants.StringComma + CommonConstants.StringEnter;
                        parameterNames += CommonConstants.StringComma + CommonConstants.StringEnter;
                    }

                    fieldsToInsert += insertableProperty.Key;
                    parameterNames += DAPPER_PARAMETER_INDICATOR + insertableProperty.Key;

                    if (insertableProperty.Value is not null)
                        dapperParameters.Add(tableColumnDefinition.DbFieldName, insertableProperty.Value, size: tableColumnDefinition.Size);
                    else
                        dapperParameters.Add(tableColumnDefinition.DbFieldName, DBNull.Value);
                }
            }

            insertScript += fieldsToInsert + ")" + CommonConstants.StringEnter + "Values(" + CommonConstants.StringEnter;

            insertScript += parameterNames + ");";

            return new CommandDefinition(insertScript, dapperParameters);
        }

        public CommandDefinition UpdateCommandByEntityUpdatedPropertiesWithCriteria(
            IBaseEntity baseEntity,
            Dictionary<string, object> filterCriteria,
            IList<DapperTableColumnDefinitions> tableColumnDefinitions,
            string tableName)
        {
            var updatedProperties = baseEntity.GetUpdatedProperties();

            var updateScript = "Update " + tableName + " Set" + CommonConstants.StringEnter;

            var fieldsToUpdate = string.Empty;
            var dapperParameters = new DynamicParameters();

            foreach (var updatedProperty in updatedProperties)
            {
                var tableColumnDefinition = tableColumnDefinitions.FirstOrDefault(d => d.EntityFieldName == updatedProperty.Key);

                if (tableColumnDefinition is not null)
                {
                    if (!fieldsToUpdate.IsEmpty())
                        fieldsToUpdate += CommonConstants.StringComma + CommonConstants.StringEnter;

                    fieldsToUpdate += tableColumnDefinition.DbFieldName + " = " + DAPPER_PARAMETER_INDICATOR + tableColumnDefinition.DbFieldName;

                    if (updatedProperty.Value is not null)
                        dapperParameters.Add(tableColumnDefinition.DbFieldName, updatedProperty.Value, size: tableColumnDefinition.Size);
                    else
                        dapperParameters.Add(tableColumnDefinition.DbFieldName, DBNull.Value);
                }
            }

            updateScript += fieldsToUpdate + CommonConstants.StringEnter + "Where" + CommonConstants.StringEnter;

            var filterFieldNamesScript = string.Empty;

            foreach (var filterCriteriaPart in filterCriteria)
            {
                var tableColumnDefinition = tableColumnDefinitions.FirstOrDefault(d => d.EntityFieldName == filterCriteriaPart.Key);

                if (tableColumnDefinition is not null)
                {
                    if (!filterFieldNamesScript.IsEmpty())
                        filterFieldNamesScript += CommonConstants.StringEnter + "And ";

                    if (filterCriteriaPart.Value is not null)
                        filterFieldNamesScript += tableColumnDefinition.DbFieldName + " = " + DAPPER_PARAMETER_INDICATOR + tableColumnDefinition.DbFieldName;
                    else
                        filterFieldNamesScript += tableColumnDefinition.DbFieldName + " is null";

                    if (filterCriteriaPart.Value is not null)
                        dapperParameters.Add(tableColumnDefinition.DbFieldName, filterCriteriaPart.Value, size: tableColumnDefinition.Size);
                }
            }

            updateScript += filterFieldNamesScript + ";";

            return new CommandDefinition(updateScript, dapperParameters);
        }

        public CommandDefinition DeleteCommandWithEntityAndIdCriteria(
          IBaseEntity baseEntity,
          IList<DapperTableColumnDefinitions> tableColumnDefinitions,
          string tableName)
        {
            var idFieldDefinition = tableColumnDefinitions.FirstOrDefault(d => d.EntityFieldName == nameof(baseEntity.Id));
            return DeleteCommandWithCriteria(
                new Dictionary<string, object>() { { nameof(baseEntity.Id), baseEntity.Id } },
                [idFieldDefinition],
                tableName);
        }

        public CommandDefinition DeleteCommandWithCriteria(
        Dictionary<string, object> filterCriteria,
        IList<DapperTableColumnDefinitions> tableColumnDefinitions,
        string tableName)
        {
            var deleteScript = "Delete From" + CommonConstants.StringEnter + tableName + CommonConstants.StringEnter + "Where" + CommonConstants.StringEnter;

            var filterFieldNamesScript = string.Empty;

            var parameters = new DynamicParameters();

            foreach (var filterCriteriaPart in filterCriteria)
            {
                var tableColumnDefinition = tableColumnDefinitions.FirstOrDefault(d => d.EntityFieldName == filterCriteriaPart.Key);

                if (tableColumnDefinition is not null)
                {
                    if (!filterFieldNamesScript.IsEmpty())
                        filterFieldNamesScript += CommonConstants.StringEnter + "And ";

                    if (filterCriteriaPart.Value is not null)
                        filterFieldNamesScript += tableColumnDefinition.DbFieldName + " = " + DAPPER_PARAMETER_INDICATOR + tableColumnDefinition.DbFieldName;
                    else
                        filterFieldNamesScript += tableColumnDefinition.DbFieldName + " is null";

                    if (filterCriteriaPart.Value is not null)
                        parameters.Add(tableColumnDefinition.DbFieldName, filterCriteriaPart.Value, size: tableColumnDefinition.Size);
                }
            }

            deleteScript += filterFieldNamesScript + ";";

            return new CommandDefinition(deleteScript, parameters);
        }

        public virtual IList<CommandDefinition> GetCommandDefinitions(Entity entity)
        {
            return [];
        }
    }
}