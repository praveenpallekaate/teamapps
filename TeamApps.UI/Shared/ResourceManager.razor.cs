using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Utilities;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using TeamApps.Shared;

namespace TeamApps.UI
{
    /// <summary>
    /// Resource manager
    /// </summary>
    public class ResourceManagerBase : ComponentBase
    {
        /// <summary>
        /// Section enum
        /// </summary>
        protected enum Sections
        {
            /// <summary>
            /// Allocation
            /// </summary>
            Allocation,

            /// <summary>
            /// Work breakdown
            /// </summary>
            WorkBreakdown,
        }

        /// <summary>
        /// Gets or sets param detail
        /// </summary>
        [Parameter]
        public ResourceDetail Detail { get; set; }

        /// <summary>
        /// Gets or sets param teams
        /// </summary>
        [Parameter]
        public string[] Teams { get; set; }
            = new string[] { };

        /// <summary>
        /// Gets or sets param supervisors
        /// </summary>
        [Parameter]
        public string[] Supervisors { get; set; }
            = new string[] { };

        /// <summary>
        /// Gets or sets a value indicating whether gets or sets disable flag
        /// </summary>
        [Parameter]
        public bool Disabled { get; set; } = false;

        /// <summary>
        /// Gets or sets update callback
        /// </summary>
        [Parameter]
        public EventCallback<ResourceDetail> OnUpdate { get; set; }

        /// <summary>
        /// Gets or sets jsruntime
        /// </summary>
        [Inject]
        public IJSRuntime JSRuntime { get; set; }

        /// <summary>
        /// Gets or sets page model
        /// </summary>
        public ResourceManagerPage ResourceManagerPage { get; set; }
            = new ResourceManagerPage();

        /// <summary>
        /// On init
        /// </summary>
        protected override void OnInitialized()
        {
            if (Detail is ResourceDetail)
            {
                ResourceManagerPage.ResourceDetail = Detail;
                ResourceManagerPage.EditedDetail = Detail.Copy();
                ResourceManagerPage.EditedDetail.Allocations = CommonUtils.IsValidCollection(Detail.Allocations)
                    ? Detail.Allocations.ToList() : Enumerable.Empty<KeyValue>();
                ResourceManagerPage.ResourceMilestones = GetMilestones();
            }

            ResourceManagerPage.CanEdit = !Disabled;
        }

        /// <summary>
        /// Toggles mode
        /// </summary>
        /// <param name="mode">Mode type</param>
        protected void ToggleMode(Modes mode)
        {
            switch (mode)
            {
                case Modes.add:
                    ResourceManagerPage.Mode = mode;

                    break;
                case Modes.edit:
                    ResourceManagerPage.Mode = mode;

                    break;
                case Modes.read:
                    ResourceManagerPage.EditedDetail = ResourceManagerPage.ResourceDetail.Copy();
                    ResourceManagerPage.EditedDetail.Allocations = CommonUtils.IsValidCollection(ResourceManagerPage.ResourceDetail.Allocations)
                        ? ResourceManagerPage.ResourceDetail.Allocations.ToList() : Enumerable.Empty<KeyValue>();
                    ResourceManagerPage.Mode = mode;

                    break;
                case Modes.prompt:
                    ResourceManagerPage.Mode = mode;

                    break;
                default:
                    break;
            }

            ResetDetails();
        }

