using System.Linq.Expressions;

namespace Best.Practices.Core.Extensions
{
    public static class EnumerableExtension
    {
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (T element in source)
                action(element);
        }

        public static bool IsNullOrEmpty<T>(this IEnumerable<T> collection)
        {
            return collection == null || !collection.Any();
        }

        public static bool HasDuplicates<T, TKey>(this IEnumerable<T> source, params Expression<Func<T, TKey>>[] keySelectors)
        {
            var uniqueKeys = new HashSet<string>();

            foreach (var item in source)
            {
                var keyValues = keySelectors.Select(selector =>
                {
                    var compiledSelector = selector.Compile();
                    return compiledSelector(item);
                });

                var compositeKey = string.Join("|", keyValues);


                if (!uniqueKeys.Add(compositeKey))
                {
                    return true;
                }
            }

            return false;
        }
    }
}