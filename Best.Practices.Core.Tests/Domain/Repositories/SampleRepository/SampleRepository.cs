﻿using Best.Practices.Core.Domain.Repositories;
using Best.Practices.Core.Tests.Domain.Cqrs;
using Best.Practices.Core.Tests.Domain.Models;

namespace Best.Practices.Core.Tests.Domain.Repositories.SampleRepository
{
    public class SampleRepository : Repository<SampleEntity>, ISampleRepository
    {
        private readonly ISampleCqrsCommandProvider _commandProvider;

        public SampleRepository(ISampleCqrsCommandProvider commandProvider) : base(commandProvider)
        {
            _commandProvider = commandProvider;
        }

        public SampleEntity GetBySampleName(string sampleName)
        {
            return HandleAfterGetFromCommandProvider(_commandProvider.GetBySampleName(sampleName));
        }
    }
}