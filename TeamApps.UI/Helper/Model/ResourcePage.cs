using System;
using System.Collections.Generic;
using System.Linq;
using TeamApps.Shared;

namespace TeamApps.UI
{
    /// <summary>
    /// Resource page model
    /// </summary>
    public class ResourcePage
    {
        /// <summary>
        /// Gets or sets resources
        /// </summary>
        public List<ResourceDetail> Resources { get; set; }
            = new List<ResourceDetail>();

        /// <summary>
        /// Gets or sets grid resources
        /// </summary>
        public IEnumerable<ResourceGrid> GridResources { get; set; }
            = Enumerable.Empty<ResourceGrid>();

        /// <summary>
        /// Gets or sets resource details
        /// </summary>
        public ResourceDetail ResourceDetail { get; set; }
            = new ResourceDetail();

        /// <summary>
        /// Gets or sets modal details
        /// </summary>
        public AppModalDetail AppModalDetails { get; set; }
            = new AppModalDetail();

        /// <summary>
        /// Gets or sets page filters
        /// </summary>
        public ResourceFilter Filter { get; set; }
            = new ResourceFilter();

        /// <summary>
        /// Gets or sets grid header
        /// </summary>
        public string[] GridHeader { get; set; }
            = new string[] { };

        /// <summary>
        /// Gets or sets modal type
        /// </summary>
        public ModalTypes ModalType { get; set; }
            = ModalTypes.close;

        /// <summary>
        /// Gets or sets view type
        /// </summary>
        public ResourcesViewTypes ViewType { get; set; }
            = ResourcesViewTypes.None;

        /// <summary>
        /// Gets or sets new resource team
        /// </summary>
        public string NewResourceTeam { get; set; }

        /// <summary>
        /// Gets or sets new resource name
        /// </summary>
        public string NewResourceName { get; set; }

        /// <summary>
        /// Gets or sets new resource email
        /// </summary>
        public string NewResourceEmail { get; set; }

        /// <summary>
        /// Gets or sets new resource supervisor
        /// </summary>
        public string NewResourceSupervisor { get; set; }

        /// <summary>
        /// Gets or sets new resource start date
        /// </summary>
        public DateTime? NewResourceStartDate { get; set; }

        /// <summary>
        /// Gets or sets new resource wow id
        /// </summary>
        public string NewResourceWowId { get; set; }

        /// <summary>
        /// Gets or sets page loading message
        /// </summary>
        public string LoadingMessage { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether gets or sets flag for edtt
        /// </summary>
        public bool CanEdit { get; set; } = false;

        /// <summary>
        /// Gets or sets a value indicating whether gets or sets flag for detail
        /// </summary>
        public bool IsDetailLoading { get; set; } = false;

        /// <summary>
        /// Gets or sets a value indicating whether gets or sets flag for grid
        /// </summary>
        public bool IsGridLoading { get; set; } = false;

        /// <summary>
        /// Gets or sets a value indicating whether gets or sets flag for loading
        /// </summary>
        public bool IsLoading { get; set; } = false;
    }

    /// <summary>
    /// Page filter
    /// </summary>
    public class ResourceFilter
    {
        /// <summary>
        /// Gets or sets teams
        /// </summary>
        public string[] Teams { get; set; }
            = new string[] { };

        /// <summary>
        /// Gets or sets supervisors
        /// </summary>
        public string[] Supervisors { get; set; }
            = new string[] { };

        /// <summary>
        /// Gets or sets years
        /// </summary>
        public int[] Years { get; set; }
            = new int[] { };

        /// <summary>
        /// Gets or sets types
        /// </summary>
        public string[] Types { get; set; }
            = new string[] { };

        /// <summary>
        /// Gets or sets selected team
        /// </summary>
        public string SelectedTeam { get; set; }

        /// <summary>
        /// Gets or sets selected name
        /// </summary>
        public string SelectedYear { get; set; }

        /// <summary>
        /// Gets or sets selected email
        /// </summary>
        public string SelectedType { get; set; }

        /// <summary>
        /// Gets or sets selected supervisor
        /// </summary>
        public string SelectedSupervisor { get; set; }
    }

    /// <summary>
    /// View type
    /// </summary>
    public enum ResourcesViewTypes
    {
        /// <summary>
        /// Resource detail view
        /// </summary>
        ResourceDetails,

        /// <summary>
        /// Team stats view
        /// </summary>
        TeamStats,

        /// <summary>
        /// No view
        /// </summary>
        None,
    }
}
