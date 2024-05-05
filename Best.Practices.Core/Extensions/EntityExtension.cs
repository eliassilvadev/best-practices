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

        public static Entity ThrowsInvalidInputIfEntityExists<Entity>(this Entity entity, string errorMessage) where Entity : IBaseEntity
        {
            if (entity is not null)
            {
                throw new InvalidInputException(errorMessage);
            }

            return entity;
        }
    }
}
