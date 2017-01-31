using BankingApp.DataRepository.UnitOfWork;
using BankingApp.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace BankingApp.Core.UserServices
{
    public class UserService:IUserService
    {
        private IUnitOfWorkFactory _unitOfWorkFactory;
        
        public UserService(IUnitOfWorkFactory unitOfWorkFactory)
        {
            _unitOfWorkFactory = unitOfWorkFactory;            
        }

        public ResponseViewModel<List<UserViewModel>> GetRegisteredUsers(int userToExclude)
        {
            if (userToExclude<=0)
            {
                return new ResponseViewModel<List<UserViewModel>> { message = "Wrong id", success = false };
            }
            
            using (var unitOfWork = _unitOfWorkFactory.GetUnitOfWork())
            {
                var users = unitOfWork.Users.GetAll()
                    .Where(s => s.Id != userToExclude)
                    .Select(r => new UserViewModel
                    {
                        Id = r.Id,
                        Name = r.Name

                    }).ToList();

                return new ResponseViewModel<List<UserViewModel>> { responseContent = users, success = true };
            }
        }       
    }
}
