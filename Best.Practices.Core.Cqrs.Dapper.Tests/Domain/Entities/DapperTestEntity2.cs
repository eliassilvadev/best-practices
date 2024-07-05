using Best.Practices.Core.Domain.Entities;

namespace Best.Practices.Core.Cqrs.Dapper.Tests.Domain.Entities
{
    public class DapperTestEntity2 : BaseEntity
    {
        public DapperTestEntity ChildEntity { get; set; }
    }
}