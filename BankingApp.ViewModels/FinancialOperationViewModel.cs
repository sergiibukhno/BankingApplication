
namespace BankingApp.ViewModels
{
    public class FinancialOperationViewModel
    {
        public int userId { get; set; }       
        public int toUserId { get; set; }
        public double amount { get; set; }
    }

    public class DepositViewModel : FinancialOperationViewModel { }
    public class TransferViewModel : FinancialOperationViewModel { }
    public class WithdrawViewModel : FinancialOperationViewModel { }
}
