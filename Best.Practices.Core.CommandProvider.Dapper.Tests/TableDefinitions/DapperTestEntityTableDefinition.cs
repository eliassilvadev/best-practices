using Best.Practices.Core.CommandProvider.Dapper.CommandProviders;
using Best.Practices.Core.CommandProvider.Dapper.Tests.Domain.Models;
using System.Data;

namespace Best.Practices.Core.CommandProvider.Dapper.Tests.TableDefinitions
{
    public static class DapperTestEntityTableDefinition
    {
        public static readonly DapperTableDefinition TableDefinition = new DapperTableDefinition
        {
            TableName = "EntityTestTable",
            ColumnDefinitions = new List<DapperTableColumnDefinition>()
            {
                new DapperTableColumnDefinition
                {
                    DbFieldName = "Id",
                    EntityFieldName = nameof(DapperTestEntity.Id),
                    Type = DbType.Guid
                },
                new DapperTableColumnDefinition
                {
                    DbFieldName = "Code",
                    EntityFieldName = nameof(DapperTestEntity.Code),
                    Type = DbType.AnsiString,
                    Size = 5
                },
                new DapperTableColumnDefinition
                {
                    DbFieldName = "Name",
                    EntityFieldName = nameof(DapperTestEntity.Name),
                    Type = DbType.AnsiString,
                    Size = 100
                }
            }
        };
    }
}