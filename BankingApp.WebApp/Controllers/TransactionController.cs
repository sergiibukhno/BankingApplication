using BankingApp.Core.FinancialServices;
using System.Web.Http;

namespace BankingApp.WebApp.Controllers
{
    [Authorize]
    public class TransactionController : BaseApiController
    {
        private IFinancialService financialService;
        
        public TransactionController(IFinancialService FinancialService)
        {
            financialService = FinancialService;
        }
        
        public IHttpActionResult Get()
        {
            int userId = GetCurrentUserId();
            var requestResult = financialService.GetTransactionsStatements(userId);

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
