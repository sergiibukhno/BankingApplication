using BankingApp.Core.AccountServices;
using BankingApp.ViewModels;
using System.Web.Http;

namespace BankingApp.WebApp.Controllers
{
    public class LoginController : ApiController
    {
        private IAccountService _accountService;

        public LoginController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public IHttpActionResult Post(LoginViewModel loginDetails)
        {
            if (ModelState.IsValid)
            {
                var loginResult = _accountService.Login(loginDetails);
                
                if (loginResult.success)
                {
                    return Ok(loginResult);
                }
                else
                {
                    return BadRequest(loginResult.message);
                }
            }
            else
            {
                return BadRequest("Check input data");
            }
        }
    }
}
