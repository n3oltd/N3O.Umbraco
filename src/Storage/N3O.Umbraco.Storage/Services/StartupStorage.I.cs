using Umbraco.Cms.Core.DependencyInjection;

namespace N3O.Umbraco.Storage;

public interface IStartupStorage {
    IStorageFolder GetStorageFolder(IUmbracoBuilder builder, string folderPath);
}