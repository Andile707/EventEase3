using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Eventease.Models;
using Microsoft.Extensions.Options;

namespace Eventease.Services
{
    public class AzureBlobService : IAzureService
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly BlobContainerClient _blobContainerClient;
        private readonly AzureOptions _azureOptions;

        public AzureBlobService(IOptions<AzureOptions> azureOptions)
        {
            _azureOptions = azureOptions.Value;

            _blobServiceClient =
                new BlobServiceClient(_azureOptions.ConnectionString);

            _blobContainerClient =
                _blobServiceClient.GetBlobContainerClient(_azureOptions.Container);
        }

        public async Task<string> UploadFiles(IFormFile file)
        {
            using MemoryStream fileUploadStream = new MemoryStream();

            await file.CopyToAsync(fileUploadStream);
            fileUploadStream.Position = 0;

            string originalName = Path.GetFileName(file.FileName);

            string safeFileName = string.Concat(
                originalName.Split(Path.GetInvalidFileNameChars()));

            string uniqueName = $"{Guid.NewGuid()}_{safeFileName}";

            BlobClient blobClient =
                _blobContainerClient.GetBlobClient(uniqueName);

            await blobClient.UploadAsync(
                fileUploadStream,
                new BlobHttpHeaders
                {
                    ContentType = file.ContentType
                });

            return blobClient.Uri.ToString();
        }

        public async Task<List<BlobItem>> GetUploadedBlob()
        {
            var items = new List<BlobItem>();

            await foreach (BlobItem file in _blobContainerClient.GetBlobsAsync())
            {
                items.Add(file);
            }

            return items;
        }
    }
}
