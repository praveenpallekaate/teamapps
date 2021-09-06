using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Core.Utilities;
using TeamApps.Shared;

namespace TeamApps.API
{
    /// <summary>
    /// Resource service
    /// </summary>
    public class ResourceManagement : IResourceManagement
    {
        private readonly ResourceRepository _repository = null;
        private readonly IUserManagement _userManagement = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceManagement"/> class.
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="userManagement"></param>
        public ResourceManagement(
            ResourceRepository repository,
            IUserManagement userManagement)
        {
            _repository = repository;
            _userManagement = userManagement;
        }

        /// <summary>
        /// Fetch resource item
        /// </summary>
        /// <param name="id">Item id</param>
        /// <returns>Resource item</returns>
        public async Task<Resource> GetItemAsync(int id)
        {
            var result = await _repository
                .GetItemAsync(i => i.Id == id);

            return result;
        }

        /// <summary>
        /// Fetch resource details item
        /// </summary>
        /// <param name="id">Item id</param>
        /// <returns>Resource detail item</returns>
        public async Task<ResourceDetail> GetItemDetailsAsync(int id)
        {
            var resource = await GetItemAsync(id);
            var usersDetails = await GetUserDetailsAsync(new List<Resource> { resource });

            return resource is Resource ? ToViewModel(resource, usersDetails) : null;
        }

        /// <summary>
        /// Fetch resource items
        /// </summary>
        /// <param name="ids">Item ids</param>
        /// <returns>Resource items</returns>
        public async Task<IEnumerable<Resource>> GetItemsAsync(int[] ids)
        {
            var result = await _repository
                .GetItemsAsync(i => ids.Contains(i.Id));

            return result;
        }

        /// <summary>
        /// Fetch resource items
        /// </summary>
        /// <param name="predicate">Resource filter</param>
        /// <returns></returns>
        public async Task<IEnumerable<Resource>> GetItemsAsync(Expression<Func<Resource, bool>> predicate)
        {
            var result = await _repository
                .GetItemsAsync(predicate);

            return result;
        }

        /// <summary>
        /// Fetch resource details items
        /// </summary>
        /// <param name="ids">Item ids</param>
        /// <returns>Resource detail items</returns>
        public async Task<IEnumerable<ResourceDetail>> GetItemsDetailsAsync(int[] ids)
        {
            var resources = await GetItemsAsync(ids);
            var usersDetails = await GetUserDetailsAsync(resources);

            return resources?
                .ToList()?
                .Select(i => ToViewModel(i, usersDetails));
        }

        /// <summary>
        /// Fetch resource detail items
        /// </summary>
        /// <param name="predicate">Resource filter</param>
        /// <returns></returns>
        public async Task<IEnumerable<ResourceDetail>> GetItemsDetailsAsync(Expression<Func<Resource, bool>> predicate)
        {
            var resources = await GetItemsAsync(predicate);
            var usersDetails = await GetUserDetailsAsync(resources);

            return resources?
                .ToList()?
                .Select(i => ToViewModel(i, usersDetails));
        }

        /// <summary>
        /// Fetch resource detail items
        /// </summary>
        /// <param name="predicate">Resource filter</param>
        /// <param name="isForList">Flag for list</param>
        /// <returns></returns>
        public async Task<IEnumerable<ResourceDetail>> GetItemsDetailsAsync(Expression<Func<Resource, bool>> predicate, bool isForList = false)
        {
            var resources = await GetItemsAsync(predicate);
            var usersDetails = await GetUserDetailsAsync(resources);

            return resources?
                .ToList()?
                .Select(i => ToViewModel(i, usersDetails, isForList));
        }

