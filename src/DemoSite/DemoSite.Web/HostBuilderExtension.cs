using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using N3O.Umbraco.Hosting;
using System.IO;

namespace DemoSite.Web;

public class HostBuilderExtension : IHostBuilderExtension {
    public void Run(IHostBuilder hostBuilder) {
        AddConfigFileIfExists(hostBuilder, "Authentication");
        AddConfigFileIfExists(hostBuilder, "Environment");
    }

    private void AddConfigFileIfExists(IHostBuilder hostBuilder, string suffix) {
        if (File.Exists($"appsettings.{suffix}.json")) {
            hostBuilder.ConfigureAppConfiguration(config => {
                config.AddJsonFile($"appsettings.{suffix}.json");
            });
        }
    }
}