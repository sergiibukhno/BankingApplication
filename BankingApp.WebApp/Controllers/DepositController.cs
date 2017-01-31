using BankingApp.Core.FinancialServices;
using BankingApp.ViewModels;
using System.Web.Http;

namespace BankingApp.WebApp.Controllers
{
    [Authorize]
    public class DepositController : ApiController
    {
        private IFinancialService financialService;
        
        public DepositController(IFinancialService FinancialService)
        {
            financialService = FinancialService;
        }
        
        public IHttpActionResult Post(DepositViewModel depositModel)
        {
            if (depositModel == null || depositModel.amount <= 0)
                return BadRequest("Check input data");

            var requestResult = financialService.PerformFinancialOperation(depositModel);

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
