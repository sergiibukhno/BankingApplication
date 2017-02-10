using BankingApp.Core.FinancialServices;
using BankingApp.ViewModels;
using System.Web.Http;
using System.Linq;

namespace BankingApp.WebApp.Controllers
{
    [Authorize]
    public class DepositController : BaseApiController
    {
        private IFinancialService financialService;
        
        public DepositController(IFinancialService FinancialService)
        {
            financialService = FinancialService;
        }
        
        public IHttpActionResult Post(DepositViewModel depositModel)
        {
            if (ModelState.IsValid)
            {
                depositModel.userId = GetCurrentUserId();
                var requestResult = financialService.Deposit(depositModel);

                if (requestResult.success)
                {
                    return Ok(requestResult.responseContent);
                }

                else
                {
                    return BadRequest(requestResult.message);
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
