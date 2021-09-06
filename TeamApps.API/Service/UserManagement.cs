using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Core.Utilities;
using Microsoft.Extensions.Options;
using TeamApps.Shared;

namespace TeamApps.API
{
    /// <summary>
    /// User service
    /// </summary>
    public class UserManagement : IUserManagement
    {
        private readonly IOptions<AppSettings> _options = null;
        private readonly UserRepository _repository = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserManagement"/> class.
        /// ctor
        /// </summary>
        /// <param name="options"></param>
        /// <param name="repository"></param>
        public UserManagement(
            IOptions<AppSettings> options,
            UserRepository repository)
        {
            _options = options;
            _repository = repository;
        }

        /// <summary>
        /// Fetch user item
        /// </summary>
        /// <param name="id">Item id</param>
        /// <returns>User item</returns>
        public async Task<User> GetItemAsync(int id)
        {
            var result = await _repository
                .GetItemAsync(i => i.Id == id);

            return result;
        }

        /// <summary>
        /// Fetch user details item
        /// </summary>
        /// <param name="id">Item id</param>
        /// <returns>User detail item</returns>
        public async Task<UserDetail> GetItemDetailsAsync(int id)
        {
            var user = await GetItemAsync(id);

            return user is User ? ToViewModel(user) : null;
        }

        /// <summary>
        /// Fetch user items
        /// </summary>
        /// <param name="ids">Item ids</param>
        /// <returns>User items</returns>
        public async Task<IEnumerable<User>> GetItemsAsync(int[] ids)
        {
            var result = await _repository
                .GetItemsAsync(i => ids.Contains(i.Id));

            return result;
        }

        /// <summary>
        /// Fetch user details items
        /// </summary>
        /// <param name="ids">Item ids</param>
        /// <returns>User detail items</returns>
        public async Task<IEnumerable<UserDetail>> GetItemsDetailsAsync(int[] ids)
        {
            var users = await GetItemsAsync(ids);

            return users?
                .Select(i => ToViewModel(i));
        }

        /// <summary>
        /// Insert item to collection
        /// </summary>
        /// <param name="item">Application item</param>
        /// <returns></returns>
        public async Task<int> InsertItemAsync(UserDetail item)
        {
            var user = ToDataModel(item);

            item.Id = await MaxIdAsync();

            var result = await _repository
                .InsertItemAsync(user);

            return result.AsInt32;
        }

        /// <summary>
        /// YET TO IMPLEMENT
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public Task<bool> UpdateItemAsync(UserDetail item)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Check if user exists
        /// </summary>
        /// <param name="userDetail">User details</param>
        /// <returns>User id</returns>
        public async Task<int?> CheckIfUserExists(UserDetail userDetail)
        {
            int? result = null;

            if (!string.IsNullOrEmpty(userDetail.Email))
            {
                var userFilter = await GetItemsDetailsAsync(i => i.Email == userDetail.Email);

                result = CommonUtils.IsValidCollection(userFilter) ? userFilter.FirstOrDefault().Id : 0;
            }

            return result;
        }

        /// <summary>
        /// Fetch application items
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        private async Task<IEnumerable<User>> GetItemsAsync(Expression<Func<User, bool>> predicate)
        {
            var result = await _repository
                .GetItemsAsync(predicate);

            return result;
        }

        /// <summary>
        /// Fetch application detail items
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        private async Task<IEnumerable<UserDetail>> GetItemsDetailsAsync(Expression<Func<User, bool>> predicate)
        {
            var users = await GetItemsAsync(predicate);

            return users?
                .Select(i => ToViewModel(i));
        }

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
        /// <param name="userDetail">User detail item</param>
        /// <returns>Data model</returns>
        private User ToDataModel(UserDetail userDetail) =>
            new User
            {
                Id = userDetail.Id,
                CreatedBy = userDetail.CreatedBy,
                CreatedOn = userDetail.CreatedOn,
                Email = userDetail.Email,
                IsActive = userDetail.IsActive,
                Name = userDetail.Name,
                UpdatedBy = userDetail.UpdatedBy,
                UpdatedOn = userDetail.UpdatedOn,
            };

        /// <summary>
        /// To view model
        /// </summary>
        /// <param name="user">User model</param>
        /// <returns>View model</returns>
        private UserDetail ToViewModel(User user) =>
            new UserDetail
            {
                Id = user.Id,
                CreatedBy = user.CreatedBy,
                CreatedOn = user.CreatedOn,
                Email = user.Email,
                IsActive = user.IsActive,
                Name = user.Name,
                UpdatedBy = user.UpdatedBy,
                UpdatedOn = user.UpdatedOn,
            };
    }
}
