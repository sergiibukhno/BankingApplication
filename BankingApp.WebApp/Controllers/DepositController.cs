using BankingApp.Core.FinancialServices;
using BankingApp.ViewModels;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BankingApp.WebApp.Controllers
{
    public class DepositController : ApiController
    {
        private IFinancialService financialService;
        
        public DepositController(IFinancialService FinancialService)
        {
            financialService = FinancialService;
        }
        
        public HttpResponseMessage Post(Deposit depositModel)
        {
            if (depositModel == null || depositModel.amount <= 0)
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            
            var result = financialService.Deposite(depositModel.userId, depositModel.amount);
            return result;
        }
    }
}
