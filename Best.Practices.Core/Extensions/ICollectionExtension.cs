using Best.Practices.Core.Exceptions;
using System.Linq.Expressions;

namespace Best.Practices.Core.Extensions
{
    public static class ICollectionExtension
    {
        public static void AddIfNotExists<T, TKey>(
            this ICollection<T> source,
            T newItem,
            string errorMessageToThrowIfExists,
            params Expression<Func<T, object>>[] propertySelectors)
        {
            var propertyFuncs = propertySelectors.Select(ps => ps.Compile())
                .ToList();

            bool itemExists = source.Any(existingItem =>
                propertyFuncs.All(func =>
                    func(existingItem).Equals(func(newItem))));

            if (!itemExists)
            {
                source.Add(newItem);
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(errorMessageToThrowIfExists))
                    throw new ValidationException(errorMessageToThrowIfExists);
            }
        }

        public static void AddIfNotExists<T, TKey>(
            this ICollection<T> source,
            T newItem,
            params Expression<Func<T, object>>[] propertySelectors)
        {
            source.AddIfNotExists<T, TKey>(newItem, string.Empty, propertySelectors);
        }
    }
}