using Best.Practices.Core.Application.Cqrs.QueryProviders;
using Best.Practices.Core.Application.Dtos.Input;
using Best.Practices.Core.Application.Dtos.Output;
using Best.Practices.Core.Common;
using Best.Practices.Core.Exceptions;
using Best.Practices.Core.Extensions;
using FluentValidation;

namespace Best.Practices.Core.Application.UseCases
{
    public class GetPaginatedResultsUseCase<QueryProvider, Output> : BaseUseCase<GetPaginatedResultsInput, PaginatedOutput<Output>>
        where QueryProvider : IListItemOutputCqrsQueryProvider<Output>
    {
        private readonly QueryProvider _queryProvider;
        private readonly IValidator<GetPaginatedResultsInput> _inputValidator;
        public GetPaginatedResultsUseCase(
            IValidator<GetPaginatedResultsInput> inputValidator,
            QueryProvider queryProvider)
            : base()
        {
            _queryProvider = queryProvider;
            _inputValidator = inputValidator;
        }

        public override async Task<UseCaseOutput<PaginatedOutput<Output>>> InternalExecuteAsync(GetPaginatedResultsInput input)
        {
            _inputValidator.ValidateAndThrow(input);

            var resultsInPage = await _queryProvider.GetPaginatedResults(input.Filters, input.PageNumber, input.ItemsPerPage);

            var resultsCount = await _queryProvider.Count(input.Filters);

            var maxPage = (int)(resultsCount / input.ItemsPerPage);
            var remainder = (resultsCount % input.ItemsPerPage);

            maxPage += ((remainder > CommonConstants.QuantityZeroItems) ? CommonConstants.FirstIndex : CommonConstants.ZeroBasedFirstIndex);

            if ((resultsCount > CommonConstants.QuantityZeroItems) && (input.PageNumber > maxPage))
                throw new InvalidInputException(CommonConstants.ErrorMessages.PageNumberMustBeLessOrEqualMaxPage.Format(maxPage));

            return CreateSuccessOutput(new PaginatedOutput<Output>(input.PageNumber, maxPage, resultsCount, resultsInPage));
        }
    }
}