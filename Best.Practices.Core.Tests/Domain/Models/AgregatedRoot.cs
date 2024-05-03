using Best.Practices.Core.Domain.Models;
using System.Diagnostics.CodeAnalysis;

namespace Best.Practices.Core.Tests.Domain.Models
{
    [ExcludeFromCodeCoverage]
    public class AgregatedRoot : BaseEntity
    {
        public virtual string SampleName { get; set; }
        public virtual ChildClassLevel2 ChildClassLevel2 { get; set; }
        public virtual EntityList<ChildClassListItem> Items { get; set; }

        public AgregatedRoot() : base()
        {
            Items = new EntityList<ChildClassListItem>();
        }

        public void AddChildItem(ChildClassListItem childClassListItem)
        {
            Items.Add(childClassListItem);
        }
    }

    [ExcludeFromCodeCoverage]
    public class ChildClassLevel2 : BaseEntity
    {
        public virtual string SampleName { get; set; }

        public virtual ChildClassLevel3 ChildClassLevel3 { get; set; }

        public virtual EntityList<ChildClassListItem2> Items { get; set; }

        public ChildClassLevel2() : base()
        {
            Items = new EntityList<ChildClassListItem2>();
        }

        public void AddChildItem(ChildClassListItem2 childClassListItem2)
        {
            Items.Add(childClassListItem2);
        }
    }

    [ExcludeFromCodeCoverage]
    public class ChildClassListItem : BaseEntity
    {
        public virtual string SampleName { get; set; }

        public virtual ChildClassLevel3 ChildClassLevel3 { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class ChildClassListItem2 : BaseEntity
    {
        public virtual string SampleName { get; set; }

        public virtual ChildClassLevel3 ChildClassLevel3 { get; set; }
        public virtual string SampleSurname { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class ChildClassLevel3 : BaseEntity
    {
        public virtual string SampleName { get; set; }

        public virtual AgregatedRoot AgreegatedRoot { get; set; }

        public virtual void SetSampleName(string sampleName)
        {
            SampleName = sampleName;

            NotifyEntityObserversPropertyUpdate(nameof(SampleName), sampleName);
        }
    }
}
