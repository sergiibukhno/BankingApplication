using BankingApp.DataRepository.Repositories;
using System;

namespace BankingApp.DataRepository.UnitOfWork
{
    public interface IUnitOfWork:IDisposable
    {
        IFinancialTransactionRepository Transactions { get; }
        IUserRepository Users { get; }
        void Save();
    }
}
