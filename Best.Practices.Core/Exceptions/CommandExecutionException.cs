using Best.Practices.Core.Common;

namespace Best.Practices.Core.Exceptions
{
    public class CommandExecutionException : BaseException
    {
        public CommandExecutionException(ErrorMessage errorMessage) : base(errorMessage)
        {
        }
        public CommandExecutionException(string message) : base(message)
        {
        }
    }
}