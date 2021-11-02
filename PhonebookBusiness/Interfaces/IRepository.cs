using System.Collections.Generic;
using System.Threading.Tasks;

namespace PhonebookBusiness.Interfaces
{
    public interface IRepository<T>
    {
        public Task<IEnumerable<T>> GetAll();

        public Task<T> GetOneById(int id);

        public Task<bool> Exists(int id);

        public Task<T> Create(T model);

        public Task<T> Update(int id, T model);

        public Task<bool> Delete(int id);

        public Task<IEnumerable<T>> Search(string text);
    }
}
