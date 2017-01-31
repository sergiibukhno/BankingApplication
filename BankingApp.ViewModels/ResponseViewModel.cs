
namespace BankingApp.ViewModels
{
    public class ResponseViewModel<T>
    {
        public T responseContent;
        public string message;
        public bool success;
    }
}
