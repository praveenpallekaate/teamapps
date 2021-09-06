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
    /// User repository
    /// </summary>
    public class UserRepository : RepositoryBase<User>, IRepository<User>
    {
        private readonly string _collection;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserRepository"/> class.
        /// ctor
        /// </summary>
        /// <param name="options"></param>
        public UserRepository(IOptions<AppSettings> options)
            : base(options)
        {
            _collection = AppCollections.Users.ToString();
        }

        /// <summary>
        /// Fetch user item
        /// </summary>
        /// <param name="predicate">Filter to apply</param>
        /// <returns>user item</returns>
        public async Task<User> GetItemAsync(Expression<Func<User, bool>> predicate)
        {
            return await LiteDbHelper<User>.GetItemAsync(_collection, predicate);
        }

        /// <summary>
        /// Fetches user items
        /// </summary>
        /// <param name="predicate">Filter to apply</param>
        /// <param name="skip">Items to skip</param>
        /// <param name="limit">Max number of items</param>
        /// <returns>User items</returns>
        public async Task<IEnumerable<User>> GetItemsAsync(Expression<Func<User, bool>> predicate, int skip = 0, int limit = 9999999)
        {
            return await LiteDbHelper<User>.GetItemsAsync(_collection, predicate, skip, limit);
        }

        /// <summary>
        /// Fetch the max id of collection
        /// </summary>
        /// <returns>Max id</returns>
        public async Task<int?> GetMaxIdAsync()
        {
            return await LiteDbHelper<User>.GetMaxIdAsync(_collection);
        }

        /// <summary>
        /// Insert item
        /// </summary>
        /// <param name="item">User item</param>
        /// <returns>Result</returns>
        public async Task<BsonValue> InsertItemAsync(User item)
        {
            return await LiteDbHelper<User>.InsertItemAsync(_collection, item);
        }
    }
}
