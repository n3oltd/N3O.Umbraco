using N3O.Umbraco.Hosting;
using N3O.Umbraco.Utilities;
using System;
using System.Linq;
using Umbraco.Cms.Web.Common.ApplicationBuilder;

namespace N3O.Umbraco.Extensions {
    public static class UmbracoEndpointBuilderContextExtensions {
        public static void RunExtensions(this IUmbracoEndpointBuilderContext u) {
            var extensions = OurAssemblies.GetTypes(t => t.IsConcreteClass() &&
                                                         t.HasParameterlessConstructor() &&
                                                         t.ImplementsInterface<IUmbracoEndpointExtension>())
                                          .Select(t => (IUmbracoEndpointExtension) Activator.CreateInstance(t))
                                          .ToList();

            foreach (var extension in extensions) {
                extension.Run(u);
            }
        }
    }
}