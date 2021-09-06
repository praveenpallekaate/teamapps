using System.Collections.Generic;

namespace TeamApps.Shared
{
    /// <summary>
    /// Key value data model
    /// </summary>
    public class KeyValue : IDataModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Value { get; set; }
        public string[] Values { get; set; }
        public bool? Flag { get; set; }
        public bool IsActive { get; set; }
        public KeyValueTypes Type { get; set; }
        public KeyValueTypes? SubType { get; set; }
        public IEnumerable<KeyValue> KeyValues { get; set; }
    }
}
