namespace TeamApps.Shared
{
    /// <summary>
    /// Resource view model
    /// </summary>
    public class ResourceDetail : Resource, IViewModel
    {
        public UserDetail CreatedUserDetail { get; set; }
        public UserDetail UpdatedUserDetail { get; set; }
        public int Year { get; set; }
        public string[] Supervisors { get; set; }
        public int[] Years { get; set; }
        public bool FullDetails { get; set; } = false;
    }
}
