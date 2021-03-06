using BankingApp.Core.AccountServices;
using BankingApp.Core.AppSettings;
using BankingApp.Core.FinancialServices;
using BankingApp.Core.TokenHandler;
using BankingApp.Core.UserServices;
using BankingApp.DataRepository.DataContext;
using BankingApp.DataRepository.UnitOfWork;
using Microsoft.Practices.Unity;
using System.Web.Http;
using Unity.WebApi;

namespace BankingApp.WebApp
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();
            container.RegisterType<IDataContext, DataContext>();
            container.RegisterType<IUnitOfWork, UnitOfWork>();
            container.RegisterType<IFinancialService, FinancialService>();
            container.RegisterType<IUserService, UserService>();
            container.RegisterType<IUnitOfWorkFactory, UnitOfWorkFactory>();
            container.RegisterType<IAccountService, AccountService>();
            container.RegisterType<IAppSettings, AppSettingsWrapper>();
            container.RegisterType<ITokenHandler, TokenHandler>();
            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}