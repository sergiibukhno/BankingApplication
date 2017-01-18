using BankingApp.Models;
using System.Data.Entity;

namespace BankingApp.DataRepository.DataContext
{
    public interface IDataContext
    {
        DbSet<User> Users { get; set; }
        DbSet<FinancialTransaction> FinancialTransactions { get; set; }
    }
}
