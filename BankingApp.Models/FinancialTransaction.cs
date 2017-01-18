using System;

namespace BankingApp.Models
{
    public class FinancialTransaction
    {
        public int Id { get; set; }
        public double Amount { get; set; }
        public DateTime TransactionTime { get; set; }
        public virtual User User { get; set; }
    }
}
