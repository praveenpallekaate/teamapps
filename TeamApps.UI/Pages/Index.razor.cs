using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Utilities;
using Microsoft.AspNetCore.Components;
using TeamApps.Shared;

namespace TeamApps.UI
{
    /// <summary>
    /// Index page
    /// </summary>
    public class IndexBase : ComponentBase
    {
        /// <summary>
        /// Gets or sets layout page model
        /// </summary>
        [CascadingParameter]
        public MainLayoutPage MainLayoutPageDetails { get; set; }

        /// <summary>
        /// Gets or sets page model
        /// </summary>
        protected IndexPage IndexPageDetails { get; set; }
            = new IndexPage();

        /// <summary>
        /// Gets or sets teamapp service
        /// </summary>
        [Inject]
        private ITeamAppsService TeamAppsService { get; set; } = null!;

        /// <summary>
        /// Page initialized
        /// </summary>
        /// <returns></returns>
        protected async override Task OnInitializedAsync()
        {
            // Sync user
            await MainLayoutPageDetails.CheckUserStatus.InvokeAsync(string.Empty);

            ToggleLoading("Loading team details!!");

            // Load team details
            await LoadTeamsDetailsAsync();

            IndexPageDetails.CanEdit = MainLayoutPageDetails.IsAuthenticated;

            if (IndexPageDetails.CanEdit)
            {
                await LoadTeamsAsync();
            }

            ToggleLoading();
        }

        /// <summary>
        /// Handle add
        /// </summary>
        /// <param name="modalType">Modal type</param>
        /// <returns></returns>
        protected async Task HandleAddAsync(ModalTypes modalType)
        {
            switch (modalType)
            {
                case ModalTypes.addteam:
                    IndexPageDetails.AppModalDetails = ModalHelper.ToggleModal(IndexPageDetails.AppModalDetails, "Add Team");

                    break;
                case ModalTypes.saveteam:
                    if (
                        string.IsNullOrEmpty(IndexPageDetails.NewTeamName) ||
                        IndexPageDetails.NewTeamName.Length < 3 ||
                        IndexPageDetails.ExistingTeams.Select(i => i.ToTrimmedLower()).Contains(IndexPageDetails.NewTeamName.ToTrimmedLower()))
                    {
                        IndexPageDetails.NewTeamNameInvalid = true;
                        return;
                    }

                    IndexPageDetails.AppModalDetails.IsLoading = true;

                    bool response = await TeamAppsService
                        .AddTeamAsync(new ApplicationDetail
                        {
                            Team = IndexPageDetails.NewTeamName.Trim(),
                            UpdatedBy = MainLayoutPageDetails.LoggedUser.Id,
                        });

                    if (response)
                    {
                        IndexPageDetails.AppModalDetails = ModalHelper.ToggleModal(IndexPageDetails.AppModalDetails);
                        IndexPageDetails.NewTeamName = string.Empty;

                        ToggleLoading("Loading team details!!");

                        // Load team details
                        await LoadTeamsDetailsAsync();

                        ToggleLoading();
                    }

                    break;
                case ModalTypes.addapplication:
                    IndexPageDetails.AppModalDetails = ModalHelper.ToggleModal(IndexPageDetails.AppModalDetails, "Add Application");

                    break;
                case ModalTypes.saveapplication:
                    bool isValid = true;

                    if (
                        string.IsNullOrEmpty(IndexPageDetails.NewApplication.Team) ||
                        IndexPageDetails.NewApplication.Team.Length < 3)
                    {
                        IndexPageDetails.NewTeamNameInvalid = true;
                        isValid = false;
                    }

                    if (
                        string.IsNullOrEmpty(IndexPageDetails.NewApplication.Name) ||
                        IndexPageDetails.NewApplication.Name.Length < 3 ||
                        IndexPageDetails.ExistingTeams.Select(i => i.ToTrimmedLower()).Contains(IndexPageDetails.NewApplication.Name.ToTrimmedLower()))
                    {
                        IndexPageDetails.NewApplicationNameInvalid = true;
                        isValid = false;
                    }

                    if (!isValid)
                    {
                        return;
                    }

                    IndexPageDetails.AppModalDetails.IsLoading = true;

                    int addApplicationResponse = await TeamAppsService
                        .AddApplicationToTeamAsync(new ApplicationDetail
                        {
                            IsActive = true,
                            Name = IndexPageDetails.NewApplication.Name.Trim(),
                            Team = IndexPageDetails.NewApplication.Team.Trim(),
                            Tags = CommonUtils.IsValidCollection(IndexPageDetails.NewApplication.Tags)
                                ? IndexPageDetails.NewApplication.Tags : new string[] { },
                            CreatedBy = MainLayoutPageDetails.LoggedUser.Id,
                            UpdatedBy = MainLayoutPageDetails.LoggedUser.Id,
                        });

                    if (addApplicationResponse > 0)
                    {
                        IndexPageDetails.AppModalDetails = ModalHelper.ToggleModal(IndexPageDetails.AppModalDetails);
                        IndexPageDetails.NewTeamName = string.Empty;
                        IndexPageDetails.NewTag = string.Empty;
                        IndexPageDetails.NewApplication = new Application();

                        ToggleLoading("Loading team details!!");

                        // Load team details
                        await LoadTeamsDetailsAsync();

                        ToggleLoading();
                    }

                    break;
                case ModalTypes.close:
                    IndexPageDetails.AppModalDetails = ModalHelper.ToggleModal(IndexPageDetails.AppModalDetails);

                    break;
                default:
                    break;
            }

            IndexPageDetails.ModalType = modalType;
        }

        /// <summary>
        /// Handle tag add
        /// </summary>
        protected void HandleTagAdd()
        {
            List<string> tags = CommonUtils.IsValidCollection(IndexPageDetails.NewApplication.Tags)
                ? IndexPageDetails.NewApplication.Tags?.ToList() : new List<string>();

            if (!tags.Contains(IndexPageDetails.NewTag))
            {
                tags.Add(IndexPageDetails.NewTag);

                IndexPageDetails.NewApplication.Tags = tags.ToArray();
                IndexPageDetails.NewTag = string.Empty;
            }
        }

        /// <summary>
        /// Load teams details
        /// </summary>
        /// <returns></returns>
        private async Task LoadTeamsDetailsAsync()
        {
            var teamAppsDetails = await TeamAppsService
                .GetTeamDetailsAsync(0, 10, MainLayoutPageDetails.LoggedUser.Id);

            if (CommonUtils.IsValidCollection(teamAppsDetails))
            {
                IndexPageDetails.TeamsDetails = teamAppsDetails;
            }

            if (IndexPageDetails.CanEdit)
            {
                await LoadTeamsAsync();
            }
        }

        /// <summary>
        /// Load teams list
        /// </summary>
        /// <returns></returns>
        private async Task LoadTeamsAsync()
        {
            var teams = await TeamAppsService.GetAllTeamAsync();

            if (CommonUtils.IsValidCollection(teams))
            {
                IndexPageDetails.ExistingTeams = teams
                    .Select(i => i.Name)
                    .ToArray();
            }
        }

        /// <summary>
        /// Toggles loading
        /// </summary>
        private void ToggleLoading(string message = "")
        {
            IndexPageDetails.IsLoading = !string.IsNullOrEmpty(message);
            IndexPageDetails.LoadingMessage = message;
            IndexPageDetails.CanEdit = string.IsNullOrEmpty(message) && MainLayoutPageDetails.IsAuthenticated;
        }
    }
}
