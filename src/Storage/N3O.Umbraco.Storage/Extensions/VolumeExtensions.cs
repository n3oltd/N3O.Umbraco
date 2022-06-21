using N3O.Umbraco.Storage.Services;
using System.Threading.Tasks;

namespace N3O.Umbraco.Storage.Extensions;

public static class VolumeExtensions {
    public static async Task<Blob> MoveFileAsync(this IVolume volume,
                                                 string filename,
                                                 string fromFolderName,
                                                 string toFolderName) {
        var fromStorageFolder = await volume.GetStorageFolderAsync(fromFolderName);
        var toStorageFolder = await volume.GetStorageFolderAsync(toFolderName);

        var fromBlob = await fromStorageFolder.GetFileAsync(filename);
        Blob toBlob;
        using (fromBlob.Stream) {
            await toStorageFolder.AddFileAsync(filename, fromBlob.Stream);

            toBlob = await toStorageFolder.GetFileAsync(filename);
        }
        
        await fromStorageFolder.DeleteFileAsync(filename);
        
        return toBlob;
    }
}
