using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Functions
{
    public static class UploadImage
    {
        [FunctionName("UploadImage")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "upload")]HttpRequestMessage req, TraceWriter log)
        {
            log.Info($"C# HTTP trigger function processed a request. RequestUri={req.RequestUri}");

            HttpResponseMessage result = null;

            if(req.Content.IsMimeMultipartContent())
            {
                var memoryProvider = new MultipartMemoryStreamProvider();
                await req.Content.ReadAsMultipartAsync(memoryProvider);
                

                CloudStorageAccount storageAccount = 
                    CloudStorageAccount.Parse(System.Environment.GetEnvironmentVariable("StorageConnectionString"));
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                CloudBlobContainer container = blobClient.GetContainerReference(System.Environment.GetEnvironmentVariable("ContainerName"));
                string fileName = string.Empty;

                foreach (var content in memoryProvider.Contents)
                {
                    string uploadFileName = content.Headers.ContentDisposition.FileName;
                    int startIndex = uploadFileName.IndexOf(".");
                    string extension = uploadFileName.Substring(startIndex, 4);
                    fileName = string.Format("{0}{1}", Guid.NewGuid(), extension);
                    CloudBlockBlob blob = container.GetBlockBlobReference(fileName);
                    Stream s = new MemoryStream();
                    var stream = content.ReadAsStreamAsync().Result;
                    stream.CopyTo(s);
                    s.Seek(0, SeekOrigin.Begin);
                    await blob.UploadFromStreamAsync(s);
                }

                result = req.CreateResponse(HttpStatusCode.OK, fileName);
            }
            else
            {
                result = req.CreateResponse(HttpStatusCode.NotAcceptable, "This request is not properly formatted");
            }

            return result;  

        }
    }
}
