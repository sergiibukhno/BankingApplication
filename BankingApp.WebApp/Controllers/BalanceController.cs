using BankingApp.Core.FinancialServices;
using System.Net.Http;
using System.Web.Http;

namespace BankingApp.WebApp.Controllers
{
    public class BalanceController : ApiController
    {
        private IFinancialService financialService;
        
        public BalanceController(IFinancialService FinancialService)
        {
            financialService = FinancialService;
        }
        
        public HttpResponseMessage Get(int id)
        {
            var result = financialService.GetBalance(id);
            return result;
        }       
    }
}
