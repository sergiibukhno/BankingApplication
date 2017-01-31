using BankingApp.DataRepository.Repositories;
using System;

namespace BankingApp.DataRepository.UnitOfWork
{
    public class UnitOfWork:IUnitOfWork
    {
        private DataContext.DataContext _dataContext;
        private IFinancialTransactionRepository transactionRepo;
        private IUserRepository userRepo;

        public UnitOfWork()
        {
            _dataContext = new DataContext.DataContext(); 
        }

        public IFinancialTransactionRepository Transactions
        {
            get
            {
                if (transactionRepo == null)
                    transactionRepo = new FinancialTransactionRepository(_dataContext);
                return transactionRepo;
            }
        }

        public virtual IUserRepository Users
        {
            get
            {
                if (userRepo == null)
                    userRepo = new UserRepository(_dataContext);
                return userRepo;
            }
        }

        public void Save()
        {
            _dataContext.SaveChanges();
        }

        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _dataContext.Dispose();
                }
                this.disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
