using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Authorization;

namespace TeamApps.UI
{
    /// <summary>
    /// Get logged user details
    /// </summary>
    public class UserContext
    {
        private readonly AuthenticationStateProvider _authenticationStateProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserContext"/> class.
        /// </summary>
        /// <param name="authenticationStateProvider">provider</param>
        public UserContext(AuthenticationStateProvider authenticationStateProvider)
        {
            _authenticationStateProvider = authenticationStateProvider;
        }

        /// <summary>
        /// Is user authed
        /// </summary>
        /// <returns>Flag to show authed</returns>
        public async Task<bool> IsAuthenticatedAsync()
        {
            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();

            var identity = authState?.User?.Identity;

            return identity == null ? false : identity.IsAuthenticated;
        }

        /// <summary>
        /// Get user name
        /// </summary>
        /// <returns>Awaitable user name</returns>
        public async Task<string> GetUserNameAsync()
        {
            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();

            return authState.User.Identity.Name;
        }

        /// <summary>
        /// Get user details
        /// </summary>
        /// <param name="domain">App domain</param>
        /// <returns>Awaitable user details</returns>
        public async Task<ClaimsPrincipal> GetCurrentUserDetailsAsync()
        {
            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();

            return authState.User;
        }
    }
}
