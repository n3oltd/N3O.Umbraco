using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using N3O.Umbraco.Hosting;
using System.IO;

namespace DemoSite.Web;

public class HostBuilderExtension : IHostBuilderExtension {
    public void Run(IHostBuilder hostBuilder) {
        if (File.Exists("appsettings.Authentication.json")) {
            hostBuilder.ConfigureAppConfiguration(config => {
                config.AddJsonFile("appsettings.Authentication.json");
            });
        }
    }
}