using Azure.Storage.Blobs.Models;

namespace Eventease.Services
{
    public interface IAzureService
    {
        Task<string> UploadFiles(IFormFile file);

        Task<List<BlobItem>> GetUploadedBlob();
    }
}