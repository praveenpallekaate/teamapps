using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using TeamApps.Shared;

namespace TeamApps.UI
{
    /// <summary>
    /// User service
    /// </summary>
    public class UserService : IUserService
    {
        private const string _api = "api/user";
        private readonly HttpClient _httpClient = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserService"/> class.
        /// ctor
        /// </summary>
        public UserService()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserService"/> class.
        /// ctor
        /// </summary>
        /// <param name="httpClient">HttpClient</param>
        public UserService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        /// <summary>
        /// Get user id for details
        /// </summary>
        /// <param name="userDetail">User details</param>
        /// <returns>User id</returns>
        public async Task<int> GetUserIdAsync(UserDetail userDetail)
        {
            int result = 0;
            var operation = await _httpClient.PostAsJsonAsync<UserDetail>(_api, userDetail);

            if (operation.IsSuccessStatusCode)
            {
                string id = await operation.Content.ReadAsStringAsync();

                result = int.Parse(id);
            }

            return result;
        }
    }
}
