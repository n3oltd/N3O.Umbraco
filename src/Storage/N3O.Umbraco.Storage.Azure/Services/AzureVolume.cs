using Azure.Storage.Blobs;
using Humanizer;
using Microsoft.Extensions.Options;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Storage.Services;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using Umbraco.StorageProviders.AzureBlob.IO;

namespace N3O.Umbraco.Storage.Azure.Services {
    public class AzureVolume : IVolume {
        private readonly BlobServiceClient _serviceClient;
        private readonly ConcurrentDictionary<string, BlobContainerClient> _containers = new();
        
        private readonly AzureBlobFileSystemOptions _options;

        public AzureVolume(IOptions<AzureBlobFileSystemOptions> options) {
            _serviceClient = new BlobServiceClient(options.Value.ConnectionString);
        }
        
        public async Task<IStorageFolder> GetStorageFolderAsync(string name) {
            var container = await GetContainerAsync(name);
            
            return new AzureStorageFolder(container, () => ContainerDeleted(name));
        }
        
        private async Task<BlobContainerClient> GetContainerAsync(string folderName) {
            var result = await _containers.GetOrAddAtomicAsync(folderName, async () => {
                var blobContainer = _serviceClient.GetBlobContainerClient($"storage_${folderName.Camelize()}");

                await blobContainer.CreateIfNotExistsAsync();

                return blobContainer;
            });

            return result;
        }

        private void ContainerDeleted(string name) {
            _containers.Remove(name, out _);
        }
    }
}