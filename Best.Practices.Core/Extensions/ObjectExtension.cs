namespace Best.Practices.Core.Extensions
{
    public static class ObjectExtension
    {
        public static bool In(this object inputObject, params object[] values)
        {
            foreach (var value in values)
            {
                if (inputObject.Equals(value))
                    return true;
            }

            return false;
        }
        public static bool In<T>(this T inputObject, IEnumerable<T> values)
        {
            foreach (T obj in values)
            {
                if (inputObject.Equals(obj))
                {
                    return true;
                }
            }

            return false;
        }

        public static bool PropertyIsUpdated(this object inputObject, string propertyName, object propertyValue)
        {
            var objectType = inputObject.GetType();
            var currentValue = objectType.GetProperty(propertyName).GetValue(inputObject, null);

            return ((((propertyValue == null) && (currentValue != null)) ||
                        ((propertyValue != null) && (currentValue == null))) ||
                        ((propertyValue != null) && (currentValue != null) &&
                        (!propertyValue.Equals(currentValue))));
        }
    }
}