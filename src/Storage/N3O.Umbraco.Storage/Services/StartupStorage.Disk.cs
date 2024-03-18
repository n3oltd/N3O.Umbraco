using N3O.Umbraco.Attributes;
using N3O.Umbraco.Composing;
using Umbraco.Cms.Core.DependencyInjection;

namespace N3O.Umbraco.Storage;

[Order(int.MaxValue)]
public class DiskStartupStorage : IStartupStorage {
    public IStorageFolder GetStorageFolder(IUmbracoBuilder builder, string folderPath) {
        return new DiskStorageFolder(Composer.WebHostEnvironment, folderPath);
    }
}