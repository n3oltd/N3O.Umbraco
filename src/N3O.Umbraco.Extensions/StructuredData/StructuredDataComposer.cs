using Microsoft.Extensions.DependencyInjection;
using N3O.Umbraco.Composing;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Utilities;
using System;
using System.Linq;
using Umbraco.Cms.Core.DependencyInjection;

namespace N3O.Umbraco.StructuredData;

public class StructuredDataComposer : Composer {
    public override void Compose(IUmbracoBuilder builder) {
        RegisterAll(t => t.ImplementsGenericInterface(typeof(IProvideStructuredDataFor<>)),
                    t => RegisterProvider(builder, t));
    }

    private void RegisterProvider(IUmbracoBuilder builder, Type providerType) {
        var providerContentType = providerType.GetGenericParameterTypesForImplementedGenericInterface(typeof(IProvideStructuredDataFor<>))
                                              .Single();

        var allowedContentTypes = OurAssemblies.GetTypes(t => t.IsConcreteClass() &&
                                                              providerContentType.IsAssignableFrom(t))
                                               .ToList();

        foreach (var allowedContentType in allowedContentTypes) {
            var interfaceType = typeof(IProvideStructuredDataFor<>).MakeGenericType(allowedContentType);
                
            builder.Services.AddTransient(interfaceType, providerType);
        }
    }
}
