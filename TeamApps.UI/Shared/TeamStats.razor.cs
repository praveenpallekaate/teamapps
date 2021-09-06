using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Utilities;
using Microsoft.AspNetCore.Components;
using TeamApps.Shared;

namespace TeamApps.UI
{
    /// <summary>
    /// Team stats page
    /// </summary>
    public class TeamStatsBase : ComponentBase
    {
        /// <summary>
        /// Gets or sets param supervisors
        /// </summary>
        [Parameter]
        public string[] Supervisors { get; set; }
            = new string[] { };

        /// <summary>
        /// Gets or sets page model
        /// </summary>
        public TeamStatsPage TeamStatsPage { get; set; }
            = new TeamStatsPage();

        /// <summary>
        /// Gets or sets resource service
        /// </summary>
        [Inject]
        private IResourceService ResourceService { get; set; } = null!;

        /// <summary>
        /// Fetch resources
        /// </summary>
        /// <returns></returns>
        protected async Task FetchResourcesAsync()
        {
            if (!string.IsNullOrEmpty(TeamStatsPage.Supervisor) && !string.IsNullOrEmpty(TeamStatsPage.Year))
            {
                ToggleLoading("Loading resource stats!!");

                var filter = new ResourceDetail
                {
                    FullDetails = true,
                    Team = string.Empty,
                    Supervisor = TeamStatsPage.Supervisor,
                    Year = int.Parse(TeamStatsPage.Year),
                };

                var resources = await ResourceService
                    .GetResourcesAsync(filter);

                TeamStatsPage.Resources = CommonUtils.IsValidCollection(resources) ? resources.ToList() : new List<ResourceDetail>();
                BuildChartData();

                ToggleLoading();
            }
        }

        /// <summary>
        /// Builds chart data
        /// </summary>
        private void BuildChartData()
        {
            if (CommonUtils.IsValidCollection(TeamStatsPage.Resources))
            {
                List<ChartDetail> resourcesStats = new List<ChartDetail>();

                foreach (var resource in TeamStatsPage.Resources)
                {
                    ChartDetail resourceStats = new ChartDetail { Title = resource.Name };

                    KeyValue yearDetails = CommonUtils.IsValidCollection(resource.Allocations)
                        ? resource.Allocations.FirstOrDefault(i => i.Id == int.Parse(TeamStatsPage.Year))
                        : null;

                    if (yearDetails is KeyValue)
                    {
                        string[] projects = yearDetails.KeyValues?.Select(i => i.Name)?.Distinct()?.ToArray() ?? new string[] { };

                        if (CommonUtils.IsValidCollection(projects))
                        {
                            Dictionary<string, int> workTypes = AppConstants
                                .WorkTypes
                                .Select(l => new { Key = l, Value = 0 })
                                .ToDictionary(m => m.Key, m => m.Value);

                            foreach (var project in projects)
                            {
                                IEnumerable<KeyValue> projectKeyValues = yearDetails.KeyValues.Where(j => j.Name == project);

                                if (CommonUtils.IsValidCollection(projectKeyValues))
                                {
                                    bool hasInnovation = false;
                                    bool hasCollaboration = false;
                                    bool hasSafety = false;
                                    bool hasIntegrity = false;

                                    // Set values for work type chart
                                    foreach (var projectKeyValue in projectKeyValues)
                                    {
                                        if (CommonUtils.IsValidCollection(projectKeyValue.KeyValues))
                                        {
                                            if (!hasCollaboration)
                                            {
                                                var filter = projectKeyValue.KeyValues.FirstOrDefault(n => n.Name == AppConstants.WorkTypes[0]);

                                                hasCollaboration = filter is KeyValue;
                                            }

                                            if (!hasInnovation)
                                            {
                                                var filter = projectKeyValue.KeyValues.FirstOrDefault(n => n.Name == AppConstants.WorkTypes[1]);

                                                hasInnovation = filter is KeyValue;
                                            }

                                            if (!hasSafety)
                                            {
                                                var filter = projectKeyValue.KeyValues.FirstOrDefault(n => n.Name == AppConstants.WorkTypes[2]);

                                                hasSafety = filter is KeyValue;
                                            }

                                            if (!hasIntegrity)
                                            {
                                                var filter = projectKeyValue.KeyValues.FirstOrDefault(n => n.Name == AppConstants.WorkTypes[3]);

                                                hasIntegrity = filter is KeyValue;
                                            }
                                        }
                                    }

                                    if (hasInnovation || hasCollaboration || hasSafety || hasIntegrity)
                                    {
                                        workTypes[AppConstants.WorkTypes[0]] = hasCollaboration
                                            ? workTypes[AppConstants.WorkTypes[0]] += 1
                                            : workTypes[AppConstants.WorkTypes[0]];

                                        workTypes[AppConstants.WorkTypes[1]] = hasInnovation
                                            ? workTypes[AppConstants.WorkTypes[1]] += 1
                                            : workTypes[AppConstants.WorkTypes[1]];

                                        workTypes[AppConstants.WorkTypes[2]] = hasSafety
                                            ? workTypes[AppConstants.WorkTypes[2]] += 1
                                            : workTypes[AppConstants.WorkTypes[2]];

                                        workTypes[AppConstants.WorkTypes[3]] = hasIntegrity
                                            ? workTypes[AppConstants.WorkTypes[3]] += 1
                                            : workTypes[AppConstants.WorkTypes[3]];
                                    }
                                }
                            }

                            resourceStats.Labels = AppConstants.WorkTypes;
                            resourceStats.Values = workTypes.Values.ToArray();

                            resourcesStats.Add(resourceStats);
                        }
                    }
                }

                TeamStatsPage.ResourcesStats = resourcesStats;
            }
            else
            {
                TeamStatsPage.ResourcesStats = Enumerable.Empty<ChartDetail>();
            }
        }

        /// <summary>
        /// Toggles loading
        /// </summary>
        /// <param name="message">Loading message</param>
        private void ToggleLoading(string message = "")
        {
            TeamStatsPage.LoadingMessage = message;
            TeamStatsPage.IsLoading = !string.IsNullOrEmpty(message);
        }
    }
}
