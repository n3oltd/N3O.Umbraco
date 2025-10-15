using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Utilities;

namespace N3O.Umbraco.Cms;

public static class UmbracoCms {
    public static void Run<TStartup>(string[] args, string ourAssembliesPrefix, bool useIISIntegration = false)
        where TStartup : CmsStartup {
        OurAssemblies.Configure(ourAssembliesPrefix);
        
        var hostBuilder = Host.CreateDefaultBuilder(args)
                              .ConfigureLogging(x => x.ClearProviders())
                              .ConfigureUmbracoDefaults()
                              .ConfigureWebHostDefaults(webBuilder => {
                                  webBuilder.RunExtensions();
                                  webBuilder.UseStaticWebAssets();
                                  webBuilder.UseStartup<TStartup>();

                                  if (useIISIntegration) {
                                      webBuilder.UseIISIntegration();
                                  }
                              });

        hostBuilder.RunExtensions();

        hostBuilder.Build().Run();
    }
}
