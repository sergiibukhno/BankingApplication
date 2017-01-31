using BankingApp.Models;
using System.Collections.Generic;

namespace BankingApp.DataRepository.Repositories
{
    public interface IFinancialTransactionRepository
    {
        List<FinancialTransaction> GetAll();
        List<FinancialTransaction> Get(int id);
        void Create(FinancialTransaction item);
    }
}
