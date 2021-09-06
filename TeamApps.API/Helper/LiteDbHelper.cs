using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Threading.Tasks;
using LiteDB;
using LiteDB.Async;
using Microsoft.Extensions.Options;
using TeamApps.Shared;

namespace TeamApps.API
{
    /// <summary>
    /// Lite Db helper
    /// </summary>
    /// <typeparam name="T">Data model</typeparam>
    public static class LiteDbHelper<T>
        where T : IDataModel
    {
        private static bool _isStream = false;
        private static string _streamConnectionString = string.Empty;
        private static string _dbFilePath = string.Empty;
        private static string _dbPassword = string.Empty;

        /// <summary>
        /// Fetches all items in collection
        /// </summary>
        /// <param name="collection">Collection name</param>
        /// <returns><see cref="T"/> collection</returns>
        public static async Task<IEnumerable<T>> GetAllItemsAsync(string collection)
        {
            using (var db = GetInstanceForAsync())
            {
                var coll = db.GetCollection<T>(collection);

                return await coll.FindAllAsync();
            }
        }

        /// <summary>
        /// Fetches items on predicate
        /// </summary>
        /// <param name="collection">Collection name</param>
        /// <param name="predicate">Filter to apply</param>
        /// <param name="skip">Items to skip</param>
        /// <param name="limit">Max number of items</param>
        /// <returns><see cref="T"/> collection</returns>
        public static async Task<IEnumerable<T>> GetItemsAsync(string collection, Expression<Func<T, bool>> predicate, int skip = 0, int limit = 9999999)
        {
            using (var db = GetInstanceForAsync())
            {
                var coll = db.GetCollection<T>(collection);

                return await coll.FindAsync(predicate, skip, limit);
            }
        }

        /// <summary>
        /// Fetch item on predicate
        /// </summary>
        /// <param name="collection">Collection name</param>
        /// <param name="predicate">Filter to apply</param>
        /// <returns><see cref="T"/> item</returns>
        public static async Task<T> GetItemAsync(string collection, Expression<Func<T, bool>> predicate)
        {
            using (var db = GetInstanceForAsync())
            {
                var coll = db.GetCollection<T>(collection);

                return await coll.FindOneAsync(predicate);
            }
        }

        /// <summary>
        /// Fetch the max id of collection
        /// </summary>
        /// <param name="collection">Collection name</param>
        /// <returns>Max id</returns>
        public static async Task<int?> GetMaxIdAsync(string collection)
        {
            using (var db = GetInstanceForAsync())
            {
                var coll = db.GetCollection<T>(collection);

                var result = await coll.FindOneAsync(Query.All(Query.Descending));

                return result?.Id;
            }
        }

        /// <summary>
        /// Insert item to collection
        /// </summary>
        /// <param name="collection">Collection name</param>
        /// <param name="item"><see cref="T"/> item</param>
        /// <returns>Result</returns>
        public static async Task<BsonValue> InsertItemAsync(string collection, T item)
        {
            using (var db = GetInstanceForAsync())
            {
                var coll = db.GetCollection<T>(collection);

                var result = await coll.InsertAsync(item);

                return result;
            }
        }

        /// <summary>
        /// Insert items to collection
        /// </summary>
        /// <param name="collection">Collection name</param>
        /// <param name="items"><see cref="T"/> collection</param>
        /// <returns>Result</returns>
        public static async Task<int> InsertItemsAsync(string collection, IEnumerable<T> items)
        {
            using (var db = GetInstanceForAsync())
            {
                var coll = db.GetCollection<T>(collection);

                var result = await coll.InsertAsync(items);

                return result;
            }
        }

        /// <summary>
        /// Updates item to collection
        /// </summary>
        /// <param name="collection">Collection name</param>
        /// <param name="item"><see cref="T"/> item</param>
        /// <returns>Result</returns>
        public static async Task<bool> UpdateItemAsync(string collection, T item)
        {
            using (var db = GetInstanceForAsync())
            {
                var coll = db.GetCollection<T>(collection);

                var result = await coll.UpdateAsync(item);

                return result;
            }
        }

        /// <summary>
        /// Initialize helper
        /// </summary>
        /// <param name="options">App configs</param>
        public static void Initialize(IOptions<AppSettings> options)
        {
            if (!_isStream)
            {
                _isStream = options.Value.DatabaseSettings.LiteDbSettings.IsInBlob;
            }

            if (string.IsNullOrEmpty(_streamConnectionString))
            {
                _streamConnectionString = options.Value.DatabaseSettings.LiteDbSettings.BlobConnectionString;
            }

            if (string.IsNullOrEmpty(_dbFilePath))
            {
                _dbFilePath = options.Value.DatabaseSettings.LiteDbSettings.DbFilePath;
            }

            if (string.IsNullOrEmpty(_dbPassword))
            {
                _dbPassword = options.Value.DatabaseSettings.LiteDbSettings.DbPassword;
            }
        }

        /// <summary>
        /// For disposing
        /// </summary>
        public static void Dispose()
        {
        }

        /// <summary>
        /// YET TO IMPLEMENT FOR AZURE
        /// </summary>
        /// <returns></returns>
        private static Stream GetStream()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Builds connection string for database
        /// </summary>
        /// <returns></returns>
        private static ConnectionString GetConnectionString() =>
            new ConnectionString { Filename = _dbFilePath, Password = _dbPassword, Connection = ConnectionType.Shared };

        /// <summary>
        /// Creates database instance
        /// </summary>
        /// <returns></returns>
        private static LiteDatabase GetInstance() => _isStream ? new LiteDatabase(GetStream()) : new LiteDatabase(GetConnectionString());

        /// <summary>
        /// Creates database instance async
        /// </summary>
        /// <returns></returns>
        private static LiteDatabaseAsync GetInstanceForAsync() => new LiteDatabaseAsync(GetInstance());
    }
}
