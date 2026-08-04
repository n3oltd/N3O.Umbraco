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
    private const string ContainerNameKey = "ContainerName";

    // ServiceUrl and ConnectionString are mutually exclusive. ServiceUrl selects
    // DefaultAzureCredential (workload identity, managed identity or developer
    // credentials), so no storage secret appears in configuration.
    public static bool UsesIdentity(IConfiguration configuration) {
        return configuration.GetSection(MediaSectionKey)[ServiceUrlKey].HasValue();
    }

    public static BlobContainerClient GetContainerClient(IConfiguration configuration) {
        return new BlobContainerClient(GetContainerUri(configuration), GetCredential());
    }

    public static Uri GetBlobUri(IConfiguration configuration, string blobName) {
        return new Uri($"{GetContainerUri(configuration)}/{blobName}");
    }

    public static TokenCredential GetCredential() {
        return new DefaultAzureCredential();
    }

    private static Uri GetContainerUri(IConfiguration configuration) {
        var section = configuration.GetSection(MediaSectionKey);
        var serviceUrl = section[ServiceUrlKey]?.TrimEnd('/');
        var containerName = section[ContainerNameKey];

        return new Uri($"{serviceUrl}/{containerName}");
    }
}
