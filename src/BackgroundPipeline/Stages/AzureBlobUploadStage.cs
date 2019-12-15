using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Jpp.BackgroundPipeline.Stages
{
    public class AzureBlobUploadStage : PipelineStage
    {
        public const string FILE_OUTPUT = "BlobUrl";

        public string AzureStorageAccountKey { get; set; }

        [NotMapped]
        public int MaxChunks { get; set; } = 1000;
        [NotMapped]
        public int CurrentChunk 
        { 
            get
            {
                return _currentChunk;
            }
            set
            {
                SetField(ref _currentChunk, value, nameof(CurrentChunk));
            }
        }
        private int _currentChunk;

        private AzureBlobUploadStage()
        {

        }

        public AzureBlobUploadStage(String azureStorageAccountKey)
        {
            AzureStorageAccountKey = azureStorageAccountKey;
        }

        protected async override Task<Status> RunPayload()
        {              
            CloudStorageAccount storageAccount;
            if (CloudStorageAccount.TryParse(AzureStorageAccountKey, out storageAccount))
            {
                // Create the CloudBlobClient that represents the 
                // Blob storage endpoint for the storage account.
                CloudBlobClient cloudBlobClient = storageAccount.CreateCloudBlobClient();

                // Create a container called 'quickstartblobs' and 
                // append a GUID value to it to make the name unique.
                CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference("mapping");
                await cloudBlobContainer.CreateIfNotExistsAsync();

                BlobContainerPermissions permissions = new BlobContainerPermissions
                {
                    PublicAccess = BlobContainerPublicAccessType.Blob
                };
                await cloudBlobContainer.SetPermissionsAsync(permissions);

                byte[] rawFileData = _inputs[FileUploadStage.FILE_OUTPUT] as byte[];

                using (Stream fileData = new MemoryStream(rawFileData))
                {
                    CloudBlockBlob cloudBlockBlob =
                        cloudBlobContainer.GetBlockBlobReference($"{_inputs[FileUploadStage.FILENAME]}");

                    var blockSize = 256 * 1024;
                    cloudBlockBlob.StreamWriteSizeInBytes = blockSize;
                    CurrentChunk = 1;

                    var uploadedUrl = cloudBlockBlob.StorageUri.PrimaryUri.AbsoluteUri;
                    long bytesToUpload = rawFileData.Length;

                    MaxChunks = (int)Math.Ceiling((double)bytesToUpload / (double)blockSize);

                    if (bytesToUpload < blockSize)
                    {
                        await cloudBlockBlob.UploadFromStreamAsync(fileData);
                    }
                    else
                    {
                        List<string> blockIds = new List<string>();
                        while(bytesToUpload > 0)
                        {
                            var blockId = Convert.ToBase64String(Encoding.UTF8.GetBytes(CurrentChunk.ToString("d6")));
                            blockIds.Add(blockId);
                            byte[] buffer = new byte[blockSize];
                            int readBytes = fileData.Read(buffer, 0, blockSize);

                            byte[] trimmedBuffer = new byte[readBytes];
                            Array.Copy(buffer, 0, trimmedBuffer, 0, readBytes);

                            MemoryStream chunk = new MemoryStream(trimmedBuffer);
                            await cloudBlockBlob.PutBlockAsync(blockId, chunk);
                            CurrentChunk++;
                            bytesToUpload = bytesToUpload - readBytes;
                        }

                        await cloudBlockBlob.PutBlockListAsync(blockIds);
                        Output[FILE_OUTPUT] = cloudBlockBlob.StorageUri;
                    }
                }
            }           

            return Status.Completed;
        }
    }
}
