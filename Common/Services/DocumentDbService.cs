using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using Retailizer.Common.DTO;

namespace Retailizer.Common.Services
{
    public class DocumentDbService
    {
        public DocumentClient DocumentClient { get; }

        internal DocumentCollection DevicesCollection { get; }

        internal DocumentCollection EventsCollection { get; }

        public DocumentCollection VisitsCollection { get; }

        public DocumentDbService(IAppConfiguration configuration)
        {

            DocumentClient = new DocumentClient(new Uri(configuration.Endpoint), configuration.Key);
            Database database = DocumentClient.CreateDatabaseQuery().Where(db => db.Id == configuration.DatabaseName).AsEnumerable().First();
            DevicesCollection =
                DocumentClient.CreateDocumentCollectionQuery(database.SelfLink)
                    .Where(c => c.Id == configuration.DevicesCollectionName)
                    .AsEnumerable()
                    .First();
            EventsCollection =
                DocumentClient.CreateDocumentCollectionQuery(database.SelfLink)
                    .Where(c => c.Id == configuration.EventsCollectionName)
                    .AsEnumerable()
                    .First();
            VisitsCollection =
                DocumentClient.CreateDocumentCollectionQuery(database.SelfLink)
                    .Where(c => c.Id == configuration.VisitsCollectionName)
                    .AsEnumerable()
                    .First();
        }

        public Device GetDevice(string deviceId)
        {
            IEnumerable<Device> deviceQuery = DocumentClient
                .CreateDocumentQuery<Device>(DevicesCollection.DocumentsLink)
                .Where(d => d.id == deviceId)
                .AsEnumerable();

            Device selectedDevice = deviceQuery.FirstOrDefault();

            return selectedDevice;
        }

        public async Task<List<Visit>> GetAllVisitsByPersonIdAsync(string personId)
        {
            string sql = $"SELECT * FROM c Where c.PersonId = {personId}";

            IDocumentQuery<dynamic> query =
                DocumentClient.CreateDocumentQuery(VisitsCollection.DocumentsLink, sql).AsDocumentQuery();

            List<Visit> visits = new List<Visit>();

            foreach (dynamic visit in await query.ExecuteNextAsync())
            {
                visits.Add(visit);
            }

            return visits;
        }

        public async Task SaveAsync(Event _event)
        {
            await DocumentClient.CreateDocumentAsync(EventsCollection.DocumentsLink, _event);
        }

        public async Task SaveAsync(Visit visit)
        {
            await DocumentClient.CreateDocumentAsync(VisitsCollection.DocumentsLink, visit);
        }

        public async Task SaveAsync(Device device)
        {
            await DocumentClient.UpsertDocumentAsync(DevicesCollection.DocumentsLink, device);
        }
    }
}