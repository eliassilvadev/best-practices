namespace Best.Practices.Core.Common
{
    public static class CommonConstants
    {
        public static readonly char ErrorMessageSeparator = ';';
        public static readonly int ZeroBasedFirstIndex = 0;
        public static readonly int FirstIndex = 1;
        public static readonly int QuantityZeroItems = 0;
        public static readonly string StringSpace = " ";
        public static readonly char CharSpace = ' ';
        public static readonly char CharComma = ',';
        public static readonly char CharFullStop = '.';
        public static readonly char CharEnter = '\n';
        public static readonly string StringComma = ",";
        public static readonly string StringEnter = "\n";
        public static readonly TimeSpan DefaultApiTokenExpirationInMinutes = TimeSpan.FromMinutes(5);

        public static class ErrorCodes
        {
            public static readonly string DefaultErrorCode = "000";
        }

        public static class ErrorMessages
        {
            public static readonly string DefaultErrorMessage = $"{ErrorCodes.DefaultErrorCode};A unexpected error occurrs.";
            public static readonly string PageNumberMustBeLessOrEqualMaxPage = "001;The page number must be less than or equal to the maximum number of pages '{0}'.";
            public static readonly string FieldIsRequired = "002;The field {0} is required.";
            public static readonly string EntityMappingError = "003;There mapping for entity {0} is invalid.";
        }
    }
}