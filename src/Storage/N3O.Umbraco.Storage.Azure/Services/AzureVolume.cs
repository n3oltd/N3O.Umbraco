using AsyncKeyedLock;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;
using N3O.Umbraco.Utilities;
using System.Threading.Tasks;

namespace N3O.Umbraco.Storage.Azure;

public class AzureVolume : IVolume {
    private readonly IConfiguration _configuration;
    private readonly AsyncKeyedLocker<string> _locker;
    private BlobContainerClient _container;

    public AzureVolume(IConfiguration configuration, AsyncKeyedLocker<string> locker) {
        _configuration = configuration;
        _locker = locker;
    }

    public async Task<IStorageFolder> GetStorageFolderAsync(string folderPath) {
        var container = await GetContainerAsync();
        var path = AzureBlobStorage.GetStorageFolderPath(_configuration, folderPath);

        return new AzureStorageFolder(container, path);
    }

    private async Task<BlobContainerClient> GetContainerAsync() {
        if (_container == null) {
            using (await _locker.LockAsync(LockKey.Generate<AzureVolume>(nameof(GetContainerAsync)))) {
                _container = AzureBlobStorage.GetStorageContainerClient(_configuration);

                // A container-scoped identity cannot create containers, so with
                // credential-based access the container must already exist.
                if (!AzureBlobStorage.UsesIdentity(_configuration)) {
                    await _container.CreateIfNotExistsAsync();
                }
            }
        }

        return _container;
    }
}
