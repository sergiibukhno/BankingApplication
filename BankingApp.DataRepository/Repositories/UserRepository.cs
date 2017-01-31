using BankingApp.DataRepository.DataContext;
using BankingApp.Models;
using System.Collections.Generic;
using System.Linq;

namespace BankingApp.DataRepository.Repositories
{
    public class UserRepository:IRepository<User>,IUserRepository
    {
        private IDataContext _dataContext;

        public UserRepository(IDataContext dataContext)
        {
            _dataContext = dataContext;
        }        
        
        public List<User> GetAll()
        {
            return _dataContext.Users.ToList();
        }

        public User GetUser(int id)
        {
            var queryResult = from user in _dataContext.Users
                               where user.Id == id
                               select user;
            return queryResult.FirstOrDefault();
        }

        public User GetUserByName(string name)
        {
            var queryResult = from user in _dataContext.Users
                              where user.Name == name
                              select user;
            return queryResult.FirstOrDefault();
        }

        public void Create(User item)
        {
            _dataContext.Users.Add(item);
        }
    }
}
