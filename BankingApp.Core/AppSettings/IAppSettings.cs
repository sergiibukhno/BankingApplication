
namespace BankingApp.Core.AppSettings
{
    public interface IAppSettings
    {
        string this[string key] { get; }
    }
}
