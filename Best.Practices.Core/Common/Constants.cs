namespace Best.Practices.Core.Common
{
    public static class CommonConstants
    {
        public static readonly char ErroMessageSeparator = ';';
        public static class ErrorCodes
        {
            public static readonly string DefaulErrorCode = "000";
            public static readonly string DefaulErrorMessage = $"{DefaulErrorCode};A unexpected error occurrs.";
        }
    }
}