using BankingApp.Models;
using System.Net.Http;

namespace BankingApp.Core.FinancialServices
{
    public interface IFinancialService
    {
        HttpResponseMessage GetBalance(int userId);
        HttpResponseMessage Deposite(int userId, double amount);
        HttpResponseMessage Withdraw(int userId, double amount);
        HttpResponseMessage Transfer(int fromUserId, int toUserId, double amount);
        HttpResponseMessage GetTransactionsStatements(int userId);
        void AddTransaction(User user, double amount);
    }
}
