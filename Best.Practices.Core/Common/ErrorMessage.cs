namespace Best.Practices.Core.Common
{
    public class ErrorMessage
    {
        public ErrorMessage(string message)
        {
            Message = message;

            if (Message.Contains(';'))
            {
                var messageParts = Message.Split(';');

                Code = messageParts[0];
                Message = messageParts[1];
            }
        }
        public ErrorMessage(string code, string message)
        {
            Code = code;
            Message = message;
        }
        public string Code { get; set; }

        public string Message { get; set; }

        public override string ToString()
        {
            return Code + Message;
        }
    }
}