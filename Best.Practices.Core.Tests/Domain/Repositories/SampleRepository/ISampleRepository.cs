using Best.Practices.Core.Domain.Repositories.Interfaces;
using Best.Practices.Core.Tests.Domain.Entities;

namespace Best.Practices.Core.Tests.Domain.Repositories.SampleRepository
{
    public interface ISampleRepository : IRepository<SampleEntity>
    {
        Task<SampleEntity> GetBySampleName(string sampleName);
    }
}