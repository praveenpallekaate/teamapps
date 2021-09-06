using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TeamApps.Shared;

namespace TeamApps.API
{
    /// <summary>
    /// AppLookup service
    /// </summary>
    public interface IAppLookupManagement :
        IService<AppLookup>,
        IReadService<AppLookup, AppLookupDetail>,
        IWriteService<AppLookup, AppLookupDetail>
    {
        /// <summary>
        /// Fetch applookup item
        /// </summary>
        /// <param name="predicate">Filter to apply</param>
        /// <returns>Applookup item</returns>
        Task<AppLookup> GetItemAsync(Expression<Func<AppLookup, bool>> predicate);

        /// <summary>
        /// To view model
        /// </summary>
        /// <param name="appLookup">AppLookup item</param>
        /// <returns>AppLookup detail</returns>
        AppLookupDetail ToViewModel(AppLookup appLookup);
    }
}
