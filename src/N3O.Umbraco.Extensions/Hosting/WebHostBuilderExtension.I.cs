using Microsoft.AspNetCore.Hosting;

namespace N3O.Umbraco.Hosting; 

public interface IWebHostBuilderExtension {
    void Run(IWebHostBuilder webBuilder);
}