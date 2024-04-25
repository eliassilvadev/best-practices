
using Best.Practices.Core.Domain.Models;

namespace Best.Practices.Core.CommandProvider.Dapper.Tests.Domain.Models
{
    public class DapperChildEntityTest : BaseEntity
    {
        public virtual int Number { get; set; }
        public virtual string Description { get; set; }
    }
}