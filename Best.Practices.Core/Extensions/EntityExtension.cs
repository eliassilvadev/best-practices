using Best.Practices.Core.Common;
using Best.Practices.Core.Domain.Models.Interfaces;
using Best.Practices.Core.Exceptions;
using System.Linq.Expressions;

namespace Best.Practices.Core.Extensions
{
    public static class EntityExtension
    {
        public static Entity ThrowResourceNotFoundIfIsNull<Entity>(this Entity entity, string errorMessage) where Entity : IBaseEntity
        {
            if (entity is null)
            {
                throw new ResourceNotFoundException(errorMessage);
            }

            return entity;
        }

        public static Entity ThrowInvalidInputIfIsNotNull<Entity>(this Entity entity, string errorMessage) where Entity : IBaseEntity
        {
            if (entity is not null)
            {
                throw new InvalidInputException(errorMessage);
            }

            return entity;
        }

        public static IEnumerable<Entity> ThrowInvalidInputIfIsHasItems<Entity>(this IEnumerable<Entity> entities, string errorMessage) where Entity : IBaseEntity
        {
            if (!entities.IsNullOrEmpty())
            {
                throw new ResourceNotFoundException(errorMessage);
            }

            return entities;
        }

        public static Entity ThrowInvalidInputIfMatches<Entity>(this Entity entity, Predicate<Entity> match, string errorMessage) where Entity : IBaseEntity
        {
            if (match(entity))
            {
                throw new InvalidInputException(errorMessage);
            }

            return entity;
        }

        public static IEnumerable<Entity> ThrowInvalidInputIfAtLeastAnItemMatches<Entity>(this IEnumerable<Entity> entities, Predicate<Entity> match, string errorMessage) where Entity : IBaseEntity
        {
            var matchedItems = entities.Where(i => match(i)).ToList();

            if (matchedItems.Count != CommonConstants.QuantityZeroItems)
            {
                throw new InvalidInputException(errorMessage);
            }

            return entities;
        }

        public static IEnumerable<Entity> ThrowInvalidInputIfAllItemsMatches<Entity>(this IEnumerable<Entity> entities, Predicate<Entity> match, string errorMessage) where Entity : IBaseEntity
        {
            var matchedItems = entities.Where(i => match(i)).ToList();

            if (matchedItems.Count == entities.Count())
            {
                throw new InvalidInputException(errorMessage);
            }

            return entities;
        }

        public static IEnumerable<Entity> ThrowInvalidInputIfDoesNotMatch<Entity>(this IEnumerable<Entity> entities, Predicate<Entity> match, string errorMessage) where Entity : IBaseEntity
        {
            if (!entities.Any(i => match(i)))
            {
                throw new InvalidInputException(errorMessage);
            }

            return entities;
        }

        public static IEnumerable<Entity> ThrowInvalidInputIfHasDuplicates<Entity, T, TKey>(this IEnumerable<Entity> entities, string errorMessage, params Expression<Func<Entity, TKey>>[] keySelectors) where Entity : IBaseEntity
        {
            if (entities.HasDuplicates(keySelectors))
            {
                throw new InvalidInputException(errorMessage);
            }

            return entities;
        }

        public static Entity ThrowInvalidInputIfDoesNotMatch<Entity>(this Entity entity, Predicate<Entity> match, string errorMessage) where Entity : IBaseEntity
        {
            if (match(entity))
            {
                throw new InvalidInputException(errorMessage);
            }

            return entity;
        }
    }
}