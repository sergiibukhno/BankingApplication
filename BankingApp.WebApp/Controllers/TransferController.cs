using BankingApp.Core.FinancialServices;
using BankingApp.ViewModels;
using System.Web.Http;

namespace BankingApp.WebApp.Controllers
{
    public class TransferController : ApiController
    {
        private IFinancialService financialService;
        
        public TransferController(IFinancialService FinancialService)
        {
            financialService = FinancialService;
        }
        
        public IHttpActionResult Post(TransferViewModel transferModel)
        {
            if (transferModel == null)
                return BadRequest("Check input data");

            var requestResult = financialService.Transfer(transferModel);

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
