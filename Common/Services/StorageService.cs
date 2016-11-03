using System.IO;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Retailizer.Common.Services
{
    public class StorageService
    {
        private readonly CloudBlobContainer _container;

        public StorageService(string storageConnectionString, string containerName)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(storageConnectionString);
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            _container = blobClient.GetContainerReference(containerName);
        }

        public MemoryStream DownloadBlockBlob(string blobName)
        {
            CloudBlockBlob blockBlob = _container.GetBlockBlobReference(blobName);
            MemoryStream resultStream = new MemoryStream();
            blockBlob.DownloadToStream(resultStream);
            resultStream.Seek(0, SeekOrigin.Begin);

            return resultStream;
        }

        public async Task DeleteBlockBlob(string blobName)
        {
            CloudBlockBlob blockBlob = _container.GetBlockBlobReference(blobName);
            await blockBlob.DeleteAsync();
        }

        public async Task UploadImageToBlobAsync(string blobName, Stream imageStream)
        {
            CloudBlockBlob blockBlob = _container.GetBlockBlobReference(blobName);
            imageStream.Seek(0, SeekOrigin.Begin);
            await blockBlob.UploadFromStreamAsync(imageStream);
        }
    }
}