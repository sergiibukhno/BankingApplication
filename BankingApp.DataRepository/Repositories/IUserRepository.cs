using BankingApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingApp.DataRepository.Repositories
{
    public interface IUserRepository
    {
        List<User> GetAll();
        User GetUser(int id);
        User GetUserByName(string name);
        void Create(User item);
    }
}
