using Best.Practices.Core.Cqrs.Dapper.TableDefinitions;
using Best.Practices.Core.Cqrs.Dapper.Tests.Domain.Entities;
using System.Data;

namespace Best.Practices.Core.Cqrs.Dapper.Tests.TableDefinitions
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
                    EntityFieldName = nameof(DapperChildEntityTest.Id),
                    Type = DbType.Guid
                },
                new DapperTableColumnDefinition
                {
                    DbFieldName = "Number",
                    EntityFieldName = nameof(DapperChildEntityTest.Number),
                    Type = DbType.AnsiString,
                    Size = 6
                },
                new DapperTableColumnDefinition
                {
                    DbFieldName = "Description",
                    EntityFieldName = nameof(DapperChildEntityTest.Description),
                    Type = DbType.AnsiString,
                    Size = 100
                },
                new DapperTableColumnDefinition
                {
                    DbFieldName = "ParentEntityId",
                    EntityFieldName = "ParentEntity",
                    Type = DbType.Guid,
                    IsParentEntity = true
                }
            }
        };
    }
}