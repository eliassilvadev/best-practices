using Best.Practices.Core.Tests.Domain.Models;

namespace Best.Practices.Core.Tests.Application.Dtos.Builders
{
    public class SampleEntityBuilder
    {
        private string _sampleName;

        public SampleEntityBuilder()
        {
            _sampleName = "SampleName";
        }

        public SampleEntityBuilder WithSampleName(string sampleName)
        {
            _sampleName = sampleName;
            return this;
        }

        public SampleEntity Build()
        {
            return new SampleEntity()
            {
                SampleName = _sampleName
            };
        }
    }
}