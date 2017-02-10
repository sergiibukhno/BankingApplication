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
        ResponseViewModel<double> Deposit(DepositViewModel depositModel);
        ResponseViewModel<double> Withdraw(WithdrawViewModel withdrawModel);
        ResponseViewModel<double> Transfer(TransferViewModel transferModel);
        void AddTransaction(User user, double amount);
    }
}
