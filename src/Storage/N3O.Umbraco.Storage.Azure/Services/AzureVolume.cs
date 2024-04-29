using AsyncKeyedLock;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;
using N3O.Umbraco.Utilities;
using System.Threading.Tasks;
using Umbraco.StorageProviders.AzureBlob.IO;

namespace N3O.Umbraco.Storage.Azure;

public class AzureVolume : IVolume {
    private readonly AsyncKeyedLocker<string> _locker;
    private readonly BlobServiceClient _serviceClient;
    private BlobContainerClient _container;

    public AzureVolume(IConfiguration configuration, AsyncKeyedLocker<string> locker) {
        _locker = locker;
        
        var options = configuration.GetSection("Umbraco:Storage:AzureBlob:Media").Get<AzureBlobFileSystemOptions>();
        _serviceClient = new BlobServiceClient(options.ConnectionString);
    }
    
    public async Task<IStorageFolder> GetStorageFolderAsync(string folderPath) {
        var container = await GetContainerAsync();
        
        return new AzureStorageFolder(container, folderPath);
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
