using N3O.Umbraco.Storage.Services;
using System.Threading.Tasks;

namespace N3O.Umbraco.Storage.Extensions {
    public static class VolumeExtensions {
        public static async Task<IStorageFolder> GetTempFolderAsync(this IVolume volume) {
            return await volume.GetStorageFolderAsync(StorageConstants.StorageFolders.Temp);
        }
        
        public static async Task<Blob> MoveFileAsync(this IVolume volume,
                                                     string filename,
                                                     string fromFolderName,
                                                     string toFolderName) {
            var fromStorageFolder = await volume.GetStorageFolderAsync(fromFolderName);
            var toStorageFolder = await volume.GetStorageFolderAsync(toFolderName);

            var fromBlob = await fromStorageFolder.GetFileAsync(filename);
            using (fromBlob.Stream) {
                await toStorageFolder.AddFileAsync(filename, fromBlob.Stream);

                var toBlob = await toStorageFolder.GetFileAsync(filename);

                await fromStorageFolder.DeleteFileAsync(filename);

                return toBlob;
            }
        }

        public static async Task<Blob> MoveTempFileAsync(this IVolume volume, string filename, string toFolderName) {
            return await MoveFileAsync(volume, filename, StorageConstants.StorageFolders.Temp, toFolderName);
        }
    }
}