using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;
using N3O.Umbraco.Attributes;
using N3O.Umbraco.Storage.Azure;
using Umbraco.StorageProviders.AzureBlob.IO;

namespace N3O.Umbraco.Storage;

[Order(0)]
public class AzureStartupStorage : IStartupStorage {
    public IStorageFolder GetStorageFolder(IConfiguration configuration, string folderPath) {
        if (AzureBlobStorage.UsesIdentity(configuration)) {
            var container = AzureBlobStorage.GetContainerClient(configuration);

            return new AzureStorageFolder(container, AzureVolume.GetPrefixedPath(folderPath));
        } else {
            var options = configuration.GetSection(AzureBlobStorage.MediaSectionKey).Get<AzureBlobFileSystemOptions>();

            var serviceClient = new BlobServiceClient(options.ConnectionString);
            var container = serviceClient.GetBlobContainerClient(AzureStorageConstants.StorageContainerName
                                                                                      .ToLowerInvariant());

            container.CreateIfNotExistsAsync().GetAwaiter().GetResult();

            return new AzureStorageFolder(container, folderPath);
        }
    }
}
