using BankingApp.Core.FinancialServices;
using BankingApp.ViewModels;
using System.Web.Http;

namespace BankingApp.WebApp.Controllers
{
    [Authorize]
    public class WithdrawController : ApiController
    {
        private IFinancialService financialService;
        
        public WithdrawController(IFinancialService FinancialService)
        {
            financialService = FinancialService;
        }
        
        public IHttpActionResult Post(WithdrawViewModel withdrawModel)
        {
            if (withdrawModel == null || withdrawModel.amount <= 0)
                return BadRequest("Check input data");

            var requestResult = financialService.PerformFinancialOperation(withdrawModel);

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
