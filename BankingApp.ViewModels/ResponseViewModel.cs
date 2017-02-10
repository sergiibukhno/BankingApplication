
namespace BankingApp.ViewModels
{
    public class ResponseViewModel<T>
    {
        public T responseContent { get; set; }
        public string message { get; set; }
        public bool success { get; set; }
    }
}
