using System.Collections.Generic;
using System.Threading.Tasks;
using ASI.Basecode.Data.Models;

namespace ASI.Basecode.Data.Repositories
{
    public interface IClassRepository
    {
        Task<List<Class>> GetAllAsync();
        Task<Class?> GetByEdpAsync(int edpCode);
        Task AddAsync(Class entity);
        Task UpdateAsync(Class entity);
        Task DeleteAsync(Class entity);
    }
}
