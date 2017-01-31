using BankingApp.ViewModels;
using System.Collections.Generic;

namespace BankingApp.Core.UserServices
{
    public interface IUserService
    {
        ResponseViewModel<List<UserViewModel>> GetRegisteredUsers(int userToExclude);       
    }
}
