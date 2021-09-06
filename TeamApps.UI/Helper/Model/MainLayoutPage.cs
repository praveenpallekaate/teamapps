using Microsoft.AspNetCore.Components;
using TeamApps.Shared;

namespace TeamApps.UI
{
    /// <summary>
    /// Layout page model
    /// </summary>
    public class MainLayoutPage
    {
        /// <summary>
        /// Gets or sets user details
        /// </summary>
        public UserDetail LoggedUser { get; set; }
            = new UserDetail();

        /// <summary>
        /// Gets or sets event callback
        /// </summary>
        public EventCallback CheckUserStatus { get; set; }

        /// <summary>
        /// Gets or sets loading message
        /// </summary>
        public string LoadingMessage { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating whether gets or sets
        /// </summary>
        public bool IsAuthenticated { get; set; } = false;

        /// <summary>
        /// Gets or sets a value indicating whether gets or sets
        /// </summary>
        public bool IsLoading { get; set; } = false;
    }
}
