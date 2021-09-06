using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeamApps.Shared;

namespace TeamApps.UI
{
    public class KeyValueUpdate
    {
        public int ParentId { get; set; }
        public int ItemId { get; set; }
        public KeyValue KeyValue { get; set; }
    }
}
