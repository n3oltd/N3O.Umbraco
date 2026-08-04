using AsyncKeyedLock;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;
using N3O.Umbraco.Utilities;
using System.Threading.Tasks;
using Umbraco.StorageProviders.AzureBlob.IO;

namespace N3O.Umbraco.Storage.Azure;

public class AzureVolume : IVolume {
    private readonly IConfiguration _configuration;
    private readonly AsyncKeyedLocker<string> _locker;
    private readonly BlobServiceClient _serviceClient;
    private BlobContainerClient _container;

    public AzureVolume(IConfiguration configuration, AsyncKeyedLocker<string> locker) {
        _configuration = configuration;
        _locker = locker;

        if (!AzureBlobStorage.UsesIdentity(configuration)) {
            var options = configuration.GetSection(AzureBlobStorage.MediaSectionKey).Get<AzureBlobFileSystemOptions>();
            _serviceClient = new BlobServiceClient(options.ConnectionString);
        }
    }

    public async Task<IStorageFolder> GetStorageFolderAsync(string folderPath) {
        if (AzureBlobStorage.UsesIdentity(_configuration)) {
            // With credential-based access a single container holds everything; volume
            // files live under the storage prefix, beside the media, the image cache
            // and the data protection keys.
            var container = AzureBlobStorage.GetContainerClient(_configuration);

            return new AzureStorageFolder(container, GetPrefixedPath(folderPath));
        } else {
            var container = await GetContainerAsync();

            return new AzureStorageFolder(container, folderPath);
        }
    }

    internal static string GetPrefixedPath(string folderPath) {
        return $"{AzureStorageConstants.StoragePrefix}/{folderPath.TrimStart('/')}";
    }

    private async Task<BlobContainerClient> GetContainerAsync() {
        if (_container == null) {
            using (await _locker.LockAsync(LockKey.Generate<AzureVolume>(nameof(GetContainerAsync)))) {
                _container = _serviceClient.GetBlobContainerClient(AzureStorageConstants.StorageContainerName
                                                                                        .ToLowerInvariant());

                await _container.CreateIfNotExistsAsync();
            }
        }

        return _container;
    }
}
