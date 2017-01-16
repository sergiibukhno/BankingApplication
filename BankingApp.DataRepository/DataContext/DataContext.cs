using BankingApp.Models;
using System.Data.Entity;

namespace BankingApp.DataRepository.DataContext
{
    public class DataContext:DbContext,IDataContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<FinancialTransaction> FinancialTransactions { get; set; }
    }
}
