
namespace BankingApp.DataRepository.UnitOfWork
{
    public class UnitOfWorkFactory:IUnitOfWorkFactory
    {
        public UnitOfWork GetUnitOfWork()
        {
            return new UnitOfWork();
        }
    }
}
