using N3O.Umbraco.Attributes;
using N3O.Umbraco.Extensions;
using System.Threading.Tasks;

namespace N3O.Umbraco.Data.Parsing;

[Order(1)]
public class StorageFolderBlobResolver : BlobResolver {
    private readonly IStorageFolder _storageFolder;

    public StorageFolderBlobResolver(IStorageFolder storageFolder) {
        _storageFolder = storageFolder;
    }

    protected override bool TryCanResolve(string value) {
        return value.HasValue();
    }

    protected override async Task<Blob> TryResolveAsync(string value) {
        return await _storageFolder.GetFileAsync(value.Trim());
    }
}
