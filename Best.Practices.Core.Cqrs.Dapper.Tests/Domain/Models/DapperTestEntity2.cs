using Best.Practices.Core.Domain.Models;

namespace Best.Practices.Core.Cqrs.Dapper.Tests.Domain.Models
{
    public class DapperTestEntity2 : BaseEntity
    {
        public DapperTestEntity ChildEntity { get; set; }
    }
}