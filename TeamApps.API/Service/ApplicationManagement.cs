using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Core.Utilities;
using Microsoft.Extensions.Options;
using TeamApps.Shared;

namespace TeamApps.API
{
    /// <summary>
    /// Application service
    /// </summary>
    public class ApplicationManagement : IApplicationManagement
    {
        private const string DEFAULTTEAMNAME = "Corporate Systems";

        private readonly IOptions<AppSettings> _options = null;
        private readonly ApplicationRepository _repository = null;
        private readonly IUserManagement _userManagement = null;
        private readonly IAppLookupManagement _appLookupManagement = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationManagement"/> class.
        /// ctor
        /// </summary>
        /// <param name="options"></param>
        /// <param name="repository"></param>
        /// <param name="userManagement"></param>
        /// <param name="appLookupManagement"></param>
        public ApplicationManagement(
            IOptions<AppSettings> options,
            ApplicationRepository repository,
            IUserManagement userManagement,
            IAppLookupManagement appLookupManagement)
        {
            _options = options;
            _repository = repository;
            _userManagement = userManagement;
            _appLookupManagement = appLookupManagement;
        }

        /// <summary>
        /// Fetch application item
        /// </summary>
        /// <param name="id">Item id</param>
        /// <returns>Application item</returns>
        public async Task<Application> GetItemAsync(int id)
        {
            var result = await _repository
                .GetItemAsync(i => i.Id == id);

            return result;
        }

        /// <summary>
        /// Fetch application details item
        /// </summary>
        /// <param name="id">Item id</param>
        /// <returns>Application detail item</returns>
        public async Task<ApplicationDetail> GetItemDetailsAsync(int id)
        {
            var application = await GetItemAsync(id);
            var usersDetails = await GetUserDetailsAsync(new List<Application> { application });

            return application is Application ? ToViewModel(application, usersDetails) : null;
        }

        /// <summary>
        /// Fetch application items
        /// </summary>
        /// <param name="ids">Item ids</param>
        /// <returns>Application items</returns>
        public async Task<IEnumerable<Application>> GetItemsAsync(int[] ids)
        {
            var result = await _repository
                .GetItemsAsync(i => ids.Contains(i.Id));

            return result;
        }

        /// <summary>
        /// Fetch application items
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Application>> GetItemsAsync(Expression<Func<Application, bool>> predicate)
        {
            var result = await _repository
                .GetItemsAsync(predicate);

            return result;
        }

        /// <summary>
        /// Fetch application details items
        /// </summary>
        /// <param name="ids">Item ids</param>
        /// <returns>Application detail items</returns>
        public async Task<IEnumerable<ApplicationDetail>> GetItemsDetailsAsync(int[] ids)
        {
            var applications = await GetItemsAsync(ids);
            var usersDetails = await GetUserDetailsAsync(applications);

            return applications?
                .ToList()?
                .Select(i => ToViewModel(i, usersDetails));
        }

        /// <summary>
        /// Fetch application detail items
        /// </summary>
        /// <param name="predicate">Application filter</param>
        /// <returns></returns>
        public async Task<IEnumerable<ApplicationDetail>> GetItemsDetailsAsync(Expression<Func<Application, bool>> predicate)
        {
            var applications = await GetItemsAsync(predicate);
            var usersDetails = await GetUserDetailsAsync(applications);

            return applications?
                .ToList()?
                .Select(i => ToViewModel(i, usersDetails));
        }

        /// <summary>
        /// Fetch application detail items
        /// </summary>
        /// <param name="predicate">Application filter</param>
        /// <param name="isForList">Flag for list</param>
        /// <returns></returns>
        public async Task<IEnumerable<ApplicationDetail>> GetItemsDetailsAsync(Expression<Func<Application, bool>> predicate, bool isForList = false)
        {
            var applications = await GetItemsAsync(predicate);
            var usersDetails = await GetUserDetailsAsync(applications);

            return applications?
                .ToList()?
                .Select(i => ToViewModel(i, usersDetails, isForList));
        }

