﻿using Best.Practices.Core.UnitOfWork;
using System.Data;

namespace Best.Practices.Core.CommandProvider.Dapper.UnitOfWork
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
        public override bool BeforeSave()
        {
            if (_connection.State != System.Data.ConnectionState.Open)
                _connection.Open();

            _transaction = _connection.BeginTransaction();

            return base.BeforeSave();
        }

        public override bool AfterSave(bool sucess)
        {
            base.AfterSave(sucess);

            if (sucess)
                _transaction.Commit();

            return sucess;
        }

        public override void AfterRollBack()
        {
            _transaction.Rollback();

            base.AfterRollBack();
        }
    }
}