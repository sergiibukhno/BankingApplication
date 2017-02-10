using BankingApp.Core.FinancialServices;
using System.Web.Http;

namespace BankingApp.WebApp.Controllers
{
    [Authorize]
    public class BalanceController : BaseApiController
    {
        private IFinancialService _financialService;
        
        public BalanceController(IFinancialService financialService)
        {
            _financialService = financialService;
        }
        
        public IHttpActionResult Get()
        {
            int userId = GetCurrentUserId();
            var requestResult = _financialService.GetBalance(userId);

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
