namespace Best.Practices.Core.Extensions
{
    public static class ObjectExtension
    {
        public static bool In(this object inputValue, params object[] values)
        {
            foreach (var value in values)
            {
                if (inputValue.Equals(value))
                    return true;
            }

            return false;
        }
    }
}