        /// <summary>
        /// Toggles add allocation
        /// </summary>
        /// <param name="modalType">Modal type</param>
        protected void ToggleAddAllocation(ModalTypes modalType)
        {
            switch (modalType)
            {
                case ModalTypes.addallocation:
                    ResourceManagerPage.NewAllocation = new Allocation();
                    ResourceManagerPage.AppModalDetails = ModalHelper.ToggleModal(ResourceManagerPage.AppModalDetails, "Add Allocation");
                    ResourceManagerPage.ModalType = modalType;

                    break;
                case ModalTypes.saveallocation:
                    ResourceManagerPage.AppModalDetails.IsLoading = true;
                    AddAllocation();
                    ResourceManagerPage.AppModalDetails = ModalHelper.ToggleModal(ResourceManagerPage.AppModalDetails);
                    ResourceManagerPage.ModalType = ModalTypes.close;

                    break;
                case ModalTypes.close:
                    ResourceManagerPage.AppModalDetails = ModalHelper.ToggleModal(ResourceManagerPage.AppModalDetails);
                    ResourceManagerPage.ModalType = modalType;

                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Add allocation
        /// </summary>
        protected void AddAllocation()
        {
            if (IsValidAllocation())
            {
                ToggleLoading("Adding allocation");

                bool hasYear = false;
                bool hasAllocations = CommonUtils.IsValidCollection(ResourceManagerPage.EditedDetail.Allocations);
                int year = int.Parse(ResourceManagerPage.NewAllocation.Year);
                int month = Array.IndexOf(AppConstants.Months, ResourceManagerPage.NewAllocation.Month) + 1;

                if (hasAllocations)
                {
                    var yearFilter = ResourceManagerPage
                        .EditedDetail
                        .Allocations
                        .FirstOrDefault(i => i.Id == year);

                    hasYear = yearFilter is KeyValue;
                }

                if (hasYear)
                {
                    ResourceManagerPage.EditedDetail.Allocations = ResourceManagerPage
                        .EditedDetail
                        .Allocations
                        .Select(j =>
                        {
                            if (j.Id == year)
                            {
                                List<KeyValue> allocations = CommonUtils.IsValidCollection(j.KeyValues)
                                    ? j.KeyValues.ToList() : new List<KeyValue>();

                                allocations.Add(new KeyValue
                                {
                                    Id = month,
                                    Name = ResourceManagerPage.NewAllocation.Project,
                                    Description = ResourceManagerPage.NewAllocation.Demand,
                                    Value = ResourceManagerPage.NewAllocation.Value,
                                    KeyValues = ResourceManagerPage.NewAllocation.WorkTypes,
                                });

                                j.KeyValues = allocations;
                            }

                            return j;
                        })
                        .ToList();
                }
                else if (hasAllocations)
                {
                    KeyValue yearAllocation = new KeyValue
                    {
                        Id = year,
                        KeyValues = new List<KeyValue>
                        {
                            new KeyValue
                            {
                                Id = month,
                                Name = ResourceManagerPage.NewAllocation.Project,
                                Description = ResourceManagerPage.NewAllocation.Demand,
                                Value = ResourceManagerPage.NewAllocation.Value,
                                KeyValues = ResourceManagerPage.NewAllocation.WorkTypes,
                            },
                        },
                    };

                    List<KeyValue> allocations = ResourceManagerPage
                        .EditedDetail
                        .Allocations
                        .ToList();

                    allocations.Add(yearAllocation);

                    ResourceManagerPage.EditedDetail.Allocations = allocations.OrderBy(k => k.Id);
                }
                else
                {
                    List<KeyValue> allocations = new List<KeyValue>
                    {
                        new KeyValue
                        {
                            Id = year,
                            KeyValues = new List<KeyValue>
                            {
                                new KeyValue
                                {
                                    Id = month,
                                    Name = ResourceManagerPage.NewAllocation.Project,
                                    Description = ResourceManagerPage.NewAllocation.Demand,
                                    Value = ResourceManagerPage.NewAllocation.Value,
                                    KeyValues = ResourceManagerPage.NewAllocation.WorkTypes,
                                },
                            },
                        },
                    };

                    ResourceManagerPage.EditedDetail.Allocations = allocations;
                }

                ToggleLoading();
            }
        }

        /// <summary>
        /// Remove resource
        /// </summary>
        /// <returns></returns>
        protected async Task RemoveResourceAsync()
        {
            ResourceManagerPage.EditedDetail.IsActive = false;

            await UpdateResourceAsync();
        }

        /// <summary>
        /// Update resource
        /// </summary>
        /// <returns></returns>
        protected async Task UpdateResourceAsync()
        {
            if (
                !string.IsNullOrEmpty(ResourceManagerPage.EditedDetail.Name) &&
                !string.IsNullOrEmpty(ResourceManagerPage.EditedDetail.Team))
            {
                ToggleLoading("Updating Resource!!");

                ResourceManagerPage.ResourceDetail = ResourceManagerPage.EditedDetail.Copy();

                // Sort allocations
                if (CommonUtils.IsValidCollection(ResourceManagerPage.EditedDetail.Allocations))
                {
                    ResourceManagerPage.EditedDetail.Allocations = ResourceManagerPage
                        .EditedDetail
                        .Allocations
                        .OrderBy(i => i.Id)
                        .Select(j =>
                        {
                            j.KeyValues = CommonUtils.IsValidCollection(j.KeyValues)
                                ? j.KeyValues.OrderBy(k => k.Id).ToList()
                                : Enumerable.Empty<KeyValue>();

                            return j;
                        })
                        .ToList();

                    ResourceManagerPage.ResourceDetail.Allocations = CommonUtils.IsValidCollection(ResourceManagerPage.EditedDetail.Allocations)
                        ? ResourceManagerPage.EditedDetail.Allocations.ToList() : Enumerable.Empty<KeyValue>();
                    ResourceManagerPage.ResourceDetail.Milestones = CommonUtils.IsValidCollection(ResourceManagerPage.EditedDetail.Milestones)
                        ? ResourceManagerPage.EditedDetail.Milestones.ToList() : Enumerable.Empty<KeyValue>();
                }

                await OnUpdate.InvokeAsync(ResourceManagerPage.ResourceDetail);

                ResetDetails();
                ToggleLoading();
                ToggleMode(Modes.read);
            }
        }

        /// <summary>
        /// On year change
        /// </summary>
        /// <param name="section">Section type</param>
        /// <param name="mode">Mode</param>
        /// <param name="year">Year</param>
        protected void SelectYear(Sections section, Modes mode, string year)
        {
            if (!string.IsNullOrEmpty(year))
            {
                KeyValue yearDetails = CommonUtils.IsValidCollection(ResourceManagerPage.EditedDetail.Allocations)
                    ? ResourceManagerPage.EditedDetail.Allocations.FirstOrDefault(i => i.Id == int.Parse(year))
                    : null;

                if (yearDetails is KeyValue)
                {
                    ToggleLoading($"Loading details for {year}!!", section == Sections.Allocation, section == Sections.WorkBreakdown);

                    switch (section)
                    {
                        case Sections.Allocation:
                            switch (mode)
                            {
                                case Modes.edit:
                                    ResourceManagerPage.AllocationDetail.Allocations = yearDetails
                                        .KeyValues?
                                        .Select((j, index) =>
                                        {
                                            var model = ToAllocationDetail(j);
                                            model.AllocationId = index + 1;

                                            return model;
                                        }) ??
                                        Enumerable.Empty<AllocationDetail>();

                                    break;
                                case Modes.read:
                                    ResourceManagerPage.AllocationDetail = BuildData(ResourceManagerPage.AllocationDetail, section, yearDetails);

                                    break;
                                default:
                                    break;
                            }

                            break;
                        case Sections.WorkBreakdown:
                            ResourceManagerPage.WorkBreakdownDetail = BuildData(ResourceManagerPage.WorkBreakdownDetail, section, yearDetails);

                            break;
                        default:
                            break;
                    }

                    ToggleLoading();
                }
            }

            switch (section)
            {
                case Sections.Allocation:
                    ResourceManagerPage.AllocationDetail.Year = year;

                    break;
                case Sections.WorkBreakdown:
                    ResourceManagerPage.WorkBreakdownDetail.Year = year;

                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Work type update
        /// </summary>
        /// <param name="allocationId">Allocation id</param>
        /// <param name="value">Work type</param>
        /// <param name="flag">Flag</param>
        protected void UpdateWorkType(int allocationId, string value, bool? flag)
        {
            ResourceManagerPage.AllocationDetail.Allocations = ResourceManagerPage
                .AllocationDetail
                .Allocations
                .Select(i =>
                {
                    if (i.AllocationId == allocationId)
                    {
                        List<KeyValue> workTypes = CommonUtils.IsValidCollection(i.KeyValues)
                            ? i.KeyValues.ToList() : new List<KeyValue>();
                        bool toAdd = flag.HasValue ? flag.Value : false;

                        if (toAdd)
                        {
                            var hasFilter = workTypes.FirstOrDefault(j => j.Name == value);

                            if (hasFilter == null)
                            {
                                workTypes.Add(new KeyValue { Name = value });
                            }
                        }
                        else
                        {
                            workTypes = workTypes
                                .Where(j => j.Name != value)
                                .ToList();
                        }

                        i.KeyValues = workTypes;
                    }

                    return i;
                })
                .ToList();

            // Update edited detail
            ResourceManagerPage.EditedDetail.Allocations = ResourceManagerPage
                .EditedDetail
                .Allocations
                .Select(k =>
                {
                    if (k.Id == int.Parse(ResourceManagerPage.AllocationDetail.Year))
                    {
                        k.KeyValues = ResourceManagerPage
                            .AllocationDetail
                            .Allocations
                            .Select(l => ToEditedDetail(l))
                            .ToList();
                    }

                    return k;
                });
        }

        /// <summary>
        /// Work type update for new allocation
        /// </summary>
        /// <param name="value">Work type</param>
        /// <param name="flag">Flag</param>
        protected void UpdateNewAllocationWorkType(string value, bool? flag)
        {
            bool toAdd = flag.HasValue ? flag.Value : false;

            if (toAdd)
            {
                var filter = ResourceManagerPage
                    .NewAllocation
                    .WorkTypes
                    .FirstOrDefault(i => i.Name == value);

                if (filter == null)
                {
                    ResourceManagerPage.NewAllocation.WorkTypes.Add(new KeyValue { Name = value });
                }
            }
            else
            {
                ResourceManagerPage.NewAllocation.WorkTypes = ResourceManagerPage
                    .NewAllocation
                    .WorkTypes
                    .Where(j => j.Name != value)
                    .ToList();
            }
        }

        /// <summary>
        /// Open url in new tab
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        protected async Task OpenUrlAsync(string url)
        {
            if (!string.IsNullOrEmpty(url))
            {
                await JSRuntime.InvokeAsync<object>("open", url, "_blank");
            }
        }

        /// <summary>
        /// Builds milestones
        /// </summary>
        /// <returns>List of milestones</returns>
        protected List<KeyValue> GetMilestones()
        {
            List<KeyValue> result = CommonUtils.IsValidCollection(ResourceManagerPage.EditedDetail.Milestones)
                ? ResourceManagerPage.EditedDetail.Milestones.ToList() : new List<KeyValue>();

            if (ResourceManagerPage.EditedDetail.StartDate.HasValue)
            {
                List<KeyValue> anniversaries = new List<KeyValue>();

                foreach (int duration in AppConstants.AnniversaryYears)
                {
                    anniversaries.Add(new KeyValue
                    {
                        Name = "Service Anniversary",
                        Description = $"Completed {duration} Year(s)",
                        Value = ResourceManagerPage.EditedDetail.StartDate.Value.AddYears(duration).ToShortDateString(),
                    });
                }

                List<KeyValue> filtered = anniversaries
                    .Where(j => DateTime.Parse(j.Value) < DateTime.Now)
                    .ToList();

                result.AddRange(filtered);
            }

            result = result
                .OrderByDescending(i => DateTime.Parse(string.IsNullOrEmpty(i.Value) ? DateTime.MaxValue.ToString() : i.Value))
                .ToList();

            return result;
        }

        /// <summary>
        /// Add milestone
        /// </summary>
        protected void AddMilestone()
        {
            if (
                !string.IsNullOrEmpty(ResourceManagerPage.NewMilestone.Name) &&
                !string.IsNullOrEmpty(ResourceManagerPage.NewMilestone.Description) &&
                !string.IsNullOrEmpty(ResourceManagerPage.NewMilestone.Value))
            {
                var milestones = ResourceManagerPage.EditedDetail.Milestones.ToList();

                milestones.Add(new KeyValue
                {
                    Name = ResourceManagerPage.NewMilestone.Name,
                    Description = ResourceManagerPage.NewMilestone.Description,
                    Value = ResourceManagerPage.NewMilestone.Value,
                });

                ResourceManagerPage.EditedDetail.Milestones = milestones;
                ResourceManagerPage.ResourceMilestones = GetMilestones();
            }
        }

        /// <summary>
        /// Build chart data
        /// </summary>
        /// <param name="sectionDetail">Section details</param>
        /// <param name="section">Section type</param>
        /// <param name="yearDetails">Year details</param>
        /// <returns>Section detail</returns>
        private SectionDetail BuildData(SectionDetail sectionDetail, Sections section, KeyValue yearDetails)
        {
            SectionDetail result = sectionDetail;

            if (yearDetails is KeyValue)
            {
                switch (section)
                {
                    case Sections.Allocation:
                        List<ChartDetail> monthly = new List<ChartDetail>();
                        int[] months = yearDetails.KeyValues.Select(i => i.Id).Distinct().ToArray();

                        if (CommonUtils.IsValidCollection(months))
                        {
                            foreach (var month in months)
                            {
                                int val = yearDetails.KeyValues?.Where(j => j.Id == month)?.Sum(k => int.Parse(k.Value)) ?? 0;
                                val = val > 100 ? 100 : val;

                                string monthName = AppConstants.Months[month - 1];
                                string[] labels = new string[]
                                {
                                    string.Join(
                                        ", ",
                                        yearDetails
                                            .KeyValues?
                                            .Where(j => j.Id == month)?
                                            .Select(k => $"{k.Name} - {k.Value}") ??
                                        new string[] { }),
                                    "Unallocated",
                                };
                                int[] values = new int[] { val, 100 - val };

                                monthly.Add(new ChartDetail
                                {
                                    Title = AppConstants.Months[month - 1],
                                    Labels = labels,
                                    Values = values,
                                });
                            }
                        }

                        sectionDetail.Monthly = monthly;

                        break;
                    case Sections.WorkBreakdown:
                        string[] projects = yearDetails.KeyValues?.Select(i => i.Name)?.Distinct()?.ToArray() ?? new string[] { };

                        if (CommonUtils.IsValidCollection(projects))
                        {
                            List<string> projectLabels = new List<string>();
                            List<int> projectValues = new List<int>();
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

                                    // Set values for chart
                                    int val = projectKeyValues
                                        .Where(j => j.Name == project)?
                                        .Sum(k => int.Parse(k.Value)) ?? 0;
                                    val = val > 100 ? 100 : val;

                                    projectLabels.Add(project);
                                    projectValues.Add(val);

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

                            sectionDetail.Overall = new ChartDetail
                            {
                                Title = "Overall",
                                Labels = projectLabels.ToArray(),
                                Values = projectValues.ToArray(),
                            };

                            sectionDetail.OverallType = new ChartDetail
                            {
                                Title = "Work Type",
                                Labels = AppConstants.WorkTypes,
                                Values = workTypes.Values.ToArray(),
                            };
                        }

                        break;
                    default:
                        break;
                }
            }

            result = sectionDetail;

            return result;
        }

        /// <summary>
        /// Validate allocation
        /// </summary>
        /// <returns>Flag if valid</returns>
        private bool IsValidAllocation() =>
            !string.IsNullOrEmpty(ResourceManagerPage.NewAllocation.Year) &&
            !string.IsNullOrEmpty(ResourceManagerPage.NewAllocation.Month) &&
            !string.IsNullOrEmpty(ResourceManagerPage.NewAllocation.Project) &&
            !string.IsNullOrEmpty(ResourceManagerPage.NewAllocation.Value);

        /// <summary>
        /// Toggle loading
        /// </summary>
        /// <param name="message">Loading message</param>
        private void ToggleLoading(string message = "", bool isAllocationLoading = false, bool isWorkBreakdownLoading = false)
        {
            ResourceManagerPage.LoadingMessage = message;
            ResourceManagerPage.IsLoading = !string.IsNullOrEmpty(message) && !isAllocationLoading && !isWorkBreakdownLoading;
            ResourceManagerPage.AllocationDetail.IsLoading = !string.IsNullOrEmpty(message) && isAllocationLoading && !isWorkBreakdownLoading;
            ResourceManagerPage.AllocationDetail.LoadingMessage = message;
            ResourceManagerPage.WorkBreakdownDetail.IsLoading = !string.IsNullOrEmpty(message) && !isAllocationLoading && isWorkBreakdownLoading;
            ResourceManagerPage.WorkBreakdownDetail.LoadingMessage = message;
            ResourceManagerPage.CanEdit = string.IsNullOrEmpty(message) && !isAllocationLoading && !isWorkBreakdownLoading;
        }

        /// <summary>
        /// To view detail
        /// </summary>
        /// <param name="keyValue">Keyvalue</param>
        /// <returns>Allocation detail</returns>
        private AllocationDetail ToAllocationDetail(KeyValue keyValue) =>
            new AllocationDetail
            {
                Description = keyValue.Description,
                Flag = keyValue.Flag,
                Id = keyValue.Id,
                IsActive = keyValue.IsActive,
                KeyValues = keyValue.KeyValues,
                Name = keyValue.Name,
                SubType = keyValue.SubType,
                Type = keyValue.Type,
                Value = keyValue.Value,
                Values = keyValue.Values,
            };

        /// <summary>
        /// To edited detail
        /// </summary>
        /// <param name="keyValue">Keyvalue</param>
        /// <returns>Edited detail</returns>
        private KeyValue ToEditedDetail(AllocationDetail keyValue) =>
            new KeyValue
            {
                Description = keyValue.Description,
                Flag = keyValue.Flag,
                Id = keyValue.Id,
                IsActive = keyValue.IsActive,
                KeyValues = keyValue.KeyValues,
                Name = keyValue.Name,
                SubType = keyValue.SubType,
                Type = keyValue.Type,
                Value = keyValue.Value,
                Values = keyValue.Values,
            };

        /// <summary>
        /// Reset details
        /// </summary>
        private void ResetDetails()
        {
            ResourceManagerPage.AllocationDetail = new SectionDetail();
            ResourceManagerPage.WorkBreakdownDetail = new SectionDetail();
        }
    }
}
