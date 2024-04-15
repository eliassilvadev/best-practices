using Best.Practices.Core.Common;

namespace Best.Practices.Core.Application.UseCases
{
    public class UseCaseOutput<Output>
    {
        public Output OutputObject { get; }
        public IList<ErrorMessage> Errors { get; }
        public UseCaseOutput(Output responseObject)
        {
            OutputObject = responseObject;
            Errors = new List<ErrorMessage>();
        }

        public UseCaseOutput(IList<ErrorMessage> errors)
        {
            OutputObject = default;
            Errors = errors;
        }

        public bool HasErros
        {
            get
            {
                return Errors != null && Errors.Count > 0;
            }
        }
    }
}
