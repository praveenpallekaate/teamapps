using System;
using System.Collections.Generic;

namespace TeamApps.Shared
{
    /// <summary>
    /// Application data model
    /// </summary>
    public class Application : IDataModel
    {
        public int Id { get; set; }
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string Name { get; set; }
        public string Alias { get; set; }
        public string Team { get; set; }
        public IEnumerable<KeyValue> Sections { get; set; }
        public string[] Tags { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
    }
}
