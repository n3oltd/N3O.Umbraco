using Microsoft.Extensions.Configuration;
using N3O.Umbraco.Attributes;
using N3O.Umbraco.Storage.Azure;

namespace N3O.Umbraco.Storage;

[Order(0)]
public class AzureStartupStorage : IStartupStorage {
    public IStorageFolder GetStorageFolder(IConfiguration configuration, string folderPath) {
        var container = AzureBlobStorage.GetStorageContainerClient(configuration);

        if (!AzureBlobStorage.UsesIdentity(configuration)) {
            container.CreateIfNotExistsAsync().GetAwaiter().GetResult();
        }

        return new AzureStorageFolder(container, AzureBlobStorage.GetStorageFolderPath(configuration, folderPath));
    }
}