        /// <summary>
        /// Fetch applications for teams
        /// </summary>
        /// <param name="skip">Items to skip</param>
        /// <param name="limit">Max number of items</param>
        /// <param name="applicationDetail">Application detail</param>
        /// <returns>Dictonary of team details</returns>
        public async Task<IEnumerable<TeamDetail>> GetTeamDetailsAsync(int skip = 0, int limit = 9999999, ApplicationDetail applicationDetail = null)
        {
            List<TeamDetail> result = new List<TeamDetail>();

            var teamsLookup = await _appLookupManagement
                .GetItemAsync(i => i.Type == AppLookupTypes.team.ToString());

            if (teamsLookup is AppLookup)
            {
                var teams = teamsLookup
                    .KeyValues
                    .Skip(skip)
                    .Take(limit);

                if (CommonUtils.IsValidCollection(teams))
                {
                    foreach (var team in teams)
                    {
                        var foundApplications = await GetItemsDetailsAsync(j => j.Team == team.Name && j.IsActive, true);

                        // Clear detail data for list
                        foundApplications = foundApplications
                            .Select(k =>
                            {
                                bool hasSections = CommonUtils.IsValidCollection(k.Sections);

                                // Tile summary
                                if (hasSections)
                                {
                                    // Has active work
                                    var primaryFilter = k.Sections.FirstOrDefault(l => l.Name == AppConstants.PrimaryDetails);

                                    if (
                                        primaryFilter is KeyValue &&
                                        CommonUtils.IsValidCollection(primaryFilter.KeyValues))
                                    {
                                        var workFilter = primaryFilter.KeyValues.FirstOrDefault(m => m.Name == AppConstants.HasActiveWork);

                                        k.WorkIsActive = workFilter is KeyValue && workFilter.Value == AppConstants.HasActiveWorkYes;
                                    }

                                    // Hosting type
                                    var hostingFilter = k.Sections.FirstOrDefault(l => l.Name == AppConstants.HostingDetails);

                                    if (
                                        hostingFilter is KeyValue &&
                                        CommonUtils.IsValidCollection(hostingFilter.KeyValues))
                                    {
                                        var hostTypeFilter = hostingFilter.KeyValues.FirstOrDefault(m => m.Name == AppConstants.HostingType);

                                        k.HostedType = hostTypeFilter is KeyValue ? hostTypeFilter.Value : string.Empty;
                                    }

                                    // Environment
                                    if (
                                        hostingFilter is KeyValue &&
                                        CommonUtils.IsValidCollection(hostingFilter.KeyValues))
                                    {
                                        var sitesFilter = hostingFilter.KeyValues.FirstOrDefault(m => m.Name == AppConstants.Sites);

                                        if (
                                            sitesFilter is KeyValue &&
                                            CommonUtils.IsValidCollection(sitesFilter.KeyValues))
                                        {
                                            if (sitesFilter.KeyValues.FirstOrDefault(n => n.Name.ToTrimmedLower() == "prod" || n.Name.ToTrimmedLower() == "live") is KeyValue)
                                            {
                                                k.TopEnvironment = "Live";
                                            }
                                            else if (sitesFilter.KeyValues.FirstOrDefault(n => n.Name.ToTrimmedLower() == "uat" || n.Name.ToTrimmedLower() == "test") is KeyValue)
                                            {
                                                k.TopEnvironment = "UAT";
                                            }
                                            else if (sitesFilter.KeyValues.FirstOrDefault(n => n.Name.ToTrimmedLower() == "dev" || n.Name.ToTrimmedLower() == "development") is KeyValue)
                                            {
                                                k.TopEnvironment = "DEV";
                                            }
                                        }
                                    }
                                }

                                k.DetailsMessage = hasSections ? $"Has {k.Sections.Count()} section details" : "No details yet";
                                k.Sections = Enumerable.Empty<KeyValue>();

                                return k;
                            })
                            .ToList();

                        TeamDetail teamDetail = new TeamDetail
                        {
                            Id = team.Id,
                            Name = team.Name,
                            ApplicationCount = CommonUtils.IsValidCollection(foundApplications) ? foundApplications.Count() : 0,
                            ApplicationDetails = foundApplications,
                        };

                        result.Add(teamDetail);
                    }
                }
            }

            // Initialize default team
            if (!CommonUtils.IsValidCollection(result) && skip == 0)
            {
                await InitializeAsync(applicationDetail);
            }

            return result;
        }

