using Best.Practices.Core.Domain.Cqrs.CommandProviders;
using Best.Practices.Core.Tests.Domain.Models;

namespace Best.Practices.Core.Tests.Domain.Cqrs.CommandProviders
{
    public interface ISampleCqrsCommandProvider : ICqrsCommandProvider<SampleEntity>
    {
        Task<SampleEntity> GetBySampleName(string sampleName);
    }
}