using Best.Practices.Core.Domain.Entities;
using Best.Practices.Core.Domain.Entities.Interfaces;

namespace Best.Practices.Core.Cqrs.Dapper.Tests.Domain.Entities.Interfaces
{
    public interface IDapperTestEntity : IBaseEntity
    {
        string Code { get; set; }
        string Name { get; set; }

        EntityList<DapperChildEntityTest> Childs { get; set; }
    }
}