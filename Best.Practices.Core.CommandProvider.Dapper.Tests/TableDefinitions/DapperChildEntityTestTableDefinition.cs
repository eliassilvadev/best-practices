using Best.Practices.Core.CommandProvider.Dapper.CommandProviders;
using System.Data;

namespace Best.Practices.Core.CommandProvider.Dapper.Tests.TableDefinitions
{
    public static class DapperChildEntityTestTableDefinition
    {
        public static readonly DapperTableDefinition TableDefinition = new DapperTableDefinition
        {
            TableName = "ChildEntityTestTable",
            ColumnDefinitions = new List<DapperTableColumnDefinition>()
            {
                new DapperTableColumnDefinition
                {
                    DbFieldName = "Id",
                    EntityFieldName = "Id",//, nameof(DapperChildEntityTest.Id),
                    Type = DbType.Guid
                },
                new DapperTableColumnDefinition
                {
                    DbFieldName = "Number",
                    EntityFieldName ="Number",//, nameof(DapperChildEntityTest.Number),
                    Type = DbType.AnsiString,
                    Size = 6
                },
                new DapperTableColumnDefinition
                {
                    DbFieldName = "Description",
                    EntityFieldName ="Description",//, nameof(DapperChildEntityTest.Description),
                    Type = DbType.AnsiString,
                    Size = 100
                }
            }
        };
    }
}