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
    /// Reource repository
    /// </summary>
    public class ResourceRepository : RepositoryBase<Resource>, IRepository<Resource>
    {
        private readonly string _collection;

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceRepository"/> class.
        /// ctor
        /// </summary>
        /// <param name="options"></param>
        public ResourceRepository(IOptions<AppSettings> options)
            : base(options)
        {
            _collection = AppCollections.Resources.ToString();
        }

        /// <summary>
        /// Fetch resource item
        /// </summary>
        /// <param name="predicate">Filter to apply</param>
        /// <returns>Application item</returns>
        public async Task<Resource> GetItemAsync(Expression<Func<Resource, bool>> predicate)
        {
            return await LiteDbHelper<Resource>.GetItemAsync(_collection, predicate);
        }

        /// <summary>
        /// Fetches resource items
        /// </summary>
        /// <param name="predicate">Filter to apply</param>
        /// <param name="skip">Items to skip</param>
        /// <param name="limit">Max number of items</param>
        /// <returns>Application items</returns>
        public async Task<IEnumerable<Resource>> GetItemsAsync(Expression<Func<Resource, bool>> predicate, int skip = 0, int limit = 9999999)
        {
            return await LiteDbHelper<Resource>.GetItemsAsync(_collection, predicate, skip, limit);
        }

        /// <summary>
        /// Fetch the max id of collection
        /// </summary>
        /// <returns>Max id</returns>
        public async Task<int?> GetMaxIdAsync()
        {
            return await LiteDbHelper<Resource>.GetMaxIdAsync(_collection);
        }

        /// <summary>
        /// Insert item
        /// </summary>
        /// <param name="item">Resource item</param>
        /// <returns>Result</returns>
        public async Task<BsonValue> InsertItemAsync(Resource item)
        {
            return await LiteDbHelper<Resource>.InsertItemAsync(_collection, item);
        }

        /// <summary>
        /// Update item
        /// </summary>
        /// <param name="item">Resource item</param>
        /// <returns>Result</returns>
        public async Task<bool> UpdateItemAsync(Resource item)
        {
            return await LiteDbHelper<Resource>.UpdateItemAsync(_collection, item);
        }
    }
}
