using BankingApp.Models;
using BankingApp.ViewModels;

namespace BankingApp.Core.AccountServices
{
    public interface IAccountService
    {
        string EncryptPassword(string inputString);
        string CreateToken(User user);
        ResponseViewModel<string> Register(RegisterViewModel registerModel);
        ResponseViewModel<string> Login(LoginViewModel loginModel);
    }
}
