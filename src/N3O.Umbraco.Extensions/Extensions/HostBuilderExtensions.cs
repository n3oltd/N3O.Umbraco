using Microsoft.Extensions.Hosting;
using N3O.Umbraco.Hosting;
using N3O.Umbraco.Utilities;
using System;
using System.Linq;

namespace N3O.Umbraco.Extensions; 

public static class HostBuilderExtensions {
    public static void RunExtensions(this IHostBuilder hostBuilder) {
        var extensions = OurAssemblies.GetTypes(t => t.IsConcreteClass() &&
                                                     t.HasParameterlessConstructor() &&
                                                     t.ImplementsInterface<IHostBuilderExtension>())
                                      .Select(t => (IHostBuilderExtension) Activator.CreateInstance(t))
                                      .ToList();

        foreach (var extension in extensions) {
            extension.Run(hostBuilder);
        }
    }
}