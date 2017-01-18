using BankingApp.Core.FinancialServices;
using System.Web.Http;

namespace BankingApp.WebApp.Controllers
{
    public class TransactionController : ApiController
    {
        private IFinancialService financialService;
        
        public TransactionController(IFinancialService FinancialService)
        {
            financialService = FinancialService;
        }
        
        public IHttpActionResult Get(int id)
        {
            var requestResult = financialService.GetTransactionsStatements(id);

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
