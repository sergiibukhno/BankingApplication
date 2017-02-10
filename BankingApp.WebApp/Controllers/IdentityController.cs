using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Web.Http;

namespace BankingApp.WebApp.Controllers
{
    public class BaseApiController : ApiController
    {
        [NonAction]
        protected int GetCurrentUserId()
        {
            int id = 0;
            var userIdentity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = userIdentity.Claims;
            foreach (var item in claims)
            {
                if (item.Type == ClaimTypes.UserData)
                {
                    id = Int32.Parse(item.Value);
                }
            }
            return id;
        }
    }
}
