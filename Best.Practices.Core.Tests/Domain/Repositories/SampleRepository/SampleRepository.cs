using Best.Practices.Core.Domain.Repositories;
using Best.Practices.Core.Tests.Domain.Cqrs.CommandProviders;
using Best.Practices.Core.Tests.Domain.Entities;

namespace Best.Practices.Core.Tests.Domain.Repositories.SampleRepository
{
    public class SampleRepository : Repository<SampleEntity>, ISampleRepository
    {
        private readonly ISampleCqrsCommandProvider _commandProvider;

        public SampleRepository(ISampleCqrsCommandProvider commandProvider) : base(commandProvider)
        {
            _commandProvider = commandProvider;
        }

        public async Task<SampleEntity> GetBySampleName(string sampleName)
        {
            return HandleAfterGetFromCommandProvider(await _commandProvider.GetBySampleName(sampleName));
        }
    }
}