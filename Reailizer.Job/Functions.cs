using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Documents.Linq;
using Microsoft.Azure.WebJobs;
using Microsoft.ProjectOxford.Face;
using Microsoft.ProjectOxford.Face.Contract;
using Newtonsoft.Json.Linq;
using Retailizer.Common;
using Retailizer.Common.DTO;
using Retailizer.Common.Services;

namespace Retailizer.Job
{
    public class Functions
    {
        private const string personGroupId = "retailizer";

        private static DocumentDbService ddbService;
        private static FaceApiService faceApiService;
        private static StorageService storageService;

        private static void InitializeServices()
        {
            IAppConfiguration config = new AppConfiguration();
            ddbService = new DocumentDbService(config);

            faceApiService = new FaceApiService(ConfigurationManager.AppSettings["FaceApiSubscriptionKey"]);

            storageService =
                new StorageService(ConfigurationManager.ConnectionStrings["AzureWebJobsStorage"].ConnectionString,
                    "faces");
        }

        public static async void ProcessQueueMessage([ServiceBusTrigger("events")] string message, TextWriter log)
        {
            InitializeServices();

            log.WriteLine(message);

            JObject mess = JObject.Parse(message);

            string deviceId = mess["deviceId"].ToString();
            JToken deviceName = mess["IoTHub"]["ConnectionDeviceId"];
            JToken blobName = mess["blobName"];
            string cameraId = mess["cameraId"].ToString().ToLower();

            Device device = ddbService.GetDevice(deviceId);
            string eventType = device.Cameras.Where(c => c.Id.ToLower() == cameraId).FirstOrDefault().EventType;
                //TODO: co když camera nebude

            MemoryStream faceStream = new MemoryStream();
            faceStream = storageService.DownloadBlockBlob($"{deviceId}/{blobName}");
            MemoryStream detectStream = new MemoryStream();
            faceStream.CopyTo(detectStream);
            detectStream.Seek(0, SeekOrigin.Begin);

            Face[] detectionResult =
                await
                    faceApiService.DetectAsync(detectStream,
                        returnFaceAttributes:
                        new List<FaceAttributeType>
                        {
                            FaceAttributeType.Age,
                            FaceAttributeType.Gender,
                            FaceAttributeType.Smile
                        });
            Debug.WriteLine($"Detecting done. Got {detectionResult.Count()} faces. Image: {blobName}");

            if (!detectionResult.Any())
            {
                Debug.WriteLine("No faces detected. Deleting blob.");
                //TODO: zapsat event, bez fotky a dalších dat

                //Smazat blob
                //await storageService.DeleteBlockBlob($"{deviceId}/{blobName}");
                return;
            }

            Event newEvent = new Event
            {
                CameraId = cameraId,
                DeviceId = device.DeviceId,
                EventType = eventType,
                StoreId = device.StoreId,
                TenantId = device.TenantId,
                Person = new VisitPerson
                {
                    Age = detectionResult[0].FaceAttributes.Age,
                    Gender = detectionResult[0].FaceAttributes.Gender,
                    Smile = detectionResult[0].FaceAttributes.Smile
                },
                TimeStamp = DateTime.Now
            };

            string personId;

            IdentifyResult[] identifyResults =
                await faceApiService.IdentifyAsync(personGroupId, detectionResult.Select(f => f.FaceId).ToArray());
            Debug.WriteLine($"Identification done. Got {identifyResults.Count()} results.");

            if (!identifyResults.Any() || !identifyResults.FirstOrDefault().Candidates.Any())
            {
                Debug.WriteLine("Unable to identify person for this face. Creating new person.");

                CreatePersonResult persResult =
                    await faceApiService.CreatePersonAsync(personGroupId, Guid.NewGuid().ToString());

                Debug.WriteLine($"New person created with PersonId: {persResult.PersonId}");

                personId = persResult.PersonId.ToString();
            }
            else
            {
                Candidate candidate = identifyResults.FirstOrDefault().Candidates.FirstOrDefault();
                if (candidate.Confidence < 0.8)
                {
                    Debug.WriteLine(
                        $"Identification not confident enough ({candidate.Confidence}). Creating new person.");

                    CreatePersonResult persResult =
                        await faceApiService.CreatePersonAsync(personGroupId, Guid.NewGuid().ToString());

                    Debug.WriteLine($"New person created with PersonId: {persResult.PersonId}");

                    newEvent.SuggestedPersonId = candidate.PersonId.ToString();
                    personId = persResult.PersonId.ToString();
                    newEvent.Confidence = candidate.Confidence;
                }
                else
                {
                    Person pers = await faceApiService.GetPersonAsync(personGroupId, candidate.PersonId);

                    Debug.WriteLine(
                        $"Person recognized: {pers.PersonId}. We have {pers.PersistedFaceIds.Length} faces recorded for this person.");

                    if (pers.PersistedFaceIds.Length == 248)
                    {
                        Guid persistedFaceId = pers.PersistedFaceIds.OrderBy(x => Guid.NewGuid()).FirstOrDefault();
                        await faceApiService.DeletePersonFaceAsync(personGroupId, candidate.PersonId, persistedFaceId);
                    }

                    personId = candidate.PersonId.ToString();
                    newEvent.Confidence = candidate.Confidence;
                }
            }

            newEvent.PersonId = personId;

            MemoryStream addFaceStream = new MemoryStream();
            faceStream.Seek(0, SeekOrigin.Begin);
            faceStream.CopyTo(addFaceStream);
            addFaceStream.Seek(0, SeekOrigin.Begin);

            await faceApiService.AddPersonFaceAsync(personGroupId, new Guid(newEvent.PersonId), addFaceStream);
            await faceApiService.TrainPersonGroupAsync(personGroupId);

            await ddbService.SaveAsync(newEvent);

            await ProcessEventAsync(newEvent);

            faceStream.Dispose();

            //await storageService.DeleteBlockBlob($"{deviceId}/{blobName}");

            Debug.WriteLine("Processing done.");
        }

