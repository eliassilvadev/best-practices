using Best.Practices.Core.CommandProvider.Dapper.CommandProviders;
using Best.Practices.Core.CommandProvider.Dapper.Tests.Domain.Models;
using System.Data;

namespace Best.Practices.Core.CommandProvider.Dapper.Tests.TableDefinitions
{
    public class DapperChildEntityTestTableDefinition
    {
        public string TableName { get; set; }
        public static List<DapperTableColumnDefinitions> TableColumnDefinitions
        {
            get
            {
                return new List<DapperTableColumnDefinitions>()
                {
                    new DapperTableColumnDefinitions()
                    {
                        DbFieldName = "Id",
                        EntityFieldName = nameof(DapperChildEntityTest.Id),
                        Type = DbType.Guid
                    },
                    new DapperTableColumnDefinitions()
                    {
                        DbFieldName = "Number",
                        EntityFieldName = nameof(DapperChildEntityTest.Number),
                        Type = DbType.AnsiString,
                        Size = 6
                    },
                    new DapperTableColumnDefinitions()
                    {
                        DbFieldName = "Description",
                        EntityFieldName = nameof(DapperChildEntityTest.Description),
                        Type = DbType.AnsiString,
                        Size = 100
                    }
                };
            }
        }
    }
}
