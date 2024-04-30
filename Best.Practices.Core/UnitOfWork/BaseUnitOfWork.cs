using Best.Practices.Core.Domain.Cqrs;
using Best.Practices.Core.UnitOfWork.Interfaces;

namespace Best.Practices.Core.UnitOfWork
{
    public abstract class BaseUnitOfWork : IUnitOfWork
    {
        public IList<IEntityCommand> Commands { get; }

        public BaseUnitOfWork()
        {
            Commands = [];
        }

        public void AddComand(IEntityCommand command)
        {
            Commands.Add(command);
        }

        public void SetEntitiesPersistedState()
        {
            foreach (var command in Commands)
            {
                command.AffectedEntity.SetStateAsPersisted();
            }
        }

        public virtual async Task<bool> BeforeSaveAsync()
        {
            return await Task.FromResult(true);
        }

        public virtual async Task<bool> AfterSave(bool sucess)
        {
            return await Task.FromResult(sucess);
        }

        public virtual async Task AfterRollBackAsync()
        {
            await Task.FromResult(() => { });
        }

        public virtual async Task<bool> SaveChangesAsync()
        {
            bool sucess = await BeforeSaveAsync();

            if (!sucess)
                return false;

            try
            {
                foreach (var command in Commands)
                {
                    sucess = await command.ExecuteAsync();

                    if (!sucess)
                        break;
                }
            }
            catch (Exception)
            {
                sucess = false;
            }
            finally
            {
                sucess = sucess && await AfterSave(sucess);

                if (sucess)
                    SetEntitiesPersistedState();

                Commands.Clear();
            }

            return sucess;
        }
        public abstract void Dispose();

        public async Task RollbackAsync()
        {
            await Task.FromResult(() => { });

            Commands.Clear();
        }
    }
}