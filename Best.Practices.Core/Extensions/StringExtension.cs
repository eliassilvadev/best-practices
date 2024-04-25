namespace Best.Practices.Core.Extensions
{
    public static class StringExtension
    {
        public static string Format(this string inputString, params object[] values)
        {
            return string.Format(inputString, values);
        }

        public static bool IsEmpty(this string inputString)
        {
            return string.IsNullOrWhiteSpace(inputString);
        }
    }
}