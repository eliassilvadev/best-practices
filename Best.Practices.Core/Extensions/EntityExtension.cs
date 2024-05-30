using Best.Practices.Core.Domain.Models.Interfaces;
using Best.Practices.Core.Exceptions;

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

        public static Entity ThrowsInvalidInputIfIsNotNull<Entity>(this Entity entity, string errorMessage) where Entity : IBaseEntity
        {
            if (entity is not null)
            {
                throw new InvalidInputException(errorMessage);
            }

            return entity;
        }

        public static Entity ThrowInvalidInputIfMatches<Entity>(this Entity entity, Predicate<Entity> match, string errorMessage) where Entity : IBaseEntity
        {
            if (match(entity))
            {
                throw new InvalidInputException(errorMessage);
            }

            return entity;
        }

        public static Entity ThrowInvalidInputIfDoesNotMatch<Entity>(this Entity entity, Predicate<Entity> match, string errorMessage) where Entity : IBaseEntity
        {
            if (!match(entity))
            {
                throw new InvalidInputException(errorMessage);
            }

            return entity;
        }

        public static IEnumerable<Entity> ThrowInvalidInputIfMatches<Entity>(this IEnumerable<Entity> entities, Predicate<Entity> match, string errorMessage) where Entity : IBaseEntity
        {
            if (entities.Any(i => match(i)))
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
    }
}