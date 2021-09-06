using System;
using System.Collections.Generic;
using System.Linq;
using TeamApps.Shared;

namespace TeamApps.UI
{
    /// <summary>
    /// Team stats model
    /// </summary>
    public class TeamStatsPage
    {
        /// <summary>
        /// Gets or sets resources
        /// </summary>
        public List<ResourceDetail> Resources { get; set; }
            = new List<ResourceDetail>();

        /// <summary>
        /// Gets or sets monthly
        /// </summary>
        public IEnumerable<ChartDetail> ResourcesStats { get; set; }
            = Enumerable.Empty<ChartDetail>();

        /// <summary>
        /// Gets or sets page years
        /// </summary>
        public int[] Years { get; set; }
            = new int[] { DateTime.Now.Year - 1, DateTime.Now.Year, DateTime.Now.Year + 1, DateTime.Now.Year + 2 };

        /// <summary>
        /// Gets or sets selected supervisor
        /// </summary>
        public string Supervisor { get; set; }

        /// <summary>
        /// Gets or sets selected year
        /// </summary>
        public string Year { get; set; }

        /// <summary>
        /// Gets or sets page loading message
        /// </summary>
        public string LoadingMessage { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether gets or sets flag for loading
        /// </summary>
        public bool IsLoading { get; set; } = false;
    }
}
