using BankingApp.ViewModels;
using System.Collections.Generic;
using System.Net.Http;

namespace BankingApp.Core.UserServices
{
    public interface IUserService
    {
        ResponseViewModel<List<UserViewModel>> GetRegisteredUsers(int userToExclude);
    }
}
