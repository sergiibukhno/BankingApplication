using System.Collections.Generic;

namespace BankingApp.DataRepository.Repositories
{
    public interface IRepository<T> where T:class
    {
        List<T> GetAll();
        void Create(T item);
    }
}
