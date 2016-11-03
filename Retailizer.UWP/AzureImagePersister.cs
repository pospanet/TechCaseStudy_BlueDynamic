using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Devices.Client;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;

namespace Retailizer.UWP
{
    internal class AzureImagePersister : IImagePersiter
    {
        private readonly DeviceClient _deviceClient;
        private readonly IConfiguration _configuration;
        private readonly IDeviceConfiguration _deviceConfiguration;

        public AzureImagePersister(IConfiguration configuration, IDeviceConfiguration deviceConfiguration)
        {
            _configuration = configuration;
            _deviceConfiguration = deviceConfiguration;
            _deviceClient = DeviceClient.Create(configuration.IotHubUrl,
                new DeviceAuthenticationWithRegistrySymmetricKey(deviceConfiguration.DeviceIdString,
                    deviceConfiguration.DeviceKey));
        }

        public async Task PersistAsync(IEnumerable<Stream> faceImages, string cameraId)
        {
            foreach (Stream faceImage in faceImages)
            {
                Guid blobGuid = Guid.NewGuid();

                var data = new
                {
                    deviceId = _deviceConfiguration.DeviceIdString,
                    blobName = blobGuid + ".jpg",
                    cameraId
                };
                string messageString = JsonConvert.SerializeObject(data);
                Message message = new Message(Encoding.ASCII.GetBytes(messageString));

                await UploadImageToBlobAsync($"{data.deviceId}/{data.blobName}", faceImage);
                Debug.WriteLine("Image uploaded.");

                await _deviceClient.SendEventAsync(message);
                Debug.WriteLine("Message sent.");

                faceImage.Dispose();
            }
        }

        private async Task UploadImageToBlobAsync(string blobName, Stream imageStream)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(_configuration.StorageConnectionString);
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference(_configuration.StorageContainer);
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(blobName);
            imageStream.Seek(0, SeekOrigin.Begin);
            await blockBlob.UploadFromStreamAsync(imageStream);
        }
    }
}