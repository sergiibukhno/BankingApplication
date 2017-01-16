using BankingApp.DataRepository.DataContext;
using BankingApp.Models;
using System.Collections.Generic;
using System.Linq;

namespace BankingApp.DataRepository.Repositories
{
    public class UserRepository:IRepository<User>
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

        public List<User> Get(int id)
        {
            var queryResult = (from user in _dataContext.Users
                               where user.Id == id
                               select user).ToList();
            return queryResult;
        }

        public void Create(User item)
        {
            _dataContext.Users.Add(item);
        }
    }
}