        /// <summary>
        /// Fetches all teams
        /// </summary>
        /// <returns>Keyvalue collection</returns>
        public async Task<IEnumerable<KeyValue>> GetAllTeamsAsync()
        {
            IEnumerable<KeyValue> result = Enumerable.Empty<KeyValue>();
            var teamsLookup = await _appLookupManagement
                .GetItemAsync(i => i.Type == AppLookupTypes.team.ToString());

            if (teamsLookup is AppLookup)
            {
                result = teamsLookup.KeyValues;
            }

            return result;
        }

        /// <summary>
        /// Insert item to collection
        /// </summary>
        /// <param name="item">Application item</param>
        /// <returns>Result</returns>
        public async Task<int> InsertItemAsync(ApplicationDetail item)
        {
            int result = 0;
            DateTime now = DateTime.Now;
            var application = ToDataModel(item);

            application.CreatedOn = now;
            application.UpdatedOn = now;

            await CheckAddIfTeamNotExistsAsync(item, item.Team);

            var applicationFilter = await GetItemsAsync(i =>
                i.Team == item.Team &&
                i.Name == item.Name &&
                i.IsActive);

            if (!CommonUtils.IsValidCollection(applicationFilter))
            {
                // Check with case as extensions not supported in expression
                var activeApplications = await GetItemsAsync(j => j.IsActive);
                var caseFilter = activeApplications
                    .Where(k =>
                    k.Team.ToTrimmedLower() == item.Team.ToTrimmedLower() &&
                    k.Name.ToTrimmedLower() == item.Name.ToTrimmedLower());

                if (!CommonUtils.IsValidCollection(caseFilter))
                {
                    item.Id = await MaxIdAsync();

                    var response = await _repository
                        .InsertItemAsync(application);

                    result = response.AsInt32;
                }
            }

            return result;
        }

        /// <summary>
        /// Insert team
        /// </summary>
        /// <param name="item">Application item</param>
        /// <returns>Result</returns>
        public async Task<bool> InsertTeamAsync(ApplicationDetail item)
        {
            return await CheckAddIfTeamNotExistsAsync(item, item.Team);
        }

        /// <summary>
        /// Update item
        /// </summary>
        /// <param name="item">Application details</param>
        /// <returns>Result</returns>
        public async Task<bool> UpdateItemAsync(ApplicationDetail item)
        {
            bool result = false;
            DateTime now = DateTime.Now;
            var applicationFilter = await GetItemsAsync(i =>
                    i.Team == item.Team &&
                    i.Name == item.Name);

            if (CommonUtils.IsValidCollection(applicationFilter))
            {
                Application application = applicationFilter.FirstOrDefault();

                application.Alias = item.Alias;
                application.CreatedBy = item.CreatedBy;
                application.CreatedOn = item.CreatedOn;
                application.Id = item.Id;
                application.IsActive = item.IsActive;
                application.Name = item.Name;
                application.Sections = item.Sections;
                application.Tags = item.Tags;
                application.Team = item.Team;
                application.UpdatedBy = item.UpdatedBy;
                application.UpdatedOn = now;

                result = await _repository
                    .UpdateItemAsync(application);
            }

            return result;
        }

