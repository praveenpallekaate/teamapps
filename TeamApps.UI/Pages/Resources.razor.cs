using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Utilities;
using Microsoft.AspNetCore.Components;
using TeamApps.Shared;

namespace TeamApps.UI
{
    /// <summary>
    /// Resource page
    /// </summary>
    public class ResourceBase : ComponentBase
    {
        /// <summary>
        /// Gets or sets layout page model
        /// </summary>
        [CascadingParameter]
        public MainLayoutPage MainLayoutPageDetails { get; set; }

        /// <summary>
        /// Gets or sets page model
        /// </summary>
        protected ResourcePage ResourcePage { get; set; }
            = new ResourcePage();

        /// <summary>
        /// Gets or sets teamapp service
        /// </summary>
        [Inject]
        private ITeamAppsService TeamAppsService { get; set; } = null!;

        /// <summary>
        /// Gets or sets resource service
        /// </summary>
        [Inject]
        private IResourceService ResourceService { get; set; } = null!;

        /// <summary>
        /// Page initialized
        /// </summary>
        /// <returns></returns>
        protected override async Task OnInitializedAsync()
        {
            await LoadFiltersAsync();

            // Sync user
            await MainLayoutPageDetails.CheckUserStatus.InvokeAsync(string.Empty);
        }

