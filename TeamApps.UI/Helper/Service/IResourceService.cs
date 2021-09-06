using System.Collections.Generic;
using System.Threading.Tasks;
using TeamApps.Shared;

namespace TeamApps.UI
{
    /// <summary>
    /// Resource service
    /// </summary>
    public interface IResourceService
    {
        /// <summary>
        /// Fetch resource filters
        /// </summary>
        /// <returns>Filter details</returns>
        Task<ResourceDetail> GetFiltersAsync();

        /// <summary>
        /// Fetch resource detail
        /// </summary>
        /// <param name="id">Resource id</param>
        /// <returns>Resource details</returns>
        Task<ResourceDetail> GetResourceByIdAsync(int id);

        /// <summary>
        /// Fetch resources
        /// </summary>
        /// <param name="resourceDetail">Filter details</param>
        /// <returns>Resource collection</returns>
        Task<IEnumerable<ResourceDetail>> GetResourcesAsync(ResourceDetail resourceDetail);

        /// <summary>
        /// Add resource
        /// </summary>
        /// <param name="resourceDetail">Resource details</param>
        /// <returns>Result</returns>
        Task<int> AddResourceAsync(ResourceDetail resourceDetail);

        /// <summary>
        /// Update resource
        /// </summary>
        /// <param name="resourceDetail">Resource details</param>
        /// <returns>Result</returns>
        Task<bool> UpdateResourceAsync(ResourceDetail resourceDetail);
    }
}
