using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using N3O.Umbraco.Composing;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Search.Typesense.Builders;
using N3O.Umbraco.Search.Typesense.Indexing;
using N3O.Umbraco.Search.Typesense.Services;
using System.Net;
using System.Net.Http;
using Typesense;
using Typesense.Setup;
using Umbraco.Cms.Core.DependencyInjection;

namespace N3O.Umbraco.Search.Typesense;

public class TypesenseSearchComposer : Composer {
    public override void Compose(IUmbracoBuilder builder) {
        builder.Services.AddTransient<ITypesenseService, TypesenseService>();
        
        builder.Services.AddTransient<ISearchDocumentBuilder, SearchDocumentBuilder>();
        
        builder.Services
               .AddHttpClient(nameof(TypesenseClient), client => {
                   client.DefaultRequestVersion = HttpVersion.Version30;
               })
               .ConfigurePrimaryHttpMessageHandler(_ => new HttpClientHandler {
                   AutomaticDecompression = DecompressionMethods.All
               });
        
        builder.Services.AddSingleton<IConfigureOptions<Config>, TypesenseOptions>();

        builder.Services.AddScoped<ITypesenseClient>(serviceProvider => {
            var httpClientFactory = serviceProvider.GetService<IHttpClientFactory>();
            var httpClient = httpClientFactory.CreateClient(nameof(TypesenseClient));
            var configOptions = serviceProvider.GetRequiredService<IOptions<Config>>();

            return new TypesenseClient(configOptions, httpClient);
        });
        
        RegisterAll(t => t.ImplementsInterface<ISearchIndexer>(),
                    t => builder.Services.AddTransient(typeof(ISearchIndexer), t));
    }
} 
