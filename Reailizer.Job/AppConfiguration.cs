using System.Configuration;
using Retailizer.Common;

namespace Retailizer.Job
{
    public class AppConfiguration : IAppConfiguration
    {
        public AppConfiguration()
        {
            Endpoint = ConfigurationManager.AppSettings["DocumentDBEndpoint"];
            Key = ConfigurationManager.AppSettings["DocumentDBKey"];
            DatabaseName = ConfigurationManager.AppSettings["DocumentDBDatabaseName"];
            DevicesCollectionName = "devices";
            EventsCollectionName = "events";
            VisitsCollectionName = "visits";
        }

        public string Endpoint { get; }
        public string Key { get; }
        public string DatabaseName { get; }
        public string DevicesCollectionName { get; }
        public string EventsCollectionName { get; }
        public string VisitsCollectionName { get; }
    }
}