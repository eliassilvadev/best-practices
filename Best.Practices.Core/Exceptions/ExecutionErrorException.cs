using Best.Practices.Core.Common;

namespace Best.Practices.Core.Exceptions
{
    public class ExecutionErrorException : BaseException
    {
        public ExecutionErrorException(string message) : base(message)
        {
        }

        public ExecutionErrorException(IList<ErrorMessage> errors)
            : base(errors)
        {
        }

        public ExecutionErrorException(ErrorMessage errorMessage)
            : base(errorMessage)
        {
        }
    }
}