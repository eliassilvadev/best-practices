using System.Data;

namespace Best.Practices.Core.CommandProvider.Dapper.CommandProviders
{
    public class DapperTableColumnDefinition
    {
        public string EntityFieldName { get; set; }
        public string DbFieldName { get; set; }
        public DbType Type { get; set; }
        public int? Size { get; set; }
        public bool IsParentEntity { get; set; } = false;
    }
}