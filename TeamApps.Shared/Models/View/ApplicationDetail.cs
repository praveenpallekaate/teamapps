namespace TeamApps.Shared
{
    public class ApplicationDetail : Application, IViewModel
    {
        public string DetailsMessage { get; set; }
        public string HostedType { get; set; }
        public string TopEnvironment { get; set; }
        public bool WorkIsActive { get; set; }
        public UserDetail CreatedUserDetail { get; set; }
        public UserDetail UpdatedUserDetail { get; set; }
    }
}
