using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Utilities;
using Microsoft.AspNetCore.Components;
using TeamApps.Shared;

namespace TeamApps.UI
{
    /// <summary>
    /// Application detail page
    /// </summary>
    public class ApplicationDetailedBase : ComponentBase
    {
        /// <summary>
        /// Gets or sets layout page model
        /// </summary>
        [CascadingParameter]
        public MainLayoutPage MainLayoutPageDetails { get; set; }

        /// <summary>
        /// Gets or sets parameter application id
        /// </summary>
        [Parameter]
        public string AppId { get; set; }

        /// <summary>
        /// Gets or sets page model
        /// </summary>
        protected ApplicationDetailPage ApplicationDetailPageDetails { get; set; }
            = new ApplicationDetailPage();

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
            ToggleLoading("Loading application details!!");

            // Sync user
            await MainLayoutPageDetails.CheckUserStatus.InvokeAsync(string.Empty);

            ApplicationDetailPageDetails.ApplicationDetail = await TeamAppsService
                .GetApplicationDetailsByIdAsync(int.Parse(AppId));

            ApplicationDetailPageDetails.CanEdit = MainLayoutPageDetails.IsAuthenticated;

            if (CommonUtils.IsValidCollection(ApplicationDetailPageDetails.ApplicationDetail.Sections))
            {
                ApplicationDetailPageDetails.ApplicationDetail.Sections = EntityHelper<KeyValue>
                    .AssignIdsIfNotExists(ApplicationDetailPageDetails.ApplicationDetail.Sections);

                BuildSections();
            }

            ToggleLoading();
        }

        /// <summary>
        /// Update application
        /// </summary>
        /// <param name="updateDetails">Section details</param>
        /// <returns></returns>
        protected async Task UpdateApplicationAsync(KeyValue updateDetails)
        {
            if (updateDetails is KeyValue)
            {
                ToggleLoading("Updating application details");

                ApplicationDetailPageDetails.EditedSections = ApplicationDetailPageDetails
                    .EditedSections
                    .Select(i =>
                    {
                        if (i.Section.Id == updateDetails.Id)
                        {
                            i.EditedSection = updateDetails;
                            i.Section = i.EditedSection.Copy();
                        }

                        return i;
                    })
                    .ToList();

                // Update data model
                ApplicationDetailPageDetails.ApplicationDetail.Sections = ApplicationDetailPageDetails
                    .EditedSections
                    .Select(j => j.Section);
                ApplicationDetailPageDetails.ApplicationDetail.UpdatedBy = MainLayoutPageDetails.LoggedUser.Id;

                var updateResponse = await TeamAppsService
                    .UpdateApplicationAsync(ApplicationDetailPageDetails.ApplicationDetail);

                if (updateResponse)
                {
                    // Success
                }

                // Update defaults prop
                var sections = ApplicationDetailPageDetails
                    .ApplicationDetail
                    .Sections
                    .Select(i => new DetailSectionPage
                    {
                        EditedSection = i,
                        IsDisabled = ApplicationDetailPageDetails.CanEdit,
                    })
                    .ToList()
                    .Select(j =>
                    {
                        j.Section = j.EditedSection.Copy();

                        return j;
                    });

                ApplicationDetailPageDetails.Sections = sections.ToList();
                ApplicationDetailPageDetails.EditedSections = sections.ToList();

                ToggleLoading();
            }
        }

        /// <summary>
        /// Build sections
        /// </summary>
        private void BuildSections()
        {
            var sections = ToSectionModel(ApplicationDetailPageDetails.ApplicationDetail.Sections);

            ApplicationDetailPageDetails.Sections = sections.ToList();
            ApplicationDetailPageDetails.EditedSections = sections.ToList();
        }

        /// <summary>
        /// Convert to section model
        /// </summary>
        /// <param name="sections">Sections details</param>
        /// <returns>Sections model collection</returns>
        private IEnumerable<DetailSectionPage> ToSectionModel(IEnumerable<KeyValue> sections) =>
            sections
            .Select(i => new DetailSectionPage
            {
                Section = i.Copy(),
                EditedSection = i.Copy(),
                IsDisabled = !ApplicationDetailPageDetails.CanEdit,
            });

        /// <summary>
        /// Toggles loading
        /// </summary>
        private void ToggleLoading(string message = "")
        {
            ApplicationDetailPageDetails.IsLoading = !ApplicationDetailPageDetails.IsLoading;
            ApplicationDetailPageDetails.CanEdit = string.IsNullOrEmpty(message) && MainLayoutPageDetails.IsAuthenticated;
            ApplicationDetailPageDetails.LoadingMessage = message;
        }
    }
}
