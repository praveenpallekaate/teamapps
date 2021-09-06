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
    /// Application collection repository
    /// </summary>
    public class ApplicationRepository : RepositoryBase<Application>, IRepository<Application>
    {
        private readonly string _collection;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationRepository"/> class.
        /// ctor
        /// </summary>
        /// <param name="options"></param>
        public ApplicationRepository(IOptions<AppSettings> options)
            : base(options)
        {
            _collection = AppCollections.Applications.ToString();
        }

        /// <summary>
        /// Fetch application item
        /// </summary>
        /// <param name="predicate">Filter to apply</param>
        /// <returns>Application item</returns>
        public async Task<Application> GetItemAsync(Expression<Func<Application, bool>> predicate)
        {
            return await LiteDbHelper<Application>.GetItemAsync(_collection, predicate);
        }

        /// <summary>
        /// Fetches application items
        /// </summary>
        /// <param name="predicate">Filter to apply</param>
        /// <param name="skip">Items to skip</param>
        /// <param name="limit">Max number of items</param>
        /// <returns>Application items</returns>
        public async Task<IEnumerable<Application>> GetItemsAsync(Expression<Func<Application, bool>> predicate, int skip = 0, int limit = 9999999)
        {
            return await LiteDbHelper<Application>.GetItemsAsync(_collection, predicate, skip, limit);
        }

        /// <summary>
        /// Fetch the max id of collection
        /// </summary>
        /// <returns>Max id</returns>
        public async Task<int?> GetMaxIdAsync()
        {
            return await LiteDbHelper<Application>.GetMaxIdAsync(_collection);
        }

        /// <summary>
        /// Insert item
        /// </summary>
        /// <param name="item">Application item</param>
        /// <returns>Result</returns>
        public async Task<BsonValue> InsertItemAsync(Application item)
        {
            return await LiteDbHelper<Application>.InsertItemAsync(_collection, item);
        }

        /// <summary>
        /// Update item
        /// </summary>
        /// <param name="item">Application item</param>
        /// <returns>Result</returns>
        public async Task<bool> UpdateItemAsync(Application item)
        {
            return await LiteDbHelper<Application>.UpdateItemAsync(_collection, item);
        }
    }
}
