using Humanizer.Bytes;
using Microsoft.AspNetCore.Hosting;
using N3O.Umbraco.Utilities;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Storage.Services {
    public class DiskStorageFolder : IStorageFolder {
        private readonly string _folderName;
        private readonly string _storageRootPath;

        public DiskStorageFolder(IWebHostEnvironment webHostEnvironment,  string folderName) {
            _folderName = folderName;
            _storageRootPath = Path.Combine(webHostEnvironment.WebRootPath,
                                            StorageConstants.StorageFolderName,
                                            folderName);
        }
        
        public async Task AddFileAsync(string filename, Stream stream) {
            using (var fileStream = File.OpenWrite(GetPath(filename))) {
                await stream.CopyToAsync(fileStream);
            }
        }

        public async Task AddFileAsync(string filename, byte[] contents) {
            await File.WriteAllBytesAsync(GetPath(filename), contents);
        }

        public Task DeleteAllFilesAsync() {
            foreach (var file in Directory.GetFiles(_storageRootPath)) {
                File.Delete(file);
            }

            return Task.CompletedTask;
        }

        public Task DeleteFileAsync(string filename) {
            File.Delete(GetPath(filename));
            
            return Task.CompletedTask;
        }

        public Task<Blob> GetFileAsync(string filename, CancellationToken cancellationToken = default) {
            var file = File.OpenRead(GetPath(filename));
            var blob = new Blob(filename,
                                _folderName,
                                FileUtility.GetContentType(filename),
                                ByteSize.FromBytes(file.Length),
                                file);

            return Task.FromResult(blob);
        }
        
        private string GetPath(string filename) {
            return Path.Combine(_storageRootPath, filename);
        }
    }
}