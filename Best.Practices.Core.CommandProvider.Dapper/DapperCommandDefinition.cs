using Best.Practices.Core.Domain.Models.Interfaces;
using Dapper;
using System.Data.Common;

namespace Best.Practices.Core.CommandProvider.Dapper
{
    public delegate void AfterExecuteMethod(IBaseEntity affectedEntity);
    public class DapperCommandDefinition
    {
        public CommandDefinition CommandDefinition { get; }

        protected AfterExecuteMethod AfterExecute { get; }

        public DapperCommandDefinition(string script, AfterExecuteMethod afterExecuteMethod = null)
        {
            AfterExecute = afterExecuteMethod;

            CommandDefinition = new CommandDefinition(script);
        }

        public void Execute(DbConnection connection)
        {
            connection.Execute(CommandDefinition);
        }
    }
}