        /// <summary>
        /// Fetch resource details for team
        /// </summary>
        /// <param name="resourceDetail">Resource filter details</param>
        /// <returns>Resource details</returns>
        public async Task<IEnumerable<ResourceDetail>> GetResourcesForTeamAsync(ResourceDetail resourceDetail)
        {
            IEnumerable<ResourceDetail> result = Enumerable.Empty<ResourceDetail>();
            bool hasSuperVisor = !string.IsNullOrEmpty(resourceDetail.Supervisor);
            var resources = await GetItemsAsync(i => i.IsActive);

            if (CommonUtils.IsValidCollection(resources))
            {
                // Team filter
                if (!string.IsNullOrEmpty(resourceDetail.Team))
                {
                    resources = resources
                        .Where(j => j.Team == resourceDetail.Team);
                }

                // Supervisor filter
                if (hasSuperVisor && CommonUtils.IsValidCollection(resources))
                {
                    resources = resources
                        .Where(j => j.Supervisor == resourceDetail.Supervisor);
                }

                // Year filter
                if (resourceDetail.Year > 0 && CommonUtils.IsValidCollection(resources))
                {
                    resources = resources
                        .Where(j =>
                        {
                            var yearFilter = j.Allocations?
                                .Where(k => k.Id == resourceDetail.Year)?
                                .ToList();

                            return CommonUtils.IsValidCollection(yearFilter);
                        })
                        .ToList();
                }

                // Format result
                if (CommonUtils.IsValidCollection(resources))
                {
                    if (!resourceDetail.FullDetails)
                    {
                        resources = resources
                            .Select(o =>
                            {
                                if (CommonUtils.IsValidCollection(o.Allocations))
                                {
                                    List<KeyValue> formattedAllocation = new List<KeyValue>();
                                    int[] distinctYears = o.Allocations
                                        .Select(p => p.Id)
                                        .Distinct()
                                        .ToArray();

                                    foreach (var year in distinctYears)
                                    {
                                        KeyValue formattedYear = new KeyValue { Id = year };
                                        List<KeyValue> months = new List<KeyValue>();
                                        KeyValue yearAllocation = o.Allocations
                                            .FirstOrDefault(q => q.Id == year);
                                        int[] distinctMonths = yearAllocation
                                            .KeyValues
                                            .Select(r => r.Id)
                                            .Distinct()
                                            .ToArray();

                                        foreach (var month in distinctMonths)
                                        {
                                            int? sum = yearAllocation
                                                .KeyValues
                                                .Where(s => s.Id == month)?
                                                .Sum(t => !string.IsNullOrEmpty(t.Value) && int.TryParse(t.Value, out int value) ? value : 0)
                                                ?? 0;

                                            sum = sum > 100 ? 100 : sum;

                                            months.Add(new KeyValue
                                            {
                                                Id = month,
                                                Value = sum.ToString(),
                                            });
                                        }

                                        formattedYear.KeyValues = months;
                                        formattedAllocation.Add(formattedYear);
                                    }

                                    o.Allocations = formattedAllocation;
                                }

                                return o;
                            })
                            .ToList();
                    }

                    var usersDetail = await GetUserDetailsAsync(resources);

                    result = resources
                        .Select(u => ToViewModel(u, usersDetail));
                }
            }

            return result;
        }

        /// <summary>
        /// Get filter details
        /// </summary>
        /// <returns>Filter details</returns>
        public async Task<ResourceDetail> GetFiltersAsync()
        {
            ResourceDetail result = new ResourceDetail { Supervisors = new string[] { }, Years = new int[] { } };

            var resources = await GetItemsAsync(i => i.IsActive);

            if (CommonUtils.IsValidCollection(resources))
            {
                List<int> years = new List<int>();

                result.Supervisors = resources.Select(j => j.Name).ToArray();

                foreach (var resource in resources)
                {
                    int[] distinctYears = resource.Allocations?
                        .Select(k => k.Id)
                        .Distinct()
                        .ToArray();

                    if (CommonUtils.IsValidCollection(distinctYears))
                    {
                        years.AddRange(distinctYears);
                    }
                }

                result.Years = CommonUtils.IsValidCollection(years) ? years.Distinct().ToArray() : result.Years;
            }

            return result;
        }

        /// <summary>
        /// Insert item to collection
        /// </summary>
        /// <param name="item">Application item</param>
        /// <returns>Result</returns>
        public async Task<int> InsertItemAsync(ResourceDetail item)
        {
            int result = 0;
            var exists = await CheckIfResourceExistsAsync(item);

            if (!exists)
            {
                DateTime now = DateTime.Now;
                var resource = ToDataModel(item);

                resource.CreatedOn = now;
                resource.UpdatedOn = now;

                resource.Id = await MaxIdAsync();

                var response = await _repository
                    .InsertItemAsync(resource);

                result = response.AsInt32;
            }

            return result;
        }

        /// <summary>
        /// Update item
        /// </summary>
        /// <param name="item">Application details</param>
        /// <returns>Result</returns>
        public async Task<bool> UpdateItemAsync(ResourceDetail item)
        {
            bool result = false;
            DateTime now = DateTime.Now;
            var resourceFilter = await GetItemsAsync(i =>
                    i.Team == item.Team &&
                    i.Name == item.Name);

            if (CommonUtils.IsValidCollection(resourceFilter))
            {
                Resource resource = resourceFilter.FirstOrDefault();

                resource.CreatedBy = item.CreatedBy;
                resource.CreatedOn = item.CreatedOn;
                resource.Allocations = item.Allocations;
                resource.Email = item.Email;
                resource.Id = item.Id;
                resource.IsActive = item.IsActive;
                resource.Name = item.Name;
                resource.Team = item.Team;
                resource.UpdatedBy = item.UpdatedBy;
                resource.UpdatedOn = now;
                resource.Supervisor = item.Supervisor;
                resource.WowId = item.WowId;
                resource.Milestones = item.Milestones;
                resource.StartDate = item.StartDate;

                result = await _repository
                    .UpdateItemAsync(resource);
            }

            return result;
        }

