using OpsMain.StorageLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;

namespace OpsMain.StorageLayer.IService
{
    public class CommonRepoService<T> : ICommonRepoService<T> where T : BaseEntity
    {
        OpsDbContext context;

        public CommonRepoService(OpsDbContext context)
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

        #region Powered By: Dynamic.Linq.Core
        /// <summary>
        /// 批量删除，,会自动SaveChanges
        /// </summary>
        /// <param name="listToDelete"></param>
        /// <returns></returns>
        public async Task DeleteBatchAsync(IQueryable<T> listToDelete)
        {
            await listToDelete.DeleteFromQueryAsync();
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

        /// <summary>
        /// 更新数据,会自动SaveChanges
        /// </summary>
        /// <param name="listToUpdate"></param>
        /// <param name="updateExpression">如：x => new Customer {IsActive = false}，表示将listToUpdate里所有数据的IsActive设置为false</param>
        /// <returns></returns>
        public async Task UpdateBatchAsync(IQueryable<T> listToUpdate, Expression<Func<T, T>> updateExpression)
        {
            await listToUpdate.UpdateFromQueryAsync(updateExpression);
        }
        /// <summary>
        /// 批量新增，,会自动SaveChanges
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public async Task CreateBulkyAsync(IEnumerable<T> list)
        {
            await context.BulkInsertAsync(list);
        } 
        #endregion
    }
}
