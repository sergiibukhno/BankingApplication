using System.Collections.Specialized;
using System.Configuration;

namespace BankingApp.Core.AppSettings
{
    public class AppSettingsWrapper:IAppSettings
    {
        private readonly NameValueCollection appSettings;

        public AppSettingsWrapper()
        {
            this.appSettings = ConfigurationManager.AppSettings;
        }
        public string this[string key]
        {
            get
            {
                return this.appSettings[key];
            }
        }
    }
}
