namespace Retailizer.UWP
{
    public interface IConfiguration
    {
        string BackendUrl { get; }
        string IotHubUrl { get; }

        string StorageConnectionString { get; }

        string StorageContainer { get; }
    }

    internal class StaticConfiguration : IConfiguration
    {
        public static string BackendUrl => "<BackEndUrl>";
        public static string IotHubUrl => "<IotHub>";
        public static string StorageConnectionString
            =>
            "<StorageAccount>"
            ;
        public static string StorageContainer => "faces";

        string IConfiguration.IotHubUrl
        {
            get { return IotHubUrl; }
        }

        string IConfiguration.StorageConnectionString
        {
            get { return StorageConnectionString; }
        }

        string IConfiguration.StorageContainer
        {
            get { return StorageContainer; }
        }

        string IConfiguration.BackendUrl
        {
            get { return BackendUrl; }
        }
    }
}