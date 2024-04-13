using Best.Practices.Core.Application.Dtos.Input;
using Best.Practices.Core.Application.UseCases;
using Best.Practices.Core.Domain.Cqrs.QueryProvider;
using Best.Practices.Core.Tests.Application.SampleUseCasesDtos;
using FluentValidation;
using Moq;

namespace Best.Practices.Core.Tests.Application.UseCases
{
    public class GetPaginatedResultsUseCaseTests
    {
        private readonly GetPaginatedResultsUseCase<ICqrsQueryProvider<SampleChildUseCaseOutput>, SampleChildUseCaseOutput> _useCase;
        private readonly Mock<IValidator<GetPaginatedResultsInput>> _inputValidator;
        private readonly Mock<ICqrsQueryProvider<SampleChildUseCaseOutput>> _queryProvider;

        public GetPaginatedResultsUseCaseTests()
        {
            _inputValidator = new Mock<IValidator<GetPaginatedResultsInput>>();
            _queryProvider = new Mock<ICqrsQueryProvider<SampleChildUseCaseOutput>>();

            _useCase = new GetPaginatedResultsUseCase<ICqrsQueryProvider<SampleChildUseCaseOutput>, SampleChildUseCaseOutput>(
                _inputValidator.Object,
                _queryProvider.Object);
        }
    }
}