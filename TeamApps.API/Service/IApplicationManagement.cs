using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TeamApps.Shared;

namespace TeamApps.API
{
    /// <summary>
    /// Application service
    /// </summary>
    public interface IApplicationManagement :
        IService<Application>,
        IReadService<Application, ApplicationDetail>,
        IWriteService<Application, ApplicationDetail>
    {
        /// <summary>
        /// Fetch applications for teams
        /// </summary>
        /// <param name="skip">Items to skip</param>
        /// <param name="limit">Max number of items</param>
        /// <param name="applicationDetail">Application detail</param>
        /// <returns>Dictonary of team details</returns>
        Task<IEnumerable<TeamDetail>> GetTeamDetailsAsync(int skip = 0, int limit = 9999999, ApplicationDetail applicationDetail = null);

        /// <summary>
        /// Fetch application detail items
        /// </summary>
        /// <param name="predicate">Application filter</param>
        /// <param name="IsForList">Flag for list</param>
        /// <returns></returns>
        Task<IEnumerable<ApplicationDetail>> GetItemsDetailsAsync(Expression<Func<Application, bool>> predicate, bool isForList = false);

        /// <summary>
        /// Fetches all teams
        /// </summary>
        /// <returns>Keyvalue collection</returns>
        Task<IEnumerable<KeyValue>> GetAllTeamsAsync();

        /// <summary>
        /// Insert team
        /// </summary>
        /// <param name="item">Application item</param>
        /// <returns>Result</returns>
        Task<bool> InsertTeamAsync(ApplicationDetail item);
    }
}
