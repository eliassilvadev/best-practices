using Best.Practices.Core.Cqrs.Dapper.TableDefinitions;
using Best.Practices.Core.Cqrs.Dapper.Tests.Domain.Entities;
using System.Data;

namespace Best.Practices.Core.Cqrs.Dapper.Tests.TableDefinitions
{
    public static class DapperTestEntity2TableDefinition
    {
        public static readonly DapperTableDefinition TableDefinition = new DapperTableDefinition
        {
            TableName = "EntityTestTable2",
            ColumnDefinitions = new List<DapperTableColumnDefinition>()
            {
                new DapperTableColumnDefinition
                {
                    DbFieldName = "Id",
                    EntityFieldName = nameof(DapperTestEntity2.Id),
                    Type = DbType.Guid
                },
                new DapperTableColumnDefinition
                {
                    DbFieldName = "ChildEntity_Id",
                    EntityFieldName = nameof(DapperTestEntity2.ChildEntity),
                    Type = DbType.Guid
                }
                ,
                new DapperTableColumnDefinition
                {
                    DbFieldName = "ChildEntity2_Id",
                    EntityFieldName = "ChildEntity2.Id",
                    Type = DbType.Guid
                }
            }
        };
    }
}