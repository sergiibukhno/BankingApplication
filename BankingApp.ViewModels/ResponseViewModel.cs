
namespace BankingApp.ViewModels
{
    public class ResponseViewModel<T>
    {
        public T responseContent;
        public string errorMessage;
        public bool success;
    }
}
