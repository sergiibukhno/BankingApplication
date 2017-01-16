using BankingApp.DataRepository.UnitOfWork;
using BankingApp.ViewModels;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;

namespace BankingApp.Core.UserServices
{
    public class UserService:IUserService
    {
        private IUnitOfWork _unitOfWork;

        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public HttpResponseMessage GetRegisteredUsers(int userNotToSelect)
        {
            try
            {
                var users = _unitOfWork.Users.GetAll()
                    .Where(s=>s.Id!=userNotToSelect)
                    .Select(r => new User
                    {
                        Id = r.Id,
                        Name = r.Name

                    }).ToList();

                HttpResponseMessage response = new HttpResponseMessage();
                response.Content = new ObjectContent(users.GetType(), users, new XmlMediaTypeFormatter());
                return response;
            }

            catch (Exception ex)
            {

            }

            return new HttpResponseMessage(HttpStatusCode.BadRequest);
        }
    }
}
