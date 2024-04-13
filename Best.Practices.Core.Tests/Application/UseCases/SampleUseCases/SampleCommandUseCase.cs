using Best.Practices.Core.Application.UseCases;
using Best.Practices.Core.Tests.Application.SampleUseCasesDtos;
using Best.Practices.Core.Tests.Domain.Models;
using Best.Practices.Core.Tests.Domain.Repositories.SampleRepository;
using Best.Practices.Core.UnitOfWork.Interfaces;

namespace Best.Practices.Core.Tests.Application.UseCases.SampleUseCases
{
    public class SampleCommandUseCase : CommandUseCase<SampleChildUseCaseInput, SampleChildUseCaseOutput>
    {
        private readonly ISampleRepository _sampleRepository;
        protected override string SaveChangesErrorMessage => "SampleChildCommandUseCase Error message";

        public SampleCommandUseCase(
            ISampleRepository sampleRepository,
            IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _sampleRepository = sampleRepository;
        }

        public override UseCaseOutput<SampleChildUseCaseOutput> InternalExecute(SampleChildUseCaseInput input)
        {
            ThrowsInvalidInputIfEntityExists(
                _sampleRepository.GetBySampleName,
                input.SampleName, $"A entity with SampleName {input.SampleName} already Exists");

            ThrowsResourceNotFoundIfEntityDoesNotExists(
                _sampleRepository.GetById,
                input.SampleLookUpId, $"A lookup entity with id {input.SampleLookUpId} does not Exists", out var lookupEntity);

            var entity = new SampleEntity()
            {
                SampleName = input.SampleName
            };

            _sampleRepository.Persist(entity, UnitOfWork);

            SaveChanges();

            return CreateSuccessOutput(new SampleChildUseCaseOutput(entity));
        }
    }
}