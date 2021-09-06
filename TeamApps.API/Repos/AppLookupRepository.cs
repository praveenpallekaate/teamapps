using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using LiteDB;
using Microsoft.Extensions.Options;
using TeamApps.Shared;

namespace TeamApps.API
{
    /// <summary>
    /// App lookup collection repository
    /// </summary>
    public class AppLookupRepository : RepositoryBase<AppLookup>, IRepository<AppLookup>
    {
        private readonly string _collection;

        /// <summary>
        /// Initializes a new instance of the <see cref="AppLookupRepository"/> class.
        /// ctor
        /// </summary>
        /// <param name="options"></param>
        public AppLookupRepository(IOptions<AppSettings> options)
            : base(options)
        {
            _collection = AppCollections.AppLookups.ToString();
        }

        /// <summary>
        /// Fetch applookup item
        /// </summary>
        /// <param name="predicate">Filter to apply</param>
        /// <returns>applookup item</returns>
        public async Task<AppLookup> GetItemAsync(Expression<Func<AppLookup, bool>> predicate)
        {
            return await LiteDbHelper<AppLookup>.GetItemAsync(_collection, predicate);
        }

        /// <summary>
        /// Fetches applookup items
        /// </summary>
        /// <param name="predicate">Filter to apply</param>
        /// <param name="skip">Items to skip</param>
        /// <param name="limit">Max number of items</param>
        /// <returns>AppLookup items</returns>
        public async Task<IEnumerable<AppLookup>> GetItemsAsync(Expression<Func<AppLookup, bool>> predicate, int skip = 0, int limit = 9999999)
        {
            return await LiteDbHelper<AppLookup>.GetItemsAsync(_collection, predicate, skip, limit);
        }

        /// <summary>
        /// Fetch the max id of collection
        /// </summary>
        /// <returns>Max id</returns>
        public async Task<int?> GetMaxIdAsync()
        {
            return await LiteDbHelper<AppLookup>.GetMaxIdAsync(_collection);
        }

        /// <summary>
        /// Insert item
        /// </summary>
        /// <param name="item">AppLookup item</param>
        /// <returns>Result</returns>
        public async Task<BsonValue> InsertItemAsync(AppLookup item)
        {
            return await LiteDbHelper<AppLookup>.InsertItemAsync(_collection, item);
        }

        /// <summary>
        /// Update item
        /// </summary>
        /// <param name="item">AppLookup item</param>
        /// <returns>Result</returns>
        public async Task<bool> UpdateItemAsync(AppLookup item)
        {
            return await LiteDbHelper<AppLookup>.UpdateItemAsync(_collection, item);
        }
    }
}
