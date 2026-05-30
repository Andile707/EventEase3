using Eventease.Models;
using Eventease.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Eventease.Controllers
{
    public class AzureBlobController : Controller
    {
        public IAzureService _service;

        public AzureBlobController(IAzureService azservice)
        {
            _service = azservice;
        
        }

        //MVC Metho to get data from Azure Blob
        
        public IActionResult GetBlob()
        {
            return View();
        }
        /*
        //MVC Method to upload to Azure
        [HttpPost]
        public  IActionResult UploadBlob(UploadModel uploadModel)
        {
           // if (uploadModel.File == null || uploadModel.File.FileName == null)
             //   return View("GetBlob");
            if (uploadModel.File == null || uploadModel.File.Length == 0)
            {
                throw new Exception("File is empty");
            }
            _service.UploadFiles(uploadModel.File);

                return View("GetBlob");

            //ViewBag.Message = "File uploaded successfully";

           // ViewBag.BlobUrl = blobUrl;

           // return View();
        }*/

        [HttpPost]
        public async Task<IActionResult> UploadBlob(IFormFile file)
        {
            await _service.UploadFiles(file);

            return View("GetBlob");
        }





        //APi method
        /*[HttpGet("BlobList")]

        public async Task<IActionResult> GetBlobList()
        {
            var response = await _service.GetUploadedBlob();
            return Ok(response);

        }*/


        public IActionResult Index()
        {
            return View();
        }
    }
}
