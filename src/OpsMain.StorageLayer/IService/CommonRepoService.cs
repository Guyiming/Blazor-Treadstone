using EFCore.BulkExtensions;
using OpsMain.StorageLayer.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace OpsMain.StorageLayer.IService
{
    public class CommonRepoService<T> : ICommonRepoService<T> where T : BaseEntity
    {
        private TreadstoneMainContext context;

        public CommonRepoService(TreadstoneMainContext context)
        {
            this.context = context;
        }

        public async Task CreateAsync(T user)
        {
            await context.Set<T>().AddAsync(user);
        }

        public async Task CreateAsync(IEnumerable<T> list)
        {
            await context.Set<T>().AddRangeAsync(list);
        }

        public async Task DeleteAsync(long id)
        {
            var user = await context.Set<T>().FindAsync(id);
            if (user != null)
            {
                context.Set<T>().Remove(user);
            }
        }

        public async Task<T> GetByIdAsync(long id)
        {
            return await context.Set<T>().FindAsync(id);
        }

        public async Task SaveChangesAsync()
        {
            await context.SaveChangesAsync();
        }

        public void Update(T t)
        {
            context.Set<T>().Update(t);
        }

        /// <summary>
        /// 批量新增，会自动SaveChanges
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public async Task CreateBulkyAsync(IEnumerable<T> list)
        {
            await context.BulkInsertAsync<T>(list.ToList());
        }

        /// <summary>
        /// 动态查询：https://dynamic-linq.net/overview， 用法：City=="Paris"&amp;&amp;Age>50
        /// </summary>
        /// <param name="predicate">搜索表达式，用法：City=="Paris"&amp;&amp;Age>50</param>
        /// <returns></returns>
        public IQueryable<T> Query(string predicate = null)
        {
            if (!string.IsNullOrEmpty(predicate))
                return context.Set<T>().Where(predicate);

            return context.Set<T>();
        }
    }
}