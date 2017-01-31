using BankingApp.Core.FinancialServices;
using System.Web.Http;

namespace BankingApp.WebApp.Controllers
{
    [Authorize]
    public class BalanceController : ApiController
    {
        private IFinancialService financialService;
        
        public BalanceController(IFinancialService FinancialService)
        {
            financialService = FinancialService;
        }
        
        public IHttpActionResult Get(int id)
        {
            var requestResult = financialService.GetBalance(id);

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
