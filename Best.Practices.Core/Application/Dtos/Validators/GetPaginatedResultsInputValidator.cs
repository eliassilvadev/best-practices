using Best.Practices.Core.Application.Dtos.Input;
using Best.Practices.Core.Common;
using FluentValidation;

namespace Best.Practices.Core.Application.Dtos.Validators
{
    public class GetPaginatedResultsInputValidator : AbstractValidator<GetPaginatedResultsInput>
    {
        public GetPaginatedResultsInputValidator()
        {
            RuleFor(x => x.PageNumber)
                .GreaterThanOrEqualTo(1)
                .WithMessage(CommonConstants.ErrorMessages.PageNumberMustBeGreaterThanOrEqualToOne);

            RuleFor(x => x.ItemsPerPage)
                .GreaterThanOrEqualTo(1)
                .WithMessage(CommonConstants.ErrorMessages.ItemsPerPageMustBeGreaterThanOrEqualToOne);

            RuleForEach(x => x.Filters)
                .SetValidator(new SearchFilterInputValidator());
        }
    }
}