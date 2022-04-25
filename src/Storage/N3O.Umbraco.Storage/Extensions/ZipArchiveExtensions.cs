using N3O.Umbraco.Storage.Services;
using System.IO.Compression;
using System.Threading.Tasks;

namespace N3O.Umbraco.Storage.Extensions {
    public static class ZipArchiveExtensions {
        public static async Task ExtractToStorageFolderAsync(this ZipArchive zipArchive, IStorageFolder storageFolder) {
            foreach (var entry in zipArchive.Entries) {
                using (var stream = entry.Open()) {
                    await storageFolder.AddFileAsync(entry.Name, stream);
                }
            }
        }
    }
}