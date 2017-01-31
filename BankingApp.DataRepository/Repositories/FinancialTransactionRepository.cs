using BankingApp.DataRepository.DataContext;
using BankingApp.Models;
using System.Collections.Generic;
using System.Linq;

namespace BankingApp.DataRepository.Repositories
{
    public class FinancialTransactionRepository : IRepository<FinancialTransaction>,IFinancialTransactionRepository
    {
        private IDataContext _dataContext;

        public FinancialTransactionRepository(IDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public List<FinancialTransaction> GetAll()
        {
            return _dataContext.FinancialTransactions.ToList();
        }

        public List<FinancialTransaction> Get(int id)
        {
            var queryResult = (from transaction in _dataContext.FinancialTransactions
                               where transaction.User.Id == id
                               select transaction).ToList();
            return queryResult;
        }

        public void Create(FinancialTransaction item)
        {
            _dataContext.FinancialTransactions.Add(item);
        }
    }
}
