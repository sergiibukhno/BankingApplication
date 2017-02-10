using BankingApp.Core.AccountServices;
using BankingApp.ViewModels;
using System.Web.Http;
using System.Linq;

namespace BankingApp.WebApp.Controllers
{
    public class AuthController : ApiController
    {
        private IAccountService _accountService;

        public AuthController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [Route("api/Auth/Register")]
        [HttpPost]
        public IHttpActionResult Register(RegisterViewModel registerModel)
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
                string validationErrors = string.Join(", ",
                    ModelState.Values.SelectMany(E => E.Errors)
                    .Select(E => E.ErrorMessage)
                    .ToArray());
                return BadRequest(validationErrors);
            }
        }

        [Route("api/Auth/Login")]
        [HttpPost]
        public IHttpActionResult Login(LoginViewModel loginDetails)
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
                string validationErrors = string.Join(", ",
                    ModelState.Values.SelectMany(E => E.Errors)
                    .Select(E => E.ErrorMessage)
                    .ToArray());
                return BadRequest(validationErrors);
            }
        }
    }
}
