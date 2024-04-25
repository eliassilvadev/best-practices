using Best.Practices.Core.CommandProvider.Dapper.CommandProviders;
using Best.Practices.Core.CommandProvider.Dapper.Tests.Domain.Models;
using System.Data;

namespace Best.Practices.Core.CommandProvider.Dapper.Tests.TableDefinitions
{
    public static class DapperTestEntityTableDefinition
    {
        public static List<DapperTableColumnDefinitions> TableColumnDefinitions
        {
            get
            {
                return new List<DapperTableColumnDefinitions>()
                {
                    new DapperTableColumnDefinitions()
                    {
                        DbFieldName = "Id",
                        EntityFieldName = nameof(DapperTestEntity.Id),
                        Type = DbType.Guid
                    },
                    new DapperTableColumnDefinitions()
                    {
                        DbFieldName = "Code",
                        EntityFieldName = nameof(DapperTestEntity.Code),
                        Type = DbType.AnsiString,
                        Size = 5
                    },
                    new DapperTableColumnDefinitions()
                    {
                        DbFieldName = "Name",
                        EntityFieldName = nameof(DapperTestEntity.Name),
                        Type = DbType.AnsiString,
                        Size = 100
                    }
                };
            }
        }
    }
}