using Microsoft.Extensions.DependencyInjection;
using N3O.Umbraco.Composing;
using N3O.Umbraco.Extensions;
using System;
using System.Linq;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Extensions;

namespace N3O.Umbraco.Lookups {
    public class LookupsComposer : Composer {
        public override void Compose(IUmbracoBuilder builder) {
            builder.Services.AddSingleton<ILookups, Lookups>();
        
            RegisterAll(t => t.ImplementsGenericInterface(typeof(ILookupsCollection<>)),
                        t => RegisterLookupsCollection(builder, t));

            RegisterAll(t => t.IsSubclassOrSubInterfaceOfGenericType(typeof(LookupContent<>)),
                        t => RegisterUmbracoLookupsCollection(builder, t));
            
            RegisterAll(t => t.Implements<IPublishedContent>() && t.Implements<ILookup>(),
                        t => RegisterUmbracoLookupsCollection(builder, t));
        }

        private void RegisterLookupsCollection(IUmbracoBuilder builder, Type collectionType) {
            var interfaceTypes = collectionType.GetInterfaces().ToList();

            foreach (var interfaceType in interfaceTypes) {
                // TODO This and below should pull lifetime from an interface
                builder.Services.AddSingleton(interfaceType, collectionType);
            }
        }
    
        private void RegisterUmbracoLookupsCollection(IUmbracoBuilder builder, Type lookupContentType) {
            var interfaceType = typeof(ILookupsCollection<>).MakeGenericType(lookupContentType);
            var collectionType = typeof(UmbracoLookupsCollection<>).MakeGenericType(lookupContentType);
            
            builder.Services.AddSingleton(interfaceType, collectionType);
        }
    }
}
