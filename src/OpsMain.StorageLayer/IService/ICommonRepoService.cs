using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace OpsMain.StorageLayer.IService
{
    public interface ICommonRepoService<T>
    {
        Task<T> GetByIdAsync(long id);

        IQueryable<T> Query(string predicate=null);

        Task CreateAsync(T user);

        Task CreateAsync(IEnumerable<T> list);

        Task CreateBulkyAsync(IEnumerable<T> list);

        void Update(T user);

        Task DeleteAsync(long id);

        Task SaveChangesAsync();
    }
}