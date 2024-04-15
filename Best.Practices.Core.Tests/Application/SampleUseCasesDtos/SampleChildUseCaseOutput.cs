namespace Best.Practices.Core.Tests.Application.SampleUseCasesDtos
{
    public record SampleChildUseCaseOutput
    {
        public Guid SampleId { get; set; }
        public string SampleName { get; set; }
    }
}