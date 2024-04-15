using Best.Practices.Core.Common;
using Best.Practices.Core.Exceptions;

namespace Best.Practices.Core.Application.UseCases
{
    public abstract class BaseUseCase<Input, Output>
    {
        public BaseUseCase()
        {
        }

        public UseCaseOutput<Output> CreateSuccessOutput(Output output)
        {
            return new UseCaseOutput<Output>(output);
        }

        public UseCaseOutput<Output> CreateOutputWithErrors(IList<ErrorMessage> errors)
        {
            return new UseCaseOutput<Output>(errors);
        }

        public abstract UseCaseOutput<Output> InternalExecute(Input input);

        public virtual UseCaseOutput<Output> Execute(Input input)
        {
            UseCaseOutput<Output> output;
            try
            {
                output = InternalExecute(input);
            }
            catch (ValidationException ex)
            {
                output = CreateOutputWithErrors(ex.Errors);
            }
            catch (FluentValidation.ValidationException ex)
            {
                if (ex.Errors.Any())
                    output = CreateOutputWithErrors(ex.Errors.Select(e => new ErrorMessage(e.ErrorMessage)).ToList());
                else
                    output = CreateOutputWithErrors([new(ex.Message)]);

            }
            catch (BaseException ex)
            {
                output = CreateOutputWithErrors(ex.Errors);
            }
            catch (Exception ex)
            {
                output = CreateOutputWithErrors([new(ex.Message)]);
            }

            return output;
        }
    }
}