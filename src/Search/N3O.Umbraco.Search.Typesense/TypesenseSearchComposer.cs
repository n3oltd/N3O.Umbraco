using System.Collections.Generic;
using Flurl;
using Microsoft.Extensions.DependencyInjection;
using N3O.Umbraco.Composing;
using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Search.Typesense.Builders;
using N3O.Umbraco.Search.Typesense.Content.Settings;
using N3O.Umbraco.Search.Typesense.Indexing;
using N3O.Umbraco.Search.Typesense.Services;
using Umbraco.Cms.Core.DependencyInjection;
using Typesense;
using Typesense.Setup;

namespace N3O.Umbraco.Search.Typesense;

public class TypesenseSearchComposer : Composer {

    public override void Compose(IUmbracoBuilder builder) {
        
        builder.Services.AddTransient<ITypesenseService, TypesenseService>();
        
        builder.Services.AddTransient<ISearchDocumentBuilder, SearchDocumentBuilder>();
        
        builder.Services.AddTypesenseClient(config =>
        {
            config.ApiKey = "abc";
            config.Nodes = new List<Node>
            {
                new Node("localhost", "8108", "http")
            };
        });
        builder.Services.AddSingleton<TypesenseService>();
        
        RegisterAll(t => t.ImplementsInterface<ISearchIndexer>(),
            t => builder.Services.AddTransient(typeof(ISearchIndexer), t));
    }
} 
