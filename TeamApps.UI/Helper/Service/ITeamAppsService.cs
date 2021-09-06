using System.Collections.Generic;
using System.Threading.Tasks;
using TeamApps.Shared;

namespace TeamApps.UI
{
    /// <summary>
    /// Teamapps service
    /// </summary>
    public interface ITeamAppsService
    {
        /// <summary>
        /// Fetch team apps
        /// </summary>
        /// <param name="skip">Items to skip</param>
        /// <param name="limit">Max number of items</param>
        /// <param name="userId">User id</param>
        /// <returns>Teamapp details</returns>
        Task<IEnumerable<TeamDetail>> GetTeamDetailsAsync(int skip, int limit, int userId);

        /// <summary>
        /// Fetch application details
        /// </summary>
        /// <param name="id">Application id</param>
        /// <returns>Application details</returns>
        Task<ApplicationDetail> GetApplicationDetailsByIdAsync(int id);

        /// <summary>
        /// Fetch all teams
        /// </summary>
        /// <returns>Keyvalue collection</returns>
        Task<IEnumerable<KeyValue>> GetAllTeamAsync();

        /// <summary>
        /// Add team
        /// </summary>
        /// <param name="applicationDetail">Application details</param>
        /// <returns>Result</returns>
        Task<bool> AddTeamAsync(ApplicationDetail applicationDetail);

        /// <summary>
        /// Add application
        /// </summary>
        /// <param name="applicationDetail">Application details</param>
        /// <returns>Result</returns>
        Task<int> AddApplicationToTeamAsync(ApplicationDetail applicationDetail);

        /// <summary>
        /// Update application
        /// </summary>
        /// <param name="applicationDetail">Application details</param>
        /// <returns>Result</returns>
        Task<bool> UpdateApplicationAsync(ApplicationDetail applicationDetail);
    }
}
