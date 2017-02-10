using System.ComponentModel.DataAnnotations;

namespace BankingApp.ViewModels
{
    public class DepositViewModel
    {
        public int userId { get; set; }
        
        [Required(ErrorMessage = "Amount is required")]
        [Range(1, 10000, ErrorMessage = "Amount should be between 1 and 10000")]
        public double amount { get; set; }
    }
}
