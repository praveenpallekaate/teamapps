using System.Collections.Generic;
using TeamApps.Shared;

namespace TeamApps.UI
{
    /// <summary>
    /// Application detail page model
    /// </summary>
    public class ApplicationDetailPage
    {
        /// <summary>
        /// Gets or sets application details
        /// </summary>
        public ApplicationDetail ApplicationDetail { get; set; }
            = new ApplicationDetail();

        /// <summary>
        /// Gets or sets application sections
        /// </summary>
        public List<DetailSectionPage> Sections { get; set; }
            = new List<DetailSectionPage>();

        /// <summary>
        /// Gets or sets application edited sections
        /// </summary>
        public List<DetailSectionPage> EditedSections { get; set; }
            = new List<DetailSectionPage>();

        /// <summary>
        /// Gets or sets page loading message
        /// </summary>
        public string LoadingMessage { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether gets or sets flag for edtt
        /// </summary>
        public bool CanEdit { get; set; } = false;

        /// <summary>
        /// Gets or sets a value indicating whether gets or sets flag for loading
        /// </summary>
        public bool IsLoading { get; set; } = false;
    }
}
