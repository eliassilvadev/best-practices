
using Best.Practices.Core.Domain.Models;

namespace Best.Practices.Core.CommandProvider.Dapper.Tests.Domain.Models
{
    public class DapperTestEntity : BaseEntity
    {
        public virtual string Code { get; set; }
        public virtual string Name { get; set; }

        public virtual EntityList<DapperChildEntityTest> Childs { get; protected set; }

        public DapperTestEntity()
        {
            Childs = new EntityList<DapperChildEntityTest>();
        }
    }
}