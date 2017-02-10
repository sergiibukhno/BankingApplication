using BankingApp.Core.FinancialServices;
using BankingApp.ViewModels;
using System.Web.Http;

namespace BankingApp.WebApp.Controllers
{
    [Authorize]
    public class TransferController : BaseApiController
    {
        private IFinancialService financialService;
        
        public TransferController(IFinancialService FinancialService)
        {
            financialService = FinancialService;
        }
        
        public IHttpActionResult Post(TransferViewModel transferModel)
        {
            if (transferModel == null || transferModel.toUserId <= 0 || transferModel.amount <= 0)
                return BadRequest("Check input data");

            transferModel.userId = GetCurrentUserId();

            if (transferModel.userId == transferModel.toUserId)
                return BadRequest("Cannot transfer to yourself");

            var requestResult = financialService.Transfer(transferModel);

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
