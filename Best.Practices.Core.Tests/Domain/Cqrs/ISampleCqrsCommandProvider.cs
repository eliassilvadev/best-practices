using Best.Practices.Core.Domain.Cqrs.CommandProvider;
using Best.Practices.Core.Tests.Domain.Models;

namespace Best.Practices.Core.Tests.Domain.Cqrs
{
    public interface ISampleCqrsCommandProvider : ICqrsCommandProvider<SampleEntity>
    {
        SampleEntity GetBySampleName(string sampleName);
    }
}