using Microsoft.AspNetCore.Hosting;
using System.Threading.Tasks;

namespace N3O.Umbraco.Storage;

public class DiskVolume : IVolume {
    private readonly IWebHostEnvironment _webHostEnvironment;

    public DiskVolume(IWebHostEnvironment webHostEnvironment) {
        _webHostEnvironment = webHostEnvironment;
    }
    
    public Task<IStorageFolder> GetStorageFolderAsync(string folderPath) {
        return Task.FromResult<IStorageFolder>(new DiskStorageFolder(_webHostEnvironment, folderPath));
    }
}
