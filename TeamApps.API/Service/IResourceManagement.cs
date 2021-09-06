using System.Collections.Generic;
using System.Threading.Tasks;
using TeamApps.Shared;

namespace TeamApps.API
{
    /// <summary>
    /// Resource Service
    /// </summary>
    public interface IResourceManagement :
        IService<Resource>,
        IReadService<Resource, ResourceDetail>,
        IWriteService<Resource, ResourceDetail>
    {
        /// <summary>
        /// Fetch resource details for team
        /// </summary>
        /// <param name="resourceDetail">Resource filter details</param>
        /// <returns>Resource details</returns>
        Task<IEnumerable<ResourceDetail>> GetResourcesForTeamAsync(ResourceDetail resourceDetail);

        /// <summary>
        /// Get filter details
        /// </summary>
        /// <returns>Filter details</returns>
        Task<ResourceDetail> GetFiltersAsync();
    }
}
