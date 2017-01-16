using BankingApp.Core.FinancialServices;
using BankingApp.ViewModels;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BankingApp.WebApp.Controllers
{
    public class WithdrawController : ApiController
    {
        private IFinancialService financialService;
        
        public WithdrawController(IFinancialService FinancialService)
        {
            financialService = FinancialService;
        }
        
        public HttpResponseMessage Post(Withdraw withdrawModel)
        {
            if (withdrawModel == null || withdrawModel.amount <= 0)
                return new HttpResponseMessage(HttpStatusCode.BadRequest);

            var result = financialService.Withdraw(withdrawModel.userId, withdrawModel.amount);
            return result;
        }
    }
}
