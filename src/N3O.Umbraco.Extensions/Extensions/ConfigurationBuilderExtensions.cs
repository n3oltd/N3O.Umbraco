using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using N3O.Umbraco.Hosting;
using N3O.Umbraco.Utilities;
using System;
using System.Linq;

namespace N3O.Umbraco.Extensions;

public static class ConfigurationBuilderExtensions {
    public static void RunExtensions(this IConfigurationBuilder configurationBuilder, WebHostBuilderContext context) {
        var extensions = OurAssemblies.GetTypes(t => t.IsConcreteClass() &&
                                                     t.HasParameterlessConstructor() &&
                                                     t.ImplementsInterface<IConfigurationBuilderExtension>())
                                      .Select(t => (IConfigurationBuilderExtension) Activator.CreateInstance(t))
                                      .ToList();

        foreach (var extension in extensions) {
            extension.Run(configurationBuilder, context);
        }
    }
}
