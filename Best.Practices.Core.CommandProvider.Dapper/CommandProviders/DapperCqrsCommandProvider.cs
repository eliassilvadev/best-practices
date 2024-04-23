using Best.Practices.Core.Common;
using Best.Practices.Core.Domain.Cqrs;
using Best.Practices.Core.Domain.Cqrs.CommandProvider;
using Best.Practices.Core.Domain.Models.Interfaces;
using Best.Practices.Core.Extensions;
using Dapper;
using System.Data.Common;

namespace Best.Practices.Core.CommandProvider.Dapper.CommandProviders
{
    public abstract class DapperCqrsCommandProvider<Entity> : ICqrsCommandProvider<Entity> where Entity : IBaseEntity
    {
        const char DAPPER_PARAMETER_INDICATOR = '@';
        protected DbConnection Connection { get; }

        public DapperCqrsCommandProvider(DbConnection connection)
        {
            Connection = connection;
        }

        public abstract Entity GetById(Guid id);

        public abstract IEntityCommand GetAddCommand(Entity entity);

        public abstract IEntityCommand GetDeleteCommand(Entity entity);

        public abstract IEntityCommand GetUpdateCommand(Entity entity);

        protected void AddStringNullableParameter(DynamicParameters parameters, string parameterName, object parameterValue, int? size = null)
        {
            if (string.IsNullOrWhiteSpace(parameterName))
                parameters.Add(parameterName, DBNull.Value, size: size);
            else
                parameters.Add(parameterName, parameterValue, size: size);
        }

        protected CommandDefinition CreateInsertCommandFromParameters(IList<string> fieldNamesToInsert, string tableName)
        {
            var insertScript = "Insert Into " + tableName + "(" + tableName;

            var fieldsToInsert = string.Empty;
            var parameterNames = string.Empty;

            foreach (var fieldName in fieldNamesToInsert)
            {
                if (!fieldsToInsert.IsEmpty())
                {
                    fieldsToInsert += CommonConstants.CharComma;
                    parameterNames += CommonConstants.CharComma + CommonConstants.CharEnter;
                }

                fieldsToInsert += fieldName + CommonConstants.CharEnter;
                parameterNames += DAPPER_PARAMETER_INDICATOR + fieldName + CommonConstants.CharEnter;
            }

            insertScript += fieldsToInsert + " Values (" + CommonConstants.CharEnter;

            insertScript += parameterNames + ");";

            return new CommandDefinition(insertScript);
        }

        protected CommandDefinition CreateUpdateCommandFromParameters(IList<string> fieldNamesToUpdate, IList<string> filterFieldNames, string tableName)
        {
            var updateScript = "Update " + tableName + " Set " + CommonConstants.CharEnter;

            var fieldsToUpdate = string.Empty;

            foreach (var parameterName in fieldNamesToUpdate)
            {
                if (!fieldsToUpdate.IsEmpty())
                    fieldsToUpdate += CommonConstants.CharEnter;

                fieldsToUpdate += parameterName + " = " + DAPPER_PARAMETER_INDICATOR + parameterName;
            }

            updateScript += fieldsToUpdate + " Where " + CommonConstants.CharEnter;

            var filterFieldNamesScript = string.Empty;

            foreach (var filterFieldName in filterFieldNames)
            {
                if (!filterFieldNamesScript.IsEmpty())
                    filterFieldNamesScript += CommonConstants.CharEnter;

                filterFieldNamesScript += filterFieldName + " = " + DAPPER_PARAMETER_INDICATOR + filterFieldName;
            }

            updateScript += filterFieldNamesScript + CommonConstants.CharEnter + ";";

            return new CommandDefinition(updateScript);
        }

        protected virtual Dictionary<string, string> GetEntityFieldMappings(IBaseEntity baseEntity);

        protected CommandDefinition CreateUpdateCommandFromUpdatedFields(IBaseEntity baseEntity, IList<string> filterFieldNames, string tableName)
        {
            var updatedFields = baseEntity.get
        }
    }
}