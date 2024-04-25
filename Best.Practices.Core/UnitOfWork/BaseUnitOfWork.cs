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

        public virtual bool BeforeSave()
        {
            return true;
        }

        public virtual bool AfterSave(bool sucess)
        {
            return sucess;
        }

        public virtual void AfterRollBack()
        {
        }

        public virtual bool SaveChanges()
        {
            bool sucess = BeforeSave();

            if (!sucess)
                return false;

            try
            {
                foreach (var command in Commands)
                {
                    sucess = command.Execute();

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
                sucess = sucess && AfterSave(sucess);

                if (sucess)
                    SetEntitiesPersistedState();

                Commands.Clear();
            }

            return sucess;
        }
        public abstract void Dispose();

        public void Rollback()
        {
            Commands.Clear();
        }
    }
}