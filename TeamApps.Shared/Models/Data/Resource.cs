using System;
using System.Collections.Generic;

namespace TeamApps.Shared
{
    /// <summary>
    /// Resource model
    /// </summary>
    public class Resource : IDataModel
    {
        public int Id { get; set; }
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Team { get; set; }
        public string Supervisor { get; set; }
        public string WowId { get; set; }
        public DateTime? StartDate { get; set; }
        public IEnumerable<KeyValue> Allocations { get; set; }
        public IEnumerable<KeyValue> Milestones { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
    }
}
