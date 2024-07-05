using Best.Practices.Core.Domain.Entities;
using Best.Practices.Core.Domain.Entities.Interfaces;

namespace Best.Practices.Core.Tests.Domain.Entities
{
    public class SampleEntityWithChilds : BaseEntity
    {
        public virtual string SampleCode { get; set; }
        public virtual string SampleName { get; set; }
        public virtual IEntityList<ChildClassListItem> Childs { get; set; }

        public SampleEntityWithChilds()
        {
            Childs = new EntityList<ChildClassListItem>();
        }

        public void AddChild(ChildClassListItem item)
        {
            Childs.Add(item);
        }
    }
}