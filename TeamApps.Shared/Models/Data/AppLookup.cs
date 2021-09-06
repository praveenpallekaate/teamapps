using System;
using System.Collections.Generic;

namespace TeamApps.Shared
{
    /// <summary>
    /// Lookup data model
    /// </summary>
    public class AppLookup : IDataModel
    {
        public int Id { get; set; }
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string Type { get; set; }
        public IEnumerable<KeyValue> KeyValues { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
    }
}
