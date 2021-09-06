using System.Collections.Generic;
using System.Linq;
using TeamApps.Shared;

namespace TeamApps.UI
{
    /// <summary>
    /// Index page model
    /// </summary>
    public class IndexPage
    {
        /// <summary>
        /// Gets or sets team details
        /// </summary>
        public IEnumerable<TeamDetail> TeamsDetails { get; set; }
            = Enumerable.Empty<TeamDetail>();

        /// <summary>
        /// Gets or sets modal details
        /// </summary>
        public AppModalDetail AppModalDetails { get; set; }
            = new AppModalDetail();

        /// <summary>
        /// Gets or sets modal type
        /// </summary>
        public ModalTypes ModalType { get; set; } = ModalTypes.addteam;

        /// <summary>
        /// Gets or sets new team name
        /// </summary>
        public string NewTeamName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether gets or sets
        /// </summary>
        public bool NewTeamNameInvalid { get; set; }

        /// <summary>
        /// Gets or sets new application
        /// </summary>
        public Application NewApplication { get; set; }
            = new Application();

        /// <summary>
        /// Gets or sets a value indicating whether gets or sets
        /// </summary>
        public bool NewApplicationNameInvalid { get; set; }

        /// <summary>
        /// Gets or sets new tag
        /// </summary>
        public string NewTag { get; set; }

        /// <summary>
        /// Gets or sets existing teams
        /// </summary>
        public string[] ExistingTeams { get; set; } = new string[] { };

        /// <summary>
        /// Gets or sets loading message
        /// </summary>
        public string LoadingMessage { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating whether gets or sets
        /// </summary>
        public bool IsLoggedIn { get; set; } = false;

        /// <summary>
        /// Gets or sets a value indicating whether gets or sets
        /// </summary>
        public bool CanEdit { get; set; } = false;

        /// <summary>
        /// Gets or sets a value indicating whether gets or sets
        /// </summary>
        public bool IsLoading { get; set; } = false;
    }
}
