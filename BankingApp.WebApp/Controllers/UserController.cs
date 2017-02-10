using BankingApp.Core.UserServices;
using System.Web.Http;

namespace BankingApp.WebApp.Controllers
{
    [Authorize]
    public class UserController : BaseApiController
    {
        private IUserService userService;
        
        public UserController(IUserService UserService)
        {
            userService = UserService;
        }

        public IHttpActionResult Get()
        {
            int userId = GetCurrentUserId();
            var requestResult = userService.GetRegisteredUsers(userId);

            if (requestResult.success)
            {
                return Ok(requestResult.responseContent);
            }

            else
            {
                return BadRequest(requestResult.message);
            }
        }      
    }
}
