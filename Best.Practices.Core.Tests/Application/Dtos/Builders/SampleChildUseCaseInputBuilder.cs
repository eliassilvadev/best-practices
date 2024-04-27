using Best.Practices.Core.Tests.Application.SampleUseCasesDtos;
using System;

namespace Best.Practices.Core.Tests.Application.Dtos.Builders
{
    public class SampleChildUseCaseInputBuilder
    {
        private Guid _sampleId;
        private Guid _sampleLookUpId;
        private string _sampleName;
        private decimal _monthlySalary;

        public SampleChildUseCaseInputBuilder()
        {
            _sampleId = Guid.NewGuid();
            _sampleLookUpId = Guid.NewGuid();
            _sampleName = "Sample Name";
            _monthlySalary = 1000m;
        }

        public SampleChildUseCaseInputBuilder WithSampleId(Guid sampleId)
        {
            _sampleId = sampleId;
            return this;
        }

        public SampleChildUseCaseInputBuilder WithSampleLookUpId(Guid sampleLookUpId)
        {
            _sampleLookUpId = sampleLookUpId;
            return this;
        }

        public SampleChildUseCaseInputBuilder WithSampleName(string sampleName)
        {
            _sampleName = sampleName;
            return this;
        }

        public SampleChildUseCaseInputBuilder WithMonthlySalary(decimal monthlySalary)
        {
            _monthlySalary = monthlySalary;
            return this;
        }

        public SampleChildUseCaseInput Build()
        {
            return new SampleChildUseCaseInput
            {
                SampleId = _sampleId,
                SampleLookUpId = _sampleLookUpId,
                SampleName = _sampleName,
                MonthlySalary = _monthlySalary
            };
        }
    }
}