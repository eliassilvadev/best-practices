using Best.Practices.Core.Tests.Application.SampleUseCasesDtos;

namespace Best.Practices.Core.Tests.Application.Dtos.Builders
{
    public class SampleChildUseCaseOutputBuilder
    {
        private Guid _sampleId;
        private string _sampleName;

        public SampleChildUseCaseOutputBuilder()
        {
            _sampleId = Guid.NewGuid();
            _sampleName = "Sample Name";
        }

        public SampleChildUseCaseOutputBuilder WithSampleId(Guid sampleId)
        {
            _sampleId = sampleId;
            return this;
        }

        public SampleChildUseCaseOutputBuilder WithSampleName(string sampleName)
        {
            _sampleName = sampleName;
            return this;
        }

        public SampleChildUseCaseOutput Build()
        {
            return new SampleChildUseCaseOutput()
            {
                SampleId = _sampleId,
                SampleName = _sampleName
            };
        }
    }
}