using Best.Practices.Core.Tests.Domain.Models;

namespace Best.Practices.Core.Tests.Application.SampleUseCasesDtos
{
    public class SampleChildUseCaseOutput
    {
        public SampleChildUseCaseOutput(SampleEntity entity)
        {
            SampleId = entity.Id;
            SampleName = entity.SampleName;
        }
        public Guid SampleId { get; protected set; }
        public string SampleName { get; protected set; }
    }
}