using Microsoft.AspNetCore.Hosting;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace N3O.Umbraco.Hosting;

public static class WebRoot {
    public static void DeleteFile(IWebHostEnvironment webHostEnvironment, string path) {
        var filePath = Path.Combine(webHostEnvironment.WebRootPath, path);

        if (File.Exists(filePath)) {
            File.Delete(filePath);
        }
    }

    public static DirectoryInfo GetDirectory(IWebHostEnvironment webHostEnvironment, string path) {
        var directory = new DirectoryInfo(Path.Combine(webHostEnvironment.WebRootPath, path));

        if (directory.Exists) {
            return directory;
        } else {
            return null;
        }
    }

    public static IReadOnlyList<FileInfo> GetFiles(IWebHostEnvironment webHostEnvironment, string searchPattern) {
        var directory = new DirectoryInfo(webHostEnvironment.WebRootPath);

        if (directory.Exists) {
            return directory.GetFiles(searchPattern);
        } else {
            return new List<FileInfo>();
        }
    }

    public static async Task SaveFileAsync(IWebHostEnvironment webHostEnvironment, string path, Stream content) {
        var filePath = Path.Combine(webHostEnvironment.WebRootPath, path);
        
        using (var fileStream = File.Create(filePath)) {
            await content.CopyToAsync(fileStream);
        }
    }
    
    public static async Task SaveTextAsync(IWebHostEnvironment webHostEnvironment, string path, string text) {
        using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(text))) {
            await SaveFileAsync(webHostEnvironment, path, stream);
        }
    }
}