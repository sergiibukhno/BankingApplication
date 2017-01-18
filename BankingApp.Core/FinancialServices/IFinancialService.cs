using BankingApp.Models;
using BankingApp.ViewModels;
using System.Collections.Generic;
using System.Net.Http;

namespace BankingApp.Core.FinancialServices
{
    public interface IFinancialService
    {
        ResponseViewModel<double> GetBalance(int userId);
        ResponseViewModel<double> Deposite(int userId, double amount);
        ResponseViewModel<double> Withdraw(int userId, double amount);
        ResponseViewModel<double> Transfer(TransferViewModel transferModel);
        ResponseViewModel<List<TransactionViewModel>> GetTransactionsStatements(int userId);
        void AddTransaction(User user, double amount);
    }
}
