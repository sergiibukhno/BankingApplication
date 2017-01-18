using BankingApp.Core.UserServices;
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

        public IHttpActionResult Get(int id)
        {
            var requestResult = userService.GetRegisteredUsers(id);

            if (requestResult.success)
            {
                return Ok(requestResult.responseContent);
            }

            else
            {
                return BadRequest(requestResult.errorMessage);
            }
        }      
    }
}
