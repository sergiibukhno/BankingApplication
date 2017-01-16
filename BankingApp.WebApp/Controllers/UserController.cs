using BankingApp.Core.UserServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BankingApp.WebApp.Controllers
{
    public class UserController : ApiController
    {
        private IUserService userService;
        
        public UserController(IUserService UserService)
        {
            userService = UserService;
        }

        public HttpResponseMessage Get(int id)
        {
            var result = userService.GetRegisteredUsers(id);
            return result;
        }      
    }
}
