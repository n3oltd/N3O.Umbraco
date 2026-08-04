# N3O.Umbraco.Storage.Azure

Azure Blob Storage for Umbraco 13 sites: media, ImageSharp cache, data protection keys and
general file storage, configured from a single `Umbraco:Storage:AzureBlob:Media` section. The
presence of `ServiceUrl` or `ConnectionString` in that section selects one of two mutually
exclusive modes.

## Identity mode — no storage secrets

Set a `ServiceUrl` and `ContainerName` and no secret is configured anywhere. All blob access
authenticates through `DefaultAzureCredential`, so anything it supports works: workload
identity on AKS, managed identity on App Service, or Azure CLI credentials during local
development. The identity needs `Storage Blob Data Contributor` on the container, which can
be scoped to that single container.

```json
{
  "Umbraco": {
    "Storage": {
      "AzureBlob": {
        "Media": {
          "ServiceUrl": "https://mystorageaccount.blob.core.windows.net",
          "ContainerName": "mysite"
        }
      }
    }
  }
}
```

One container holds everything, under fixed prefixes:

| Path | Contents |
| --- | --- |
| `media/` | Umbraco media |
| `cache/` | ImageSharp image cache (regenerable) |
| `storage/` | General file storage (`IVolume` / startup storage) |
| `datakeys.xml` | ASP.NET Core data protection keyring |

The container must already exist — a container-scoped identity cannot create it.

## Connection string mode

Set a `ConnectionString` and `ContainerName` and everything authenticates with the account
key carried in the string. Media, cache and the keyring live in the named container; general
file storage uses a separate `storage` container in the same account, created on demand.

```json
{
  "Umbraco": {
    "Storage": {
      "AzureBlob": {
        "Media": {
          "ConnectionString": "DefaultEndpointsProtocol=https;AccountName=…;AccountKey=…",
          "ContainerName": "umbraco"
        }
      }
    }
  }
}
```

## Moving between modes

The blob layout for media (`media/…`) and the cache (`cache/…`) is identical in both modes,
so a site can switch by changing configuration alone, provided the identity has been granted
access and any `storage` container contents are copied under the `storage/` prefix. The
keyring is intentionally per-container: a site switching containers mints a fresh keyring,
which invalidates existing auth cookies and anti-forgery tokens once.
