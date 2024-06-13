using Best.Practices.Core.UnitOfWork;
using System.Data;

namespace Best.Practices.Core.Cqrs.Dapper.UnitOfWork
{
    public class DapperUnitOfWork : BaseUnitOfWork
    {
        private readonly IDbConnection _connection;
        private IDbTransaction _transaction;

        public DapperUnitOfWork(IDbConnection connection) : base()
        {
            _connection = connection;
        }

        public override void Dispose()
        {
            GC.SuppressFinalize(this);
        }
        public override async Task<bool> BeforeSaveAsync()
        {
            if (_connection.State != System.Data.ConnectionState.Open)
                _connection.Open();

            _transaction = _connection.BeginTransaction();

            return await base.BeforeSaveAsync();
        }

        public override async Task<bool> AfterSave(bool sucess)
        {
            await base.AfterSave(sucess);

            if (sucess)
                _transaction.Commit();

            return sucess;
        }

        public override async Task AfterRollBackAsync()
        {
            _transaction.Rollback();

            await base.AfterRollBackAsync();
        }
    }
}