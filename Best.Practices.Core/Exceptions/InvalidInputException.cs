using Best.Practices.Core.Common;

namespace Best.Practices.Core.Exceptions
{
    public class InvalidInputException : BaseException
    {
        public InvalidInputException(string message) : base(message)
        {
        }

        public InvalidInputException(IList<ErrorMessage> errors)
            : base(errors)
        {
        }

        public InvalidInputException(ErrorMessage errorMessage)
            : base(errorMessage)
        {
        }
    }
}
