namespace TeamApps.Shared
{
    /// <summary>
    /// Application config model
    /// </summary>
    public class AppSettings
    {
        public AppDefault AppDefaults { get; set; }
        public DatabaseSettings DatabaseSettings { get; set; }
    }

    public class AppDefault
    {
        public string Environment { get; set; }
        public bool SetDefaultTeam { get; set; }
        public string[] UserBaseOptions { get; set; }
        public string[] RegionOptions { get; set; }
        public string[] ApplicationTypeOptions { get; set; }
        public string[] HostingTypeOptions { get; set; }
        public string[] CloudOptions { get; set; }
        public string[] EnvironmentOptions { get; set; }
        public string[] DatabaseOptions { get; set; }
    }

    public class DatabaseSettings
    {
        public LiteDbSettings LiteDbSettings { get; set; }
    }

    public class LiteDbSettings
    {
        public bool IsInBlob { get; set; }
        public string BlobConnectionString { get; set; }
        public string DbFilePath { get; set; }
        public string DbPassword { get; set; }
    }
}
