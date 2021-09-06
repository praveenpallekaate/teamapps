namespace TeamApps.UI
{
    /// <summary>
    /// App modal
    /// </summary>
    public class AppModalDetail
    {
        /// <summary>
        /// Gets or sets a value indicating whether gets or sets
        /// </summary>
        public bool Open { get; set; } = false;

        /// <summary>
        /// Gets or sets a value indicating whether gets or sets
        /// </summary>
        public bool ShowBackDrop { get; set; } = false;

        /// <summary>
        /// Gets or sets modal title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets modal class
        /// </summary>
        public string Class { get; set; }

        /// <summary>
        /// Gets or sets title class
        /// </summary>
        public string TitleClass { get; set; }

        /// <summary>
        /// Gets or sets content class
        /// </summary>
        public string ContentClass { get; set; }

        /// <summary>
        /// Gets or sets action area class
        /// </summary>
        public string ActionClass { get; set; }

        /// <summary>
        /// Gets or sets confirm message
        /// </summary>
        public string ConfirmMessage { get; set; }

        /// <summary>
        /// Gets or sets confirm title
        /// </summary>
        public string ConfirmTitle { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether gets or sets
        /// </summary>
        public bool IsUpdate { get; set; } = false;

        /// <summary>
        /// Gets or sets a value indicating whether gets or sets
        /// </summary>
        public bool IsDelete { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether gets or sets
        /// </summary>
        public bool IsLoading { get; set; }

        /// <summary>
        /// Gets a value indicating whether gets or sets
        /// </summary>
        public bool CanEdit
        {
            get
            {
                return IsLoading;
            }
        }
    }
}
