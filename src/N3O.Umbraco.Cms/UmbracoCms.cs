using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using N3O.Umbraco.Composing;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Utilities;
using System;

namespace N3O.Umbraco.Cms;

public static class UmbracoCms {
    public static void Run<TStartup>(string[] args, string ourAssembliesPrefix, bool useIISIntegration = false)
        where TStartup : CmsStartup {
        OurAssemblies.Configure(ourAssembliesPrefix);
        
        var hostBuilder = Host.CreateDefaultBuilder(args)
                              .ConfigureLogging(x => x.ClearProviders())
                              .ConfigureUmbracoDefaults()
                              .ConfigureWebHostDefaults(webBuilder => {
                                  webBuilder.UseStaticWebAssets();
                                  webBuilder.UseStartup<TStartup>();

                                  if (useIISIntegration) {
                                      webBuilder.UseIISIntegration();
                                  }
                                  
                                  webBuilder.ConfigureKestrel(opt => {
                                      opt.Limits.MaxRequestHeadersTotalSize = 128_000;
                                      opt.Limits.MaxRequestBodySize = 1_073_741_824;
                                  });
                                  
                                  webBuilder.ConfigureAppConfiguration((context, _) => {
                                      Composer.WebHostEnvironment = context.HostingEnvironment;
                                      
                                      webBuilder.RunExtensions();
                                  });
                              });

        hostBuilder.RunExtensions();

        hostBuilder.Build().Run();
    }
}
