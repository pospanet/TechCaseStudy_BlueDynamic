namespace Retailizer.Common
{
    public interface IAppConfiguration
    {
        string Endpoint { get; }
        string Key { get; }
        string DatabaseName { get; }
        string DevicesCollectionName { get; }
        string EventsCollectionName { get; }
        string VisitsCollectionName { get; }
        string IotHubConnectionString { get; }
    }
}