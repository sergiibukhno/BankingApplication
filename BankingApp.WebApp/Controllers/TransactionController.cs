using BankingApp.Core.FinancialServices;
using System.Net.Http;
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
        
        public HttpResponseMessage Get(int id)
        {
            var result = financialService.GetTransactionsStatements(id);
            return result;
        }
    }
}
