using Microsoft.AspNetCore.Hosting;
using N3O.Umbraco.Hosting;
using N3O.Umbraco.Utilities;
using System;
using System.Linq;

namespace N3O.Umbraco.Extensions; 

public static class WebHostBuilderExtensions {
    public static void RunExtensions(this IWebHostBuilder webBuilder) {
        var extensions = OurAssemblies.GetTypes(t => t.IsConcreteClass() &&
                                                     t.HasParameterlessConstructor() &&
                                                     t.ImplementsInterface<IWebHostBuilderExtension>())
                                      .Select(t => (IWebHostBuilderExtension) Activator.CreateInstance(t))
                                      .ToList();

        foreach (var extension in extensions) {
            extension.Run(webBuilder);
        }
    }
}