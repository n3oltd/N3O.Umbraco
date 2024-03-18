using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;
using N3O.Umbraco.Attributes;
using N3O.Umbraco.Storage.Azure;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.StorageProviders.AzureBlob.IO;

namespace N3O.Umbraco.Storage;

[Order(0)]
public class AzureStartupStorage : IStartupStorage {
    public IStorageFolder GetStorageFolder(IUmbracoBuilder builder, string folderPath) {
        var options = new AzureBlobFileSystemOptions();
        builder.Config.GetSection("Umbraco:Storage:AzureBlob:Media").Bind(options);
        
        var serviceClient = new BlobServiceClient(options.ConnectionString);
        var container = serviceClient.GetBlobContainerClient(AzureStorageConstants.StorageContainerName
                                                                                  .ToLowerInvariant());

        container.CreateIfNotExistsAsync().GetAwaiter().GetResult();
        
        return new AzureStorageFolder(container, folderPath);
    }
}