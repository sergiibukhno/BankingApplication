using System.Collections.Generic;

namespace BankingApp.Models
{
    public class User
    {
        public User()
        {
            FinancialTransactions = new List<FinancialTransaction>();
        }
        
        public int Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public double Balance { get; set; }
        public virtual ICollection<FinancialTransaction> FinancialTransactions { get; set; }
    }
}
