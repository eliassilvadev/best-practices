namespace Best.Practices.Core.Tests.Application.SampleUseCasesDtos
{
    public record SampleChildUseCaseInput
    {
        public Guid SampleId { get; set; }
        public Guid SampleLookUpId { get; set; }
        public decimal MonthlySalary { get; set; }
        public string SampleName { get; set; }
    }
}