        /// <summary>
        /// Fetch users from application
        /// </summary>
        /// <param name="applications">Application items</param>
        /// <returns>User detail collection</returns>
        private async Task<IEnumerable<UserDetail>> GetUserDetailsAsync(IEnumerable<Application> applications)
        {
            List<int> userIdsToLookup = new List<int>();
            IEnumerable<UserDetail> result = Enumerable.Empty<UserDetail>();

            if (CommonUtils.IsValidCollection(applications))
            {
                foreach (var application in applications)
                {
                    if (!userIdsToLookup.Contains(application.CreatedBy))
                    {
                        userIdsToLookup.Add(application.CreatedBy);
                    }

                    if (!userIdsToLookup.Contains(application.UpdatedBy))
                    {
                        userIdsToLookup.Add(application.UpdatedBy);
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
        /// Check if team exists
        /// </summary>
        /// <param name="item">Applciation item</param>
        /// <param name="teamToFind">Team name</param>
        /// <returns>Result</returns>
        private async Task<bool> CheckAddIfTeamNotExistsAsync(ApplicationDetail item, string teamToFind)
        {
            bool result = false;

            AppLookup teamsLookup = await _appLookupManagement
                .GetItemAsync(i => i.Type == AppLookupTypes.team.ToString());

            if (teamsLookup is AppLookup)
            {
                KeyValue teamFilter = teamsLookup
                    .KeyValues
                    .FirstOrDefault(k => k.Name.ToTrimmedLower() == teamToFind.ToTrimmedLower());

                if (teamFilter == null)
                {
                    result = await AddToTeamAsync(item, teamsLookup, teamToFind);
                }
            }
            else
            {
                var addDefaultTeam = await InitializeAsync(item);

                if (addDefaultTeam > 0)
                {
                    result = await AddToTeamAsync(item, teamsLookup, teamToFind);
                }
            }

            return result;
        }

        /// <summary>
        /// Add team to lookup
        /// </summary>
        /// <param name="item">Application item</param>
        /// <param name="teamLookup">Team lookup</param>
        /// <param name="teamToFind">Team name</param>
        /// <returns>Result</returns>
        private async Task<bool> AddToTeamAsync(
            ApplicationDetail item,
            AppLookup teamLookup,
            string teamToFind)
        {
            bool result = false;
            int id = 1;
            List<KeyValue> teams = teamLookup.KeyValues?.ToList();

            if (CommonUtils.IsValidCollection(teams))
            {
                id = teams.Max(j => j.Id) + 1;
            }
            else
            {
                teams = new List<KeyValue>();
            }

            teams.Add(new KeyValue
            {
                Id = id,
                Name = teamToFind,
                IsActive = true,
            });
            teamLookup.KeyValues = teams;

            AppLookupDetail appLookupDetail = _appLookupManagement.ToViewModel(teamLookup);

            appLookupDetail.UpdatedBy = item.UpdatedBy;
            appLookupDetail.UpdatedOn = item.UpdatedOn;

            result = await _appLookupManagement
                .UpdateItemAsync(appLookupDetail);

            return result;
        }

        /// <summary>
        /// Initialize default team
        /// </summary>
        /// <param name="applicationDetail">Application detail</param>
        /// <returns></returns>
        private async Task<int> InitializeAsync(ApplicationDetail applicationDetail)
        {
            int result = 0;

            // Set default team on config
            if (_options.Value.AppDefaults.SetDefaultTeam)
            {
                DateTime now = DateTime.Now;

                // Existing teams
                var teamLookup = await _appLookupManagement
                    .GetItemAsync(i => i.Type == AppLookupTypes.team.ToString());

                if (teamLookup is AppLookup)
                {
                    // Look for default team
                    var defaultTeamFilter = teamLookup
                        .KeyValues?
                        .FirstOrDefault(j => j.Name.ToTrimmedLower() == DEFAULTTEAMNAME.ToTrimmedLower());

                    if (defaultTeamFilter == null)
                    {
                        int maxId = teamLookup
                            .KeyValues
                            .Max(k => k.Id);

                        var newTeam = NewDefaultTeamAsync(maxId + 1);

                        if (!CommonUtils.IsValidCollection(teamLookup.KeyValues))
                        {
                            teamLookup.KeyValues = new List<KeyValue>();
                        }

                        List<KeyValue> keyValues = teamLookup.KeyValues.ToList();

                        keyValues.Add(newTeam);
                        teamLookup.KeyValues = keyValues;

                        AppLookupDetail appLookupDetail = _appLookupManagement.ToViewModel(teamLookup);

                        appLookupDetail.UpdatedBy = applicationDetail.UpdatedBy;
                        appLookupDetail.UpdatedOn = now;

                        bool updated = await _appLookupManagement.UpdateItemAsync(appLookupDetail);

                        result = updated ? appLookupDetail.Id : 0;
                    }
                }
                else
                {
                    var newTeam = NewDefaultTeamAsync(1);
                    AppLookupDetail appLookupDetail = new AppLookupDetail
                    {
                        CreatedBy = applicationDetail.UpdatedBy,
                        CreatedOn = now,
                        IsActive = true,
                        KeyValues = new List<KeyValue> { newTeam },
                        Type = AppLookupTypes.team.ToString(),
                        UpdatedBy = applicationDetail.UpdatedBy,
                        UpdatedOn = now,
                    };

                    result = await _appLookupManagement.InsertItemAsync(appLookupDetail);
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
        /// Default team key value
        /// </summary>
        /// <param name="teamId">Team id</param>
        /// <returns>Keyvalue item</returns>
        private KeyValue NewDefaultTeamAsync(int teamId) =>
            new KeyValue
            {
                Id = teamId,
                Name = DEFAULTTEAMNAME,
                IsActive = true,
            };

        /// <summary>
        /// Default application sections
        /// </summary>
        /// <returns>Section collection</returns>
        private IEnumerable<KeyValue> DefaultSections() =>
            new List<KeyValue>
            {
                new KeyValue
                {
                    Id = 1,
                    Name = AppConstants.PrimaryDetails,
                    Type = KeyValueTypes.section,
                    IsActive = true,
                    KeyValues = new List<KeyValue>
                    {
                        new KeyValue
                        {
                            Name = AppConstants.Description,
                            Type = KeyValueTypes.textsummaryitem,
                            IsActive = true,
                        },
                        new KeyValue
                        {
                            Name = AppConstants.Alias,
                            Type = KeyValueTypes.item,
                            IsActive = true,
                        },
                        new KeyValue
                        {
                            Name = AppConstants.VSTS,
                            Type = KeyValueTypes.clicklink,
                            IsActive = true,
                        },
                        new KeyValue
                        {
                            Name = AppConstants.Wiki,
                            Type = KeyValueTypes.clicklink,
                            IsActive = true,
                        },
                        new KeyValue
                        {
                            Name = AppConstants.UserBase,
                            Type = KeyValueTypes.boolean,
                            IsActive = true,
                            KeyValues = _options
                                .Value
                                .AppDefaults
                                .UserBaseOptions
                                .OrderBy(i => i)
                                .Select((j, index) => new KeyValue { Id = index + 1, Name = j }),
                        },
                        new KeyValue
                        {
                            Name = AppConstants.UserBaseRegion,
                            Type = KeyValueTypes.list,
                            SubType = KeyValueTypes.options,
                            IsActive = true,
                            KeyValues = _options
                                .Value
                                .AppDefaults
                                .RegionOptions
                                .OrderBy(i => i)
                                .Select((j, index) => new KeyValue { Id = index + 1, Name = j }),
                        },
                        new KeyValue
                        {
                            Name = AppConstants.ApplicationType,
                            Type = KeyValueTypes.list,
                            SubType = KeyValueTypes.options,
                            IsActive = true,
                            KeyValues = _options
                                .Value
                                .AppDefaults
                                .ApplicationTypeOptions
                                .OrderBy(i => i)
                                .Select((j, index) => new KeyValue { Id = index + 1, Name = j }),
                        },
                        new KeyValue
                        {
                            Name = AppConstants.HasActiveWork,
                            Type = KeyValueTypes.boolean,
                            IsActive = true,
                            KeyValues = new List<KeyValue>
                                {
                                    new KeyValue
                                    {
                                        Id = 1,
                                        Name = AppConstants.HasActiveWorkNo,
                                    },
                                    new KeyValue
                                    {
                                        Id = 2,
                                        Name = AppConstants.HasActiveWorkYes,
                                    },
                                },
                        },
                    },
                },
                new KeyValue
                {
                    Id = 2,
                    Name = AppConstants.ContactDetails,
                    Type = KeyValueTypes.section,
                    IsActive = true,
                    KeyValues = new List<KeyValue>
                    {
                        new KeyValue
                        {
                            Name = AppConstants.BusinessOwner,
                            Type = KeyValueTypes.list,
                            SubType = KeyValueTypes.person,
                            IsActive = true,
                        },
                        new KeyValue
                        {
                            Name = AppConstants.ProjectManager,
                            Type = KeyValueTypes.list,
                            SubType = KeyValueTypes.person,
                            IsActive = true,
                        },
                        new KeyValue
                        {
                            Name = AppConstants.Developer,
                            Type = KeyValueTypes.list,
                            SubType = KeyValueTypes.person,
                            IsActive = true,
                        },
                        new KeyValue
                        {
                            Name = AppConstants.QualityAnalyst,
                            Type = KeyValueTypes.list,
                            SubType = KeyValueTypes.person,
                            IsActive = true,
                        },
                    },
                },
                new KeyValue
                {
                    Id = 3,
                    Name = AppConstants.TechnicalDetails,
                    Type = KeyValueTypes.section,
                    IsActive = true,
                    KeyValues = new List<KeyValue>
                    {
                        new KeyValue
                        {
                            Name = AppConstants.Technology,
                            Type = KeyValueTypes.list,
                            SubType = KeyValueTypes.item,
                            IsActive = true,
                        },
                        new KeyValue
                        {
                            Name = AppConstants.Tools,
                            Type = KeyValueTypes.list,
                            SubType = KeyValueTypes.item,
                            IsActive = true,
                        },
                        new KeyValue
                        {
                            Name = AppConstants.DatabaseType,
                            Type = KeyValueTypes.list,
                            SubType = KeyValueTypes.options,
                            IsActive = true,
                            KeyValues = _options
                                .Value
                                .AppDefaults
                                .DatabaseOptions
                                .OrderBy(i => i)
                                .Select((j, index) => new KeyValue { Id = index + 1, Name = j }),
                        },
                        new KeyValue
                        {
                            Name = AppConstants.ExternalInterfaces,
                            Type = KeyValueTypes.list,
                            SubType = KeyValueTypes.item,
                            IsActive = true,
                        },
                    },
                },
                new KeyValue
                {
                    Id = 4,
                    Name = AppConstants.HostingDetails,
                    Type = KeyValueTypes.section,
                    IsActive = true,
                    KeyValues = new List<KeyValue>
                    {
                        new KeyValue
                        {
                            Name = AppConstants.HostingType,
                            Type = KeyValueTypes.boolean,
                            IsActive = true,
                            KeyValues = _options
                                .Value
                                .AppDefaults
                                .HostingTypeOptions
                                .OrderBy(i => i)
                                .Select((j, index) => new KeyValue { Id = index + 1, Name = j }),
                        },
                        new KeyValue
                        {
                            Name = AppConstants.InternetAccessible,
                            Type = KeyValueTypes.boolean,
                            IsActive = true,
                            KeyValues = new List<KeyValue>
                                {
                                    new KeyValue
                                    {
                                        Id = 1,
                                        Name = "Yes",
                                    },
                                    new KeyValue
                                    {
                                        Id = 2,
                                        Name = "No",
                                    },
                                },
                        },
                        new KeyValue
                        {
                            Name = AppConstants.CloudProviders,
                            Type = KeyValueTypes.list,
                            SubType = KeyValueTypes.options,
                            IsActive = true,
                            KeyValues = _options
                                .Value
                                .AppDefaults
                                .CloudOptions
                                .OrderBy(i => i)
                                .Select((j, index) => new KeyValue { Id = index + 1, Name = j }),
                        },
                        new KeyValue
                        {
                            Name = AppConstants.HostedRegion,
                            Type = KeyValueTypes.list,
                            SubType = KeyValueTypes.options,
                            IsActive = true,
                            KeyValues = _options
                                .Value
                                .AppDefaults
                                .RegionOptions
                                .OrderBy(i => i)
                                .Select((j, index) => new KeyValue { Id = index + 1, Name = j }),
                        },
                        new KeyValue
                        {
                            Name = AppConstants.Environments,
                            Type = KeyValueTypes.list,
                            SubType = KeyValueTypes.options,
                            IsActive = true,
                            KeyValues = _options
                                .Value
                                .AppDefaults
                                .EnvironmentOptions
                                .OrderBy(i => i)
                                .Select((j, index) => new KeyValue { Id = index + 1, Name = j }),
                        },
                        new KeyValue
                        {
                            Name = AppConstants.Servers,
                            Type = KeyValueTypes.list,
                            SubType = KeyValueTypes.tagitem,
                            IsActive = true,
                        },
                        new KeyValue
                        {
                            Name = AppConstants.Sites,
                            Type = KeyValueTypes.list,
                            SubType = KeyValueTypes.link,
                            IsActive = true,
                        },
                    },
                },
                new KeyValue
                {
                    Id = 5,
                    Name = AppConstants.Comments,
                    Type = KeyValueTypes.section,
                    IsActive = true,
                    KeyValues = new List<KeyValue>
                    {
                        new KeyValue
                        {
                            Name = $"User {AppConstants.Comments}",
                            Type = KeyValueTypes.list,
                            SubType = KeyValueTypes.comment,
                            IsActive = true,
                        },
                    },
                },
                new KeyValue
                {
                    Id = 6,
                    Name = AppConstants.Timeline,
                    Type = KeyValueTypes.section,
                    IsActive = true,
                    KeyValues = new List<KeyValue>
                    {
                        new KeyValue
                        {
                            Name = $"Application {AppConstants.Timeline}",
                            Type = KeyValueTypes.list,
                            SubType = KeyValueTypes.timeline,
                            IsActive = true,
                        },
                    },
                },
            };

        /// <summary>
        /// To data model
        /// </summary>
        /// <param name="applicationDetail">Application detail item</param>
        /// <returns>Data model</returns>
        private Application ToDataModel(ApplicationDetail applicationDetail) =>
            new Application
            {
                Id = applicationDetail.Id,
                CreatedBy = applicationDetail.CreatedBy,
                CreatedOn = applicationDetail.CreatedOn,
                IsActive = applicationDetail.IsActive,
                Name = applicationDetail.Name,
                Alias = applicationDetail.Alias,
                Tags = applicationDetail.Tags,
                Team = applicationDetail.Team,
                Sections = applicationDetail.Sections,
                UpdatedBy = applicationDetail.UpdatedBy,
                UpdatedOn = applicationDetail.UpdatedOn,
            };

        /// <summary>
        /// To view model
        /// </summary>
        /// <param name="application">application item</param>
        /// <returns>View model</returns>
        private ApplicationDetail ToViewModel(Application application, IEnumerable<UserDetail> userDetails, bool isForList = false) =>
            new ApplicationDetail
            {
                Id = application.Id,
                CreatedBy = application.CreatedBy,
                CreatedOn = application.CreatedOn,
                IsActive = application.IsActive,
                Name = application.Name,
                Alias = application.Alias,
                Tags = application.Tags,
                Team = application.Team,
                Sections = (CommonUtils.IsValidCollection(application.Sections) || isForList) ? application.Sections : DefaultSections(),
                UpdatedBy = application.UpdatedBy,
                UpdatedOn = application.UpdatedOn,
                CreatedUserDetail = userDetails?.FirstOrDefault(i => i.Id == application.CreatedBy),
                UpdatedUserDetail = userDetails?.FirstOrDefault(i => i.Id == application.UpdatedBy),
            };
    }
}
