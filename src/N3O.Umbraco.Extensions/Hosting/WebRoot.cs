using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace N3O.Umbraco.Hosting;

public static class WebRoot {
    public static async Task SaveFileAsync(IWebHostEnvironment webHostEnvironment, string filename, Stream content) {
        var path = Path.Combine(webHostEnvironment.WebRootPath, filename);
        
        using (var fileStream = File.Create(path)) {
            await content.CopyToAsync(fileStream);
        }
    }
    
    public static async Task SaveTextAsync(IWebHostEnvironment webHostEnvironment, string filename, string text) {
        using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(text))) {
            await SaveFileAsync(webHostEnvironment, filename, stream);
        }
    }
}