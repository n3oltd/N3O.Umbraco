using Microsoft.AspNetCore.Hosting;
using System.Threading.Tasks;

namespace N3O.Umbraco.Storage.Services;

public class DiskVolume : IVolume {
    private readonly IWebHostEnvironment _webHostEnvironment;

    public DiskVolume(IWebHostEnvironment webHostEnvironment) {
        _webHostEnvironment = webHostEnvironment;
    }
    
    public Task<IStorageFolder> GetStorageFolderAsync(string foldePath) {
        return Task.FromResult<IStorageFolder>(new DiskStorageFolder(_webHostEnvironment, foldePath));
    }
}
