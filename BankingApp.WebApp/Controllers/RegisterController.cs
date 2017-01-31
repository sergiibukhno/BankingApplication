using BankingApp.Core.AccountServices;
using BankingApp.ViewModels;
using System.Web.Http;

namespace BankingApp.WebApp.Controllers
{
    public class RegisterController : ApiController
    {
        private IAccountService _accountService;

        public RegisterController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public IHttpActionResult Post(RegisterViewModel registerModel)
        {
            if (ModelState.IsValid)
            {
                var registrationResult = _accountService.Register(registerModel);
                
                if (registrationResult.success)
                {
                    return Ok(registrationResult);
                }
                else
                {
                    return BadRequest(registrationResult.message);
                }
            }
            else
            {
                return BadRequest("Check input data");
            }
        }
    }
}
