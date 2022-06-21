using System.Threading.Tasks;

namespace N3O.Umbraco.Storage.Services;

public interface IVolume {
    Task<IStorageFolder> GetStorageFolderAsync(string folderPath);
}
