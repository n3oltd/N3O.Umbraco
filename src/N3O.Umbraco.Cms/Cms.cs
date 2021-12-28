using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Utilities;

namespace N3O.Umbraco;

public static class Cms {
    public static void Run<TStartup>(string[] args, string ourAssembliesPrefix)
        where TStartup : StartupBase {
        OurAssemblies.Configure(ourAssembliesPrefix);

        var hostBuilder = Host.CreateDefaultBuilder(args)
                              .ConfigureLogging(x => x.ClearProviders())
                              .ConfigureWebHostDefaults(webBuilder => {
                                  webBuilder.RunExtensions();
                                  
                                  webBuilder.UseStartup<TStartup>();
                              });

        hostBuilder.RunExtensions();

        hostBuilder.Build().Run();
    }
}
