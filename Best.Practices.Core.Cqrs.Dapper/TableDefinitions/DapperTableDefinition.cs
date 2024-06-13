namespace Best.Practices.Core.Cqrs.Dapper.TableDefinitions
{
    public class DapperTableDefinition
    {
        public string TableName { get; set; }
        public List<DapperTableColumnDefinition> ColumnDefinitions { get; set; }
    }
}