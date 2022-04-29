using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Storage.Services {
    public interface IStorageFolder {
        Task<Blob> AddFileAsync(string filename, Stream stream);
        Task<Blob> AddFileAsync(string filename, byte[] contents);
        Task DeleteAllFilesAsync();
        Task DeleteFileAsync(string filename);
        Task<Blob> GetFileAsync(string filename, CancellationToken cancellationToken = default);
    }
}