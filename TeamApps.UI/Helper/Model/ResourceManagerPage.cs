using System;
using System.Collections.Generic;
using System.Linq;
using TeamApps.Shared;

namespace TeamApps.UI
{
    /// <summary>
    /// Resource manager page model
    /// </summary>
    public class ResourceManagerPage
    {
        /// <summary>
        /// Gets or sets resource details
        /// </summary>
        public ResourceDetail ResourceDetail { get; set; }
            = new ResourceDetail();

        /// <summary>
        /// Gets or sets edited resource details
        /// </summary>
        public ResourceDetail EditedDetail { get; set; }
            = new ResourceDetail();

        /// <summary>
        /// Gets or sets allocation details
        /// </summary>
        public SectionDetail AllocationDetail { get; set; }
            = new SectionDetail();

        /// <summary>
        /// Gets or sets work breakdown details
        /// </summary>
        public SectionDetail WorkBreakdownDetail { get; set; }
            = new SectionDetail();

        /// <summary>
        /// Gets or sets modal details
        /// </summary>
        public AppModalDetail AppModalDetails { get; set; }
            = new AppModalDetail();

        /// <summary>
        /// Gets or sets modal type
        /// </summary>
        public ModalTypes ModalType { get; set; } = ModalTypes.close;

        /// <summary>
        /// Gets or sets page years
        /// </summary>
        public int[] Years { get; set; }
            = new int[] { DateTime.Now.Year - 1, DateTime.Now.Year, DateTime.Now.Year + 1, DateTime.Now.Year + 2 };

        /// <summary>
        /// Gets or sets page mode
        /// </summary>
        public Modes Mode { get; set; }
            = Modes.read;

        /// <summary>
        /// Gets or sets new allocation
        /// </summary>
        public Allocation NewAllocation { get; set; }
            = new Allocation();

        /// <summary>
        /// Gets or sets new milestone
        /// </summary>
        public KeyValue NewMilestone { get; set; }
            = new KeyValue();

        /// <summary>
        /// Gets or sets resource milestone
        /// </summary>
        public List<KeyValue> ResourceMilestones { get; set; }
            = new List<KeyValue>();

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

    /// <summary>
    /// Section detail model
    /// </summary>
    public class SectionDetail
    {
        /// <summary>
        /// Gets or sets year
        /// </summary>
        public string Year { get; set; }

        /// <summary>
        /// Gets or sets overall
        /// </summary>
        public ChartDetail Overall { get; set; }
            = new ChartDetail();

        /// <summary>
        /// Gets or sets overall
        /// </summary>
        public ChartDetail OverallType { get; set; }
            = new ChartDetail();

        /// <summary>
        /// Gets or sets monthly
        /// </summary>
        public IEnumerable<ChartDetail> Monthly { get; set; }
            = Enumerable.Empty<ChartDetail>();

        /// <summary>
        /// Gets or sets allocation
        /// </summary>
        public IEnumerable<AllocationDetail> Allocations { get; set; }
            = Enumerable.Empty<AllocationDetail>();

        /// <summary>
        /// Gets or sets page loading message
        /// </summary>
        public string LoadingMessage { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether gets or sets flag for loading
        /// </summary>
        public bool IsLoading { get; set; } = false;
    }

    /// <summary>
    /// Chart detail model
    /// </summary>
    public class ChartDetail
    {
        /// <summary>
        /// Gets or sets year
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets labels
        /// </summary>
        public string[] Labels { get; set; }
            = new string[] { };

        /// <summary>
        /// Gets or sets values
        /// </summary>
        public int[] Values { get; set; }
            = new int[] { };
    }

    /// <summary>
    /// Allocation model
    /// </summary>
    public class Allocation
    {
        /// <summary>
        /// Gets or sets new value
        /// </summary>
        public string Year { get; set; }

        /// <summary>
        /// Gets or sets new value
        /// </summary>
        public string Month { get; set; }

        /// <summary>
        /// Gets or sets new value
        /// </summary>
        public string Project { get; set; }

        /// <summary>
        /// Gets or sets new value
        /// </summary>
        public string Demand { get; set; }

        /// <summary>
        /// Gets or sets new value
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Gets or sets work types
        /// </summary>
        public List<KeyValue> WorkTypes { get; set; }
            = new List<KeyValue>();
    }
}
