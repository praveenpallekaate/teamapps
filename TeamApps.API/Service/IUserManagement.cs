using System.Threading.Tasks;
using TeamApps.Shared;

namespace TeamApps.API
{
    /// <summary>
    /// User service
    /// </summary>
    public interface IUserManagement :
        IService<User>,
        IReadService<User, UserDetail>,
        IWriteService<User, UserDetail>
    {
        /// <summary>
        /// Check if user exists
        /// </summary>
        /// <param name="userDetail">User details</param>
        /// <returns>User id</returns>
        Task<int?> CheckIfUserExists(UserDetail userDetail);
    }
}
