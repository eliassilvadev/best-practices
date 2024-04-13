﻿using Best.Practices.Core.Application.Dtos.Input;
using Best.Practices.Core.Application.Dtos.Output;
using Best.Practices.Core.Common;
using Best.Practices.Core.Domain.Cqrs.QueryProvider;
using Best.Practices.Core.Exceptions;
using FluentValidation;

namespace Best.Practices.Core.Application.UseCases
{
    public class GetPaginatedResultsUseCase<QueryProvider, Output> : BaseUseCase<GetPaginatedResultsInput, PaginatedOutput<Output>>
        where QueryProvider : ICqrsQueryProvider<Output>
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

        public override UseCaseOutput<PaginatedOutput<Output>> InternalExecute(GetPaginatedResultsInput input)
        {
            _inputValidator.ValidateAndThrow(input);

            var results = _queryProvider.GetPaginatedResults(input.Filters, input.PageNumber, input.ItemsPerPage);

            var resultsCount = _queryProvider.Count(input.Filters);

            var maxPage = (int)(resultsCount / input.ItemsPerPage);
            var remainder = (resultsCount % input.ItemsPerPage);

            maxPage += ((remainder > CommonConstants.CountZeroItems) ? CommonConstants.FirstIndex : CommonConstants.ZeroBasedFirstIndex);

            if ((resultsCount > CommonConstants.CountZeroItems) && (input.PageNumber > maxPage))
                throw new InvalidInputException(string.Format(CommonConstants.ErrorMessages.PageNumberMustBeLessOrEqualMaxPage, maxPage));

            return CreateSuccessOutput(new PaginatedOutput<Output>(input.PageNumber, maxPage, results));
        }
    }
}