namespace Best.Practices.Core.CommandProvider.Dapper.CommandProviders
{
    public class DapperTableDefinition
    {
        public string TableName { get; set; }
        public List<DapperTableColumnDefinition> ColumnDefinitions { get; set; }
    }
}