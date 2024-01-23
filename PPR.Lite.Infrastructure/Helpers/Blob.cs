using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PPR.Lite.Infrastructure.Helpers
{
    public static class Blob
    {
        public static async Task UploaToAzureAsync(IFormFile file,string containerName)
        {
            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=cloudhrms;AccountKey=alzWsviz0VwbxzUHvbAZG2HQaAtTJFeDlkC+0QflYtMTrDzmBSYQLiUUwqkHSeoJ+o5Nfd+QoooHExnbK3yELg==;EndpointSuffix=core.windows.net");
            var cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
            var cloudBlobContainer = cloudBlobClient.GetContainerReference("cloudhrmstest");
            if (await cloudBlobContainer.CreateIfNotExistsAsync())
            {
                await cloudBlobContainer.SetPermissionsAsync(new
                    BlobContainerPermissions
                {
                    PublicAccess =
                    BlobContainerPublicAccessType.Blob
                });
            }
            var cloudBlockBlob =
                cloudBlobContainer.GetBlockBlobReference(file.FileName);
            cloudBlockBlob.Properties.ContentType = file.ContentType;

            await
                cloudBlockBlob.UploadFromStreamAsync(file.OpenReadStream());
            var AbsoluteUri = cloudBlockBlob.Uri.AbsoluteUri;

        }
    }
}
