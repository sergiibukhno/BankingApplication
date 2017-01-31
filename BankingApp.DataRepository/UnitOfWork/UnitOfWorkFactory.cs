
namespace BankingApp.DataRepository.UnitOfWork
{
    public class UnitOfWorkFactory:IUnitOfWorkFactory
    {
        public IUnitOfWork GetUnitOfWork()
        {
            return new UnitOfWork();
        }
    }
}
