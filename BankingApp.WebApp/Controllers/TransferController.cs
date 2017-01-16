using BankingApp.Core.FinancialServices;
using BankingApp.ViewModels;
using System.Net;
using System.Net.Http;
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
        
        public HttpResponseMessage Post(Transfer transferModel)
        {
            if (transferModel == null)
                return new HttpResponseMessage(HttpStatusCode.BadRequest);

            var result = financialService.Transfer(transferModel.fromUserId, transferModel.toUserId, transferModel.amount);
            return result;
        }
    }
}
