using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Storage.Services {
    public interface ITempStorage {
        Task<Blob> AddFileAsync(string name, Stream stream);
        Task DeleteFileAsync(string name);
        Task<Blob> GetFileAsync(string name, CancellationToken cancellationToken = default);
    }
}