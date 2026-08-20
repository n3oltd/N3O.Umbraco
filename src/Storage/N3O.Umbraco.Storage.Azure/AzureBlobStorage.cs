using Azure.Core;
using Azure.Identity;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;
using N3O.Umbraco.Extensions;
using System;

namespace N3O.Umbraco.Storage.Azure;

public static class AzureBlobStorage {
    public const string MediaSectionKey = "Umbraco:Storage:AzureBlob:Media";
    private const string ServiceUrlKey = "ServiceUrl";
    private const string ConnectionStringKey = "ConnectionString";
    private const string ContainerNameKey = "ContainerName";
    private const string SingleContainerKey = "SingleContainer";

    // ServiceUrl and ConnectionString are mutually exclusive. ServiceUrl selects
    // DefaultAzureCredential (workload identity, managed identity or developer
    // credentials), so no storage secret appears in configuration.
    public static bool UsesIdentity(IConfiguration configuration) {
        return GetSection(configuration)[ServiceUrlKey].HasValue();
    }

    // Independent of the credential choice: with SingleContainer general file storage
    // shares the media container under the storage prefix; otherwise it uses a
    // separate storage container in the same account.
    public static bool UsesSingleContainer(IConfiguration configuration) {
        return bool.TryParse(GetSection(configuration)[SingleContainerKey], out var single) && single;
    }

    public static BlobContainerClient GetMediaContainerClient(IConfiguration configuration) {
        var section = GetSection(configuration);

        return GetContainerClient(section, section[ContainerNameKey]);
    }

    public static BlobContainerClient GetStorageContainerClient(IConfiguration configuration) {
        var section = GetSection(configuration);
        var containerName = UsesSingleContainer(configuration)
                                ? section[ContainerNameKey]
                                : AzureStorageConstants.StorageContainerName.ToLowerInvariant();

        return GetContainerClient(section, containerName);
    }

    public static string GetStorageFolderPath(IConfiguration configuration, string folderPath) {
        return UsesSingleContainer(configuration)
                   ? $"{AzureStorageConstants.StoragePrefix}/{folderPath.TrimStart('/')}"
                   : folderPath;
    }

    public static Uri GetBlobUri(IConfiguration configuration, string blobName) {
        var section = GetSection(configuration);

        return new Uri($"{GetContainerUri(section[ServiceUrlKey], section[ContainerNameKey])}/{blobName}");
    }

    public static TokenCredential GetCredential() {
        return new DefaultAzureCredential();
    }

    private static BlobContainerClient GetContainerClient(IConfigurationSection section, string containerName) {
        if (section[ServiceUrlKey].HasValue()) {
            return new BlobContainerClient(GetContainerUri(section[ServiceUrlKey], containerName), GetCredential());
        } else {
            return new BlobContainerClient(section[ConnectionStringKey], containerName);
        }
    }

    private static Uri GetContainerUri(string serviceUrl, string containerName) {
        return new Uri($"{serviceUrl?.TrimEnd('/')}/{containerName}");
    }

    private static IConfigurationSection GetSection(IConfiguration configuration) {
        return configuration.GetSection(MediaSectionKey);
    }
}
