using Best.Practices.Core.Application.Dtos.Input;
using Best.Practices.Core.Application.UseCases;
using Best.Practices.Core.Common;
using Best.Practices.Core.Domain.Cqrs.QueryProvider;
using Best.Practices.Core.Domain.Enumerators;
using Best.Practices.Core.Extensions;
using Best.Practices.Core.Tests.Application.Dtos.Builders;
using Best.Practices.Core.Tests.Application.SampleUseCasesDtos;
using FluentAssertions;
using FluentValidation;
using Moq;
using Xunit;

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

        [Fact]
        public void Execute_InputIsValid_ReturnsResults()
        {
            // Arrange
            var filter = new SearchFilterInputBuilder()
                .WithFilterProperty("Name")
                .WithFilterType(FilterType.Equals)
                .WithFilterValue("SampleNameValue")
                .Build();

            var input = new GetPaginatedResultsInputBuilder()
                .WithFilters([filter])
                .WithPageNumber(1)
                .WithItemsPerPage(10)
                .Build();

            var resultOutPut = new SampleChildUseCaseOutputBuilder().Build();

            var results = new List<SampleChildUseCaseOutput>() { resultOutPut };

            _queryProvider.Setup(s => s.GetPaginatedResults(input.Filters, input.PageNumber, input.ItemsPerPage))
                .Returns(results);

            _queryProvider.Setup(x => x.Count(input.Filters))
                .Returns(results.Count);

            // Act
            var output = _useCase.Execute(input);

            // Assert
            output.HasErros.Should().BeFalse();
            output.OutputObject.ActualPage.Should().Be(input.PageNumber);
            output.OutputObject.MaxPage.Should().Be(1);
            output.OutputObject.ResultsInPage.Should().Contain(resultOutPut);
            output.OutputObject.TotalResultsCount.Should().Be(1);
            _queryProvider.Verify(x => x.GetPaginatedResults(It.IsAny<IList<SearchFilterInput>>(), input.PageNumber, input.ItemsPerPage), Times.Once);
            _queryProvider.Verify(x => x.Count(It.IsAny<IList<SearchFilterInput>>()), Times.Once);
        }

        [Fact]
        public void Execute_QueryProviderReturnsNoItems_ReturnsEmptyResults()
        {
            // Arrange
            var filter = new SearchFilterInputBuilder()
                .WithFilterProperty("Name")
                .WithFilterType(FilterType.Equals)
                .WithFilterValue("SampleNameValue")
                .Build();

            var input = new GetPaginatedResultsInputBuilder()
                .WithFilters(new List<SearchFilterInput>() { filter })
                .WithPageNumber(1)
                .WithItemsPerPage(10)
                .Build();

            _queryProvider.Setup(s => s.GetPaginatedResults(input.Filters, input.PageNumber, input.ItemsPerPage))
                .Returns([]);

            _queryProvider.Setup(x => x.Count(input.Filters))
                .Returns(0);

            // Act
            var output = _useCase.Execute(input);

            // Assert
            output.HasErros.Should().BeFalse();
            output.OutputObject.ActualPage.Should().Be(input.PageNumber);
            output.OutputObject.MaxPage.Should().Be(0);
            output.OutputObject.TotalResultsCount.Should().Be(0);
            _queryProvider.Verify(x => x.GetPaginatedResults(It.IsAny<IList<SearchFilterInput>>(), input.PageNumber, input.ItemsPerPage), Times.Once);
            _queryProvider.Verify(x => x.Count(It.IsAny<IList<SearchFilterInput>>()), Times.Once);
        }

        [Fact]
        public void Execute_PagaNumberGreaterThanMaxPage_ReturnsError()
        {
            // Arrange
            var filter = new SearchFilterInputBuilder()
                .WithFilterProperty("Name")
                .WithFilterType(FilterType.Equals)
                .WithFilterValue("SampleNameValue")
                .Build();

            var input = new GetPaginatedResultsInputBuilder()
                .WithFilters([filter])
                .WithPageNumber(2)
                .WithItemsPerPage(10)
                .Build();

            var resultOutPut = new SampleChildUseCaseOutputBuilder().Build();

            var results = new List<SampleChildUseCaseOutput>() { resultOutPut };

            _queryProvider.Setup(s => s.GetPaginatedResults(input.Filters, input.PageNumber, input.ItemsPerPage))
                .Returns(results);

            _queryProvider.Setup(x => x.Count(input.Filters))
                .Returns(results.Count);

            // Act
            var output = _useCase.Execute(input);

            // Assert
            output.HasErros.Should().BeTrue();
            output.Errors.Should().ContainEquivalentOf(new ErrorMessage(CommonConstants.ErrorMessages.PageNumberMustBeLessOrEqualMaxPage.Format(1)));
            output.OutputObject.Should().BeNull();
            _queryProvider.Verify(x => x.GetPaginatedResults(It.IsAny<IList<SearchFilterInput>>(), input.PageNumber, input.ItemsPerPage), Times.Once);
            _queryProvider.Verify(x => x.Count(It.IsAny<IList<SearchFilterInput>>()), Times.Once);
        }
    }
}