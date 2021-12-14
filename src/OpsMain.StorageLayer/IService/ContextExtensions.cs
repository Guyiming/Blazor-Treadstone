using EFCore.BulkExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OpsMain.StorageLayer.IService
{
    public static class ContextExtensions
    {
        /// <summary>
        /// 批量删除，,会自动SaveChanges
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static async Task DeleteBatchAsync<T>(this IQueryable<T> data)
        {
            await data.BatchDeleteAsync();
        }

        /// <summary>
        /// 更新数据,会自动SaveChanges
        /// </summary>
        /// <param name="data"></param>
        /// <param name="updateExpression">如：x => new Customer {IsActive = false}，表示将listToUpdate里所有数据的IsActive设置为false</param>
        /// <returns></returns>
        public static async Task UpdateBatchAsync<T>(this IQueryable<T> data, Expression<Func<T, T>> updateExpression)
        {
            await data.BatchUpdateAsync(updateExpression);
        }
    }
}
