using System.Net.Http;

namespace BankingApp.Core.UserServices
{
    public interface IUserService
    {
        HttpResponseMessage GetRegisteredUsers(int userNotToSelect);
    }
}
