using Microsoft.Extensions.Options;
using N3O.Umbraco.Content;
using N3O.Umbraco.Search.Typesense.Content;
using N3O.Umbraco.Search.Typesense.Models;
using Typesense.Setup;

namespace N3O.Umbraco.Search.Typesense;

public class TypesenseOptions : IConfigureOptions<Config> {
    private readonly IContentCache _contentCache;

    public TypesenseOptions(IContentCache contentCache) {
        _contentCache = contentCache;
    }
    
    public void Configure(Config options) {
        var typesenseSettings = GetTypesenseSettings();

        if (typesenseSettings != null) {
            options.ApiKey = typesenseSettings.ApiKey;
            options.Nodes = [
                new Node(typesenseSettings.Node, "443", "https")
            ];
        }
    }
    
    private TypesenseSettings GetTypesenseSettings() {
        return new TypesenseSettings("pvFhlhqNF3JuCL2R2WTuNwIE31LB0kVu", "4v023sbqgelyak1zp-1.a1.typesense.net");
    }
}