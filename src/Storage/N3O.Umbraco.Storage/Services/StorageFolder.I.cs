using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Storage;

public interface IStorageFolder {
    Task AddFileAsync(string filename, Stream stream);
    Task AddFileAsync(string filename, byte[] contents);
    Task DeleteAllFilesAsync();
    Task DeleteFileAsync(string filename);
    Task<Blob> GetFileAsync(string filename, CancellationToken cancellationToken = default);
}