        /// <summary>
        /// Toggle modal
        /// </summary>
        /// <param name="type">Modal type</param>
        /// <returns></returns>
        protected async Task ToggleModal(ModalTypes type)
        {
            switch (type)
            {
                case ModalTypes.addresource:
                    ResourcePage.NewResourceTeam = string.Empty;
                    ResourcePage.NewResourceName = string.Empty;
                    ResourcePage.NewResourceEmail = string.Empty;
                    ResourcePage.NewResourceSupervisor = string.Empty;
                    ResourcePage.NewResourceWowId = string.Empty;
                    ResourcePage.AppModalDetails = ModalHelper.ToggleModal(ResourcePage.AppModalDetails, "Add Resource");
                    ResourcePage.ModalType = type;

                    break;
                case ModalTypes.saveresource:
                    ResourcePage.AppModalDetails.IsLoading = true;

                    bool response = await HandleResourceAsync(Modes.add);

                    if (response)
                    {
                        ResourcePage.AppModalDetails = ModalHelper.ToggleModal(ResourcePage.AppModalDetails);
                        ResourcePage.ModalType = type;

                        await LoadFiltersAsync();
                    }
                    else
                    {
                        ResourcePage.AppModalDetails.IsLoading = false;
                    }

                    break;
                case ModalTypes.close:
                    ResourcePage.AppModalDetails = ModalHelper.ToggleModal(ResourcePage.AppModalDetails);
                    ResourcePage.ModalType = type;

                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Fetch resources
        /// </summary>
        /// <returns></returns>
        protected async Task FetchResourcesAsync()
        {
            ToggleLoading("Loading resources!!", true);

            var filter = new ResourceDetail
            {
                Team = ResourcePage.Filter.SelectedTeam,
                Supervisor = ResourcePage.Filter.SelectedSupervisor,
                Year = string.IsNullOrEmpty(ResourcePage.Filter.SelectedYear) ? 0 : int.Parse(ResourcePage.Filter.SelectedYear),
            };

            ResourcePage.GridHeader = new string[]
            {
                "Name", $"Jan {ResourcePage.Filter.SelectedYear}", $"Feb {ResourcePage.Filter.SelectedYear}", $"Mar {ResourcePage.Filter.SelectedYear}",
                $"Apr {ResourcePage.Filter.SelectedYear}", $"May {ResourcePage.Filter.SelectedYear}", $"Jun {ResourcePage.Filter.SelectedYear}",
                $"Jul {ResourcePage.Filter.SelectedYear}", $"Aug {ResourcePage.Filter.SelectedYear}", $"Sep {ResourcePage.Filter.SelectedYear}",
                $"Oct {ResourcePage.Filter.SelectedYear}", $"Nov {ResourcePage.Filter.SelectedYear}", $"Dec {ResourcePage.Filter.SelectedYear}",
            };

            var resources = await ResourceService
                .GetResourcesAsync(filter);

            ResourcePage.Resources = CommonUtils.IsValidCollection(resources) ? resources.ToList() : new List<ResourceDetail>();
            ResourcePage.GridResources = !CommonUtils.IsValidCollection(resources)
                ? Enumerable.Empty<ResourceGrid>()
                : resources.Select(i => ToResourceGrid(i));

            ToggleLoading();
        }

        /// <summary>
        /// Handle resource
        /// </summary>
        /// <param name="mode">Mode</param>
        /// <param name="id">Resource id</param>
        /// <returns></returns>
        protected async Task<bool> HandleResourceAsync(Modes mode, int id = 0)
        {
            bool result = false;

            switch (mode)
            {
                case Modes.add:
                    if (!string.IsNullOrEmpty(ResourcePage.NewResourceTeam) && !string.IsNullOrEmpty(ResourcePage.NewResourceName))
                    {
                        var response = await ResourceService
                            .AddResourceAsync(new ResourceDetail
                            {
                                Team = ResourcePage.NewResourceTeam,
                                Name = ResourcePage.NewResourceName,
                                Email = ResourcePage.NewResourceEmail,
                                Supervisor = ResourcePage.NewResourceSupervisor,
                                WowId = ResourcePage.NewResourceWowId,
                                CreatedBy = MainLayoutPageDetails.LoggedUser.Id,
                                UpdatedBy = MainLayoutPageDetails.LoggedUser.Id,
                                IsActive = true,
                            });

                        if (response > 0)
                        {
                            result = true;
                        }
                    }

                    break;
                case Modes.edit:
                    if (!string.IsNullOrEmpty(ResourcePage.ResourceDetail.Team) && !string.IsNullOrEmpty(ResourcePage.ResourceDetail.Name))
                    {
                        ToggleLoading("Updating resource!!");

                        ResourcePage.ResourceDetail.UpdatedBy = MainLayoutPageDetails.LoggedUser.Id;

                        var response = await ResourceService
                            .UpdateResourceAsync(ResourcePage.ResourceDetail);

                        if (response)
                        {
                            result = true;
                        }

                        ToggleLoading();
                    }

                    break;
                case Modes.read:
                    ToggleLoading("Fetching resource details!!", false, true);

                    ResourcePage.ResourceDetail = await ResourceService.GetResourceByIdAsync(id);

                    ToggleLoading();

                    break;
                default:
                    break;
            }

            return result;
        }

        /// <summary>
        /// Handles row click
        /// </summary>
        /// <param name="resource">Resource row</param>
        /// <returns></returns>
        protected async Task HandleResourceClickAsync(object resource)
        {
            ResourcePage.ViewType = ResourcesViewTypes.ResourceDetails;

            await HandleResourceAsync(Modes.read, ((ResourceGrid)resource).Id);
        }

        /// <summary>
        /// Update resource
        /// </summary>
        /// <param name="resourceDetail">Resource detail</param>
        /// <returns></returns>
        protected async Task UpdateResourceAsync(ResourceDetail resourceDetail)
        {
            resourceDetail.UpdatedBy = MainLayoutPageDetails.LoggedUser.Id;

            var response = await ResourceService
                .UpdateResourceAsync(resourceDetail);

            if (response)
            {
                // Success
            }
        }

        /// <summary>
        /// Load filters
        /// </summary>
        /// <returns></returns>
        private async Task LoadFiltersAsync()
        {
            ToggleLoading("Loading filters!!");

            var filters = await ResourceService
                .GetFiltersAsync();

            if (filters is ResourceDetail)
            {
                await SetFilterAsync(filters);
            }

            ToggleLoading();
        }

        /// <summary>
        /// Set filters
        /// </summary>
        /// <param name="resourceDetail">Resource details</param>
        /// <returns></returns>
        private async Task SetFilterAsync(ResourceDetail resourceDetail)
        {
            List<string> types = new List<string>();
            var teams = await TeamAppsService
                .GetAllTeamAsync();

            types.AddRange(AppConstants.quarterFilters);
            types.AddRange(AppConstants.halfYearlyFilters);

            ResourcePage.Filter.Teams = teams.Select(i => i.Name).ToArray();
            ResourcePage.Filter.Supervisors = resourceDetail.Supervisors;
            ResourcePage.Filter.Years = resourceDetail.Years;
            ResourcePage.Filter.Types = types.ToArray();
        }

        /// <summary>
        /// To grid model
        /// </summary>
        /// <param name="resourceDetail">Resource detail</param>
        /// <returns>Resource grid detail</returns>
        private ResourceGrid ToResourceGrid(ResourceDetail resourceDetail) =>
            new ResourceGrid
            {
                Id = resourceDetail.Id,
                Name = resourceDetail.Name,
                Jan = resourceDetail.Allocations?.FirstOrDefault()?.KeyValues?.FirstOrDefault(i => i.Id == 1)?.Value,
                Feb = resourceDetail.Allocations?.FirstOrDefault()?.KeyValues?.FirstOrDefault(i => i.Id == 2)?.Value,
                Mar = resourceDetail.Allocations?.FirstOrDefault()?.KeyValues?.FirstOrDefault(i => i.Id == 3)?.Value,
                Apr = resourceDetail.Allocations?.FirstOrDefault()?.KeyValues?.FirstOrDefault(i => i.Id == 4)?.Value,
                May = resourceDetail.Allocations?.FirstOrDefault()?.KeyValues?.FirstOrDefault(i => i.Id == 5)?.Value,
                Jun = resourceDetail.Allocations?.FirstOrDefault()?.KeyValues?.FirstOrDefault(i => i.Id == 6)?.Value,
                Jul = resourceDetail.Allocations?.FirstOrDefault()?.KeyValues?.FirstOrDefault(i => i.Id == 7)?.Value,
                Aug = resourceDetail.Allocations?.FirstOrDefault()?.KeyValues?.FirstOrDefault(i => i.Id == 8)?.Value,
                Sep = resourceDetail.Allocations?.FirstOrDefault()?.KeyValues?.FirstOrDefault(i => i.Id == 9)?.Value,
                Oct = resourceDetail.Allocations?.FirstOrDefault()?.KeyValues?.FirstOrDefault(i => i.Id == 10)?.Value,
                Nov = resourceDetail.Allocations?.FirstOrDefault()?.KeyValues?.FirstOrDefault(i => i.Id == 11)?.Value,
                Dec = resourceDetail.Allocations?.FirstOrDefault()?.KeyValues?.FirstOrDefault(i => i.Id == 12)?.Value,
            };

        /// <summary>
        /// Toggles loading
        /// </summary>
        /// <param name="message">Loading message</param>
        /// <param name="isGridLoad">Flag for grid</param>
        /// <param name="isDetailLoad">Flag for detail</param>
        private void ToggleLoading(string message = "", bool isGridLoad = false, bool isDetailLoad = false)
        {
            ResourcePage.LoadingMessage = message;
            ResourcePage.IsLoading = !string.IsNullOrEmpty(message) && !isGridLoad && !isDetailLoad;
            ResourcePage.IsGridLoading = !string.IsNullOrEmpty(message) && isGridLoad && !isDetailLoad;
            ResourcePage.IsDetailLoading = !string.IsNullOrEmpty(message) && !isGridLoad && isDetailLoad;
            ResourcePage.CanEdit = string.IsNullOrEmpty(message) && MainLayoutPageDetails.IsAuthenticated;
        }
    }
}
