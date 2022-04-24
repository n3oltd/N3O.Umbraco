using Azure.Storage.Blobs;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Storage.Services;
using System;
using System.IO;
using System.Threading.Tasks;

namespace N3O.Umbraco.Storage.Azure.Services {
    public class AzureStorageFolder : IStorageFolder {
        private readonly BlobContainerClient _container;
        private readonly Action _onContainerDeleted;

        public AzureStorageFolder(BlobContainerClient container, Action onContainerDeleted) {
            _container = container;
            _onContainerDeleted = onContainerDeleted;
        }
        
        public Task AddFileAsync(string name, Stream stream) {
            throw new System.NotImplementedException();
        }

        public Task AddFileAsync(string name, byte[] contents) {
            throw new System.NotImplementedException();
        }

        public async Task DeleteAllFilesAsync() {
            await _container.DeleteAsync();

            _onContainerDeleted();
        }

        public async Task DeleteFileAsync(string name) {
            var blob = await GetBlobAsync(name);

            await blob.DeleteAsync();
        }
        
        private async Task<BlobClient> GetBlobAsync(string name) {
            var blob = _container.GetBlobClient(name);
            var exists = await blob.ExistsAsync();

            if (!exists) {
                throw new FileNotFoundException($"File {name.Quote()} does not exist in folder {_container.Name.Quote()}");
            }

            return blob;
        }
    }
}