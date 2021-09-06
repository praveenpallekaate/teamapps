using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using TeamApps.Shared;

namespace TeamApps.UI
{
    /// <summary>
    /// Resource service
    /// </summary>
    public class ResourceService : IResourceService
    {
        private const string _api = "api/resource";
        private readonly HttpClient _httpClient = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceService"/> class.
        /// </summary>
        public ResourceService()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceService"/> class.
        /// </summary>
        /// <param name="httpClient">Httpclient</param>
        public ResourceService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        /// <summary>
        /// Fetch resource filters
        /// </summary>
        /// <returns>Filter details</returns>
        public async Task<ResourceDetail> GetFiltersAsync()
        {
            var result = await _httpClient
                .GetFromJsonAsync<ResourceDetail>($"{_api}/filter");

            return result;
        }

        /// <summary>
        /// Fetch resource detail
        /// </summary>
        /// <param name="id">Resource id</param>
        /// <returns>Resource details</returns>
        public async Task<ResourceDetail> GetResourceByIdAsync(int id)
        {
            var result = await _httpClient
                .GetFromJsonAsync<ResourceDetail>($"{_api}/{id}");

            return result;
        }

        /// <summary>
        /// Fetch resources
        /// </summary>
        /// <param name="resourceDetail">Filter details</param>
        /// <returns>Resource collection</returns>
        public async Task<IEnumerable<ResourceDetail>> GetResourcesAsync(ResourceDetail resourceDetail)
        {
            IEnumerable<ResourceDetail> result = Enumerable.Empty<ResourceDetail>();
            var request = await _httpClient
                .PostAsJsonAsync<ResourceDetail>($"{_api}/teamresources", resourceDetail);

            if (request.IsSuccessStatusCode)
            {
                result = await request.Content.ReadFromJsonAsync<IEnumerable<ResourceDetail>>();
            }

            return result;
        }

        /// <summary>
        /// Add resource
        /// </summary>
        /// <param name="resourceDetail">Resource details</param>
        /// <returns>Result</returns>
        public async Task<int> AddResourceAsync(ResourceDetail resourceDetail)
        {
            int result = 0;
            var request = await _httpClient
                .PostAsJsonAsync<ResourceDetail>($"{_api}", resourceDetail);

            if (request.IsSuccessStatusCode)
            {
                var response = await request.Content.ReadAsStringAsync();

                result = int.Parse(response);
            }

            return result;
        }

        /// <summary>
        /// Update resource
        /// </summary>
        /// <param name="resourceDetail">Resource details</param>
        /// <returns>Result</returns>
        public async Task<bool> UpdateResourceAsync(ResourceDetail resourceDetail)
        {
            bool result = false;
            var request = await _httpClient
                .PutAsJsonAsync<ResourceDetail>($"{_api}/updateresource", resourceDetail);

            if (request.IsSuccessStatusCode)
            {
                var response = await request.Content.ReadAsStringAsync();

                result = bool.Parse(response);
            }

            return result;
        }
    }
}