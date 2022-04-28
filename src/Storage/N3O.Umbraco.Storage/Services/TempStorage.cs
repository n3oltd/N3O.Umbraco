using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Storage.Services {
    public class TempStorage : ITempStorage {
        private readonly Lazy<ILogger<TempStorage>> _logger;
        private readonly Lazy<IVolume> _volume;
        private IStorageFolder _storageFolder;

        public TempStorage(Lazy<ILogger<TempStorage>> logger, Lazy<IVolume> volume) {
            _logger = logger;
            _volume = volume;
        }
        
        public async Task<Blob> AddFileAsync(string name, Stream stream) {
            var storageFolder = await GetStorageFolderAsync();

            return await storageFolder.AddFileAsync(name, stream);
        }

        public async Task DeleteFileAsync(string name) {
            try {
                var storageFolder = await GetStorageFolderAsync();

                await storageFolder.DeleteFileAsync(name);
            } catch (Exception ex) {
                _logger.Value.LogError(ex, "Error deleting {File} from temp storage", name);
            }
        }

        public async Task<Blob> GetFileAsync(string name, CancellationToken cancellationToken = default) {
            var storageFolder = await GetStorageFolderAsync();

            return await storageFolder.GetFileAsync(name, cancellationToken);
        }

        private async Task<IStorageFolder> GetStorageFolderAsync() {
            if (_storageFolder == null) {
                _storageFolder = await _volume.Value.GetStorageFolderAsync("Temp");
            }
            
            return _storageFolder;
        }
    }
}