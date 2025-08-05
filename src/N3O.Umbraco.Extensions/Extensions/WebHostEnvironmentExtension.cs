using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Threading.Tasks;

namespace N3O.Umbraco.Extensions;

public static class WebHostEnvironmentExtension {
    public static async Task SaveFiletoWwwroot(this IWebHostEnvironment webHostEnvironment,
                                               string fileName,
                                               string fileContent) {
        var filePath = Path.Combine(webHostEnvironment.WebRootPath, fileName);
        
        await File.WriteAllTextAsync(filePath, fileContent);
    }
}