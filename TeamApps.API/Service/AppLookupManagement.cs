using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using TeamApps.Shared;

namespace TeamApps.API
{
    /// <summary>
    /// App lookup service
    /// </summary>
    public class AppLookupManagement : IAppLookupManagement
    {
        private readonly IOptions<AppSettings> _options = null;
        private readonly AppLookupRepository _repository = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="AppLookupManagement"/> class.
        /// ctor
        /// </summary>
        /// <param name="options"></param>
        /// <param name="repository"></param>
        public AppLookupManagement(
            IOptions<AppSettings> options,
            AppLookupRepository repository)
        {
            _options = options;
            _repository = repository;
        }

        /// <summary>
        /// Fetch applookup item
        /// </summary>
        /// <param name="id">Item id</param>
        /// <returns>AppLookup item</returns>
        public async Task<AppLookup> GetItemAsync(int id)
        {
            var result = await _repository
                .GetItemAsync(i => i.Id == id);

            return result;
        }

        /// <summary>
        /// Fetch applookup item
        /// </summary>
        /// <param name="predicate">Filter to apply</param>
        /// <returns>Applookup item</returns>
        public async Task<AppLookup> GetItemAsync(Expression<Func<AppLookup, bool>> predicate)
        {
            var result = await _repository
                .GetItemAsync(predicate);

            return result;
        }

        /// <summary>
        /// Fetch applookup details item
        /// </summary>
        /// <param name="id">Item id</param>
        /// <returns>AppLookup detail item</returns>
        public async Task<AppLookupDetail> GetItemDetailsAsync(int id)
        {
            var application = await GetItemAsync(id);

            return ToViewModel(application);
        }

        /// <summary>
        /// Fetch applookup details item
        /// </summary>
        /// <param name="predicate">Filter to apply</param>
        /// <returns>Applookup detail item</returns>
        public async Task<AppLookupDetail> GetItemDetailsAsync(Expression<Func<AppLookup, bool>> predicate)
        {
            var application = await GetItemAsync(predicate);

            return ToViewModel(application);
        }

        /// <summary>
        /// Fetch applookup items
        /// </summary>
        /// <param name="ids">Item ids</param>
        /// <returns>AppLookup items</returns>
        public async Task<IEnumerable<AppLookup>> GetItemsAsync(int[] ids)
        {
            var result = await _repository
                .GetItemsAsync(i => ids.Contains(i.Id));

            return result;
        }

        /// <summary>
        /// Fetch applookup details items
        /// </summary>
        /// <param name="ids">Item ids</param>
        /// <returns>AppLookup detail items</returns>
        public async Task<IEnumerable<AppLookupDetail>> GetItemsDetailsAsync(int[] ids)
        {
            var applications = await GetItemsAsync(ids);

            return applications?
                .Select(i => ToViewModel(i));
        }

        /// <summary>
        /// Insert item to collection
        /// </summary>
        /// <param name="item">AppLookup detail item</param>
        /// <returns>Result</returns>
        public async Task<int> InsertItemAsync(AppLookupDetail item)
        {
            var appLookup = ToDataModel(item);

            item.Id = await MaxIdAsync();

            var result = await _repository
                .InsertItemAsync(appLookup);

            return result.AsInt32;
        }

        /// <summary>
        /// Update item
        /// </summary>
        /// <param name="item">AppLookup item</param>
        /// <returns>Result</returns>
        public async Task<bool> UpdateItemAsync(AppLookupDetail item)
        {
            bool result = false;
            var applookup = await GetItemAsync(i => i.Id == item.Id);

            if (applookup is AppLookup)
            {
                applookup.IsActive = item.IsActive;
                applookup.KeyValues = item.KeyValues;
                applookup.Type = item.Type;
                applookup.UpdatedBy = item.UpdatedBy;
                applookup.UpdatedOn = item.UpdatedOn;

                result = await _repository.UpdateItemAsync(applookup);
            }

            return result;
        }

        /// <summary>
        /// To view model
        /// </summary>
        /// <param name="appLookup">AppLookup item</param>
        /// <returns>AppLookup detail</returns>
        public AppLookupDetail ToViewModel(AppLookup appLookup) =>
            new AppLookupDetail
            {
                CreatedBy = appLookup.CreatedBy,
                CreatedOn = appLookup.CreatedOn,
                Id = appLookup.Id,
                IsActive = appLookup.IsActive,
                KeyValues = appLookup.KeyValues,
                Type = appLookup.Type,
                UpdatedBy = appLookup.UpdatedBy,
                UpdatedOn = appLookup.UpdatedOn,
            };

        /// <summary>
        /// Fetch max id in collection
        /// </summary>
        /// <returns>Max id</returns>
        private async Task<int> MaxIdAsync()
        {
            int? maxId = await _repository.GetMaxIdAsync();

            return maxId.HasValue ? maxId.Value + 1 : 1;
        }

        /// <summary>
        /// To data model
        /// </summary>
        /// <param name="appLookupDetail">AppLookup detail item</param>
        /// <returns>AppLookup</returns>
        private AppLookup ToDataModel(AppLookupDetail appLookupDetail) =>
            new AppLookup
            {
                CreatedBy = appLookupDetail.CreatedBy,
                CreatedOn = appLookupDetail.CreatedOn,
                Id = appLookupDetail.Id,
                IsActive = appLookupDetail.IsActive,
                KeyValues = appLookupDetail.KeyValues,
                Type = appLookupDetail.Type,
                UpdatedBy = appLookupDetail.UpdatedBy,
                UpdatedOn = appLookupDetail.UpdatedOn,
            };
    }
}
