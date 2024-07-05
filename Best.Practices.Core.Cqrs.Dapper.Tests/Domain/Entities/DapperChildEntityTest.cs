
using Best.Practices.Core.Domain.Entities;

namespace Best.Practices.Core.Cqrs.Dapper.Tests.Domain.Entities
{
    public class DapperChildEntityTest : BaseEntity
    {
        public virtual int Number { get; set; }
        public virtual string Description { get; set; }
    }
}