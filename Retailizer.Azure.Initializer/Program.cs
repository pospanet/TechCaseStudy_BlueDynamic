using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.ProjectOxford.Face;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Retailizer.Azure.Initializer
{
    class Program
    {
        private static FaceServiceClient faceClient;
        private static DocumentClient documentClient;
        private static CloudStorageAccount storageAccount;

        static void Main(string[] args)
        {
            Console.WriteLine("Creating clients...");
            documentClient = new DocumentClient(new Uri(ConfigurationManager.AppSettings["DocumentDbEndpoint"]), ConfigurationManager.AppSettings["DocumentDbKey"]);
            faceClient = new FaceServiceClient(ConfigurationManager.ConnectionStrings["FaceApiKey"].ConnectionString);
            storageAccount = CloudStorageAccount.Parse(ConfigurationManager.ConnectionStrings["StorageConnectionString"].ConnectionString);

            Console.WriteLine("Initializing DocumentDB...");
            InitializeDocumentDb().Wait();

            Console.WriteLine("Initializing Face recognition service...");
            InitializeFaceRecognitionAsync().Wait();

            Console.WriteLine("Initializing Storage...");
            InitializeStorageAsync().Wait();

            Console.WriteLine("Done.");
            Console.ReadKey();
        }

        private static async Task InitializeDocumentDb()
        {
            Database db = await GetOrCreateDatabaseAsync(ConfigurationManager.AppSettings["DocumentDbDatabaseName"]);

            Console.WriteLine("\tEvents collection");
            DocumentCollection eventsCollection = await documentClient.CreateDocumentCollectionAsync(db.SelfLink, new DocumentCollection() { Id = ConfigurationManager.AppSettings["EventsCollectionName"]});
            Console.WriteLine("\tVisits collection");
            DocumentCollection visitsCollection = await documentClient.CreateDocumentCollectionAsync(db.SelfLink, new DocumentCollection() { Id = ConfigurationManager.AppSettings["VisitsCollectionName"] });
            Console.WriteLine("\tDevices collection");
            DocumentCollection devicesCollection = await documentClient.CreateDocumentCollectionAsync(db.SelfLink, new DocumentCollection() { Id = ConfigurationManager.AppSettings["DevicesCollectionName"] });
        }

        private static async Task InitializeFaceRecognitionAsync()
        {
            try
            {
                Console.WriteLine("\tCreating Person group...");
                await faceClient.CreatePersonGroupAsync(ConfigurationManager.AppSettings["FaceApiPersonGroupName"], ConfigurationManager.AppSettings["FaceApiPersonGroupName"]);
            }
            catch (FaceAPIException ex) when (ex.ErrorCode == "PersonGroupExists")
            {
                Console.WriteLine("\t\tPerson group already exists. Skipping...");
            }

            Console.WriteLine("\tCreating a fake person for initial training");
            var newPerson = await faceClient.CreatePersonAsync(ConfigurationManager.AppSettings["FaceApiPersonGroupName"], "Initializer");

            Console.WriteLine("\tAdding face");
            using (Stream s = File.OpenRead("satya.jpg"))
            {
                await faceClient.AddPersonFaceAsync(ConfigurationManager.AppSettings["FaceApiPersonGroupName"], newPerson.PersonId, s);
            }

            Console.WriteLine("\tTraining Person group...");
            await faceClient.TrainPersonGroupAsync(ConfigurationManager.AppSettings["FaceApiPersonGroupName"]);
        }

        private static async Task InitializeStorageAsync()
        {
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference(ConfigurationManager.AppSettings["FacesContainerName"]);
            await container.CreateIfNotExistsAsync();
        }

        private static async Task<Database> GetOrCreateDatabaseAsync(string id)
        {
            IEnumerable<Database> query = from db in documentClient.CreateDatabaseQuery()
                                          where db.Id == id
                                          select db;

            Database database = query.FirstOrDefault();
            if (database == null)
            {
                database = await documentClient.CreateDatabaseAsync(new Database { Id = id });
            }

            return database;
        }
    }
}