        public static async Task ProcessEventAsync(Event _event)
        {
            if (ddbService == null)
                InitializeServices();

            if (string.IsNullOrEmpty(_event.PersonId))
                return;

            List<Visit> visits = new List<Visit>();
            Visit visit = null;

            switch (_event.EventType)
            {
                case Event.EVENT_TYPE_ENTER:
                    visit = new Visit(_event);
                    _event.VisitId = visit.Id;
                    await ddbService.SaveAsync(visit);
                    await ddbService.SaveAsync(_event);
                    break;
                default:
                    visit = await FindOpenVisitForEventAsync(_event);
                    if (visit == null)
                    {
                        visit = new Visit(_event);
                        await ddbService.SaveAsync(visit);
                    }
                    else
                    {
                        switch (_event.EventType)
                        {
                            case Event.EVENT_TYPE_PAYMENT:
                                visit.PaymentOn = _event.TimeStamp;
                                break;
                            case Event.EVENT_TYPE_LEAVE:
                                visit.LeaveOn = _event.TimeStamp;
                                visit.VisitStatus = VisitStatus.Left;
                                break;
                        }
                        await ddbService.SaveAsync(visit);
                    }
                    _event.VisitId = visit.Id;
                    await ddbService.SaveAsync(_event);
                    break;
            }
        }

        private static async Task<Visit> FindOpenVisitForEventAsync(Event _event)
        {
            if (ddbService == null)
                InitializeServices();

            if (_event.EventType == Event.EVENT_TYPE_ENTER)
                return null;

            string sql =
                $"SELECT * FROM c Where c.TenantId = \"{_event.TenantId}\" AND c.StoreId = \"{_event.StoreId}\" AND c.PersonId = \"{_event.PersonId}\" AND c.VisitStatus != {(int) VisitStatus.Left} AND c.VisitStatus != {(int) VisitStatus.None} ORDER BY c.EnterOn DESC";
            IDocumentQuery<dynamic> query =
                ddbService.DocumentClient.CreateDocumentQuery(ddbService.VisitsCollection.DocumentsLink, sql)
                    .AsDocumentQuery();

            Visit selectedVisit = null;
            foreach (Visit visit in await query.ExecuteNextAsync())
            {
                selectedVisit = visit;
                break;
            }

            return selectedVisit;
        }
    }
}