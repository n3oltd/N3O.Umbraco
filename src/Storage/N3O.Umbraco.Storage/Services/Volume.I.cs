using System.Threading.Tasks;

namespace N3O.Umbraco.Storage;

public interface IVolume {
    Task<IStorageFolder> GetStorageFolderAsync(string folderPath);
}
