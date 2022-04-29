using Azure.Storage.Blobs;
using HeyRed.Mime;
using Humanizer.Bytes;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Storage.Services;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Storage.Azure.Services {
    public class AzureStorageFolder : IStorageFolder {
        private readonly BlobContainerClient _container;
        private readonly string _folderName;

        public AzureStorageFolder(BlobContainerClient container, string folderName) {
            _container = container;
            _folderName = folderName;
        }
        
        public async Task<Blob> AddFileAsync(string filename, Stream stream) {
            await _container.UploadBlobAsync(GetBlobName(filename), stream);

            stream.Rewind();

            return new Blob(filename, GetContentType(filename), _folderName, ByteSize.FromBytes(stream.Length), stream);
        }

        public async Task<Blob> AddFileAsync(string name, byte[] contents) {
            using (var stream = new MemoryStream(contents)) {
                return await AddFileAsync(name, stream);
            }
        }

        public async Task DeleteAllFilesAsync() {
            var result = _container.GetBlobsAsync(prefix: _folderName);

            await foreach (var page in result.AsPages()) {
                foreach (var blob in page.Values) {
                    await _container.DeleteBlobAsync(blob.Name);
                }
            }
        }

        public async Task DeleteFileAsync(string name) {
            var blobClient = await GetBlobClientAsync(name);

            await blobClient.DeleteAsync();
        }

        public async Task<Blob> GetFileAsync(string filename, CancellationToken cancellationToken = default) {
            var blobClient = await GetBlobClientAsync(filename);
            var properties = await blobClient.GetPropertiesAsync(cancellationToken: cancellationToken);

            return new Blob(blobClient.Name,
                            _folderName,
                            GetContentType(filename),
                            ByteSize.FromBytes(properties.Value.ContentLength),
                            await blobClient.OpenReadAsync(cancellationToken: cancellationToken));
        }

        private async Task<BlobClient> GetBlobClientAsync(string filename) {
            var blobClient = _container.GetBlobClient(GetBlobName(filename));
            var exists = await blobClient.ExistsAsync();

            if (!exists) {
                throw new FileNotFoundException($"File {filename.Quote()} does not exist in folder {_folderName.Quote()}");
            }

            return blobClient;
        }

        private string GetBlobName(string filename) {
            return $"{_folderName}/{filename}";
        }

        private string GetContentType(string filename) {
            try {
                return MimeTypesMap.GetMimeType(filename);
            } catch {
                return "application/octet-stream";
            }
        }
    }
}