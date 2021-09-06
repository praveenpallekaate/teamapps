using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using LiteDB;
using TeamApps.Shared;

namespace TeamApps.API
{
    /// <summary>
    /// Repository interface
    /// </summary>
    /// <typeparam name="T">Data model</typeparam>
    public interface IRepository<T>
        where T : IDataModel
    {
        /// <summary>
        /// Fetch items for filter
        /// </summary>
        /// <param name="predicate">Filter to apply</param>
        /// <param name="skip">Items to skip</param>
        /// <param name="limit">Max number of items</param>
        /// <returns><see cref="T"/> collection</returns>
        Task<IEnumerable<T>> GetItemsAsync(Expression<Func<T, bool>> predicate, int skip = 0, int limit = 9999999);

        /// <summary>
        /// Fetch item for filter
        /// </summary>
        /// <param name="predicate">Filter to apply</param>
        /// <returns><see cref="T"/> item</returns>
        Task<T> GetItemAsync(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Insert item to collection
        /// </summary>
        /// <param name="item"><see cref="T"/> item</param>
        /// <returns>Result</returns>
        Task<BsonValue> InsertItemAsync(T item);
    }
}
