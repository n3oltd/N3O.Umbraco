using Microsoft.Extensions.Configuration;

namespace N3O.Umbraco.Storage;

public interface IStartupStorage {
    IStorageFolder GetStorageFolder(IConfiguration configuration, string folderPath);
}