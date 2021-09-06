using System.Collections.Generic;

namespace TeamApps.Shared
{
    /// <summary>
    /// Team detail view model
    /// </summary>
    public class TeamDetail
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ApplicationCount { get; set; }
        public IEnumerable<ApplicationDetail> ApplicationDetails { get; set; }
    }
}
