
namespace BankingApp.DataRepository.UnitOfWork
{
    public interface IUnitOfWorkFactory
    {
        UnitOfWork GetUnitOfWork();
    }
}
