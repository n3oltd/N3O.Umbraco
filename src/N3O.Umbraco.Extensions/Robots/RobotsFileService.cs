using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Threading.Tasks;

namespace N3O.Umbraco.Robots;

public class RobotsFileService : IRobotsFileService {
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly IRobotsTxt _robotsTxt;

    public RobotsFileService(IWebHostEnvironment webHostEnvironment, IRobotsTxt robotsTxt) {
        _webHostEnvironment = webHostEnvironment;
        _robotsTxt = robotsTxt;
    }

    public async Task SaveRobotsFileToWwwroot() {

        var robotsContent = _robotsTxt.GetContent();
        
        string wwwrootPath = _webHostEnvironment.WebRootPath;
        
        string filePath = Path.Combine(wwwrootPath, "robots.txt");
        
        await File.WriteAllTextAsync(filePath, robotsContent);
    }
}