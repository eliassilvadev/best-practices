using Best.Practices.Core.Application.Dtos.Input;
using Best.Practices.Core.Common;
using Best.Practices.Core.Extensions;
using FluentValidation;

namespace Best.Practices.Core.Application.Dtos.Validators
{
    public class SearchFilterInputValidator : AbstractValidator<SearchFilterInput>
    {
        public SearchFilterInputValidator()
        {
            RuleFor(x => x.FilterType)
                .IsInEnum()
                .WithMessage(CommonConstants.ErrorMessages.PropertyIsInvalid.Format(nameof(SearchFilterInput.FilterType)));

            RuleFor(x => x.FilterProperty)
                .NotEmpty()
                .NotNull()
                .WithMessage(CommonConstants.ErrorMessages.PropertyIsRequired.Format(nameof(SearchFilterInput.FilterProperty)));
        }
    }
}