        /// <summary>
        /// Check if resource exists
        /// </summary>
        /// <param name="item">Resource details</param>
        /// <returns>Flag if exists</returns>
        private async Task<bool> CheckIfResourceExistsAsync(ResourceDetail item)
        {
            bool result = false;

            var filter = await GetItemsAsync(i =>
                i.Team == item.Team &&
                i.Name == item.Name &&
                i.IsActive);

            if (CommonUtils.IsValidCollection(filter))
            {
                result = true;
            }
            else
            {
                // Check with case as extensions not supported in expression
                var activeResourcess = await GetItemsAsync(j => j.IsActive);
                var caseFilter = activeResourcess
                    .Where(k =>
                        k.Team.ToTrimmedLower() == item.Team.ToTrimmedLower() &&
                        k.Name.ToTrimmedLower() == item.Name.ToTrimmedLower());

                if (CommonUtils.IsValidCollection(caseFilter))
                {
                    result = true;
                }
            }

            return result;
        }

        /// <summary>
        /// Fetch users from resource
        /// </summary>
        /// <param name="resources">Application items</param>
        /// <returns>User detail collection</returns>
        private async Task<IEnumerable<UserDetail>> GetUserDetailsAsync(IEnumerable<Resource> resources)
        {
            List<int> userIdsToLookup = new List<int>();
            IEnumerable<UserDetail> result = Enumerable.Empty<UserDetail>();

            if (CommonUtils.IsValidCollection(resources))
            {
                foreach (var resource in resources)
                {
                    if (!userIdsToLookup.Contains(resource.CreatedBy))
                    {
                        userIdsToLookup.Add(resource.CreatedBy);
                    }

                    if (!userIdsToLookup.Contains(resource.UpdatedBy))
                    {
                        userIdsToLookup.Add(resource.UpdatedBy);
                    }
                }

                if (CommonUtils.IsValidCollection(userIdsToLookup))
                {
                    result = await _userManagement
                        .GetItemsDetailsAsync(userIdsToLookup.ToArray());
                }
            }

            return result;
        }

        /// <summary>
        /// Fetch max id in collection
        /// </summary>
        /// <returns>Max id</returns>
        private async Task<int> MaxIdAsync()
        {
            int? maxId = await _repository.GetMaxIdAsync();

            return maxId.HasValue ? maxId.Value + 1 : 1;
        }

        /// <summary>
        /// To view model
        /// </summary>
        /// <param name="resource">Resource details</param>
        /// <param name="userDetails">User details</param>
        /// <param name="isForList">Flag for list</param>
        /// <returns></returns>
        private ResourceDetail ToViewModel(Resource resource, IEnumerable<UserDetail> userDetails, bool isForList = false) =>
            new ResourceDetail
            {
                Allocations = resource.Allocations,
                CreatedBy = resource.CreatedBy,
                CreatedOn = resource.CreatedOn,
                CreatedUserDetail = userDetails?.FirstOrDefault(i => i.Id == resource.CreatedBy),
                Email = resource.Email,
                Team = resource.Team,
                Id = resource.Id,
                IsActive = resource.IsActive,
                Name = resource.Name,
                UpdatedBy = resource.UpdatedBy,
                UpdatedOn = resource.UpdatedOn,
                UpdatedUserDetail = userDetails?.FirstOrDefault(i => i.Id == resource.UpdatedBy),
                Supervisor = resource.Supervisor,
                WowId = resource.WowId,
                Milestones = resource.Milestones,
                StartDate = resource.StartDate,
            };

        /// <summary>
        /// To data model
        /// </summary>
        /// <param name="resourceDetail">Resource details</param>
        /// <returns></returns>
        private Resource ToDataModel(ResourceDetail resourceDetail) =>
            new Resource
            {
                Allocations = resourceDetail.Allocations,
                CreatedBy = resourceDetail.CreatedBy,
                CreatedOn = resourceDetail.CreatedOn,
                Email = resourceDetail.Email,
                Team = resourceDetail.Team,
                Id = resourceDetail.Id,
                IsActive = resourceDetail.IsActive,
                Name = resourceDetail.Name,
                UpdatedBy = resourceDetail.UpdatedBy,
                UpdatedOn = resourceDetail.UpdatedOn,
                Supervisor = resourceDetail.Supervisor,
                WowId = resourceDetail.WowId,
                Milestones = resourceDetail.Milestones,
                StartDate = resourceDetail.StartDate,
            };
    }
}
