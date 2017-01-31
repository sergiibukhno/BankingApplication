using BankingApp.Models;
using BankingApp.ViewModels;
using System.Collections.Generic;
using System.Net.Http;

namespace BankingApp.Core.FinancialServices
{
    public interface IFinancialService
    {
        ResponseViewModel<double> GetBalance(int userId);
        ResponseViewModel<List<TransactionViewModel>> GetTransactionsStatements(int userId);
        ResponseViewModel<double> PerformFinancialOperation(FinancialOperationViewModel financialOperation);
        void AddTransaction(User user, double amount);
    }
}
