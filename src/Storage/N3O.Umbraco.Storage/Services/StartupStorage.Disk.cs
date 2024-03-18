using Microsoft.Extensions.Configuration;
using N3O.Umbraco.Attributes;
using N3O.Umbraco.Composing;

namespace N3O.Umbraco.Storage;

[Order(int.MaxValue)]
public class DiskStartupStorage : IStartupStorage {
    public IStorageFolder GetStorageFolder(IConfiguration configuration, string folderPath) {
        return new DiskStorageFolder(Composer.WebHostEnvironment, folderPath);
    }
}