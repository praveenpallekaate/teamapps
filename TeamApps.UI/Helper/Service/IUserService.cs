using System.Threading.Tasks;
using TeamApps.Shared;

namespace TeamApps.UI
{
    /// <summary>
    /// User service
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Fetch user id
        /// </summary>
        /// <param name="userDetail"></param>
        /// <returns></returns>
        Task<int> GetUserIdAsync(UserDetail userDetail);
    }
}
