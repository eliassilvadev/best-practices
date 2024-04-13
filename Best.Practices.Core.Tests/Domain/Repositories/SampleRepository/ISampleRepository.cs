using Best.Practices.Core.Domain.Repositories.Interfaces;
using Best.Practices.Core.Tests.Domain.Models;

namespace Best.Practices.Core.Tests.Domain.Repositories.SampleRepository
{
    public interface ISampleRepository : IRepository<SampleEntity>
    {
        SampleEntity GetBySampleName(string sampleName);
    }
}