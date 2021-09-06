using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using TeamApps.Shared;

namespace TeamApps.UI
{
    /// <summary>
    /// Teamapps service
    /// </summary>
    public class TeamAppsService : ITeamAppsService
    {
        private const string _api = "api/application";
        private readonly HttpClient _httpClient = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="TeamAppsService"/> class.
        /// ctor
        /// </summary>
        public TeamAppsService()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TeamAppsService"/> class.
        /// ctor
        /// </summary>
        /// <param name="httpClient">Httpclient</param>
        public TeamAppsService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        /// <summary>
        /// Fetch team apps
        /// </summary>
        /// <param name="skip">Items to skip</param>
        /// <param name="limit">Max number of items</param>
        /// <param name="userId">User id</param>
        /// <returns>Teamapp details</returns>
        public async Task<IEnumerable<TeamDetail>> GetTeamDetailsAsync(int skip, int limit, int userId)
        {
            var result = await _httpClient
                .GetFromJsonAsync<IEnumerable<TeamDetail>>($"{_api}/{skip}/{limit}/{userId}");

            return result;
        }

        /// <summary>
        /// Fetch application details
        /// </summary>
        /// <param name="id">Application id</param>
        /// <returns>Application details</returns>
        public async Task<ApplicationDetail> GetApplicationDetailsByIdAsync(int id)
        {
            var result = await _httpClient
                .GetFromJsonAsync<ApplicationDetail>($"{_api}/{id}");

            return result;
        }

        /// <summary>
        /// Fetch all teams
        /// </summary>
        /// <returns>Keyvalue collection</returns>
        public async Task<IEnumerable<KeyValue>> GetAllTeamAsync()
        {
            var result = await _httpClient
                .GetFromJsonAsync<IEnumerable<KeyValue>>($"{_api}/allteams");

            return result;
        }

        /// <summary>
        /// Add team
        /// </summary>
        /// <param name="applicationDetail">Application details</param>
        /// <returns>Result</returns>
        public async Task<bool> AddTeamAsync(ApplicationDetail applicationDetail)
        {
            bool result = false;
            var request = await _httpClient
                .PostAsJsonAsync<ApplicationDetail>($"{_api}/addteam", applicationDetail);

            if (request.IsSuccessStatusCode)
            {
                var response = await request.Content.ReadAsStringAsync();

                result = bool.Parse(response);
            }

            return result;
        }

        /// <summary>
        /// Add application
        /// </summary>
        /// <param name="applicationDetail">Application details</param>
        /// <returns>Result</returns>
        public async Task<int> AddApplicationToTeamAsync(ApplicationDetail applicationDetail)
        {
            int result = 0;
            var request = await _httpClient
                .PostAsJsonAsync<ApplicationDetail>($"{_api}", applicationDetail);

            if (request.IsSuccessStatusCode)
            {
                var response = await request.Content.ReadAsStringAsync();

                result = int.Parse(response);
            }

            return result;
        }

        /// <summary>
        /// Update application
        /// </summary>
        /// <param name="applicationDetail">Application details</param>
        /// <returns>Result</returns>
        public async Task<bool> UpdateApplicationAsync(ApplicationDetail applicationDetail)
        {
            bool result = false;
            var request = await _httpClient
                .PutAsJsonAsync<ApplicationDetail>($"{_api}/updateapplication", applicationDetail);

            if (request.IsSuccessStatusCode)
            {
                var response = await request.Content.ReadAsStringAsync();

                result = bool.Parse(response);
            }

            return result;
        }
    }
}
