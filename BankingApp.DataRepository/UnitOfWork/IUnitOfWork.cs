using BankingApp.DataRepository.Repositories;
using System;

namespace BankingApp.DataRepository.UnitOfWork
{
    public interface IUnitOfWork:IDisposable
    {
        FinancialTransactionRepository Transactions { get; }
        UserRepository Users { get; }
        void Save();
    }
}
