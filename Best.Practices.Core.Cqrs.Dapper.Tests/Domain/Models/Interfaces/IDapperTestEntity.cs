using Best.Practices.Core.Domain.Models;
using Best.Practices.Core.Domain.Models.Interfaces;

namespace Best.Practices.Core.Cqrs.Dapper.Tests.Domain.Models.Interfaces
{
    public interface IDapperTestEntity : IBaseEntity
    {
        string Code { get; set; }
        string Name { get; set; }

        EntityList<DapperChildEntityTest> Childs { get; set; }
    }
}