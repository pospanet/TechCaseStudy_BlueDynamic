using Microsoft.Extensions.Configuration;
using Retailizer.Common;

namespace Retailizer.Backend
{
    internal class AppConfiguration : IAppConfiguration
    {
        public AppConfiguration(IConfigurationRoot configuration)
        {
            Endpoint = configuration["DocumentDb:endpoint"];
            Key = configuration["DocumentDb:key"];
            DatabaseName = configuration["DocumentDb:database"];
            DevicesCollectionName = configuration["DocumentDb:deviceCollection"];
            EventsCollectionName = configuration["DocumentDb:eventCollection"];
            VisitsCollectionName = configuration["DocumentDb:visitCollection"];
            IotHubConnectionString = configuration["IotHub:connectionString"];
        }

        public string Endpoint { get; }
        public string Key { get; }
        public string DatabaseName { get; }
        public string DevicesCollectionName { get; }
        public string EventsCollectionName { get; }
        public string VisitsCollectionName { get; }
        public string IotHubConnectionString { get; }
    }
}