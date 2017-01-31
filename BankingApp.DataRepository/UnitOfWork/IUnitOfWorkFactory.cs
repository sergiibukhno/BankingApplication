
namespace BankingApp.DataRepository.UnitOfWork
{
    public interface IUnitOfWorkFactory
    {
        IUnitOfWork GetUnitOfWork();
    }
}
