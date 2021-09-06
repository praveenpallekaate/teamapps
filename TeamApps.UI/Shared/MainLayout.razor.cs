using System;
using System.Linq;
using System.Threading.Tasks;
using MatBlazor;
using Microsoft.AspNetCore.Components;
using TeamApps.Shared;

namespace TeamApps.UI
{
    /// <summary>
    /// Main layout page
    /// </summary>
    public class MainLayoutBase : LayoutComponentBase
    {
        /// <summary>
        /// Gets or sets user context
        /// </summary>
        [Inject]
        public UserContext LoggedUserContext { get; set; }

        /// <summary>
        /// Gets or sets user service
        /// </summary>
        [Inject]
        public IUserService UserService { get; set; }

        /// <summary>
        /// Gets or sets page model
        /// </summary>
        public MainLayoutPage MainLayoutPageDetails { get; set; }
            = new MainLayoutPage();

        /// <summary>
        /// Gets or sets current theme
        /// </summary>
        protected string CurrentTheme { get; set; }
            = AppThemes.light.ToString();

        /// <summary>
        /// Gets or sets menu mode
        /// </summary>
        protected string MenuMode { get; set; } = "full";

        /// <summary>
        /// Gets or sets app theme
        /// </summary>
        protected MatTheme AppTheme { get; set; }
            = new MatTheme();

        /// <summary>
        /// Page intialized
        /// </summary>
        /// <returns></returns>
        protected override async Task OnInitializedAsync()
        {
            ToggleLoading("Checking if user Authenticated!!");

            // Toggle theme
            ToggleTheme(AppThemes.light);

            // Set call back for childs
            MainLayoutPageDetails.CheckUserStatus = EventCallback.Factory.Create(this, UpdateUserOnLoginAsync);

            // Load user
            await UpdateUserOnLoginAsync();

            ToggleLoading();
        }

        /// <summary>
        /// Toggles theme
        /// </summary>
        /// <param name="theme">Theme type</param>
        protected void ToggleTheme(AppThemes theme)
        {
            CurrentTheme = theme.ToString();

            switch (theme)
            {
                case AppThemes.dark:
                    AppTheme = new MatTheme()
                    {
                        Primary = "#212121",
                        Secondary = "#424242",
                        Background = "#616161",
                        Surface = "#424242",
                    };

                    break;
                case AppThemes.light:
                    AppTheme = new MatTheme()
                    {
                        Primary = "#c14559",
                        Secondary = "#cc402c",
                        Background = "#fafafa",
                        Surface = "#ffffff",
                    };

                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Load user
        /// </summary>
        /// <returns></returns>
        private async Task UpdateUserOnLoginAsync()
        {
            bool isAuthenticated = await LoggedUserContext.IsAuthenticatedAsync();

            if (isAuthenticated && !(MainLayoutPageDetails.LoggedUser.Id > 0))
            {
                MainLayoutPageDetails.LoadingMessage = "Loading user details!!";

                var userPrinicipal = await LoggedUserContext.GetCurrentUserDetailsAsync();

                UserDetail user = new UserDetail
                {
                    CreatedBy = 0,
                    CreatedOn = DateTime.Now,
                    Name = userPrinicipal.Identity.Name,
                    Email = userPrinicipal.Claims.FirstOrDefault(i => i.Type == "preferred_username").Value,
                    IsActive = true,
                };

                user.Id = await UserService.GetUserIdAsync(user);
                MainLayoutPageDetails.LoggedUser = user;
                MainLayoutPageDetails.LoadingMessage = "Loaded user!!";
            }
            else if (!isAuthenticated)
            {
                MainLayoutPageDetails.LoggedUser = new UserDetail();
            }

            MainLayoutPageDetails.IsAuthenticated = isAuthenticated;
        }

        /// <summary>
        /// Toggles loader
        /// </summary>
        /// <param name="message"></param>
        private void ToggleLoading(string message = "")
        {
            MainLayoutPageDetails.IsLoading = !MainLayoutPageDetails.IsLoading;
            MainLayoutPageDetails.LoadingMessage = message;
        }
    }
}
