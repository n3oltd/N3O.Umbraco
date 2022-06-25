using N3O.Umbraco.Storage;
using System.Collections.Generic;

namespace N3O.Umbraco.Data.Parsing;

public static class BlobResolvers {
    public static IEnumerable<IBlobResolver> All(IStorageFolder storageFolder) {
        return GetResolvers(new UrlBlobResolver(), new StorageFolderBlobResolver(storageFolder));
    }
    
    public static IEnumerable<IBlobResolver> Url() {
        return GetResolvers(new UrlBlobResolver());
    }
    
    private static IEnumerable<IBlobResolver> GetResolvers(params IBlobResolver[] blobResolvers) {
        return blobResolvers;
    }
}
