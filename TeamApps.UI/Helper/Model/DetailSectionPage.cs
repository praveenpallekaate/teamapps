using TeamApps.Shared;

namespace TeamApps.UI
{
    /// <summary>
    /// Section page model
    /// </summary>
    public class DetailSectionPage
    {
        /// <summary>
        /// Gets or sets section
        /// </summary>
        public KeyValue Section { get; set; }
            = new KeyValue();

        /// <summary>
        /// Gets or sets edited section
        /// </summary>
        public KeyValue EditedSection { get; set; }
            = new KeyValue();

        /// <summary>
        /// Gets or sets mode
        /// </summary>
        public Modes Mode { get; set; }
            = Modes.read;

        /// <summary>
        /// Gets or sets section class
        /// </summary>
        public string Class { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether gets or sets
        /// </summary>
        public bool IsDisabled { get; set; } = false;

        /// <summary>
        /// Gets or sets a value indicating whether gets or sets
        /// </summary>
        public bool IsLoading { get; set; } = false;
    }
}
